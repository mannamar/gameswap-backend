using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gameswap_backend.Models;
using gameswap_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace gameswap_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly MatchService _data;
        public MatchController(MatchService data)
        {
            _data = data;
        }

        [HttpGet]
        [Route("GetMatches/{UserId}")]
        public dynamic GetMatches(int userId){
            return _data.GetMatches(userId);
        }
    }
}