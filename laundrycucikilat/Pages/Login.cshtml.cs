using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace laundrycucikilat.Pages
{
    public class LoginModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Check if user is already logged in
            var userId = HttpContext.Session.GetString("UserId");
            if (!string.IsNullOrEmpty(userId))
            {
                // User already logged in, redirect to homepage
                return Redirect("/");
            }

            // User not logged in, show login page
            return Page();
        }
    }
}