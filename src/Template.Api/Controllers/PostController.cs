using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Template.Api.Responses;
using Template.Core.CustomEntities;
using Template.Core.Dtos;
using Template.Core.Entities;
using Template.Core.Interfaces;
using Template.Core.QueryFilters;
using Template.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Template.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public PostController(IPostService postService, IMapper mapper, IUriService uriService)
        {
            _postService = postService;
            _mapper = mapper;
            _uriService = uriService;
        }

        [HttpGet(Name = nameof(GetPosts))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<PostDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<IEnumerable<PostDto>>))]
        public async Task<IActionResult> GetPosts([FromQuery] PostQueryFilter filters)
        {
            var posts = await _postService.GetPosts(filters);
            var postDto = _mapper.Map<IEnumerable<PostDto>>(posts);
            var response = new ApiResponse<IEnumerable<PostDto>>(postDto);

            var metaData = new Metadata
            {
                TotalCount = posts.TotalCount,
                PageSize = posts.PageSize,
                CurrentPage = posts.CurrentPage,
                TotalPages = posts.TotalPages,
                HasNextPage = posts.HasNextPage,
                HasPreviousPage = posts.HasPreviousPage,
                NextPageUrl = posts.HasNextPage
                    ? _uriService.GetPageUri(posts.CurrentPage + 1, posts.PageSize, filters.Filter, Request.Path.Value!).ToString()
                    : null,
                PreviousPageUrl = posts.HasPreviousPage
                    ? _uriService.GetPageUri(posts.CurrentPage - 1, posts.PageSize, filters.Filter, Request.Path.Value!).ToString()
                    : null,
                Links = new List<LinkInfo>
                {
                    new() { Rel = "self", Href = Request.Path.Value!, Method = "GET" },
                    new() { Rel = "create", Href = Request.Path.Value!, Method = "POST" },
                }
            };

            response.Meta = metaData;
            response.Links = metaData.Links;
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metaData));

            return Ok(response);
        }

        [HttpGet("{id}", Name = nameof(GetPostById))]
        public async Task<IActionResult> GetPostById(Guid id)
        {
            var post = await _postService.GetPost(id);
            if (post == null) return NotFound();
            var postDto = _mapper.Map<PostDto>(post);
            var response = new ApiResponse<PostDto>(postDto);
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
        public async Task<IActionResult> Post(PostDto postDto)
        {
            var post = _mapper.Map<Post>(postDto);

            await _postService.InsertPost(post);

            postDto = _mapper.Map<PostDto>(post);

            var response = new ApiResponse<PostDto>(postDto);
            return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, PostDto postDto)
        {
            var post = _mapper.Map<Post>(postDto);
            post.Id = id;

            await _postService.UpdatePost(post);

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(Guid id, [FromBody] JsonPatchDocument<PostDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            var post = await _postService.GetPost(id);
            if (post == null)
                return NotFound();

            var postDto = _mapper.Map<PostDto>(post);
            patchDoc.ApplyTo(postDto, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _mapper.Map(postDto, post);
            await _postService.UpdatePost(post);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _postService.DeletePost(id);
            return NoContent();
        }
    }
}
