using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpotMeBackend.Models;

public class RegisterModel
{
    [Required]
    public string? Username { get; set; }
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }
    [Required]
    public string? Firstname { get; set; }
    [Required]
    public string? Lastname { get; set; }
    [Column(TypeName = "varchar(100)")]
    public string? ProfileName { get; set; }
    [NotMapped]
    public IFormFile ProfileLogo { get; set; }
    
    public string? Logo { get; set; }
}