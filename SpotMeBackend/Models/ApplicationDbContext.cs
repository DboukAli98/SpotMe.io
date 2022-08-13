using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SpotMeBackend.Models;

public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Job> Jobs { get; set; }
    public DbSet<Enterprise> Enterprises { get; set; }
    public DbSet<Recruiter> Recruiters { get; set; }
    public DbSet<Applicant> Applicants { get; set; }
    public DbSet<JobApplication> JobApplications { get; set; }
    public DbSet<Education> Educations { get; set; }
}