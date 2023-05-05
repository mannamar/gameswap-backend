using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gameswap_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace gameswap_backend.Services.Context
{
    public class DataContext : DbContext
    {
        public DbSet<UserModel> UserInfo { get; set; }
        public DbSet<WishListItemModel> WishListItemInfo { get; set; }
        public DbSet<TradeItemModel> TradeItemInfo { get; set; }

        public DataContext(DbContextOptions options): base(options){}

        protected override void OnModelCreating(ModelBuilder builder){
            base.OnModelCreating(builder);
        }
    }
}