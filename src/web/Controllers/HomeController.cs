using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using Mitheti.Web.Models;

namespace Mitheti.Web.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {   }

        public IActionResult Index() =>  View();

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
