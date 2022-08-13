using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotMeBackend.Models;

namespace SpotMeBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobApplicationController : Controller
{
    
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private IWebHostEnvironment _hostingEnvironment;

    public JobApplicationController(ApplicationDbContext context , UserManager<ApplicationUser> userManager , IWebHostEnvironment hostingEnvironment)
    {
        _context = context;
        _userManager = userManager;
        _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));

    }
    
    [HttpPost]
    [Route("CreateEnterprise")]
    public async Task<ActionResult<JobApplication>> CreateEnterprise([FromForm] CreateApplyModel model , string appId , int JobId)
    {
        var applicant =  await _context.Applicants.Where(a => a.User.Id == appId).FirstOrDefaultAsync();
        var job = await _context.Jobs.Where(j => j.JobId == JobId).FirstOrDefaultAsync();
        var fileName = await SaveFile(model.pdfFile);

        var applyJob = new JobApplication()
        {
            Applicant = applicant,
            Job = job,
            fileName = await SaveFile(model.pdfFile),
            cvFile = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase,
                fileName),
        };
        
        
       
        await _context.JobApplications.AddAsync(applyJob);
        await _context.Jobs.AddAsync(job);
        _context.Jobs.Attach(job);
        await _context.Applicants.AddAsync(applicant);
        _context.Applicants.Attach(applicant);

        job.Applicants = new List<Applicant>();
        job.Applicants.Add(applicant);
        await _context.SaveChangesAsync();

        return StatusCode(201);



    }

    [HttpGet]
    [Route("GetJobApp")]
    public async Task<IActionResult> GetJobApp(string appId)

    {
        var appilcant = await _userManager.FindByIdAsync(appId);
        var application =   _context.JobApplications.Where(e => e.Applicant.User.Id == appilcant.Id).Select(e =>e.Job);
        return Ok(application);
    }
    [HttpGet]
    [Route("RecruitersJobsApps")]
    public async Task<IActionResult> GetRecruiterApp(string recruitId)

    {
        var user = await _userManager.FindByIdAsync(recruitId);
        var recruiter = await _context.Recruiters.Where(r => r.ApplicationUser == user).FirstOrDefaultAsync();
        var application =   _context.JobApplications.Where(j => j.Job.Recruiter == recruiter).Include(j => j.Applicant);
        return Ok(application);
    }


    [NonAction]
    public async Task<string> SaveFile(IFormFile imageFile)
    {
        string imageName =new string(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ','-');
        imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
        var imagePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Images", imageName);
        using (var fileStream = new FileStream(imagePath,FileMode.Create))
        {
            await imageFile.CopyToAsync(fileStream);

        }

        return imageName;

    }
    
    
    
}