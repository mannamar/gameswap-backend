using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using gameswap_backend.Models;
using gameswap_backend.Models.DTO;
using gameswap_backend.Services.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace gameswap_backend.Services
{
    public class UserService : ControllerBase
    {
        private readonly DataContext _context;
        public UserService(DataContext context){
            _context = context;
        }
        
        //function that checks the database table to see if the username already exists. If 1 item matches the condition, we wil return that item. If no item matches, we will return 'null'. If mutiple items match, an error will occur and our table is likely busted
        public bool DoesUserExist(string? Username){
            return _context.UserInfo.SingleOrDefault(user => user.Username == Username && !user.isDeleted) != null;
        }

        public bool DoesEmailExist(string? Email){
            return _context.UserInfo.SingleOrDefault(user => user.Email == Email && !user.isDeleted) != null;
        }

        //function that creates a new user account
        public string AddUser(CreateAccountDTO UserToAdd){
            //check to see if the username or email exists or not. If they don't, then proceed to account creation. Else, return an error msg
            string result = "Success";
            if (DoesUserExist(UserToAdd.Username) && DoesEmailExist(UserToAdd.Email)) {
                result = "Username & Email already taken";
            } else if (DoesEmailExist(UserToAdd.Email)){
                result = "Email already taken";
            } else if (DoesUserExist(UserToAdd.Username)){
                result = "Username already taken";
            } else {
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
                _context.SaveChanges();
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

        //Function that logs user into website via JWT - Json Web Token - that securely sends data for authentication purposes via encrypted json token
        public IActionResult Login(LoginDTO User){
            //Returns an error if an incorrect username or password is inputted
            IActionResult Result = Unauthorized();
            
            //Check to see if user exists
            if(DoesUserExist(User.Username)){
                //If user exists, store in foundUser object
                UserModel foundUser = GetUserByUserName(User.Username);
                //check if password is correct
                if(VerifyUserPassword(User.Password, foundUser.Hash, foundUser.Salt)){
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    var tokeOptions = new JwtSecurityToken(
                        issuer: "http://localhost:5000",
                        audience: "http://localhost:5000",
                        claims: new List<Claim>(),
                        expires: DateTime.Now.AddDays(30),
                        signingCredentials: signinCredentials
                    );
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                    Result = Ok(new { Token = tokenString });
                }
            }
            return Result;
        }

        //Helper function to find user in database by username
        public UserModel GetUserByUserName(string? username){
            return _context.UserInfo.SingleOrDefault(user => user.Username == username);
        }

        //Function that updates a user's information stored in the database - here for reference, not going to use
        // public bool UpdateUser(UserModel userToUpdate){
        //     _context.Update<UserModel>(userToUpdate);
        //     return _context.SaveChanges() != 0;
        // }

        //Function to update specific information in each usermodel, specifically just the username. May expand later to update more.
        public bool UpdateUsername(int id, string username){
            //This one is sending over just the id and the username. So we have to get the object to then be updated
            UserModel foundUser = GetUserById(id);
            bool result = false;
            //check to see if the user exists; if it does - proceed with rewriting the username
            if(foundUser != null){
                if (!DoesUserExist(username)){
                    foundUser.Username = username;
                    _context.Update<UserModel>(foundUser);
                    result = _context.SaveChanges() != 0;
                }
            }
            return result;
        }

        //Helper function to get the user by id
        public UserModel GetUserById(int id){
            return _context.UserInfo.SingleOrDefault(user => user.Id == id);
        }

        //Function to hard delete a user from the user database. Not suggested to use in website; only for testing, admin, and referencing purposes.
        public bool HardDeleteUser(string userToDelete){
            //this one is just sending over the username; we have to get the object to be deleted.
            UserModel foundUser = GetUserByUserName(userToDelete);
            bool result = false;
            //if statement to find if the user was found or not. If true, proceed to delete
            if (foundUser != null){
                _context.Remove<UserModel>(foundUser);
                result = _context.SaveChanges() != 0;
            }
            return result;
        }

        //Function to soft delete a user so that they disappear from the front end, but there is still a record of them in the database.
        public bool DeleteUser(string username){
            UserModel foundUser = GetUserByUserName(username);
            bool result = false;
            if(foundUser != null){
                foundUser.isDeleted = true;
                _context.Update<UserModel>(foundUser);
                result = _context.SaveChanges() != 0;
            }
            return result;
        }

        //Function to retrieve user's information minus some senstitive material, laundered through a DTO
        public UserInfoDTO GetByUserName(string? username){
            var UserInformation = new UserInfoDTO();
            if(DoesUserExist(username)){
                var foundUser = _context.UserInfo.SingleOrDefault(user => user.Username == username);
                UserInformation.Id = foundUser.Id;
                UserInformation.Name = foundUser.Name;
                UserInformation.Username = foundUser.Username;
                UserInformation.Email = foundUser.Email;
                UserInformation.Birthday = foundUser.Birthday;
                UserInformation.Zipcode = foundUser.Zipcode;
                UserInformation.CreationTime = foundUser.CreationTime;
            }
            return UserInformation;
        }
    }
}