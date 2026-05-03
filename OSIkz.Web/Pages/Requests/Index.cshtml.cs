using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OSIkz.Web.Data;
using OSIkz.Web.Models;

namespace OSIkz.Web.Pages.Requests
{
    [Authorize] 
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Request> Requests { get; set; } = new List<Request>();

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                if (user.Role == "Chairman")
                {
                    Requests = await _context.Requests
                        .Include(r => r.User) 
                        .OrderByDescending(r => r.CreatedAt)
                        .ToListAsync();
                }
                else
                {
                    Requests = await _context.Requests
                        .Where(r => r.UserId == user.Id)
                        .OrderByDescending(r => r.CreatedAt)
                        .ToListAsync();
                }
            }
        }
    }
}
