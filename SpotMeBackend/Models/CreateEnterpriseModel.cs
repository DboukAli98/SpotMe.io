using System.ComponentModel.DataAnnotations.Schema;

namespace SpotMeBackend.Models;

public class CreateEnterpriseModel
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
}