using Microsoft.AspNetCore.Mvc;

namespace Movies.Client.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
