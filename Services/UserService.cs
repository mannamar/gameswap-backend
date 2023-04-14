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
        public UserService(DataContext context)
        {
            _context = context;
        }
        public bool DoesUserExist(string Username)
        {
            // Check the table to see if the username exists
                // If one item matches the condition return the item
                // If no item matches the condition return null
                // If multipel items match an error will occur
            return _context.UserInfo.SingleOrDefault(user => user.Username == Username) != null;
        }

        public bool DoesEmailExist(string Email)
        {
            return _context.UserInfo.SingleOrDefault(user => user.Email == Email) != null;
        }
        public bool AddUser(CreateAccountDTO UserToAdd)
        {
            // Check if the user exists
            // If the user does not exist
                // Create the account
            if (!DoesUserExist(UserToAdd.Username))
        }
    }
}