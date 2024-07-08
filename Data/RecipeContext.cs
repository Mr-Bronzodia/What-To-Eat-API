using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection.Metadata;
using WhatToEat.Entities;

namespace WhatToEat.Data
{
    public class RecipeContext(DbContextOptions<RecipeContext> options) : DbContext(options)
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Ingridient> Ingridients { get; set; }
        public DbSet<RecipeIngridient> RecipeIngridients { get; set; }
        public DbSet<RecipeTag> RecipeTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Recipe>().HasMany(r => r.Ingridients).WithMany(i => i.Recipes).UsingEntity<RecipeIngridient>();
            modelBuilder.Entity<Recipe>().HasMany(r => r.Tags).WithMany(t => t.Recipes).UsingEntity<RecipeTag>();


        }
    }
}
