using System.ComponentModel.DataAnnotations.Schema;

namespace WhatToEat.Entities
{
    public class Ingridient
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Recipe> Recipes { get; set; }
    }
}
