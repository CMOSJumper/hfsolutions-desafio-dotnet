using FluentValidation;
using FluentValidation.AspNetCore;
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

            var userTask = await _userTaskService.CreateAsync(createUserTaskDto);

            return userTask != null
                ? Ok(userTask)
                : StatusCode(StatusCodes.Status500InternalServerError, "An error ocurred creating the user task.");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserTaskDto>> Get(int id)
        {
            var userTask = await _userTaskService.ReadAsync(id);

            return Ok(userTask);
        }

        [HttpGet("AllTasks")]
        public async Task<ActionResult<IEnumerable<UserTaskDto>>> GetAll()
        {
            var userTasks = await _userTaskService.ReadAllAsync();

            return Ok(userTasks);
        }

        [HttpGet("All")]
        public async Task<ActionResult<PagedResponse<UserTaskDto>>> GetAll([FromQuery] UserTaskQueryParams? userTaskQueryParams = null, [FromQuery] PaginationQueryParams? paginationQueryParams = null)
        {
            var userTasks = await _userTaskService.ReadAllAsync(userTaskQueryParams, paginationQueryParams);

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

            var userTask = await _userTaskService.UpdateAsync(id, updateUserTaskDto);

            return userTask != null
                ? Ok(userTask)
                : StatusCode(StatusCodes.Status500InternalServerError, "An error ocurred creating the user task.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            var result = await _userTaskService.DeleteAsync(id);

            return result > 0
                ? Ok(result)
                : Problem();
        }
    }
}
