﻿namespace ToDoListAPI.Models.UserManagement.DTOs
{
    public class TokenRequestModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool? RememberMe { get; set; }

    }
}
