using Moq;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using SITracker.Controllers;
using SITracker.Data;
using SITracker.Exceptions;
using SITracker.Models;
using SITracker.Services;
using SITracker.Dtos;
using SITracker.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using SITracker.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;
using NuGet.Frameworks;

namespace SITracker.nUnitTests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private TrackerDbContext _mockContext;
        private Mock<DbSet<User>> _mockDbSet; 
        private Mock<IPasswordHasher<User>> _mockPasswordHasher;
        private Mock<IJwtService> _mockJwtService;
        private UserService _userService;
        private UsersController _usersController;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TrackerDbContext>()
                .UseInMemoryDatabase(databaseName: "TrackerDb")
                .Options;

            _mockContext = new TrackerDbContext(options);
            _mockDbSet = new Mock<DbSet<User>>();
            _mockPasswordHasher = new Mock<IPasswordHasher<User>>();
            _mockJwtService = new Mock<IJwtService>();
            _userService = new UserService(_mockContext, _mockPasswordHasher.Object, _mockJwtService.Object);
            _usersController = new UsersController(_userService);

            // Mock HttpContext with a ClaimsPrincipal containing the necessary claims
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "testuser")
            }, "mock"));

            var httpContext = new DefaultHttpContext
            {
                User = user
            };

            var controllerContext = new ControllerContext
            {
                HttpContext = httpContext,
                RouteData = new RouteData(),
                ActionDescriptor = new ControllerActionDescriptor()
            };

            _usersController.ControllerContext = controllerContext;
        }

        [TearDown]
        public void TearDown()
        {
            _mockContext.Database.EnsureDeleted();
            _mockContext.Dispose();
        }

        [Test]
        public async Task RegisterUserAsync_ReturnsUser_WhenRegistrationIsSuccessful()
        {
            // Assign
            var registerDto = new RegisterDto
            {
                Username = "username",
                Email = "test@test.com",
                Password = "password"
            };

            _mockPasswordHasher
                .Setup(ph => ph.HashPassword(It.IsAny<User>(), registerDto.Password))
                .Returns("hashedPassword");

            // Act
            var user = await _userService.RegisterUserAsync(registerDto);

            // Assert
            Assert.IsNotNull(user);
            Assert.That(user.Username, Is.EqualTo("username"));
            Assert.That(user.Email, Is.EqualTo("test@test.com"));
            Assert.That(user.Password, Is.EqualTo("hashedPassword"));
        }

        [Test]
        public async Task UpdateUsername_ReturnsUpdatedUser_WhenValidNewUsername()
        {
            // Assign 
            long userId = 1;
            var oldUsername = "oldUsername";
            var newUsername = "newUsername";

            // Add user to the mock db
            var user = new User 
            { 
                Id = userId, 
                Username = oldUsername,
                Password = "password",
                Email = "test@test.com"
            };
            _mockContext.Users.Add(user);
            await _mockContext.SaveChangesAsync();

            var updateUsernameDto = new UpdateUsernameDto { NewUsername = newUsername };

            // Act
            var result = await _userService.UpdateUsername(userId, updateUsernameDto);

            // Assert
            Assert.NotNull(result);

            // Check that return value matches expected
            Assert.That(result.Value.Username, Is.EqualTo(newUsername));

            // Check that the db is updated as well
            Assert.That(_mockContext.Users.Find(userId).Username, Is.EqualTo(newUsername));
        }

        [Test]
        public async Task UpdateUsername_ReturnsUnauthorized_WhenInvalidId()
        {
            // Arrange
            var updateUsernameDto = new UpdateUsernameDto
            {
                NewUsername = "newtestuser"
            };

            // Act
            var result = await _usersController.UpdateUsername(2, updateUsernameDto);

            // Assert
            Assert.IsInstanceOf<UnauthorizedResult>(result.Result);
        }

        [Test]
        public async Task UpdateUsername_ReturnsUsernameAlreadyExists_WhenDuplicateNewUsername()
        {
            // Assign 
            long userId = 1;
            var oldUsername = "oldUsername";
            var newUsername = "newUsername";

            // Add user and duplicateUser to the mock db
            var user = new User
            {
                Id = userId,
                Username = oldUsername,
                Password = "password",
                Email = "test@test.com"
            };

            var duplicateUser = new User
            {
                Id = 2,
                Username = newUsername,
                Password = "password",
                Email = "another@email.com"
            };

            _mockContext.Users.Add(user);
            _mockContext.Users.Add(duplicateUser);
            await _mockContext.SaveChangesAsync();

            var updateUsernameDto = new UpdateUsernameDto { NewUsername = newUsername };

            // Act
            var result = await _userService.UpdateUsername(userId, updateUsernameDto);

            // Assert
            Assert.NotNull(result);

            Assert.IsInstanceOf<ConflictObjectResult>(result.Result);
        }

        [Test]
        public async Task UpdatePassword_ReturnsUser_WhenSuccessfulPasswordUpdate()
        {
            // Assign
            var userId = 1L;
            var oldPassword = "password";
            var hashedPassword = "hashedPassword";

            // Mock the password hasher to hash the initial password
            _mockPasswordHasher.Setup(ph => ph.HashPassword(It.IsAny<User>(), oldPassword)).Returns(hashedPassword);

            var user = new User
            {
                Id = userId,
                Username = "oldUsername",
                Password = _mockPasswordHasher.Object.HashPassword(null, oldPassword),
                Email = "test@test.com"
            };

            _mockContext.Users.Add(user);
            await _mockContext.SaveChangesAsync();

            // Mock hashing of new password
            _mockPasswordHasher.Setup(ph => ph.HashPassword(user, "newPassword")).Returns("newHashedPassword");

            // Mock the verification result
            _mockPasswordHasher.Setup(ph => ph.VerifyHashedPassword(user, hashedPassword, oldPassword))
                .Returns(PasswordVerificationResult.Success);

            var updatePasswordDto = new UpdatePasswordDto 
            { 
                OldPassword = "password", 
                NewPassword = "newPassword" 
            };

            // Act
            var result = await _userService.UpdatePassword(userId, updatePasswordDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ActionResult<User>>(result);
            Assert.IsInstanceOf<User>(result.Value);
            Assert.That(result.Value.Password, Is.EqualTo("newHashedPassword"));
        }

        [Test]
        public async Task UpdatePassword_ReturnsPasswordsDoNotMatchException_WhenOldPasswordsDoNotMatch()
        {
            // Assign 
            var userId = 1L;
            var oldPassword = "password";
            var wrongOldPassword = "wrongPassword";
            var hashedPassword = "hashedPassword";

            // Mock the password hasher to hash the initial password
            _mockPasswordHasher.Setup(ph => ph.HashPassword(It.IsAny<User>(), oldPassword)).Returns(hashedPassword);

            var user = new User
            {
                Id = userId,
                Username = "oldUsername",
                Password = _mockPasswordHasher.Object.HashPassword(null, oldPassword),
                Email = "test@test.com"
            };

            _mockContext.Users.Add(user);
            await _mockContext.SaveChangesAsync();

            // Mock the failed verification result
            _mockPasswordHasher.Setup(ph => ph.VerifyHashedPassword(user, hashedPassword, wrongOldPassword))
                .Returns(PasswordVerificationResult.Failed);

            var updatePasswordDto = new UpdatePasswordDto
            {
                OldPassword = wrongOldPassword,
                NewPassword = "newPassword"
            };

            // Act
            var exception = Assert.ThrowsAsync<PasswordsDoNotMatchException>(async () => 
                await _userService.UpdatePassword(userId, updatePasswordDto));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Passwords do not match!"));
        }

    }
}