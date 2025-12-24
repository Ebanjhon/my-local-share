using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace local_share.APIs
{
    [ApiController]
    [Route("api/localshare")]
    public class LocalShareController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello API from WPF Desktop");
        }
    }
}
