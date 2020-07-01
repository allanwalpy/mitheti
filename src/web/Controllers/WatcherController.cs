using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Mitheti.Web.Models;
using Mitheti.Web.Service;

namespace Mitheti.Web.Controllers
{
    public class WatcherController : Controller
    {
        private ILauncherService _worker { get; }
        public WatcherController(ILauncherService worker)
        {
            _worker = worker;
        }

        public IActionResult Index()
        {
            return View(new WorkerModel()
            {
                Status = _worker.State
            });
        }

        public IActionResult Run()
        {
            //TODO: add events for run and stop;
            //TODO: or pass task of action;
            _worker.StartAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Stop()
        {
            _worker.StopAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
