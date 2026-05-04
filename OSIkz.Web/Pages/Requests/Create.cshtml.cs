using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OSIkz.Web.Data;
using OSIkz.Web.Models;
using OSIkz.Web.Services; 

namespace OSIkz.Web.Pages.Requests
{
    [Authorize] 
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AIAssistantService _aiAssistant;
        private readonly IWebHostEnvironment _env;

        public CreateModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
         AIAssistantService aiAssistant, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _aiAssistant = aiAssistant;
            _env = env;
        }

        [BindProperty]
        public Request NewRequest { get; set; } = new Request();

        [BindProperty]
        public IFormFile? UploadPhoto {get;set;}

        public void OnGet(){}

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            if(UploadPhoto != null)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);
            
            
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + UploadPhoto.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            
            using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await UploadPhoto.CopyToAsync(fileStream);
                }

                NewRequest.PhotoPath = "/uploads/" + uniqueFileName;
            
            }
            
            NewRequest.UserId = user.Id;

            NewRequest.Priority = _aiAssistant.CategorizeRequest(NewRequest.Title, NewRequest.Description);
            
            _context.Requests.Add(NewRequest);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
