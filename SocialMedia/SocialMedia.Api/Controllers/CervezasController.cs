using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
using System.Net;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class CervezasController : ControllerBase
    {
        private readonly ICervezasService _cervezasService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public CervezasController(ICervezasService cervezasService, IMapper mapper, IUriService uriService)
        {
            _cervezasService = cervezasService;
            _mapper = mapper;
            _uriService = uriService;
        }

        [HttpGet(Name = nameof(GetCervezas))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<CervezasDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<IEnumerable<CervezasDto>>))]
        public async Task<IActionResult> GetCervezas([FromQuery] CervezasQueryFilter filters)
        {
            var cervezas = await _cervezasService.GetCervezas(filters);
            var cervezasDto = _mapper.Map<IEnumerable<CervezasDto>>(cervezas);
            var response = new ApiResponse<IEnumerable<CervezasDto>>(cervezasDto);

            var metaData = new Metadata
            {
                TotalCount = cervezas.TotalCount,
                PageSize = cervezas.PageSize,
                CurrentPage = cervezas.CurrentPage,
                TotalPages = cervezas.TotalPages,
                HasNextPage = cervezas.HasNextPage,
                HasPreviousPage = cervezas.HasPreviousPage,
                NextPageUrl = cervezas.HasNextPage
                    ? _uriService.GetPageUri(cervezas.CurrentPage + 1, cervezas.PageSize, filters.Filter, Request.Path.Value!).ToString()
                    : null,
                PreviousPageUrl = cervezas.HasPreviousPage
                    ? _uriService.GetPageUri(cervezas.CurrentPage - 1, cervezas.PageSize, filters.Filter, Request.Path.Value!).ToString()
                    : null,
                Links = new List<LinkInfo>
                {
                    new() { Rel = "self", Href = Request.Path.Value!, Method = "GET" },
                    new() { Rel = "create", Href = Request.Path.Value!, Method = "POST" },
                }
            };

            response.Meta = metaData;
            response.Links = metaData.Links;
            Response.Headers["X-Pagination"] = JsonConvert.SerializeObject(metaData);
            return Ok(response);
        }

        [HttpGet("{id}", Name = nameof(GetCervezaById))]
        public async Task<IActionResult> GetCervezaById(Guid id)
        {
            var cerveza = await _cervezasService.GetCerveza(id);
            if (cerveza == null) return NotFound();
            var cervezaDto = _mapper.Map<CervezasDto>(cerveza);
            var response = new ApiResponse<CervezasDto>(cervezaDto);
            response.Links = new List<LinkInfo>
            {
                new() { Rel = "self", Href = $"{Request.Path.Value}", Method = "GET" },
                new() { Rel = "update", Href = $"{Request.Path.Value}", Method = "PUT" },
                new() { Rel = "delete", Href = $"{Request.Path.Value}", Method = "DELETE" },
                new() { Rel = "collection", Href = Request.Path.Value![..Request.Path.Value!.LastIndexOf('/')], Method = "GET" },
            };
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CervezasDto cervezasDto)
        {
            var cerveza = _mapper.Map<Cervezas>(cervezasDto);
            await _cervezasService.InsertCerveza(cerveza);
            cervezasDto = _mapper.Map<CervezasDto>(cerveza);
            var response = new ApiResponse<CervezasDto>(cervezasDto);
            return CreatedAtAction(nameof(GetCervezaById), new { id = cerveza.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, CervezasDto cervezasDto)
        {
            var cerveza = _mapper.Map<Cervezas>(cervezasDto);
            cerveza.Id = id;
            await _cervezasService.UpdateCerveza(cerveza);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _cervezasService.DeleteCerveza(id);
            return NoContent();
        }
    }
}
