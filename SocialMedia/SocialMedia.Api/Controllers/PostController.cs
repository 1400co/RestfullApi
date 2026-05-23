using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia.Api.Responses;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Dtos;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[Controller]")]
    [ApiController]
    public class PostController(IPostService postService, IMapper mapper) : ControllerBase
    {

        [HttpGet(Name = nameof(GetPosts))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<PostDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<IEnumerable<PostDto>>))]
        public async Task<IActionResult> GetPosts([FromQuery] PostQueryFilter filters)
        {
            var posts = await postService.GetPosts(filters);
            var postDto = mapper.Map<IEnumerable<PostDto>>(posts);
            var response = new ApiResponse<IEnumerable<PostDto>>(postDto);

            var metaData = new Metadata
            {
                TotalCount = posts.TotalCount,
                PageSize = posts.PageSize,
                CurrentPage = posts.CurrentPage,
                TotalPages = posts.TotalPages,
                HasNextPage = posts.HasNextPage,
                HasPreviousPage = posts.HasPreviousPage,
            };

            response.Meta = metaData;
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metaData));

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var post = await postService.GetPost(id);
            if (post == null) return NotFound();
            var postDto = mapper.Map<PostDto>(post);
            var response = new ApiResponse<PostDto>(postDto);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(PostDto postDto)
        {
            var post = mapper.Map<Post>(postDto);

            await postService.InsertPost(post);

            postDto = mapper.Map<PostDto>(post);

            var response = new ApiResponse<PostDto>(postDto);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Guid id, PostDto postDto)
        {
            var post = mapper.Map<Post>(postDto);
            post.Id = id;

            await postService.UpdatePost(post);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Detele(Guid id)
        {
            await postService.DeletePost(id);

            return Ok();
        }
    }
}
