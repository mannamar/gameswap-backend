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
    public class TradeController : ControllerBase
    {
        private readonly TradeItemService _data;
        public TradeController(TradeItemService data)
        {
            _data = data;
        }

        [HttpPost]
        [Route("AddTradeItem")]
        public bool AddTradeItem(TradeItemModel newTradeItem){
            return _data.AddTradeItem(newTradeItem);
        }

        [HttpGet]
        [Route("GetTradeItemsByWishId/{WishId}")]
        public IEnumerable<TradeItemModel> GetTradeItemsByWishId(int wishId){
            return _data.GetTradeItemsByWishId(wishId);
        }
    }
}