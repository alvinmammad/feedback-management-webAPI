using AutoMapper;
using Entity;
using FeedbackManagement.API.DTO.HRFeedbackDTOs.HRFeedbackListDTOs;
using FeedbackManagement.Core.AbstractServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedbackManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HRController : ControllerBase
    {
        private readonly IUserFeedbackService _userFeedbackService;
        private readonly IMapper _mapper;
        public HRController(IUserFeedbackService userFeedbackService,
                            IMapper mapper)
        {
            _userFeedbackService = userFeedbackService;
            _mapper = mapper;
        }

        [HttpGet("hrfeedbacklist")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<ActionResult<IEnumerable<ListUserFeedbackDTO>>> HRFeedbackList()
        {
            var currentUserID = Convert.ToInt32(User.Claims.Where(x => x.Type == "Id").FirstOrDefault()?.Value);
            var feedback = await _userFeedbackService.HRFeedbackListAsync(currentUserID);
            var mappedFeedback = _mapper.Map<IEnumerable<UserFeedback>, IEnumerable<ListUserFeedbackDTO>>(feedback);
            return Ok(mappedFeedback);
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
            var mappedFeedback = _mapper.Map<UserFeedback, ListUserFeedbackDTO>(feedback);
            return Ok(mappedFeedback);
        }


        [HttpPost("hrfeedbackdetails")]
        //[Authorize(Roles = "HR")]

        public async Task<IActionResult> HRFeedbackDetails([FromQuery]string HRNote, [FromQuery] int FeedbackID)
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
    }
}
