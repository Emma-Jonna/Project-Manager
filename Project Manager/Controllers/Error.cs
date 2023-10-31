using Microsoft.AspNetCore.Mvc;
using Project_Manager.Models;
using System.Diagnostics;

namespace Project_Manager.Controllers
{
    public class Error : Controller
    {
        /*public IActionResult Index()
        {
            return View();
        }*/

        public IActionResult Errors(int? statusCode = null)
        {
            if (statusCode.HasValue)
            {
                //Log.Error($"Error statusCode: {statusCode}");
                if (statusCode == 404)
                {
                    return View("PageNotFound");
                }
                if (statusCode == 405)
                {
                    return View("AccessDenied");
                }
            }

            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
