using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SpotMeBackend.Models;
using SpotMeBackend.Services;

namespace SpotMeBackend.Controllers;


[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{

    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private IWebHostEnvironment _hostingEnvironment;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _config;
    private readonly IEmailSender _emailSender;

    public AuthenticationController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ApplicationDbContext context,
        IWebHostEnvironment hostingEnvironment,

        RoleManager<IdentityRole> roleManager,
        IConfiguration config,
        IEmailSender emailSender)
    {

        _roleManager = roleManager;
        _config = config;
        _emailSender = emailSender;
        _signInManager = signInManager;
        _hostingEnvironment = hostingEnvironment;
        _userManager = userManager;
        _context = context;

    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromForm] UserRegisterModel model)
    {


        var userToCreate = new ApplicationUser()
        {
            Email = model.Email,
            UserName = model.Username,
            SecurityStamp = Guid.NewGuid().ToString(),
            Firstname = model.Firstname,
            Lastname = model.Lastname,
            
        };

        //Create User
        var result = await _userManager.CreateAsync(userToCreate, model.Password);

        if (result.Succeeded)
        {
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            }

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(userToCreate, UserRoles.User);
            }

            var userFromDb = await _userManager.FindByNameAsync(userToCreate.UserName);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(userFromDb);

            var uriBuilder = new UriBuilder(_config["ReturnPaths:ConfirmEmail"]);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["token"] = token;
            query["userid"] = userFromDb.Id;
            uriBuilder.Query = query.ToString();
            var urlString = uriBuilder.ToString();

            var senderEmail = _config["ReturnPaths:SenderEmail"];

            await _emailSender.SendEmailAsync(senderEmail, userFromDb.Email, "Confirm your email address", urlString);

            //Add role to user
            // await _userManager.AddToRoleAsync(userFromDb, model.Role);

            var applicant = new Applicant()
            {
                User = userFromDb,
                CreatedAt = DateTime.Now,
                ProfileLogo = model.ProfileLogo,
                ProfileName = await SaveImage(model.ProfileLogo),
                Logo = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, await SaveImage(model.ProfileLogo)),
                skills = model.skills,
            };

            await _context.Applicants.AddAsync(applicant);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                token = token,
                id = userFromDb.Id,
                username = userFromDb.UserName
            });

            //return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost]
    [Route("RegisterRecruiter")]
    public async Task<IActionResult> RegisterRecruiter([FromForm] RegisterModel model, int id)
    {


        var userToCreate = new ApplicationUser()
        {
            Firstname = model.Firstname,
            Lastname = model.Lastname,
            Email = model.Email,
            UserName = model.Username,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        //Create User
        var result = await _userManager.CreateAsync(userToCreate, model.Password);

        if (result.Succeeded)
        {
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.Recruiter))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Recruiter));
            }

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(userToCreate, UserRoles.Recruiter);
            }



            var userFromDb = await _userManager.FindByNameAsync(userToCreate.UserName);





            var token = await _userManager.GenerateEmailConfirmationTokenAsync(userFromDb);

            var uriBuilder = new UriBuilder(_config["ReturnPaths:ConfirmEmail"]);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["token"] = token;
            query["userid"] = userFromDb.Id;
            uriBuilder.Query = query.ToString();
            var urlString = uriBuilder.ToString();

            var senderEmail = _config["ReturnPaths:SenderEmail"];

            await _emailSender.SendEmailAsync(senderEmail, userFromDb.Email, "Confirm your email address", urlString);

            //Add role to user
            // await _userManager.AddToRoleAsync(userFromDb, model.Role);

            var enterprise = await _context.Enterprises.FindAsync(id);

            var recruiter = new Recruiter()
            {
                ApplicationUser = userFromDb,
                Enterprise = enterprise,
                ProfileLogo = model.ProfileLogo,
                ProfileName = await SaveImage(model.ProfileLogo),
                Logo = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, await SaveImage(model.ProfileLogo)),

            };

            await _context.Recruiters.AddAsync(recruiter);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                token = token,
                id = userFromDb.Id,
                username = userFromDb.UserName
            });

            //return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet]
    [Route("GetRecruiter")]
    public async Task<ActionResult<Recruiter>> GetRecruiter(string recruiterId)
    {
        var user = await _userManager.FindByIdAsync(recruiterId);
        var recruiter = _context.Recruiters.Where(b => b.ApplicationUser == user)
            .Include(b => b.Enterprise);


        return Ok(recruiter);
    }
    
    [HttpGet]
    [Route("GetApplicant")]
    public async Task<ActionResult<Recruiter>> GetApplicant(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        var applicant = await _context.Applicants.Where(a => a.User == user)
            .Include(a => a.Jobs)
            .Include(a =>a .Educations)
            .FirstOrDefaultAsync();


        return Ok(applicant);
    }


    [HttpPost]
    [Route("RegisterAdmin")]
    public async Task<IActionResult> RegisterAdmin(RegisterModel model)
    {


        var userToCreate = new ApplicationUser()
        {
            Email = model.Email,
            UserName = model.Username,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        //Create User
        var result = await _userManager.CreateAsync(userToCreate, model.Password);

        if (result.Succeeded)
        {
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.Recruiter))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Recruiter));
            }

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(userToCreate, UserRoles.Admin);
            }

            var userFromDb = await _userManager.FindByNameAsync(userToCreate.UserName);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(userFromDb);

            var uriBuilder = new UriBuilder(_config["ReturnPaths:ConfirmEmail"]);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["token"] = token;
            query["userid"] = userFromDb.Id;
            uriBuilder.Query = query.ToString();
            var urlString = uriBuilder.ToString();

            var senderEmail = _config["ReturnPaths:SenderEmail"];

            await _emailSender.SendEmailAsync(senderEmail, userFromDb.Email, "Confirm your email address", urlString);

            //Add role to user
            // await _userManager.AddToRoleAsync(userFromDb, model.Role);

            return Ok(new
            {
                token = token,
                id = userFromDb.Id,
                username = userFromDb.UserName
            });

            //return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));

            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));

            var token = new JwtSecurityToken(

                issuer: _config["JWT:ValidIssuer"],
                audience: _config["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(5),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );


            var ctoken = new JwtSecurityTokenHandler().WriteToken(token);

            var role = userRoles.FirstOrDefault();


            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                User = user.UserName,
                role,
                user.Id,





            });

        }

        return Unauthorized();

    }


    [HttpPost]
    [Route("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailViewModel model)
    {

        var user = await _userManager.FindByIdAsync(model.UserId);

        var result = await _userManager.ConfirmEmailAsync(user, model.Token);

        if (result.Succeeded)
        {
            return Ok();
        }

        return BadRequest(error: "Something went wrong");
    }

    [HttpPost]
    [Route("Logout")]
    public async Task<IActionResult> Logout(LogoutModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        await _signInManager.SignOutAsync();
        return Ok();

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




