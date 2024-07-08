using Microsoft.EntityFrameworkCore;
using WhatToEat.Data;
using WhatToEat.DTOs;
using WhatToEat.Entities;
using WhatToEat.Mappings;

namespace WhatToEat.Endpoints
{
    public static class IngredientsEndpoint
    {
        public static RouteGroupBuilder MapIngredientEndpoints(this WebApplication webApplication)
        {
            RouteGroupBuilder group = webApplication.MapGroup("ingredients");

            group.MapGet("/", (RecipeContext dbContext) => GetAllIngridients(dbContext));

            return group;
        }

        private static List<IngredientCreator> GetAllIngridients(RecipeContext dbContext)
        {
            List<IngredientCreator> ingridientsDtos = new List<IngredientCreator>();
            dbContext.Ingridients.ToList().ForEach(x => ingridientsDtos.Add(x.ToDto()));

            return ingridientsDtos;
        }
    }
}
