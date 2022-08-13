using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SpotMeBackend.Models;

public class JobApplication
{
    public int Id { get; set; }
    [JsonIgnore]
    public Applicant Applicant { get; set; }
    [JsonIgnore]
    public Job? Job { get; set; }
    [Column(TypeName = "varchar(100)")]
    public string? fileName { get; set; }
    [NotMapped]
    public IFormFile pdfFile { get; set; }
    
    public string? cvFile { get; set; }
}