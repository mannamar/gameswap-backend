using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gameswap_backend.Models.DTO
{
    public class MatchDTO
    {
        public int TradeWithUserId { get; set; }
        public string? TradeWithUsername { get; set; }
        public string? GiveGameName { get; set; }
        public string? GivePlatform { get; set; }
        public string? GiveCoverImg { get; set; }
        public string? ReceiveGameName { get; set; }
        public string? ReceivePlatform { get; set; }
        public string? ReceiveCoverImg { get; set; }
    }
}