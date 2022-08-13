namespace SpotMeBackend.Models;

public class CreateJobModel
{
    public int JobId { get; set; }
    public string? JobTitle { get; set; }
    public string? JobDescription { get; set; }
    public string? JobLocation { get; set; }
    public string? EmploymentType { get; set; }
    public string? JobDomain { get; set; }
    public string? Requirements { get; set; }
}