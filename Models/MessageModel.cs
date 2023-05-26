using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gameswap_backend.Models
{
    public class MessageModel
    {
        public int Id { get; set; }
        public int FromUserId { get; set; }
        public string? FromUsername { get; set; }
        public int ToUserId { get; set; }
        public string? ToUsername { get; set; }
        public string? Message { get; set; }
        public string? TimeStamp { get; set; }
        public bool isDeleted { get; set; }
        public bool isRead { get; set; }

        public MessageModel(){}
    }
}