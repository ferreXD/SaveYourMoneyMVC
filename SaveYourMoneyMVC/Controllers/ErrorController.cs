using Microsoft.AspNetCore.Mvc;
using SaveYourMoneyMVC.Models.Error;

namespace SaveYourMoneyMVC.Controllers
{
    public class ErrorController : Controller
    {

        public IActionResult Index(ErrorViewModel errorViewModel)
        {
            return View(errorViewModel);
        }
    }
}
