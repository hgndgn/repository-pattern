using System;

namespace RepositoryPattern.Api.DTOs
{
    public class DeveloperDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
    }
}