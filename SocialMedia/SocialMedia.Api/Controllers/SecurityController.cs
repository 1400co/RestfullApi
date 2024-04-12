using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Responses;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Interfaces;
using System;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ISecurityService _securityService;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;

        public SecurityController(ISecurityService securityService, IMapper mapper, IPasswordService passwordService)
        {
            _securityService = securityService;
            _mapper = mapper;
            _passwordService = passwordService;
        }

        /// <summary>
        /// Create User Login.
        /// </summary>
        /// <param name="securityDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(SecurityDto securityDto)
        {
            var security = _mapper.Map<Security>(securityDto);

            security.Password = _passwordService.Hash(security.Password);
            await _securityService.RegisterUser(security);

            securityDto = _mapper.Map<SecurityDto>(security);
            var response = new ApiResponse<SecurityDto>(securityDto);
            return Ok(response);
        }

        /// <summary>
        /// Actualiza las credenciales de seguridad de un usuario existente.
        /// </summary>
        /// <param name="id">El identificador único del usuario de seguridad.</param>
        /// <param name="securityDto">Los nuevos datos de las credenciales de seguridad.</param>
        /// <returns>Un resultado HTTP que indica que las credenciales de seguridad se han actualizado con éxito.</returns>
        /// <remarks>
        /// Este método permite actualizar las credenciales de seguridad de un usuario existente en el sistema. Recibe el identificador único del usuario y los nuevos datos de las credenciales en forma de objeto `SecurityDto`. Luego, mapea estos datos a un objeto `Security` y realiza la actualización en la base de datos a través del servicio correspondiente. Además, se realiza el hash de la nueva contraseña antes de actualizarla en la base de datos por motivos de seguridad. Finalmente, devuelve una respuesta HTTP indicando que las credenciales de seguridad se han actualizado con éxito.
        /// </remarks>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, SecurityDto securityDto)
        {
            var security = _mapper.Map<Security>(securityDto);

            var userSecurity = await _securityService.GetSecurityUser(securityDto.UserId);

            if (userSecurity == null)
            {
                return NotFound();
            }

            security.Id = userSecurity.Id;
            security.Password = _passwordService.Hash(security.Password);
            await _securityService.UpdateCredentials(security);

            var securityResult = _mapper.Map<SecurityDto>(security);
            var response = new ApiResponse<SecurityDto>(securityResult);
            return Ok(response);
        }

    }
}