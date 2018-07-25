using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using XmlToCSV.Models;
using XmlToCSV.Services;

namespace XmlToCSV.Controllers
{
    public class ViewFileController : Controller
    {
        private readonly IFileProvider _fileProvider;
        private readonly IParserService _parserService;
        public ViewFileController(IFileProvider fileProvider, IParserService parserService)
        {
            _fileProvider = fileProvider;
            _parserService = parserService;

        }

        public async Task<IActionResult> ViewFile()
        {
            var result = await _parserService.GetFiles();
            return View(result);
        }
    }
}