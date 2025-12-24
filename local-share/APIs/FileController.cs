using local_share.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace local_share.APIs
{
    [ApiController]
    [Route("api/localshare")]
    public class FileController : ControllerBase
    {
        private static List<SharedFile> _files = new();

        [HttpGet("{id}")]
        public IActionResult Download(string id)
        {
            var file = _files.FirstOrDefault(f => f.Id == id);
            if (file == null || !System.IO.File.Exists(file.FullPath))
                return NotFound();

            var bytes = System.IO.File.ReadAllBytes(file.FullPath);
            return File(bytes, "application/octet-stream", file.FileName);
        }
    }
}
