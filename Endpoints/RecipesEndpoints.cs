using Microsoft.EntityFrameworkCore;
using WhatToEat.Data;
using WhatToEat.DTOs;
using WhatToEat.Entities;
using WhatToEat.Mappings;

namespace WhatToEat.Endpoints
{
    public static class RecipesEndpoints
    {
        private const string GetRecipeEndPoint = "GetRecipe";

        public static RouteGroupBuilder MapRecipeEndpoints(this WebApplication webApplication)
        {
            RouteGroupBuilder group = webApplication.MapGroup("recipes");

            group.MapGet("/", (RecipeContext dbContext) => GetAllRecipes(dbContext));

            group.MapGet("/{id}", (int id, RecipeContext dbContext) => GetRecipeByID(id, dbContext)).WithName(GetRecipeEndPoint);

            group.MapPost("/", (RecipeCreator newRecipe, RecipeContext dbContext) => AddRecipeToDatabase(newRecipe, dbContext));

            group.MapPut("/{id}", (int id, RecipeCreator updatedRecipe, RecipeContext dbContext) => UpdateRecipeInDatabase(id, updatedRecipe, dbContext));

            group.MapDelete("/{id}", (int id, RecipeContext dbContext) => DeleteRecipeFromDatabase(id, dbContext));

            return group;
        }

        private static List<RecipeCreator> GetAllRecipes(RecipeContext dbContext)
        {
            List<RecipeCreator> recipeDtos = new List<RecipeCreator>();
            foreach (Recipe recipe in dbContext.Recipes.ToList())
            {
                recipeDtos.Add(recipe.ToDto(dbContext));
            }

            return recipeDtos;
        }

        private static IResult GetRecipeByID(int id, RecipeContext dbContext)
        {
            RecipeCreator recipeCreator = dbContext.Recipes.Find(id).ToDto(dbContext);

            if (recipeCreator == null)
                return Results.NotFound();

            return Results.Ok(recipeCreator);
        }

        private static IResult AddRecipeToDatabase(RecipeCreator recipeDto, RecipeContext dbContext)
        {
            Recipe recipeEntity = recipeDto.ToEntity(dbContext);

            dbContext.Recipes.Add(recipeEntity);

            dbContext.SaveChanges();

            foreach (var ingredient in recipeEntity.Ingridients)
            {
                if (!dbContext.RecipeIngridients.Any(ri => ri.Recipe.Id == recipeEntity.Id && ri.Ingridient.Id == ingredient.Id))
                {
                    dbContext.RecipeIngridients.Add(new RecipeIngridient() { Ingridient = ingredient, Recipe = recipeEntity });
                }
            }

            foreach (var tag in recipeEntity.Tags)
            {
                if (!dbContext.RecipeTags.Any(rt => rt.Recipe.Id == recipeEntity.Id && rt.Tag.Id == tag.Id))
                {
                    dbContext.RecipeTags.Add(new RecipeTag() { Tag = tag, Recipe = recipeEntity });
                }
            }

            dbContext.SaveChanges();

            return Results.CreatedAtRoute(GetRecipeEndPoint, new { id = recipeEntity.Id }, recipeDto);
        }

        private static IResult UpdateRecipeInDatabase(int id, RecipeCreator updatedRecipeDto, RecipeContext dbContext)
        {
            Recipe recipeEntity = dbContext.Recipes.Include(r => r.Ingridients)
                                                       .Include(r => r.Tags)
                                                       .FirstOrDefault(r => r.Id == id);

            if (recipeEntity == null)
                return Results.NotFound();

            Recipe updatedRecipeEntity = updatedRecipeDto.ToEntity(dbContext);

            updatedRecipeEntity.Id = recipeEntity.Id;

            dbContext.RecipeIngridients.Where(ri => ri.Recipe.Id == recipeEntity.Id).ExecuteDelete();
            dbContext.RecipeTags.Where(rt => rt.Recipe.Id == recipeEntity.Id).ExecuteDelete();

            foreach (var ingredient in recipeEntity.Ingridients)
            {
                if (!dbContext.RecipeIngridients.Any(ri => ri.Recipe.Id == recipeEntity.Id && ri.Ingridient.Id == ingredient.Id))
                {
                    dbContext.RecipeIngridients.Add(new RecipeIngridient() { Ingridient = ingredient, Recipe = recipeEntity });
                }
            }

            foreach (var tag in recipeEntity.Tags)
            {
                if (!dbContext.RecipeTags.Any(rt => rt.Recipe.Id == recipeEntity.Id && rt.Tag.Id == tag.Id))
                {
                    dbContext.RecipeTags.Add(new RecipeTag() { Tag = tag, Recipe = recipeEntity });
                }
            }

            dbContext.Recipes.Entry(recipeEntity).CurrentValues.SetValues(updatedRecipeEntity);

            recipeEntity.Tags = updatedRecipeEntity.Tags;
            recipeEntity.Ingridients = updatedRecipeEntity.Ingridients;

            dbContext.SaveChanges();

            return Results.NoContent();
        }

        private static IResult DeleteRecipeFromDatabase(int id, RecipeContext dbContext)
        {
            Recipe recipeEntity = dbContext.Recipes.Find(id);

            if (recipeEntity == null)
                return Results.NotFound();


            dbContext.RecipeIngridients.Where(ri => ri.Recipe.Id == recipeEntity.Id).ExecuteDelete();
            dbContext.RecipeTags.Where(rt => rt.Recipe.Id == recipeEntity.Id).ExecuteDelete();
            dbContext.Recipes.Remove(recipeEntity);

            dbContext.SaveChanges();

            return Results.NoContent();
        }
    }
}
