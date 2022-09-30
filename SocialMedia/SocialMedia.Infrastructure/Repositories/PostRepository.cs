﻿
using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly SocialMediaContext _socialMediaContext;
        public PostRepository(SocialMediaContext socialMediaContext)
        {
            _socialMediaContext = socialMediaContext;
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            var posts = await this._socialMediaContext.Posts.ToListAsync();

            return posts;
        }

        public async Task<Post> GetPost(int id)
        {
            var post = await _socialMediaContext.Posts.FirstOrDefaultAsync(x => x.PostId == id);

            return post;
        }

        public async Task InsertPost(Post post)
        {
            _socialMediaContext.Posts.Add(post);
            await _socialMediaContext.SaveChangesAsync();
        }
    }
}