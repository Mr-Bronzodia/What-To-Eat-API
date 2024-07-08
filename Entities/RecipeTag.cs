using System.ComponentModel.DataAnnotations.Schema;

namespace WhatToEat.Entities
{
    public class RecipeTag
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required Tag Tag { get; set; }
        public required Recipe  Recipe { get; set; }
    }
}
