using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gameswap_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace gameswap_backend.Controllers
{
    [ApiController]
    [Route("Controller")]

    public class WishListController : ControllerBase
    {
        private readonly WishListService _data;
        public WishListController(WishListService data){
            _data = data;
        }

        //endpoints begin
    }
}