using Common.Enums;
using Data.Auth;
using FinalAssignment.DTOs.User;
using FinalAssignment.Repositories.Implements;
using FinalAssignment.Repositories.Interfaces;
using FinalAssignment.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace FinalAssignment.Services.Implements
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
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
            string num = (++number).ToString();
            staffCode = staffCode + num;
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

            bool IsAgeLessThan18Years(DateTime birthDate)
            {
                if (DateTime.Now.Year - birthDate.Year > 18)
                {
                    return false;
                }
                else if (DateTime.Now.Year - birthDate.Year < 18)
                {
                    return true;
                }
                else //if (DateTime.Now.Year - birthDate.Year == 18)
                {
                    if (birthDate.DayOfYear < DateTime.Now.DayOfYear)
                    {
                        return false;
                    }
                    else if (birthDate.DayOfYear > DateTime.Now.DayOfYear)
                    {
                        return true;
                    }
                    else //if (birthDate.DayOfYear == DateTime.Now.DayOfYear)
                    {
                        return false;
                    }
                }
            }

            if (IsAgeLessThan18Years(model.DateOfBirth))
            {
                return new Response
                {
                    Status = "Error",
                    Message = "DOB < 18!"
                };
            }

            var userNumber = _userManager.Users.Count(); // 3

            ApplicationUser user = new()
            {

                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                FullName = model.FirstName + " " + model.LastName,
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
                Message = result.Errors.FirstOrDefault().Description
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

            if (user.IsDeleted == false && await _userManager.CheckPasswordAsync(user, model.Password))
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
            if (await _userManager.CheckPasswordAsync(user, model.NewPassword))
            {
                return new Response
                {
                    Status = "Error",
                    Message = "The new password must not be the same as the old password"
                };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            user.IsFirstTime = false;
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

        public async Task<UserResponse> GetUserByUsername(string userName)
        {
            var users = await _userManager.FindByNameAsync(userName);
            if (users == null)
            {
                return new UserResponse();
            }

            var user = _userManager.Users.Where(i => i.UserName == userName).Select(user => new UserResponse()
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                JoinedDate = user.JoinedDate,
                Type = user.Type,
            }).ToList().FirstOrDefault();

            return user;
        }

        public async Task<Response?> EditUser(EditUserRequest model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null)
                return new Response
                {
                    Status = "Error",
                    Message = "User not exists!"
                };

            if (!Enum.IsDefined(typeof(UserTypeEnum), model.UserRole))
            {
                return new Response
                {
                    Status = "Error",
                    Message = "User Role is invalid!"
                };
            }

            bool IsAgeLessThan18Years(DateTime birthDate)
            {
                if (DateTime.Now.Year - birthDate.Year > 18)
                {
                    return false;
                }
                else if (DateTime.Now.Year - birthDate.Year < 18)
                {
                    return true;
                }
                else //if (DateTime.Now.Year - birthDate.Year == 18)
                {
                    if (birthDate.DayOfYear < DateTime.Now.DayOfYear)
                    {
                        return false;
                    }
                    else if (birthDate.DayOfYear > DateTime.Now.DayOfYear)
                    {
                        return true;
                    }
                    else //if (birthDate.DayOfYear == DateTime.Now.DayOfYear)
                    {
                        return false;
                    }
                }
            }

            if (IsAgeLessThan18Years(model.DateOfBirth))
            {
                return new Response
                {
                    Status = "Error",
                    Message = "DOB < 18!"
                };
            }

            bool IsJoinedDateLessThanDob(DateTime birthDate, DateTime joindedDate)
            {
                if (joindedDate.Year > birthDate.Year)
                {
                    return false;
                }
                else if (joindedDate.Year < birthDate.Year )
                {
                    return true;
                }
                else //if (DateTime.Now.Year - birthDate.Year == 18)
                {
                    if (birthDate.DayOfYear < joindedDate.DayOfYear)
                    {
                        return false;
                    }
                    else if (birthDate.DayOfYear > joindedDate.DayOfYear)
                    {
                        return true;
                    }
                    else //if (birthDate.DayOfYear == DateTime.Now.DayOfYear)
                    {
                        return false;
                    }
                }
            }

            if (IsJoinedDateLessThanDob(model.DateOfBirth, model.JoinedDate))
            {
                return new Response
                {
                    Status = "Error",
                    Message = "Joined < DoB!"
                };
            }

            user.DateOfBirth = model.DateOfBirth;
            user.Gender = model.Gender;
            user.JoinedDate = model.JoinedDate;
            user.Type = model.UserRole;

            var currentRoles = await _userManager.GetRolesAsync(user);

            var removeUserFromRolesResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!removeUserFromRolesResult.Succeeded)
            {
                return new Response
                {
                    Status = "Error",
                    Message = removeUserFromRolesResult.Errors.FirstOrDefault().Description
                };
            }

            var addUserToRoleResult = await _userManager.AddToRoleAsync(user, model.UserRole);

            if (!addUserToRoleResult.Succeeded)
            {
                return new Response
                {
                    Status = "Error",
                    Message = addUserToRoleResult.Errors.FirstOrDefault().Description
                };
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

            using (var transaction = _userRepository.DatabaseTransaction())
            {
                try
                {    // check xem co ton tai user trong Assignment khong, neu co thi bao loi
                    var user = await _userManager.FindByNameAsync(userName);

                    if (user == null)
                        return new Response
                        {
                            Status = "Error",
                            Message = "User not exists!"
                        };

                    user.IsDeleted = true;
                    await _userManager.UpdateAsync(user);
                    transaction.Commit();
                    return new Response
                    {
                        Status = "Success",
                        Message = "User delete successfully!"
                    };
                }
                catch
                {
                    transaction.RollBack();
                    return null;
                }
            }
        }

        public async Task<IEnumerable<UserResponse?>> GetAllUserDependLocation(string userName)
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
                JoinedDate = user.JoinedDate,
                Location = user.Location,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth,
            }).ToList();

            return users;
        }
    }
}