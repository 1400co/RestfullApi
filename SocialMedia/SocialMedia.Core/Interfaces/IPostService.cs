﻿using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface IPostService
    {
        Task InsertPost(Post post);
        Task<bool> UpdatePost(Post post);
        Task<Post> GetPost(Guid id);
        PagedList<Post> GetPosts(PostQueryFilter filters);
        Task DeletePost(Guid id);
    }
}