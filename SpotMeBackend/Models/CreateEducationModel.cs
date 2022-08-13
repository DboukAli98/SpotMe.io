using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpotMeBackend.Models;

public class CreateEducationModel
{
    public int EducationId { get; set; }
    public string InstitutionName { get; set; }
    [DataType(DataType.Date)]
    [Column(TypeName = "Date")]
    public DateTime StartDate { get; set; }
    [DataType(DataType.Date)]
    [Column(TypeName = "Date")]
    public DateTime EndDate { get; set; }
    public string Major { get; set; }
    public string Domain { get; set; }
    public double GPA { get; set; }
}