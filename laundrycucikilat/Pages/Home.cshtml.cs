using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace laundrycucikilat.Pages;

public class HomeModel : PageModel
{
    private readonly ILogger<HomeModel> _logger;

    public HomeModel(ILogger<HomeModel> logger)
    {
        _logger = logger;
    }

    public IActionResult OnGet()
    {
        // Check if user is logged in
        var userId = HttpContext.Session.GetString("UserId");
        
        if (string.IsNullOrEmpty(userId))
        {
            // Not logged in, redirect to login
            return Redirect("/Login");
        }

        // User is logged in, show home page
        return Page();
    }
}