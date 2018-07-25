using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace XmlToCSV.Controllers
{
    public class UploadController : Controller
    {
        public IActionResult UploadSuccess()
        {
            ViewData["Success"] = "File uploaded successfully";
            return View("Upload");
        }
        public IActionResult UploadFail()
        {
            ViewData["Success"] = "File upload failed";
            return View("Upload");
        }
    }
}