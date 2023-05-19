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

    public class WishListController : ControllerBase
    {
        private readonly WishListService _data;
        public WishListController(WishListService data){
            _data = data;
        }

        //ENDPOINTS BEGIN:
        
        //Endpoint for adding a wish list item
        [HttpPost]
        [Route("AddWishListItem")]
        public dynamic AddWishListItem(WishListItemModel NewWishListItem){
            return _data.AddWishListItem(NewWishListItem);
        }

        //Endpoint for getting all wishlist items by user Id
        [HttpGet]
        [Route("GetWishListItemsByUserId/{UserId}")]
        public IEnumerable<WishListItemModel> GetWishListItemsByUserId(int userId){
            return _data.GetWishListItemsByUserId(userId);
        }

        [HttpPost]
        [Route("DeleteWishListItem/{ItemId}")]
        public bool DeleteWishListItem(int ItemId){
            return _data.DeleteWishListItem(ItemId);
        }
    }
}