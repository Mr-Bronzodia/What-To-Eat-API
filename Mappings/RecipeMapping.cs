using Microsoft.EntityFrameworkCore;
using WhatToEat.Data;
using WhatToEat.DTOs;
using WhatToEat.Entities;

namespace WhatToEat.Mappings
{
    public static class RecipeMapping
    {
        public static Recipe ToEntity(this RecipeCreator recipe, RecipeContext dbContext)
        {
            List<Ingridient> ingredients = new List<Ingridient>();
            foreach (string ingredient in recipe.ingridients)
            {
                Ingridient existingIngredient = dbContext.Ingridients.FirstOrDefault(x => x.Name == ingredient);
                if (existingIngredient != null)
                {
                    ingredients.Add(existingIngredient);
                    continue;
                }

                Ingridient newIngredient = new Ingridient() { Name = ingredient };
                ingredients.Add(newIngredient);
            }

            List<Tag> tags = new List<Tag>();
            foreach (string tag in recipe.tags)
            {
                Tag existingTag = dbContext.Tags.FirstOrDefault(x => x.Text == tag);
                if (existingTag != null)
                {
                    tags.Add(existingTag);
                    continue;
                }

                Tag newTag = new Tag() { Text = tag };
                tags.Add(newTag);
            }

            Recipe recipeEntity = new Recipe()
            {
                Name = recipe.name,
                Description = recipe.descrition,
                ImageUrl = recipe.imageUrl,
                PreparationTime = recipe.prepTime,
                Ingridients = ingredients,
                Tags = tags
            };

            return recipeEntity;
        }

        public static RecipeCreator ToDto(this Recipe recipe, RecipeContext dbContext)
        {
            Recipe recipeEntity = dbContext.Recipes.Find(recipe.Id);

            if (recipeEntity == null)
                return null;


            List<string> ingredients = new List<string>();
            List<string> tags = new List<string>();

            var recipeIngridients = dbContext.RecipeIngridients.Include(ri => ri.Ingridient)
                                                                       .Where(ri => ri.Recipe.Id == recipeEntity.Id)
                                                                       .ToList();


            foreach (RecipeIngridient entry in recipeIngridients)
            {
                ingredients.Add(entry.Ingridient.Name);
            }

            var recipeTags = dbContext.RecipeTags.Include(rt => rt.Tag)
                                                         .Where(rt => rt.Recipe.Id == recipeEntity.Id)
                                                         .ToList();


            foreach (RecipeTag entry in recipeTags)
            {
                tags.Add(entry.Tag.Text);
            }

            RecipeCreator recipeDto = new RecipeCreator(recipeEntity.Id, recipeEntity.Name, recipeEntity.Description, recipeEntity.ImageUrl, recipeEntity.PreparationTime, ingredients, tags);

            return recipeDto;
        }
    }
}
