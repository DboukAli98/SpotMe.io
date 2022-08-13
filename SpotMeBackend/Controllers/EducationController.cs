using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotMeBackend.Models;

namespace SpotMeBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EducationController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public EducationController(ApplicationDbContext context , UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;

    }

    [HttpPost]
    [Route("AddEducation")]
    public async Task<IActionResult> AddEducation([FromForm] CreateEducationModel model ,string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        var applicant = await _context.Applicants.Where(a => a.User == user).FirstOrDefaultAsync();
        var education = new Education()
        {
            EducationId = model.EducationId,
            InstitutionName = model.InstitutionName,
            StartDate =  model.StartDate,
            EndDate = model.EndDate,
            Major = model.Major,
            Domain = model.Domain,
            GPA = model.GPA,
            Applicant = applicant
        };

        await _context.Educations.AddAsync(education);
        await _context.SaveChangesAsync();
        return Ok(education);
    }
}