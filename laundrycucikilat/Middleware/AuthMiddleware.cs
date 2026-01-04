namespace laundrycucikilat.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthMiddleware> _logger;

        public AuthMiddleware(RequestDelegate next, ILogger<AuthMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();
            _logger.LogInformation($"AuthMiddleware: Processing path: {path}");

            // Public paths that don't require authentication
            var publicPaths = new[] { 
                "/", // Homepage is public
                "/login", 
                "/layanan",
                "/kontak",
                "/status",
                "/struk",
                "/pemesanan",
                "/pembayaran",
                "/api/auth/login", 
                "/api/auth/logout",
                "/api/auth/check-session",
                "/api/debug/session",
                "/api/debug/clear-session",
                "/api/order", // Public order APIs
                "/debug-session.html",
                "/simple-login.html",
                "/lib/", 
                "/css/", 
                "/js/", 
                "/images/", 
                "/favicon.ico",
                "/_framework/",
                "/background-test.html",
                "/simple-hover-test.html",
                "/animation-test.html",
                "/test-redirect.html",
                "/test-homepage.html",
                "/test-redirect-fix.html"
            };

            // Check if current path is public
            bool isPublicPath = publicPaths.Any(p => path?.StartsWith(p) == true);

            if (isPublicPath)
            {
                _logger.LogInformation($"AuthMiddleware: Public path, allowing access: {path}");
                await _next(context);
                return;
            }

            // Protected paths that require authentication
            var protectedPaths = new[] { "/admin", "/karyawan", "/home" };
            bool isProtectedPath = protectedPaths.Any(p => path?.StartsWith(p) == true);

            if (!isProtectedPath)
            {
                // Path is not explicitly protected, allow access
                _logger.LogInformation($"AuthMiddleware: Unprotected path, allowing access: {path}");
                await _next(context);
                return;
            }

            // For protected paths, check authentication
            var userId = context.Session.GetString("UserId");
            var userRole = context.Session.GetString("UserRole");

            _logger.LogInformation($"AuthMiddleware: Checking auth for protected path {path}. UserId: {userId}, UserRole: {userRole}");

            if (string.IsNullOrEmpty(userId))
            {
                // Not authenticated, redirect to login
                _logger.LogInformation($"AuthMiddleware: Not authenticated, redirecting to login from {path}");
                context.Response.Redirect("/Login");
                return;
            }

            // User is authenticated, check role-based access
            if (path?.StartsWith("/admin") == true)
            {
                if (userRole != "Admin")
                {
                    _logger.LogInformation($"AuthMiddleware: Admin access required but user role is {userRole}");
                    context.Response.Redirect("/Login");
                    return;
                }
            }
            else if (path?.StartsWith("/karyawan") == true)
            {
                if (userRole != "Admin" && userRole != "Employee")
                {
                    _logger.LogInformation($"AuthMiddleware: Employee access required but user role is {userRole}");
                    context.Response.Redirect("/Login");
                    return;
                }
            }

            _logger.LogInformation($"AuthMiddleware: Access granted for {path}");
            await _next(context);
        }
    }

    public static class AuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthMiddleware>();
        }
    }
}