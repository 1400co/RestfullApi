using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia.Api.Responses;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Dtos;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using SocialMedia.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordRecoveryController : ControllerBase
    {
        private readonly IPasswordRecoveryService _passwordRecoveryService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public PasswordRecoveryController(IPasswordRecoveryService passwordRecoveryService, IMapper mapper, IUriService uriService)
        {
            _passwordRecoveryService = passwordRecoveryService;
            _mapper = mapper;
            _uriService = uriService;
        }

        [HttpGet(Name = nameof(GetpasswordRecoverys))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<PasswordRecoveryDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetpasswordRecoverys([FromQuery] GeneralQueryFilter filters)
        {
            var passwordRecoverys = await _passwordRecoveryService.GetRecovery(filters);
            var passwordRecoveryDto = _mapper.Map<IEnumerable<PasswordRecoveryDto>>(passwordRecoverys);
            var response = new ApiResponse<IEnumerable<PasswordRecoveryDto>>(passwordRecoveryDto);

            var metaData = new Metadata
            {
                TotalCount = passwordRecoverys.TotalCount,
                PageSize = passwordRecoverys.PageSize,
                CurrentPage = passwordRecoverys.CurrentPage,
                TotalPages = passwordRecoverys.TotalPages,
                HasNextPage = passwordRecoverys.HasNextPage,
                HasPreviousPage = passwordRecoverys.HasPreviousPage,
            };

            response.Meta = metaData;
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metaData));

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var passwordRecovery = await _passwordRecoveryService.GetRecovery(id);
            var passwordRecoveryDto = _mapper.Map<PasswordRecoveryDto>(passwordRecovery);
            var response = new ApiResponse<PasswordRecoveryDto>(passwordRecoveryDto);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(PasswordRecoveryDto passwordRecoveryDto)
        {
            var passwordRecovery = _mapper.Map<PasswordRecovery>(passwordRecoveryDto);

            await _passwordRecoveryService.InsertRecovery(passwordRecovery);


            var response = new ApiResponse<PasswordRecoveryDto>(passwordRecoveryDto);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, PasswordRecoveryDto passwordRecoveryDto)
        {
            var passwordRecovery = _mapper.Map<PasswordRecovery>(passwordRecoveryDto);
            passwordRecovery.Id = id;

            await _passwordRecoveryService.UpdateRecovery(passwordRecovery);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _passwordRecoveryService.DeleteRecovery(id);
            return Ok();
        }
    }
}
