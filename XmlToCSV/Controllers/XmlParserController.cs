using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using XmlToCSV.Services;

namespace XmlToCSV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class XmlParserController : ControllerBase
    {
        private readonly IFileProvider _fileProvider;
        private readonly IParserService _parserService;
        public XmlParserController(IFileProvider fileProvider, IParserService parserService)
        {
            _fileProvider = fileProvider;
            _parserService = parserService;
        }
        [HttpGet]
        public async Task<IActionResult> Parse(string name)
        {

            var result = await _parserService.Parse(name);
            if (result == true)
            {
                return RedirectToAction("ParseSuccess", "parse");
            }
            else
            {
                return RedirectToAction("ParseFail", "parse");
            }

        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var result = await _parserService.UploadFile(file);
            if (result == true)
            {
                return RedirectToAction("UploadSuccess", "Upload");
            }
            else
            {
                return RedirectToAction("UploadFail", "Upload");
            }
        }

    }
}