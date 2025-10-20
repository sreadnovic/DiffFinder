public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        // AddScoped used for thread safety
        builder.Services.AddSingleton<Storage>();
        builder.Services.AddScoped<IRepo, Repo>();
        builder.Services.AddScoped<IDiffFinder, DiffFinder>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.MapDiffFinderApis();

        app.Run();
    }
}

