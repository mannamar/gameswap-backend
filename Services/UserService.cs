using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gameswap_backend.Models.DTO;
using gameswap_backend.Services.Context;

namespace gameswap_backend.Services
{
    public class UserService
    {
        private readonly DataContext _context;
        public UserService(DataContext context){
            _context = context;
        }
        
        public bool DoesUserExist(string Username){
            return _context.UserInfo.SingleOrDefault(user => user.Username == Username) != null;
        }

        public bool AddUser(CreateAccountDTO UserToAdd){
            
        }
    }
}