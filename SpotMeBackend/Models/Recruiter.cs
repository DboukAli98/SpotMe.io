using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SpotMeBackend.Models;

public class Recruiter
{
    public int RecruiterId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    public int EnterpriseId { get; set; }
    public Enterprise Enterprise { get; set; }
    public ICollection<Job> Jobs { get; set; }
    [Column(TypeName = "varchar(100)")]
    public string? ProfileName { get; set; }
    [NotMapped]
    public IFormFile ProfileLogo { get; set; }
    
    public string? Logo { get; set; }
}