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

namespace FeedbackManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly JwtConfig _jwtSettings;


        private readonly IMapper _mapper;
        public AuthController(UserManager<AppUser> userManager,
                              SignInManager<AppUser> signInManager,
                              IMapper mapper,
                              RoleManager<AppRole> roleManager,
                              IOptionsMonitor<JwtConfig> jwtTokenOptions)
        {
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtTokenOptions.CurrentValue;
            _signInManager = signInManager;
        }
        [HttpPost("signin")]
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


            var user = await _userManager.FindByNameAsync(loginUserDTO.UserName);
            if (user == null)
            {
                return BadRequest(new
                {
                    StatusCode = 404,
                    Message = "Bu istifadəçi adı ilə hesab tapılmadı"
                });
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginUserDTO.Password, true, false);
            if (result.Succeeded)
            {
                var token = GenerateJwt(user);
                return Ok(new
                {
                    Token = token,
                    Message = "Uğurlu giriş"
                });
            }
            return Unauthorized("İstifadəçi adı və ya şifrə yanlışdır");
        }
        [HttpPost("signup")]
        public async Task<IActionResult> Register(RegisterUserDTO registerUserDTO)
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
            var user = _mapper.Map<RegisterUserDTO, AppUser>(registerUserDTO);
            var role = _roleManager.Roles.Where(r => r.Id == registerUserDTO.RoleId).FirstOrDefault().ToString();
            var result = await _userManager.CreateAsync(user, registerUserDTO.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role);
                var response = new
                {
                    Status = 201,
                    Message = "İstifadəçi uğurla yaradıldı"
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
        public async Task<IActionResult> AddUserToRole(string email , string roleName)
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
        public async Task<IActionResult> RemoveUserFromRole(string email , string roleName)
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

        private string GenerateJwt(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),

                Expires = DateTime.UtcNow.AddDays(_jwtSettings.ExpiryTimeInDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _jwtSettings.Audience,
                Issuer = _jwtSettings.Issuer
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
