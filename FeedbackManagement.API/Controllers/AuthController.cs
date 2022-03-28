using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using Entity;
using FeedbackManagement.API.DTO.RegisterDTOs;
using FeedbackManagement.API.Settings;
using FeedbackManagement.API.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Text;
using FeedbackManagement.API.DTO.LoginDTOs;
using Microsoft.Extensions.Options;
using FeedbackManagement.API.DTO.RoleDTOs;
using FeedbackManagement.API.Configuration;
using FeedbackManagement.Core.Models;
using FeedbackManagement.Data.DAL;
using FeedbackManagement.API.DTO.ResfreshTokenDTOs;

namespace FeedbackManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly FeedbackManagementDBContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly JwtConfig _jwtConfig;


        private readonly IMapper _mapper;
        public AuthController(UserManager<AppUser> userManager,
                              SignInManager<AppUser> signInManager,
                              IMapper mapper,
                              RoleManager<AppRole> roleManager,
                              IOptionsMonitor<JwtConfig> jwtTokenOptions,
                              TokenValidationParameters tokenValidationParameters,
                              FeedbackManagementDBContext context)
        {
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtConfig = jwtTokenOptions.CurrentValue;
            _signInManager = signInManager;
            _tokenValidationParameters = tokenValidationParameters;
            _context = context;
        }
        [HttpPost("signin")]
        //[ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(LoginUserDTO loginUserDTO)
        {
            var validator = new LoginUserValidator();
            var validationResult = await validator.ValidateAsync(loginUserDTO);
            if (!validationResult.IsValid)
            {
                var errors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    errors.Add(error.ErrorMessage);

                }
                return BadRequest(errors);
            }


            var existingUser = await _userManager.FindByNameAsync(loginUserDTO.UserName);
            if (existingUser == null)
            {
                return BadRequest(new
                {
                    StatusCode = 404,
                    Message = "Bu istifadəçi adı ilə hesab tapılmadı"
                });
            }
            var isCorrect = await _userManager.CheckPasswordAsync(existingUser, loginUserDTO.Password);

            if (!isCorrect)
            {
                return BadRequest(new
                {
                    StatusCode = 404,
                    Message = "Şifrə düzgün deyil"
                });
            }
            var result = await _signInManager.PasswordSignInAsync(existingUser, loginUserDTO.Password, true, false);
            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(existingUser);
                
                var jwtToken = await GenerateJwtToken(existingUser);
                var obj = new
                {
                    token = jwtToken,
                    message = "Ugurla girish olundu"
                };
                return Ok (obj);
            }
            return BadRequest("Istifadechi adi ve ya shifre yanlishdir");
           

            
            //var result = await _signInManager.PasswordSignInAsync(existingUser, loginUserDTO.Password, true, false);
            //if (result.Succeeded)
            //{
            //    var jwtToken = await GenerateJwtToken(existingUser);
            //    return Ok(new
            //    {
            //        Token = jwtToken,
            //        Message = "Uğurlu giriş"
            //    });
            //}
            /*return Unauthorized("İstifadəçi adı və ya şifrə yanlışdır")*/
            ;
        }
        [HttpPost("signup")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO registerUserDTO)
        {
            var validator = new RegisterUserValidator();
            var validationResult = await validator.ValidateAsync(registerUserDTO);
            if (!validationResult.IsValid)
            {
                var errors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    errors.Add(error.ErrorMessage);

                }
                return BadRequest(errors);
            }
            var existingUser = await _userManager.FindByEmailAsync(registerUserDTO.Email);
            if (existingUser != null)
            {
                return BadRequest("Bu e-poçt adresinə uyğun istifadəçi mövcuddur , zəhmət olmasa digər e-poçt adreslərinizdən birini yoxlayın.");
            }
            var user = _mapper.Map<RegisterUserDTO, AppUser>(registerUserDTO);
            var role = _roleManager.Roles.Where(r => r.Id == registerUserDTO.RoleId).FirstOrDefault().ToString();
            var result = await _userManager.CreateAsync(user, registerUserDTO.Password);
            if (result.Succeeded)
            {
                var jwtToken = await GenerateJwtToken(user);
                await _userManager.AddToRoleAsync(user, role);
                var response = new
                {
                    Status = 201,
                    Message = "İstifadəçi uğurla yaradıldı",
                    JwtToken = jwtToken
                };
                return Ok(response);
            }
            else
            {
                //var errors = new List<string>();
                var errordict = new Dictionary<string, string>();
                foreach (var resultError in result.Errors)
                {
                    errordict.Add(resultError.Code, resultError.Description);
                }
                return BadRequest(errordict);

            }

        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Çıxış edildi");
        }
        [HttpGet("roleList")]
        public IActionResult RoleList()
        {
            var roles = _roleManager.Roles;
            var roleResources = _mapper.Map<IEnumerable<AppRole>, IEnumerable<FeedbackManagement.API.DTO.RoleDTOs.RoleListDTO>>(roles);
            return Ok(roleResources);
        }

        [HttpPost("createRole")]
        public async Task<IActionResult> CreateRole(CreateRoleDTO createRoleDTO)
        {
            if (createRoleDTO.Name == null)
            {
                return BadRequest("Vəzifə adını boş buraxmayın");
            }
            var roleExist = await _roleManager.RoleExistsAsync(createRoleDTO.Name);
            if (!roleExist)
            {
                var result = await _roleManager.CreateAsync(new AppRole(createRoleDTO.Name));
                if (result.Succeeded)
                {
                    return Ok("Vəzifə uğurla yaradıldı !");
                }
                else
                {
                    var errordict = new Dictionary<string, string>();
                    foreach (var resultError in result.Errors)
                    {
                        errordict.Add(resultError.Code, resultError.Description);
                    }
                    return BadRequest(errordict);
                }
            }

            return BadRequest("Bu adda vəzifə mövcuddur");

        }
        [HttpPost("addUserToRole")]
        public async Task<IActionResult> AddUserToRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("Bu e-poçt adresi ilə istifadəçi tapılmadı");
            }
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                return BadRequest("Belə vəzifə tapılmadı");
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                return Ok(roleName + " vəzifəsi istifadəçiyə təyin olundu");
            }
            else
            {
                return BadRequest(

                      "İstifadəçiyə vəzifə təyin olunmadı"
                );
            }
        }

        [HttpGet("getUserRoles")]
        public async Task<IActionResult> GetUserRoles(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) // User does not exist
            {

                return BadRequest(new
                {
                    error = "İstifadəçi tapılmadı"
                });
            }

            // return the roles
            var roles = await _userManager.GetRolesAsync(user);

            return Ok(roles);
        }
        [HttpPost("removeUserFromRole")]
        public async Task<IActionResult> RemoveUserFromRole(string email, string roleName)
        {
            // Check if the user exist
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) // User does not exist
            {

                return BadRequest(new
                {
                    error = "İstifadəçi tapılmadı"
                });
            }

            // Check if the role exist
            var roleExist = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExist) // checks on the role exist status
            {
                return BadRequest(new
                {
                    error = "Vəzifə tapılmadı"
                });
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                return Ok(new
                {
                    result = $"{email} istifadəçisi {roleName} vəzifəsindən silindi."
                });
            }

            return BadRequest(new
            {
                error = $"{email} istifadəçini {roleName} vəzifəsindən silinərkən xəta baş verdi."
            });
        }
        [HttpDelete("id")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            if (id == null)
            {
                return StatusCode(404);
            }
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return BadRequest("Vəzifə tapılmadı");
            }
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return Ok("Vəzifə uğurla silindi");
            }
            return BadRequest("Xəta baş verdi");
        }
        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenDTO tokenDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await VerifyAndGenerateToken(tokenDTO);
                if (result == null)
                {
                    return BadRequest("Invalid tokens");
                }
            }
            return BadRequest("Invalid payload");
        }

        private async Task<AuthResult> GenerateJwtToken(AppUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var claims = await GetAllValidClaims(user);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMonths(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsUsed = false,
                IsRevorked = false,
                AppUserId = user.Id.ToString(),
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
                Token = RandomString(35) + Guid.NewGuid()
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new AuthResult()
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token
            };
        }


        private async Task<List<Claim>> GetAllValidClaims(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Getting the claims that we have assigned to the user
            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            // Get the user role and add it to the claims
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var userRole in userRoles)
            {
                var role = await _roleManager.FindByNameAsync(userRole);

                if (role != null)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole));

                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (var roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }

            return claims;
        }

        private async Task<AuthResult> VerifyAndGenerateToken(TokenDTO tokenDTO)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var tokenInVerification = jwtTokenHandler.ValidateToken(tokenDTO.Token, _tokenValidationParameters, out var validatedToken);
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if (result == false)
                    {
                        return null;
                    }
                }
                var utcExpiryDate = long.Parse(tokenInVerification.Claims
                    .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp).Value);
                var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

                if (expiryDate > DateTime.UtcNow)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>()
                        {
                            "Token has not yet expired"
                        }
                    };
                }

                var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenDTO.RefreshToken);

                if (storedToken == null)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() {
                            "Token does not exist"
                        }
                    };
                }

                if (storedToken.IsUsed)
                {
                    return new AuthResult()
                    {

                        Errors = new List<string>() {
                            "Token has been used"
                        }
                    };
                }

                if (storedToken.IsRevorked)
                {
                    return new AuthResult()
                    {

                        Errors = new List<string>() {
                            "Token has been revoked"
                        }
                    };
                }

                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                if (storedToken.JwtId != jti)
                {
                    return new AuthResult()
                    {

                        Errors = new List<string>() {
                            "Token doesn't match"
                        }
                    };
                }

                if (storedToken.ExpiryDate < DateTime.UtcNow)
                {
                    return new AuthResult()
                    {

                        Errors = new List<string>() {
                            "Refresh token has expired"
                        }
                    };
                }

                storedToken.IsUsed = true;
                _context.RefreshTokens.Update(storedToken);
                await _context.SaveChangesAsync();

                // Generate a new token
                var dbUser = await _userManager.FindByIdAsync(storedToken.AppUserId);
                return await GenerateJwtToken(dbUser);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Lifetime validation failed. The token is expired."))
                {

                    return new AuthResult()
                    {

                        Errors = new List<string>() {
                            "Token has expired please re-login"
                        }
                    };

                }
                else
                {
                    return new AuthResult()
                    {

                        Errors = new List<string>() {
                            "Something went wrong."
                        }
                    };
                }
            }
        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTimeVal;
        }


        private string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(r => r[random.Next(r.Length)]).ToArray());
        }
    }
}
