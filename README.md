# Spirit-Island-Tracker-API

## Decription
This is a ASP.NET Web API used for the application Spirit Island Tracker. This keeps track of a user's game session
and its details.

## Endpoints

All endpoints are prefixed with /api/v1

|  HTTP Method  |    Endpoint   |  Description |
| ------------- | ------------- | -------|
| GET  | /spirits | Retrieve all Spirits
| GET  | /spirits/{id}  | Retrieve a Spirit by id
| GET  | /spirits/{pathname}  | Retrieve a Spirit by pathname
| GET  | /adversaries | Retrieve all adversaries
| GET  | /adversaries/{id} | Retrieve an adversary by id
| GET  | /adversaries/{pathname} | Retrieve an adversary by pathname
| GET  | /users  | Retrieve all users
| GET  | /users/{id} | Retrieve a user by id
| GET  | /game-sessions | Retrieve all game sessions
| GET  | /game-sessions/{id} | Retrieve a game session by id
| POST | /users/register | Register a new user
| POST | /game-sessions | Create a new game session
| PATCH | /users/update-username/{id} | Update an existing user's username
| PATCH | /users/update-password/{id} | Update an existing user's password
| PUT | /game-sessions/{id} | Update an exsting game session's data
| DELETE | /users/{id} | Delete a user by id
| DELETE | /game-sessions/{id} | Delete a game session by id
| POST | /auth/register | Register a new user
| POST | /auth/login | Login to existing user
