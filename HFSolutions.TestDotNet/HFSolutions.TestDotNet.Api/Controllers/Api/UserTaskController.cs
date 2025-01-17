using FluentValidation;
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
    /// <summary>
    /// Controlador que gestiona las operaciones relacionadas con los tareas de usuarios.
    /// </summary>
    /// <param name="logger">Servicio de registro utilizado por la aplicación.</param>
    /// <param name="userTaskService">Servicio de usuarios utilizado para procesar lógica de negocio.</param>
    /// <param name="createUserTaskDtoValidator">Validador de datos de creación de tarea de usuario.</param>
    /// <param name="updateUserTaskDtoValidator">Validador de datos de actualización de tarea de usuario.</param>
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

        /// <summary>
        /// Crea una tarea relacionada con el usuario logueado.
        /// </summary>
        /// <param name="createUserTaskDto">Datos de tarea de usuario que se creará.</param>
        /// <returns>tarea de usuario creada.</returns>
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

        /// <summary>
        /// Obtiene la tarea de un usuario mediante su identificador (solo del usuario logueado).
        /// </summary>
        /// <param name="id">Identificador de tarea buscada.</param>
        /// <returns>Tarea de usuario encontrada.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserTaskDto>> Get(int id)
        {
            int userId = User.GetUserId();
            var userTask = await _userTaskService.ReadAsync(userId, id);

            return userTask != null
                ? Ok(userTask)
                : NotFound("The requested user task does not exist.");
        }

        /// <summary>
        /// Lista todas las tareas del usuario logueado.
        /// </summary>
        /// <returns>Tareas de usuario logueado</returns>
        [HttpGet("AllTasks")]
        public async Task<ActionResult<IEnumerable<UserTaskDto>>> GetAll()
        {
            int userId = User.GetUserId();
            var userTasks = await _userTaskService.ReadAllAsync(userId);

            return Ok(userTasks);
        }

        /// <summary>
        /// lista todas las tareas del usuario logueado de forma paginada.
        /// </summary>
        /// <param name="userTaskQueryParams">Filtros de tareas de usuario.</param>
        /// <param name="paginationQueryParams">Propiedades de paginación.</param>
        /// <returns>Tareas de usuario logueado paginadas.</returns>
        [HttpGet("All")]
        public async Task<ActionResult<PagedResponse<UserTaskDto>>> GetAll([FromQuery] UserTaskQueryParams? userTaskQueryParams = null, [FromQuery] PaginationQueryParams? paginationQueryParams = null)
        {
            int userId = User.GetUserId();
            var userTasks = await _userTaskService.ReadAllAsync(userId, userTaskQueryParams, paginationQueryParams);

            return Ok(userTasks);
        }

        /// <summary>
        /// Actualiza los datos de una tarea de usuario en expecífico (solo del usuario logueado).
        /// </summary>
        /// <param name="id">Identificador de tarea de usuario que se desea actualizar.</param>
        /// <param name="updateUserTaskDto">Nuevos datos de tarea de usuario.</param>
        /// <returns>Tarea de usuario actualizada.</returns>
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

        /// <summary>
        /// Borra una tarea de usuario (solo del usuario logueado).
        /// </summary>
        /// <param name="id">Identificador de tarea de usuario que se desea borrar.</param>
        /// <returns>Identificador de tarea de usuario borrada.</returns>
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
