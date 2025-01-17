﻿using FluentValidation;
using FluentValidation.AspNetCore;
using HFSolutions.TestDotNet.Api.Extensions;
using HFSolutions.TestDotNet.Application.Dtos.UserTaskDto;
using HFSolutions.TestDotNet.Application.Interfaces;
using HFSolutions.TestDotNet.Application.QueryParams;
using HFSolutions.TestDotNet.Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HFSolutions.TestDotNet.Api.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserTaskController(
        ILogger<UserTaskController> logger,
        IUserTaskService userTaskService,
        IValidator<CreateUserTaskDto> createUserTaskDtoValidator,
        IValidator<UpdateUserTaskDto> updateUserTaskDtoValidator) : ControllerBase
    {
        private readonly ILogger<UserTaskController> _logger = logger;
        private readonly IUserTaskService _userTaskService = userTaskService;
        private readonly IValidator<CreateUserTaskDto> _createUserTaskDtoValidator = createUserTaskDtoValidator;
        private readonly IValidator<UpdateUserTaskDto> _updateUserTaskDtoValidator = updateUserTaskDtoValidator;

        [HttpPost]
        public async Task<ActionResult<UserTaskDto>> Post([FromBody] CreateUserTaskDto createUserTaskDto)
        {
            var validationResult = await _createUserTaskDtoValidator.ValidateAsync(createUserTaskDto);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                _logger.LogError("The new user task does not pass the validation checks.");

                return BadRequest(ModelState);
            }

            createUserTaskDto.UserId = User.GetUserId();

            var userTask = await _userTaskService.CreateAsync(createUserTaskDto);

            return userTask != null
                ? Ok(userTask)
                : StatusCode(StatusCodes.Status500InternalServerError, "An error ocurred creating the user task.");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserTaskDto>> Get(int id)
        {
            int userId = User.GetUserId();
            var userTask = await _userTaskService.ReadAsync(userId, id);

            return userTask != null
                ? Ok(userTask)
                : NotFound("The requested user task does not exist.");
        }

        [HttpGet("AllTasks")]
        public async Task<ActionResult<IEnumerable<UserTaskDto>>> GetAll()
        {
            int userId = User.GetUserId();
            var userTasks = await _userTaskService.ReadAllAsync(userId);

            return Ok(userTasks);
        }

        [HttpGet("All")]
        public async Task<ActionResult<PagedResponse<UserTaskDto>>> GetAll([FromQuery] UserTaskQueryParams? userTaskQueryParams = null, [FromQuery] PaginationQueryParams? paginationQueryParams = null)
        {
            int userId = User.GetUserId();
            var userTasks = await _userTaskService.ReadAllAsync(userId, userTaskQueryParams, paginationQueryParams);

            return Ok(userTasks);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserTaskDto>> Put(int id, [FromBody] UpdateUserTaskDto updateUserTaskDto)
        {
            if (id != updateUserTaskDto.UserTaskId)
            {
                return BadRequest("The entered ids do not match.");
            }

            var validationResult = await _updateUserTaskDtoValidator.ValidateAsync(updateUserTaskDto);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                _logger.LogError("The user task does not pass the validation checks.");

                return BadRequest(ModelState);
            }

            updateUserTaskDto.UserId = User.GetUserId();

            var userTask = await _userTaskService.UpdateAsync(id, updateUserTaskDto);

            return userTask != null
                ? Ok(userTask)
                : StatusCode(StatusCodes.Status500InternalServerError, "An error ocurred updating the user task.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            int userId = User.GetUserId();
            var result = await _userTaskService.DeleteAsync(userId, id);

            return result > 0
                ? Ok(result)
                : StatusCode(StatusCodes.Status500InternalServerError, "An error ocurred deleting the user task.");
        }
    }
}
