using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gameswap_backend.Models;
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
            return _context.WishListItemInfo.Where(item => (item.UserId == userId && item.isDeleted == false));
        }
    }
}