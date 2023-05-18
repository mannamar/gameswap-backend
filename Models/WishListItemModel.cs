using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gameswap_backend.Models
{
    public class WishListItemModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? GameName { get; set; }
        public string? GamePlatform { get; set; }
        public int ReleaseYear { get; set; }
        public string? CoverUrl { get; set; }
        public int IgdbId { get; set; }
        public bool isComplete { get; set; }
        public bool isDeleted { get; set; }
        public string? BannerUrl { get; set; }
        public string? AllPlatforms { get; set; } 

        public WishListItemModel(){}
    }
}