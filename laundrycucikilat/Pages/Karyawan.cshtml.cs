using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace laundrycucikilat.Pages
{
    public class KaryawanModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Check if user is logged in and has appropriate role
            var userId = HttpContext.Session.GetString("UserId");
            var userRole = HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(userId))
            {
                // Not logged in, redirect to login
                return Redirect("/Login");
            }

            if (userRole != "Admin" && userRole != "Employee")
            {
                // Not authorized, redirect to homepage
                return Redirect("/");
            }

            // User is authorized, show karyawan page
            return Page();
        }
    }
}