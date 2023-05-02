using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gameswap_backend.Models;
using gameswap_backend.Services.Context;

namespace gameswap_backend.Services
{
    public class WishListService
    {
        private readonly DataContext _context;
        public WishListService(DataContext context){
            _context = context;
        }

        //function that creates a new wish list item and saves to the database
        public bool AddWishListItem(WishListItemModel NewWishListItem){
            WishListItemModel newItem = new WishListItemModel();
            newItem.ItemId = 0;
            newItem.UserId = NewWishListItem.UserId;
            newItem.GameName = NewWishListItem.GameName;
            newItem.GamePlatform = NewWishListItem.GamePlatform;
            newItem.ReleaseYear = NewWishListItem.ReleaseYear;
            newItem.ImgUrl = NewWishListItem.ImgUrl;
            newItem.IgdbId = NewWishListItem.IgdbId;
            newItem.TradeOptions = NewWishListItem.TradeOptions;
            newItem.isComplete = false;
            newItem.isDeleted = false;
            _context.Add(newItem);
            return _context.SaveChanges() != 0;
        }

        //function for getting all wish list items by user Id
        public IEnumerable<WishListItemModel> GetWishListItemsByUserId(int userId){
            return _context.WishListItemInfo.Where(item => item.UserId == userId);
        }
    }
}