using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpotMeBackend.Models;

public class Job
{
    public int JobId { get; set; }
    public string? JobTitle { get; set; }
    public string? JobDescription { get; set; }
    public string? JobLocation { get; set; }
    public string? EmploymentType { get; set; }
    public string? JobDomain { get; set; }
    public string? Requirements { get; set; }
    
    public ICollection<Applicant> Applicants { get; set; }
    
    public Recruiter Recruiter { get; set; }
    
    public Enterprise Enterprise { get; set; }
    
    
}