using Json;
using UserApi.Controllers;
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();


        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        RunJson json = new RunJson();
        UserController u = new UserController();
        
        string j = json.ReadUserFile();
        u.DeserializeFile(j);
        
        app.Run();

        j = u.SerializeFile();
        json.WriteUserFile(j);
    }
}