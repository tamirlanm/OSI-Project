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

        public CreateModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, AIAssistantService aiAssistant)
        {
            _context = context;
            _userManager = userManager;
            _aiAssistant = aiAssistant;
        }

        [BindProperty]
        public Request NewRequest { get; set; } = new Request();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            NewRequest.UserId = user.Id;

            NewRequest.Priority = _aiAssistant.CategorizeRequest(NewRequest.Title, NewRequest.Description);
            
            _context.Requests.Add(NewRequest);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
