namespace UserService.Dtos
{
    using UserService.Models;

    public class UserPublishedDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string AccountName { get; set; } = null!;

        public bool IsWheel { get; set; }

        public bool IsAdmin { get; set; }

        public Role UserRole { get; set; }

        public bool IsActive { get; set; }

        public DateTime AccountCreationTime { get; set; }
        
        public string Event { get; set; } = null!;
    }
}
