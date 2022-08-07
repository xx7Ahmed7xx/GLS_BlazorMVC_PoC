using System.ComponentModel.DataAnnotations;

namespace GLS_BlazorMVC_PoC.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        [DataType(DataType.Password)]
        public string HashedPassword { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}
