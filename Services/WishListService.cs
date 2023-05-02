using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gameswap_backend.Services.Context;
using Microsoft.AspNetCore.Mvc;

namespace gameswap_backend.Services
{
    public class WishListService : ControllerBase
    {
        private readonly DataContext _context;
        public WishListService(DataContext context){
            _context = context;
        }
    }
}