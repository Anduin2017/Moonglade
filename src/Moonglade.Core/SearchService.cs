﻿using Edi.Practice.RequestResponseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moonglade.Data.Entities;
using Moonglade.Data.Infrastructure;
using Moonglade.Model;
using Moonglade.Model.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Moonglade.Core
{
    public class SearchService : MoongladeService
    {
        private readonly IRepository<PostEntity> _postRepository;

        public SearchService(
            ILogger<PostService> logger,
            IOptions<AppSettings> settings,
            IRepository<PostEntity> postRepository) : base(logger, settings)
        {
            _postRepository = postRepository;
        }

        public Task<Response<IReadOnlyList<PostListItem>>> SearchAsync(string keyword)
        {
            return TryExecuteAsync<IReadOnlyList<PostListItem>>(async () =>
            {
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    throw new ArgumentNullException(keyword);
                }

                var postList = SearchByKeyword(keyword);

                var resultList = await postList.Select(p => new PostListItem
                {
                    Title = p.Title,
                    Slug = p.Slug,
                    ContentAbstract = p.ContentAbstract,
                    PubDateUtc = p.PostPublish.PubDateUtc.GetValueOrDefault(),
                    Tags = p.PostTag.Select(pt => new Tag
                    {
                        NormalizedTagName = pt.Tag.NormalizedName,
                        TagName = pt.Tag.DisplayName
                    }).ToList()
                }).ToListAsync();

                return new SuccessResponse<IReadOnlyList<PostListItem>>(resultList);
            }, keyParameter: keyword);
        }

        private IQueryable<PostEntity> SearchByKeyword(string keyword)
        {
            var query = _postRepository.GetAsQueryable()
                                       .Where(p => !p.PostPublish.IsDeleted && p.PostPublish.IsPublished).AsNoTracking();

            var str = Regex.Replace(keyword, @"\s+", " ");
            var rst = str.Split(' ');
            if (rst.Length > 1)
            {
                // keyword: "dot  net rocks"
                // search for post where Title containing "dot && net && rocks"
                var result = rst.Aggregate(query, (current, s) => current.Where(p => p.Title.Contains(s)));
                return result;
            }
            else
            {
                // keyword: "dotnetrocks"
                var k = rst.First();
                var result = query.Where(p => p.Title.Contains(k) ||
                                              p.PostTag.Select(pt => pt.Tag).Select(t => t.DisplayName).Contains(k));
                return result;
            }
        }
    }
}
