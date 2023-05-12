using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gameswap_backend.Models.DTO
{
    public class UserInfoDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Birthday { get; set; }
        public int Zipcode { get; set; }
        public string? CreationTime { get; set; }
    }
}