using System.Text.Json;

public static class DiffFinderApis
{
    public static void MapDiffFinderApis(this WebApplication app)
    {
        /// <summary>
        /// Stores the left diff data for a given ID
        /// </summary>
        /// <param name="req"> <see cref="HttpRequest" /> object </param>
        /// <param name="id">Id of the diff</param>
        /// <param name="repo">Implementation of <see cref="IRepo"/>/></param>
        /// <returns>
        /// <see cref="Results.Created()"/> if successfull. Otherwise <see cref="Results.BadRequest()"/>
        /// </returns>
        app.MapPut("v1/diff/{id}/left", async (HttpRequest req, string id, IRepo repo) =>
        {
            using var reader = new StreamReader(req.Body);
            var content = await reader.ReadToEndAsync();
            var input = JsonSerializer.Deserialize<Input>(content);

            if (input == null || string.IsNullOrEmpty(input.data))
            {
                return Results.BadRequest();
            }

            repo.Add($"left{id}", input.data);
            return Results.Created();
        })
        .WithName("PutLeft")
        .WithOpenApi();

        /// <summary>
        /// Stores the left diff data for a given ID
        /// </summary>
        /// <param name="req"> <see cref="HttpRequest" /> object </param>
        /// <param name="id">Id of the diff</param>
        /// <param name="repo">Implementation of <see cref="IRepo"/>/></param>
        /// <returns>
        /// <see cref="Results.Created()"/> if successfull. Otherwise <see cref="Results.BadRequest()"/>
        /// </returns>
        app.MapPut("v1/diff/{id}/right", async (HttpRequest req, string id, IRepo repo) =>
        {
            using var reader = new StreamReader(req.Body);
            var content = await reader.ReadToEndAsync();
            var input = JsonSerializer.Deserialize<Input>(content);

            if (input == null || string.IsNullOrEmpty(input.data))
            {
                return Results.BadRequest();
            }

            repo.Add($"right{id}", input.data);
            return Results.Created();
        })
        .WithName("PutRight")
        .WithOpenApi();

        /// <summary>
        /// Gets the diff result for a given ID.
        /// </summary>
        /// <param name="id">ID of the diff</param>
        /// <param name="diffFinder">Implementation of <see cref="IDiffFinder"/></param>
        /// <param name="repo">Implementation of <see cref="IRepo"/></param>
        /// <returns>
        /// <see cref="Results.Created()"/> if successfull. Otherwise <see cref="Results.NotFound()"/>
        /// </returns>
        app.MapGet("v1/diff/{id}", async (string id, IDiffFinder diffFinder, IRepo repo) =>
        {
            var left = repo.Get($"left{id}");
            var right = repo.Get($"right{id}");

            if (left is null || right is null)
            {
                return Results.NotFound();
            }

            var diff = diffFinder.GetDiff(left, right);

            return Results.Ok(diff);
        })
        .WithName("GetDiff")
        .WithOpenApi();
    }
}

public class Input
{
    public string data { get; set; }
}