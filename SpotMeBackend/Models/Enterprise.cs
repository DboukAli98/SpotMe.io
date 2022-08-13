using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SpotMeBackend.Models;

public class Enterprise
{
    public int EnterpriseId { get; set; }
    public string? EnterpriseName { get; set; }
    public string? EnterpriseEmail { get; set; }
    public string? EnterpriseDescription { get; set; }
    public string? EnterpriseLocation { get; set; }
    public string? EnterprisePhone { get; set; }
    [Column(TypeName = "varchar(100)")]
    public string? LogoName { get; set; }
    [NotMapped]
    public IFormFile EnterpriseLogo { get; set; }
    
    public string? Logo { get; set; }
    [JsonIgnore]
    public ICollection<Job> Jobs { get; set; }
    [JsonIgnore]
    public ICollection<Recruiter> Recruiters { get; set; }




}