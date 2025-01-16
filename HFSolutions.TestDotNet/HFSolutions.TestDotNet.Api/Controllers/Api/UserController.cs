using FluentValidation;
using FluentValidation.AspNetCore;
using HFSolutions.TestDotNet.Application.Dtos.UserDtos;
using HFSolutions.TestDotNet.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HFSolutions.TestDotNet.Api.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(
        ILogger<UserController> logger,
        IUserService userService,
        IValidator<CreateUserDto> createUserDtoValidator,
        IValidator<UserSecureDto> userSecureDtoValidator) : ControllerBase
    {
        private readonly ILogger<UserController> _logger = logger;
        private readonly IUserService _userService = userService;
        private readonly IValidator<CreateUserDto> _createUserDtoValidator = createUserDtoValidator;
        private readonly IValidator<UserSecureDto> _userSecureDtoValidator = userSecureDtoValidator;

        [HttpPost]
        public async Task<ActionResult<UserSecureDto?>> Post([FromBody] CreateUserDto createUserDto)
        {
            var validationResult = await _createUserDtoValidator.ValidateAsync(createUserDto);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                _logger.LogError("The new user does not pass the validation checks.");

                return BadRequest(ModelState);
            }

            var user = await _userService.CreateAsync(createUserDto);

            return user != null
                ? Ok(user)
                : StatusCode(StatusCodes.Status500InternalServerError, "An error ocurred creating the user.");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserSecureDto?>> Get(int id)
        {
            var user = await _userService.ReadAsync(id);

            return user != null
                ? Ok(user)
                : NotFound("The user with the specified id does not exist.");
        }

        [HttpGet("All")]
        public async Task<ActionResult<List<UserSecureDto>?>> GetAll()
        {
            var users = await _userService.ReadAllAsync();

            return users.ToList();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserSecureDto?>> Put(int id, [FromBody] UserSecureDto userSecureDto)
        {
            if (id != userSecureDto.UserId)
            {
                return BadRequest("The entered ids do not match.");
            }

            var validationResult = await _userSecureDtoValidator.ValidateAsync(userSecureDto);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                _logger.LogError("The user does not pass the validation checks.");

                return BadRequest(ModelState);
            }

            var user = await _userService.UpdateAsync(id, userSecureDto);

            return user != null
                ? Ok(user)
                : StatusCode(StatusCodes.Status500InternalServerError, "An error ocurred updating the user.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteAsync(id);

            return result > 0
                ? Ok(result)
                : Problem();
        }
    }
}
