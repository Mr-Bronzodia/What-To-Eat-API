using System.ComponentModel.DataAnnotations.Schema;

namespace WhatToEat.Entities
{
    public class RecipeIngridient
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required Ingridient Ingridient { get; set; }
        public required Recipe Recipe {  get; set; }
    }
}
