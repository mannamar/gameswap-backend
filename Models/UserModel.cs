using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gameswap_backend.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Birthday { get; set; }
        public int Zipcode { get; set; }
        public string? Salt { get; set; }
        public string? Hash { get; set; }
        public string? CreationTime { get; set; }
        public bool isDeleted { get; set; }
        public UserModel(){}
    }
} 