using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace UserService.Models{
    public class User{
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Phone]
        public string Phone { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string AccountName { get; set; } = null!;

        [Required]
        public bool IsWheel { get; set; }

        [Required]
        public bool IsAdmin { get; set; }

        [Required]
        [EnumDataType(typeof(Role))]
        public Role UserRole { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [DataType(DataType.Date)]
        public DateTime AccountCreationTime { get; set; }
    }

    public enum Role{
        SystemAdministrator,
        SecurityAdministrator,
        SoftwareDeveloper,
        DevopsEngineer,
        SoftwareEngineer,
        NetworkAdministrator,
        User,
        ProjectManager,
        Other
    }
}