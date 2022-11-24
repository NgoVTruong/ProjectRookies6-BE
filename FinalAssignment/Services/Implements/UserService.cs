using FinalAssignment.DTOs.User;
using FinalAssignment.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Data.Auth;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using FinalAssignment.Repositories.Implements;

namespace FinalAssignment.Services.Implements
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public UserService(RoleManager<IdentityRole> roleManager, IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _configuration = configuration;
            _userManager = userManager;
        }

        public string StaffCodeGen(int number) //35
        {
            int check = number;
            int count = 0;
            while (check > 0) //35  //3
            {
                check = check / 10; //3 //0
                count++; //1 //2
            }
            string staffCode = "SD";
            for (int i = 0; i < 4 - count; i++)  //(int i = 0; i < 2; i++)
            {
                staffCode = staffCode + "0"; // SD00
            }
            staffCode = staffCode + (check + 1).ToString(); //SD0036
            return staffCode;
        }

        public async Task<Response> Register(RegisterModelRequest model)
        {
            var userExists = await _userManager.FindByNameAsync(model.UserName);

            if (userExists != null)
                return new Response
                {
                    Status = "Error",
                    Message = "User already exists!"
                };

            var userNumber = _userManager.Users.Count(); // 3

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                FullName = model.FirstName +" "+ model.LastName,
                Type = model.UserRole,
                DateOfBirth = model.DateOfBirth, // (2000, 1, 1),
                Gender = model.Gender, // (0 or 1 or 2)
                JoinedDate = model.JoinedDate, // >(2018, 1, 1),
                StaffCode = StaffCodeGen(userNumber), //SD0036
                Location = model.Location,
                IsFirstTime = true, // First time = 0
                IsDeleted = false,
            };

            bool whiteSpace = model.Password.Contains(" ");

            if (whiteSpace)
            {
                return new Response
                {
                    Status = "Error",
                    Message = "Password cannot have white space."
                };
            }

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) return new Response
            {
                Status = "Error",
                Message = "User creation failed! Please check user details and try again."
            };

            if (!await _roleManager.RoleExistsAsync(model.UserRole)) // "Admin" if admin and "Staff" if staff
            {
                await _roleManager.CreateAsync(new IdentityRole(model.UserRole));
            }

            if (await _roleManager.RoleExistsAsync(model.UserRole)) // "Admin" if admin and "Staff" if staff
            {
                await _userManager.AddToRoleAsync(user, model.UserRole);
            }

            return new Response
            {
                Status = "Success",
                Message = "User created successfully!"
            };
        }

        public async Task<LoginResponse> Login(LoginRequest model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                // var roleList = new List<RoleEachUser>();

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    // userRole.Add(roleList);
                }

                var token = CreateToken(authClaims);
                // var refreshToken = GenerateRefreshToken();

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

                // user.RefreshToken = refreshToken;
                // user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

                await _userManager.UpdateAsync(user);

                return new LoginResponse
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    // RefreshToken = refreshToken,
                    Expiration = token.ValidTo,
                    Roles = userRoles,
                    User = model.Username,
                    Location = user.Location,
                    IsFirstTime = user.IsFirstTime
                };
            }
            return null;
        }

        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        public async Task<Response> ResetPassword(ResetPasswordRequest model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null)
                return new Response
                {
                    Status = "Error",
                    Message = "User not exists!"
                };

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var userPassword = user.PasswordHash;
            var passwordHash = _userManager.PasswordHasher.HashPassword(user, model.NewPassword);

            if (passwordHash == userPassword)
            {
                return new Response
                {
                    Status = "Error",
                    Message = "The new password must not be the same as the old password"
                };
            }

            user.IsFirstTime = false;
            // user.PasswordHash = passwordHash;
            await _userManager.UpdateAsync(user);

            var resetPassResult = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    return new Response
                    {
                        Status = "Error",
                        Message = error.ToString()
                    };
                }
            }
            return new Response
            {
                Status = "Success",
                Message = "User reset password successfully!"
            };
        }

        public async Task<Response> EditUser(EditUserRequest model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            var deleteRole = await _userManager.RemoveFromRoleAsync(user, model.UserRole);
            
            // if (!result.Succeeded)
            // {
            //     foreach (var error in result.Errors)
            //     {
            //         return new Response
            //         {
            //             Status = "Error",
            //             Message = error.ToString()
            //         };
            //     }
            // }

            if (user == null)
                return new Response
                {
                    Status = "Error",
                    Message = "User not exists!"
                };

            user.DateOfBirth = model.DateOfBirth;
            user.Gender = model.Gender;
            user.JoinedDate = model.JoinedDate;
            user.Type = model.UserRole;



            if (!await _roleManager.RoleExistsAsync(model.UserRole)) // "Admin" if admin and "Staff" if user
            {
                await _roleManager.CreateAsync(new IdentityRole(model.UserRole));
            }

            if (await _roleManager.RoleExistsAsync(model.UserRole)) // "Admin" if admin and "Staff" if user
            {
                await _userManager.AddToRoleAsync(user, model.UserRole);
            }

            await _userManager.UpdateAsync(user);

            return new Response
            {
                Status = "Success",
                Message = "User edit successfully!"
            };
        }


        public async Task<Response> DeleteUser(string userName)
        {
            // check xem co ton tai user trong Assignment khong, neu co thi bao loi
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
                return new Response
                {
                    Status = "Error",
                    Message = "User not exists!"
                };

            user.IsDeleted = true;
            await _userManager.UpdateAsync(user);

            return new Response
            {
                Status = "Success",
                Message = "User delete successfully!"
            };
        }
        public async Task<IEnumerable<UserResponse>> GetAllUserDependLocation(string userName)
        {
            // check xem co ton tai user trong Assignment khong, neu co thi bao loi
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return Enumerable.Empty<UserResponse>();
            }
            var location = user.Location;
            var users = _userManager.Users.Where(i => i.Location == location && i.IsDeleted == false).Select(user => new UserResponse()
            {
                FirstName = user.FirstName,
                FullName = user.FullName,
                StaffCode = user.StaffCode,
                UserName = user.UserName,
                Type = user.Type,
                JoinDate = user.JoinedDate
            }).ToList();

            return users;
        }
    }
}
