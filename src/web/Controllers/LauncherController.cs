using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Mitheti.Web.Models;
using Mitheti.Web.Services;

namespace Mitheti.Web.Controllers
{
    public class LauncherController : Controller
    {
        private ILauncherService _launcher { get; }
        public LauncherController(ILauncherService launcher)
        {
            _launcher = launcher;
        }

        public IActionResult Index()
        {
            return View(new LauncherModel()
            {
                Status = _launcher.State
            });
        }

        public IActionResult Run()
        {
            //TODO: add events for run and stop;
            //TODO: or pass task of action;
            _launcher.StartAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Stop()
        {
            _launcher.StopAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
