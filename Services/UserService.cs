using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using gameswap_backend.Models;
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
        
        //function that checks the database table to see if the username already exists. If 1 item matches the condition, we wil return that item. If no item matches, we will return 'null'. If mutiple items match, an error will occur and our table is likely busted
        public bool DoesUserExist(string? Username){
            return _context.UserInfo.SingleOrDefault(user => user.Username == Username) != null;
        }

        //function that creates a new user account
        public bool AddUser(CreateAccountDTO UserToAdd){
            //check to see if the user exists or not. If they don't, then proceed to account creation
            bool result = false;
            if (!DoesUserExist(UserToAdd.Username)) {
                //creating a new, empty instance of a user model for saving the user's account information
                UserModel newUser = new UserModel();
                //create our salt and hash password by calling our HashPassword helper function
                var hashPassword = HashPassword(UserToAdd.Password);
                //saving inputted user info
                newUser.Name = UserToAdd.Name;
                newUser.Username = UserToAdd.Username;
                newUser.Email = UserToAdd.Email;
                newUser.Birthday = UserToAdd.Birthday;
                newUser.Zipcode = UserToAdd.Zipcode;
                //saving salt & hashed password
                newUser.Salt = hashPassword.Salt;
                newUser.Hash = hashPassword.Hash;
                //assigning a user ID# in the backend to avoid front end errors
                newUser.Id = 0;
                //assigning an account creation time in the backend to avoid front end errors
                newUser.CreationTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //assigning 'isDeleted' to false by default on account creation
                newUser.isDeleted = false;

                //adding newUser to our databse
                _context.Add(newUser);
                //this saves to our database and returns the number of entires that was written to the database. If the returned number is not 0, then it evaluates as true; else false.
                result = _context.SaveChanges() != 0;
            }
            //will return true if new user is added; else will return false
            return result;
        }

        //Helper function to create hashed password
        public PasswordDTO HashPassword(string? password){
            //creating a new, empty instance of the Password DTO
            PasswordDTO newHashedPassword = new PasswordDTO();
            //new, empty byte array with a length of 64 for storing salted password
            byte[] SaltByte = new byte[64];
            //generating random numbers for salting so that no two passwords end up encrypted the same
            var provider = new RNGCryptoServiceProvider();
            //using an enhanced RNG without using zero
            provider.GetNonZeroBytes(SaltByte);
            //encoding the 64 digits from the byte array into a string
            var Salt = Convert.ToBase64String(SaltByte);
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, SaltByte, 10000);
            //encoding our password with salt
            var Hash = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));
            //adding the salt and hash to our empty object Password DTO so it can be returned out of the function
            newHashedPassword.Salt = Salt;
            newHashedPassword.Hash = Hash;
            return newHashedPassword;
        }

        //Helper function that verifies user's password by taking the user's input, and laundering it through our salt & hash process, then comparing it with the hashed password stored in our database
        public bool VerifyUserPassword(string? password, string? storedHash, string? storedSalt){
            //get our existing salt and change it to base 64 string for crossreferencing
            var SaltBytes = Convert.FromBase64String(storedSalt);
            //making the password that the user inputed and using the stored salt
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, SaltBytes, 10000);
            //creating the new hash
            var newHash = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));
            //comparing the new hash to see if it matches the old hash to confirm that the password is the same
            return newHash == storedHash;
        }
    }
}