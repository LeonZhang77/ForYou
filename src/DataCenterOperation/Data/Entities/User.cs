using System.ComponentModel.DataAnnotations;

namespace DataCenterOperation.Data.Entities
{
    public class User : LoggableEntity
    {
        internal const string Default_Admin_Username = "admin@dco";
        internal const string Default_Admin_Password = Default_Admin_Username;

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        public bool IsAdmin { get; set; }

        [Required]
        public bool Disabled { get; set; }
    }
}
