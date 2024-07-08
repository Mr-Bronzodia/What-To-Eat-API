using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatToEat.Entities
{
    public class Recipe
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int PreparationTime { get; set; }
        public ICollection<Ingridient> Ingridients { get; set; }
        public ICollection<Tag> Tags { get; set; }
    }
}
