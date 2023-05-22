using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gameswap_backend.Models.DTO
{
    public class MessageDTO
    {
        public int FromUserId { get; set; }
        public string? FromUsername { get; set; }
        public int ToUserId { get; set; }
        public string? ToUsername { get; set; }
        public string? Message { get; set; }
    }
}