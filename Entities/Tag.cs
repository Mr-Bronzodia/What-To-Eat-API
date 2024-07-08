using System.ComponentModel.DataAnnotations.Schema;

namespace WhatToEat.Entities
{
    public class Tag
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Text { get; set; }
        public ICollection<Recipe> Recipes { get; set; }
    }
}
