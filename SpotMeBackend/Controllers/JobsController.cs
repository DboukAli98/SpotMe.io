using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotMeBackend.Models;

namespace SpotMeBackend.Controllers;

 [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private IHostEnvironment _hostingEnvironment;
      
    
        
        private readonly IConfiguration _config;

        public JobsController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            IHostEnvironment hostingEnvironment,
           
          
            IConfiguration config)
        {
           
            _config = config;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            _context = context;
            
        }
        // GET: api/Jobs
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var jobs = await _context.Jobs.Include(e => e.Applicants).Include(e => e.Enterprise).ToListAsync();
            return Ok(jobs);
        }

        

        [HttpGet]
        [Route("JobStats")]
        public async Task<IActionResult> JobsStats(int jobId)
        {
            var stats =  _context.Jobs.Where(e => e.JobId == jobId).Include(e => e.Applicants);
            var apps = stats.SelectMany(e => e.Applicants).Count();
            
            return Ok(apps);
        }

        // GET: api/Jobs/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult> Get(int id)
        {
            var job =  _context.Jobs.Where(x => x.JobId == id)
                .Include(e => e.Enterprise)
                .Include(e => e.Applicants).ThenInclude(e => e.User);
            return Ok(job);
        }
        

        
        

        [HttpPost]
        [Route("CreateJobPost")]
        public async Task<IActionResult> CreateJob(CreateJobModel model , string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            Console.WriteLine(user.Email);
            var recruiter = await  _context.Recruiters.Where(x => x.ApplicationUser.Id == user.Id).FirstOrDefaultAsync();
            var enterprise = await _context.Recruiters.Where(r => r.RecruiterId == recruiter.RecruiterId).Select(r => r.Enterprise).FirstOrDefaultAsync();
            
            Console.WriteLine("Hello" + enterprise);
            var eFromDb = await _context.Enterprises.FindAsync(enterprise.EnterpriseId);
            

            var jobsDv = new Job()
            {
                JobTitle = model.JobTitle,
                JobDescription = model.JobDescription,
                JobDomain = model.JobDomain,
                JobLocation = model.JobLocation,
                Requirements = model.Requirements,
                EmploymentType = model.EmploymentType,
                Recruiter = recruiter,
                Enterprise = eFromDb,
            };


            await _context.Jobs.AddAsync(jobsDv);
              await _context.SaveChangesAsync();
            return Ok(new
            {
                Jobtitle = model.JobTitle,
                EnterpriseName = enterprise.EnterpriseName,
            });


        }

        [HttpGet]
        [Route("GetRecruitersJobs")]
        public async Task<IActionResult> GetRecruitersJobs(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var recruiter = await _context.Recruiters.Where(r => r.ApplicationUser == user).FirstOrDefaultAsync();
            var jobs = await _context.Jobs.Where(j => j.Recruiter == recruiter)
                .Include(j => j.Applicants)
                .ThenInclude(a => a.User)
                .Include( j => j.Enterprise)
                .ToListAsync();
            
            return Ok(jobs);
        }

        [HttpPost]
        [Route("ApplyToJob")]
        public async Task<IActionResult> ApplyToJob(string userId , int jobId)
        {
            var user = await _userManager.FindByIdAsync(userId);


            var job = await _context.Jobs.FindAsync(jobId);
            await _context.Jobs.AddAsync(job);
            _context.Jobs.Attach(job);
            var applicant = await  _context.Applicants.Where(x => x.User.Id == user.Id).FirstOrDefaultAsync();
            await _context.Applicants.AddAsync(applicant);
            _context.Applicants.Attach(applicant);

            job.Applicants = new List<Applicant>();
            job.Applicants.Add(applicant);
            
            Console.WriteLine("applicant " + applicant.User.Email);
            
            await _context.SaveChangesAsync();
            
            return Ok();

        }



        // PUT: api/Jobs/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Jobs/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }