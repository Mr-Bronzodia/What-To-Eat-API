using WhatToEat.DTOs;
using WhatToEat.Entities;

namespace WhatToEat.Mappings
{
    public static class IngredientMapping
    {
        public static IngredientCreator ToDto(this Ingridient ingredient)
        {
            return new IngredientCreator(ingredient.Id, ingredient.Name);
        }
    }
}
