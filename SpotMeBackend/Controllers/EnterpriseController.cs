using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotMeBackend.Models;

namespace SpotMeBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EnterpriseController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private IWebHostEnvironment _hostingEnvironment;

    public EnterpriseController(UserManager<ApplicationUser> userManager,IWebHostEnvironment hostingEnvironment,
        ApplicationDbContext context)
    {
        _context = context;
        _userManager = userManager;
        _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));


    }

    [HttpGet]
    [Route("SingleEnterprise")]
    public async Task<IActionResult> GetRelated(int id)
    {
        var enterprise = await _context.Enterprises.Where(x => x.EnterpriseId == id).Include(x => x.Jobs).Include(x => x.Recruiters).FirstOrDefaultAsync();
        return Ok(enterprise);
    }

    [HttpPost]
    [Route("CreateEnterprise")]
    public async Task<ActionResult<Enterprise>> CreateEnterprise([FromForm] CreateEnterpriseModel model)
    {
        
        

        var enterprise = new Enterprise()
        {
            EnterpriseName = model.EnterpriseName,
            EnterpriseDescription = model.EnterpriseDescription,
            EnterpriseLocation = model.EnterpriseLocation,
            EnterprisePhone = model.EnterprisePhone,
            EnterpriseEmail = model.EnterpriseEmail,
            EnterpriseLogo = model.EnterpriseLogo,
            LogoName = await SaveImage(model.EnterpriseLogo),
            Logo = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, await SaveImage(model.EnterpriseLogo)),
        };
       
        await _context.Enterprises.AddAsync(enterprise);
        await _context.SaveChangesAsync();

        return StatusCode(201);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEnterprise([FromForm] CreateEnterpriseModel model , int id)
    {
        var enterprise = await _context.Enterprises.Where(e => e.EnterpriseId == model.EnterpriseId).FirstOrDefaultAsync();
        if (id != enterprise.EnterpriseId)
        {
            return BadRequest();
        }

        if(enterprise.Logo != null)
        {
            DeleteImage(enterprise.LogoName);
            enterprise.LogoName =await SaveImage(model.EnterpriseLogo);
            enterprise.Logo = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase,
                await SaveImage(model.EnterpriseLogo));
        }

        _context.Entry(enterprise).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EnterpriseModelExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }
    private bool EnterpriseModelExists(int id)
    {
        return _context.Enterprises.Any(e => e.EnterpriseId == id);
    }


    [HttpGet]
    [Route("Enterprises")]
    public async Task<ActionResult<IEnumerable<Enterprise>>> GetEnterprises()
    {
        return await _context.Enterprises
            .Select(x => new Enterprise
            {
                EnterpriseId = x.EnterpriseId,
                EnterpriseName = x.EnterpriseName,
                EnterpriseDescription = x.EnterpriseDescription,
                EnterpriseLocation = x.EnterpriseLocation,
                EnterprisePhone = x.EnterprisePhone,
                EnterpriseEmail = x.EnterpriseEmail,
                LogoName = x.LogoName,
                Logo = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase,
                    x.LogoName),
                Jobs = x.Jobs,

            }).ToListAsync();
    }


[NonAction]
public void DeleteImage(string imageName)
{
    var imagePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Images", imageName);
    if (System.IO.File.Exists(imagePath))
        System.IO.File.Delete(imagePath);
}

    

    [NonAction]
    public async Task<string> SaveImage(IFormFile imageFile)
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