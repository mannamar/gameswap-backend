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
            newItem.Id = 0;
            newItem.UserId = NewWishListItem.UserId;
            newItem.GameName = NewWishListItem.GameName;
            newItem.GamePlatform = NewWishListItem.GamePlatform;
            newItem.ReleaseYear = NewWishListItem.ReleaseYear;
            newItem.CoverUrl = NewWishListItem.CoverUrl;
            newItem.IgdbId = NewWishListItem.IgdbId;
            newItem.isComplete = false;
            newItem.isDeleted = false;
            newItem.BannerUrl = NewWishListItem.BannerUrl;
            newItem.AllPlatforms = NewWishListItem.AllPlatforms;
            _context.Add(newItem);
            return _context.SaveChanges() != 0;
        }

        //function for getting all wish list items by user Id
        public IEnumerable<WishListItemModel> GetWishListItemsByUserId(int userId){
            return _context.WishListItemInfo.Where(item => (item.UserId == userId && item.isDeleted == false));
        }

        public bool DeleteWishListItem(int Id) {
            bool result = false;
            var wishItem = _context.WishListItemInfo.SingleOrDefault(item => item.Id == Id);
            if (wishItem != null) {
                wishItem.isDeleted = true;
                _context.Update<WishListItemModel>(wishItem);
                result = _context.SaveChanges() != 0;
            }
            return result;
        }
    }
}