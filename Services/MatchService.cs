using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gameswap_backend.Models;
using gameswap_backend.Models.DTO;
using gameswap_backend.Services.Context;

namespace gameswap_backend.Services
{
    public class MatchService
    {
        
        private readonly DataContext _context;
        public MatchService(DataContext context){
            _context = context;
        }

        public dynamic GetMatches(int userId){
            // Get user's active WishList
            var userWishItems = _context.WishListItemInfo.Where(item => (item.UserId == userId && item.isDeleted == false)).ToList();
            
            dynamic result = "";
            List<string> myList = new List<string>();
            List<MatchDTO> matchList = new List<MatchDTO>();

            // For each item in the user's wishlist
            foreach (var wishItem in userWishItems)
            {
                // Pull out a list of what that user would trade
                var trades = _context.TradeItemInfo.Where(item => (item.WishListItemId == wishItem.Id && item.isDeleted == false)).ToList();
                // For each item they would trade
                foreach (var tradeItem in trades)
                {
                    // Find who wants those items
                    var wishMatches = _context.WishListItemInfo.Where(item => (item.IgdbId == tradeItem.IgdbId && item.isDeleted == false)).ToList();
                    foreach(var wishMatch in wishMatches)
                    {
                        // Pull out their trades
                        var tradeMatches = _context.TradeItemInfo.Where(item => (item.WishListItemId == wishMatch.Id && item.isDeleted == false && item.IgdbId == wishItem.IgdbId)).ToList();
                        // Loop over their trades and see if they have the game you want
                        foreach(var wishTrade in tradeMatches)
                        {
                            string match = $"Trade with: {wishMatch.UserId}, You give: {tradeItem.GameName}, You get: {wishTrade.GameName};";
                            myList.Add(match);

                            var tradeUser = _context.UserInfo.SingleOrDefault(user => user.Id == wishMatch.UserId);
                            string tradeUsername = tradeUser.Username;
                            MatchDTO newMatch = new MatchDTO();
                            newMatch.TradeWithUserId = wishMatch.UserId;
                            newMatch.TradeWithUsername = tradeUsername;
                            newMatch.GiveGameName = tradeItem.GameName;
                            newMatch.GivePlatform = tradeItem.GamePlatform;
                            newMatch.GiveCoverImg = tradeItem.ImgUrl;
                            newMatch.ReceiveGameName = wishTrade.GameName;
                            newMatch.ReceivePlatform = wishTrade.GamePlatform;
                            newMatch.ReceiveCoverImg = wishTrade.ImgUrl;
                            matchList.Add(newMatch);
                        }
                        // result = tradeMatches;
                    }
                    // string res = "Trade Id " + tradeItem.Id + ": " + wishItem.GameName + " - " + tradeItem.GameName;
                    // myList.Add(res);
                }
            }
            return matchList;
        }
    }
}