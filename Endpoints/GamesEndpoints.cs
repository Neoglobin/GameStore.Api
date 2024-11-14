namespace GameStore.Api.Endpoints
{
    using GameStore.Api.Dtos;

    public static class GamesEndpoints
    {
        const string GetGameEndpointName = "GetGame";

        private static readonly List<GameDto> games = [
            new(
        1,
        "StreetFighter",
        "Fighting",
        19.99M,
        new DateOnly(1992, 7, 15)),
        new(
        2,
        "FinalFantasy",
        "RPG",
        59.99M,
        new DateOnly(2010, 9, 30)),
        new(
        3,
        "FIFA 23",
        "Sports",
        42.15M,
        new DateOnly(2007, 11, 29)),
        ];

        public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("games").WithParameterValidation();

            // GET games
            group.MapGet("/", () => games);

            // GET games/1
            group.MapGet("/{id}", (int id) =>
            {
                GameDto? game = games.Find(game => game.Id == id);

                return game is null ? Results.NotFound() : Results.Ok(game);
            })
                .WithName(GetGameEndpointName);

            // POST /games
            group.MapPost("/", (CreateGameDto newGame) =>
            {
                if (string.IsNullOrEmpty(newGame.Name))
                {
                    return Results.BadRequest("Name is required");
                }

                GameDto game = new(
                        games.Count + 1,
                        newGame.Name,
                        newGame.Genre,
                        newGame.Price,
                        newGame.ReleaseDate
                        );

                games.Add(game);

                return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);

            }).WithParameterValidation();


            // PUT /games/1

            group.MapPut("/{id}", (int id, UpdateGameDto updateGame) =>
            {
                var index = games.FindIndex(game => game.Id == id);

                if (index == -1)
                {
                    return Results.NotFound();
                }

                games[index] = new GameDto(
                    id,
                    updateGame.Name,
                    updateGame.Genre,
                    updateGame.Price,
                    updateGame.ReleaseDate
                        );

                return Results.NoContent();

            });

            // Delete /games/1

            group.MapDelete("/{id}", (int id) =>
            {
                games.RemoveAll(game => game.Id == id);

                return Results.NoContent();

            });

            return group;
        }
    }

}
