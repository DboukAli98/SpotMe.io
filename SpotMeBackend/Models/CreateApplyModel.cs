using System.ComponentModel.DataAnnotations.Schema;

namespace SpotMeBackend.Models;

public class CreateApplyModel
{
    [Column(TypeName = "varchar(100)")]
    public string? fileName { get; set; }
    [NotMapped]
    public IFormFile pdfFile { get; set; }
    
    public string? cvFile { get; set; }
}