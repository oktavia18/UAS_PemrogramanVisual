using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace laundrycucikilat.Pages
{
    public class AdminModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Check if user is logged in and has admin role
            var userId = HttpContext.Session.GetString("UserId");
            var userRole = HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(userId))
            {
                // Not logged in, redirect to login
                return Redirect("/Login");
            }

            if (userRole != "Admin")
            {
                // Not admin, redirect to homepage
                return Redirect("/");
            }

            // User is admin, show admin page
            return Page();
        }
    }
}