using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace XmlToCSV.Controllers
{
    public class ParseController : Controller
    {
        public IActionResult ParseSuccess()
        {
            ViewData["Success"] = "File parsed successfully";
            return View("Parse");
        }
        public IActionResult ParseFail()
        {
            ViewData["Success"] = "File parsing failed";
            return View("Parse");
        }
    }
}