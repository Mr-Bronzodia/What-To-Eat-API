using Microsoft.EntityFrameworkCore;
using WhatToEat.Data;
using WhatToEat.DTOs;
using WhatToEat.Entities;
using WhatToEat.Mappings;

namespace WhatToEat.Endpoints
{
    public static class TagsEndpoint
    {
        public static RouteGroupBuilder MapTagsEndpoints(this WebApplication webApplication)
        {
            RouteGroupBuilder group = webApplication.MapGroup("tags");

            group.MapGet("/", (RecipeContext dbContext) => GetAllTags(dbContext));

            return group;
        }


        private static List<TagCreator> GetAllTags(RecipeContext dbContext)
        {
            List<TagCreator> tagDtos = new List<TagCreator>();
            dbContext.Tags.ToList().ForEach(x => tagDtos.Add(x.ToDto()));

            return tagDtos;
        }
    }
}