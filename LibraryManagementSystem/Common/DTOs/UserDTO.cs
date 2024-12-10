﻿namespace Common.DTOs
{
    public class UserDTO
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
        public string Status { get; set; }
        public string Password { get; set; }
    }
}
