using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gameswap_backend.Models
{
    public class TradeItemModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? GameName { get; set; }
        public string? GamePlatform { get; set; }
        public int ReleaseYear { get; set; }
        public string? ImgUrl { get; set; }
        public int IgdbId { get; set; }
        public int WishListItemId { get; set; }
        public bool isDeleted { get; set; }
        
        public TradeItemModel(){}
    }
}