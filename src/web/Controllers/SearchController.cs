using Microsoft.AspNetCore.Mvc;

using Mitheti.Web.Services;
using Mitheti.Web.Models;

namespace Mitheti.Web.Controllers
{
    public class SearchController : Controller
    {
        private ISearchService _service;

        public SearchController(ISearchService service)
        {
            _service = service;
        }

        public IActionResult Index() => RedirectToAction("Request");

        [ActionName("Request")]
        public IActionResult RequestAction() => View();

        [ActionName("Request")]
        [HttpPost]
        public IActionResult ResultAction([Bind] SearchFilter request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            return View("Result", _service.Get(request));
        }

        public IActionResult Default()
        {
            return View("Result", _service.Get(new SearchFilter()
            {
                Hour = new FilterEqual<int>() { IsSet = false, Value = 0 }
            }));
        }
    }
}
