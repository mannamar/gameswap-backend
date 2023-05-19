using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gameswap_backend.Models;
using gameswap_backend.Services.Context;

namespace gameswap_backend.Services
{
    public class TradeItemService
    {
        private readonly DataContext _context;
        public TradeItemService(DataContext context){
            _context = context;
        }

        //function that creates a new wish list item and saves to the database
        public bool AddTradeItem(TradeItemModel NewWishListItem){
            TradeItemModel newItem = new TradeItemModel();
            newItem.Id = 0;
            newItem.UserId = NewWishListItem.UserId;
            newItem.GameName = NewWishListItem.GameName;
            newItem.GamePlatform = NewWishListItem.GamePlatform;
            newItem.ReleaseYear = NewWishListItem.ReleaseYear;
            newItem.ImgUrl = NewWishListItem.ImgUrl;
            newItem.IgdbId = NewWishListItem.IgdbId;
            newItem.WishListItemId = NewWishListItem.WishListItemId;
            newItem.isDeleted = false;
            _context.Add(newItem);
            return _context.SaveChanges() != 0;
        }

        public IEnumerable<TradeItemModel> GetTradeItemsByWishId(int wishId){
            return _context.TradeItemInfo.Where(item => (item.WishListItemId == wishId && item.isDeleted == false));
        }
    }
}