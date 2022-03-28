using AutoMapper;
using Entity;
using FeedbackManagement.API.DTO;
using FeedbackManagement.Core.Enums;
using FeedbackManagement.Core.AbstractServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeedbackManagement.Data.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FeedbackManagement.API.DTO.CheckFeedbackNoteDTOs;
using FeedbackManagement.API.EmailService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace FeedbackManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecretaryController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly FeedbackManagementDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFeedbackService _feedbackService;
        private readonly IUserService _userService;
        private readonly IUserFeedbackService _userFeedbackService;
        private readonly IMapper _mapper;
        private IEmailSender _emailSender;

        public SecretaryController(IFeedbackService feedbackService,
                                   IMapper mapper,
                                   IUserFeedbackService userFeedbackService,
                                   IUserService userService,
                                   FeedbackManagementDBContext context,
                                   IEmailSender emailSender,
                                   UserManager<AppUser> userManager,
                                   IHttpContextAccessor httpContextAccessor)
        {
            _feedbackService = feedbackService;
            _mapper = mapper;
            _userFeedbackService = userFeedbackService;
            _userService = userService;
            _context = context;
            _emailSender = emailSender;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("feedbacklist")]

        public async Task<ActionResult<IEnumerable<FeedbackDTO>>> Index()
        {
            var feedbacks = await _feedbackService.GetFeedbacksWithCategoryAsync();
            var feedbackResources = _mapper.Map<IEnumerable<Feedback>, IEnumerable<FeedbackDTO>>(feedbacks);
            return Ok(feedbackResources);
        }
        [HttpGet("feedbacks/id")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles ="Admin")]
        public async Task<ActionResult<FeedbackDTO>> FeedbackDetail(int id)
        {

            if (id == 0)
            {
                return StatusCode(404);
            }
            var feedback = await _feedbackService.GetFeedbackWithCategoryAsync(id);
            var feedbackDetail = _mapper.Map<Feedback, DTO.FeedbackDetailDTOs.FeedbackDTO>(feedback);
            if (feedbackDetail == null)
            {
                return StatusCode(404);
            }
            return Ok(feedbackDetail);
        }
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("sendtohr/id")]
        public async Task<IActionResult> SendToHR(int id)
        {
            if (id <= 0)
            {
                return StatusCode(404);
            }
            var feedback = await _feedbackService.GetFeedbackWithCategoryAsync(id);
            if (feedback == null)
            {
                return BadRequest("Daxil edilən ID-yə uyğun feedback mövcud deyil");
            }
            var departmentID = feedback.FeedbackCategory.DepartmentID;
            var user = await _userService.GetHRRoleAsync(departmentID);
            if (user == null)
            {
                return BadRequest("Bu şöbə üçün HR mövcud deyil");
            }
            UserFeedback userFeedback = new UserFeedback
            {
                FeedbackID = feedback.ID,
                AppUserID = user.Id
            };
            feedback.FeedbackStatus = FeedbackStatus.inProgress;
            _context.UserFeedbacks.Add(userFeedback);
            _context.Feedbacks.Update(feedback);
            _context.SaveChanges();
            return Ok("Feedback müvafiq şöbənin HR-ına göndərildi!");
        }

        [HttpGet("checkfeedbacknote/id")]
        //[Authorize(Roles = "Secretary")]
        //Check feedback which sended from HR (GET)
        public async Task<ActionResult<UserFeedbackCheckDTO>> CheckFeedbackNote(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            var feedback = await _userFeedbackService.CheckFeedbackNoteAsync(id);
            var mappedFeedback = _mapper.Map<UserFeedback, UserFeedbackCheckDTO>(feedback);
            return Ok(mappedFeedback);
            //return View(feedback);
        }

        [HttpPost("checkfeedbacknote")]
        //[Authorize(Roles = "Secretary")]

        public async Task<ActionResult> CheckFeedbackNote([FromQuery]int FeedbackID, [FromQuery]string secretaryNote)
        {
            if (FeedbackID == 0)
            {
                return NotFound();
            }
            var secretaryFeedback = await _userFeedbackService.CheckFeedbackNotePostAsync(FeedbackID, secretaryNote);
            await _feedbackService.UpdateAsync(secretaryFeedback.Feedback);
            return Ok("Mesajınız düzəliş edilməsi üçün HR-a göndərildi!");
        }
        [HttpGet("sendfeedbacktocustomer/id")]
        public async Task<IActionResult> SendFeedbackToCustomer(int id)
        {
            if (id == 0)
            {
                return StatusCode(404);
            }
            var customer = await _userFeedbackService.GetCustomerAsync(id);

            try
            {
                await _emailSender.SendEmailAsync(customer.Feedback.CustomerEmail, "Feedback cavabı", customer.HRNote);
                if (customer.Feedback.FeedbackStatus == FeedbackStatus.inProgress)
                {
                    customer.Feedback.FeedbackStatus = FeedbackStatus.Approved;

                }
                return Ok(customer.FeedbackID + " ID nömrəli feedback sahibinə mail vasitəsilə göndərildi");

            }
            catch (Exception e)
            {
            }
            customer.Feedback.EmailStatus = true;
            await _feedbackService.UpdateAsync(customer.Feedback);
            //_context.Feedbacks.Update(customer.Feedback);
            //_context.SaveChanges();
            return NoContent();
        }
        [HttpGet("hrfeedbacklist")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> HRFeedbackList()
        {
            var autheis = User.Identity.IsAuthenticated;
            var fff = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.FindFirstValue(ClaimTypes.Email); // will give the user's Email

            var currentUserID = Convert.ToInt32(User.Claims.Where(x => x.Type == "Id").FirstOrDefault()?.Value);
            //var uu = _userManager.GetUserId(,);
            //var currentUserID = _userManager.Users
            //   .Where(u => u.UserName == User.Identity.Name)
            //   .FirstOrDefaultAsync().Id;
            var b = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var i = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var feedback = await _userFeedbackService.HRFeedbackListAsync(currentUserID);
            return Ok(feedback);
        }

        [HttpGet("hrfeedbackdetails/id")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> HRFeedbackDetails(int id)
        {

            if (id == 0)
            {
                return StatusCode(404);
            }
            //var currentUserID = _userManager.Users
            //   .Where(u => u.UserName == User.Identity.Name)
            //   .FirstOrDefault().Id;
            var currentUserID = Convert.ToInt32(User.Claims.Where(x => x.Type == "Id").FirstOrDefault()?.Value);

            var feedback = await _userFeedbackService.GetHRFeedbackAsync(id, currentUserID);
            if (feedback == null)
            {
                return StatusCode(404);
            }
            return Ok(feedback);
        }


        [HttpPost]
        //[Authorize(Roles = "HR")]

        public async Task<IActionResult> HRFeedbackDetails(string HRNote, int FeedbackID)
        {
            if (HRNote == null && FeedbackID == 0)
            {
                return StatusCode(404);
            }
            //var currentUserID = _userManager.Users
            //    .Where(u => u.UserName == User.Identity.Name)
            //    .FirstOrDefault().Id;
            var currentUserID = Convert.ToInt32(User.Claims.Where(x => x.Type == "Id").FirstOrDefault()?.Value);


            var userFeedback = await _userFeedbackService.HRFeedbackDetailsAsync(HRNote, FeedbackID, currentUserID);
            //TempData["HRNoteSended"] = "Mesajınız yoxlanılma üçün katibə göndərildi!";
            return Ok("Mesajınız yoxlanılma üçün katibə göndərildi!");
            //return RedirectToAction("HRFeedbackList", "Account");

        }
        [HttpPut]
        [Authorize(Roles = "Secretary")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            if (id == 0)
            {
                return StatusCode(404);
            }
            var feedback = await _feedbackService.GetByID(id);
            if (feedback == null)
            {
                return StatusCode(404);
            }
            await _feedbackService.DeleteAsync(feedback);
            //TempData["DeleteSucceeded"] = "Feedback uğurla silindi";
            return Ok("Feedback uğurla silindi");
            //return RedirectToAction("Index", "Account");

        }

    }
}
