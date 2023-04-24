using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gameswap_backend.Models;
using gameswap_backend.Models.DTO;
using gameswap_backend.Services.Context;
using System.Security.Cryptography;

namespace gameswap_backend.Services
{
    public class UserService
    {   
        private readonly DataContext _context;
        public UserService(DataContext context)
        {
            _context = context;
        }
        public bool DoesUserExist(string? Username)
        {
            // Check the table to see if the username exists
                // If one item matches the condition return the item
                // If no item matches the condition return null
                // If multipel items match an error will occur
            return _context.UserInfo.SingleOrDefault(user => user.Username == Username) != null;
        }

        // We'll likely need to check if email exists as well
        public bool DoesEmailExist(string Email)
        {
            return _context.UserInfo.SingleOrDefault(user => user.Email == Email) != null;
        }
        public bool AddUser(CreateAccountDTO UserToAdd)
        {
            // Check if the user exists
            // If the user does not exist
                // Create the account
            bool result = false;
            if (!DoesUserExist(UserToAdd.Username))
            {
                UserModel newUser = new UserModel();
                // Create salt and hash password
                var hashPassword = HashPassword(UserToAdd.Password);
                newUser.Id = UserToAdd.Id;
                newUser.Username = UserToAdd.Username;
                newUser.Salt = hashPassword.Salt;
                newUser.Hash = hashPassword.Hash;
                // Add newUser to our database
                _context.Add(newUser);
                // This saves to our databse and returns the numbers of entries that was written
                result = _context.SaveChanges() != 0;
            }
            return result;
        }

        public PasswordDTO HashPassword(string? password)
        {
            PasswordDTO newHashedPassword = new PasswordDTO();
            byte[] SaltByte = new byte[64];
            var provider = new RNGCryptoServiceProvider();
            provider.GetNonZeroBytes(SaltByte);
            // Encoding to 64 digit string
            // This salt will make the hash unique to the user
            var Salt = Convert.ToBase64String(SaltByte);

            // Takes password and salt and interates 10000x to create a hash
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, SaltByte, 10000);

            // Encoding our password with our Salt
            var Hash = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));

            newHashedPassword.Salt = Salt;
            newHashedPassword.Hash = Hash;

            return newHashedPassword;
        }

        public bool VerifyUserPassword(string? Password, string? storedHash, string? storedSalt)
        {
            // Get our existing salt and change it to a base 64 string
            var SaltBytes = Convert.FromBase64String(storedSalt);
            // Making the password that the user inputted and using the stored salt
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(Password, SaltBytes, 10000);
            // Recreate hash based on entered password and stored salt
            var newHash = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));
            // Compare if the new hash matches the stored hash
            return newHash == storedHash;
        }
    }
}