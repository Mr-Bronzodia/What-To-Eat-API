using WhatToEat.Data;
using WhatToEat.Endpoints;
using Microsoft.EntityFrameworkCore.Design;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("Recipes");
string corsPolicy = "AllowSpecificOrigins";

builder.Services.AddSqlServer<RecipeContext>(connectionString);

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy,
        policy =>
        {
            policy.WithOrigins("http://whattoeat.infinityfreeapp.com")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddControllers();

WebApplication app = builder.Build();

app.UseCors(corsPolicy); 
app.UseAuthorization();
app.MapControllers(); 


app.MapRecipeEndpoints();
app.MapIngredientEndpoints();
app.MapTagsEndpoints();

app.Run();
