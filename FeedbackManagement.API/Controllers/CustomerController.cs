using AutoMapper;
using Entity;
using FeedbackManagement.API.DTO;
using FeedbackManagement.Business.ValidationRules;
using FeedbackManagement.Core.AbstractServices;
using FluentValidation.Results;
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
    public class CustomerController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IMapper _mapper;



        public CustomerController(IFeedbackService feedbackService,IMapper mapper)
        {
            _feedbackService = feedbackService;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<ActionResult<FeedbackDTO>> CreateFeedback([FromBody] SaveFeedbackDTO saveFeedbackDTO)
        {
            var feedbackToCreate = _mapper.Map<SaveFeedbackDTO, Feedback>(saveFeedbackDTO);
            var newFeedback = await _feedbackService.CreateAsync(feedbackToCreate);
            var feedback = await _feedbackService.GetByID(newFeedback.ID);
            var feedbackResource = _mapper.Map<Feedback, FeedbackDTO>(feedback);
            return Ok(feedbackResource);
        }
    }
}
