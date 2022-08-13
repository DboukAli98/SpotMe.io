using System.ComponentModel.DataAnnotations.Schema;

namespace SpotMeBackend.Models;

public class Applicant
{
    public int Id { get; set; }
    public ApplicationUser User { get; set; }
    public ICollection<Job> Jobs { get; set; }
    [Column(TypeName = "varchar(100)")]
    public string? ProfileName { get; set; }
    [NotMapped]
    public IFormFile ProfileLogo { get; set; }
    
    public string? Logo { get; set; }
    
    public string skills { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public ICollection<Education> Educations { get; set; }


}