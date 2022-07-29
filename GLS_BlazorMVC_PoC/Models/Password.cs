using System.ComponentModel.DataAnnotations;

namespace GLS_BlazorMVC_PoC.Models
{
    public class Password
    {
        [Key]
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string EncryptedPassword { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
