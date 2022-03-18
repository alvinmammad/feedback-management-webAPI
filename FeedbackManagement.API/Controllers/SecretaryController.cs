using AutoMapper;
using Entity;
using FeedbackManagement.API.DTO;
using FeedbackManagement.Core.Enums;
using FeedbackManagement.Core.AbstractServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeedbackManagement.Data.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace FeedbackManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecretaryController : ControllerBase
    {
        private readonly FeedbackManagementDBContext _context;
        private readonly IFeedbackService _feedbackService;
        private readonly IUserService _userService;
        private readonly IUserFeedbackService _userFeedbackService;
        private readonly IMapper _mapper;
        
        public SecretaryController(IFeedbackService feedbackService,
                                   IMapper mapper,
                                   IUserFeedbackService userFeedbackService,
                                   IUserService userService,
                                   FeedbackManagementDBContext context)
        {
            _feedbackService = feedbackService;
            _mapper = mapper;
            _userFeedbackService = userFeedbackService;
            _userService = userService;
            _context = context;
        }
        [HttpGet]

        public async Task<ActionResult<IEnumerable<FeedbackDTO>>> Index()
        {
            var feedbacks =await _feedbackService.GetFeedbacksWithCategoryAsync();
            var feedbackResources = _mapper.Map<IEnumerable<Feedback>, IEnumerable<FeedbackDTO>>(feedbacks);
            return Ok(feedbackResources);
        }
        [HttpGet("id")]
        public async Task<ActionResult> FeedbackDetail(int id)
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
            return Ok(feedback);
        }
        [Authorize(AuthenticationSchemes ="Bearer")]
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

    }
}
