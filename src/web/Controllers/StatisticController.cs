using Microsoft.AspNetCore.Mvc;

using Mitheti.Web.Services;
using Mitheti.Web.Models;

namespace Mitheti.Web.Controllers
{
    public class StatisticController : Controller
    {
        private IStatisticService _service;
        public StatisticController(IStatisticService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View(new StatisticModel()
                {
                    Result = _service.GetAppTimes()
                });
        }
    }
}
