namespace WhatToEat.DTOs
{
    public record class RecipeCreator(int id, string name, string descrition, string imageUrl, int prepTime, List<string> ingridients, List<string> tags);
}
