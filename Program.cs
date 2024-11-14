using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Endpoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<GameStoreContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("GameStore")));


var app = builder.Build();

app.MapGamesEndpoints();

app.Run();
