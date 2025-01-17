using FluentValidation;
using FluentValidation.AspNetCore;
using HFSolutions.TestDotNet.Application.Dtos.UserDtos;
using HFSolutions.TestDotNet.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HFSolutions.TestDotNet.Api.Controllers.Api
{

    /// <summary>
    /// Controlador que gestiona las operaciones relacionadas con los usuarios.
    /// </summary>
    /// <param name="logger">Servicio de registro utilizado por la aplicación.</param>
    /// <param name="userService">Servicio de usuarios utilizado para procesar lógica de negocio.</param>
    /// <param name="createUserDtoValidator">Validador de datos de creación de usuarios.</param>
    /// <param name="userSecureDtoValidator">Validador de datos de actualización y más de usuarios.</param>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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

        /// <summary>
        /// Autentica al usuario con las credenciales especificadas.
        /// </summary>
        /// <param name="userLoginDto">Credenciales de usuario.</param>
        /// <returns>JWT Token.</returns>
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            var token = await _userService.Login(userLoginDto);

            return token != null
                ? Ok(token)
                : StatusCode(StatusCodes.Status401Unauthorized, "Username or password incorrect.");
        }

        /// <summary>
        /// crea un usuario con la información ingresada.
        /// </summary>
        /// <param name="createUserDto">Datos de usuario que se va a crear.</param>
        /// <returns>Usuario creado.</returns>
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

        /// <summary>
        /// Obtiene un usuario mediante su id.
        /// </summary>
        /// <param name="id">Identificador de usuario que se busca.</param>
        /// <returns>Usuario encontrado.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserSecureDto?>> Get(int id)
        {
            var user = await _userService.ReadAsync(id);

            return user != null
                ? Ok(user)
                : NotFound("The user with the specified id does not exist.");
        }

        /// <summary>
        /// Obtiene todos los usuarios registrados.
        /// </summary>
        /// <returns>Usuarios registrados</returns>
        [HttpGet("All")]
        public async Task<ActionResult<List<UserSecureDto>?>> GetAll()
        {
            var users = await _userService.ReadAllAsync();

            return users.ToList();
        }

        /// <summary>
        /// Actualiza los datos de un usuario en específico.
        /// </summary>
        /// <param name="id">Identificador de usuario que se desea actualizar.</param>
        /// <param name="userSecureDto">Nuevos datos de usuario.</param>
        /// <returns>Usuario actualizado.</returns>
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

        /// <summary>
        /// Borra un usuario.
        /// </summary>
        /// <param name="id">Identificador de usuario que se desea borrar.</param>
        /// <returns>Identificador de usuario borrado.</returns>
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
