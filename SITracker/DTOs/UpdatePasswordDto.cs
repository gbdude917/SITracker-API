﻿using Newtonsoft.Json;

namespace SITracker.DTOs
{
    public class UpdatePasswordDto
    {
        [JsonProperty("old_password")]
        public string? OldPassword { get; set; }

        [JsonProperty("new_password")]
        public string? NewPassword { get; set; }
    }
}
