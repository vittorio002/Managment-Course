using Json;
using LaboratoryApi.Controllers;
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
        LabController l = new LabController();

        string j = json.ReadLabFile();
        l.DeserializeFile(j);
        
        app.Run();

        j = l.SerializeFile();
        json.WriteLabFile(j);
    }
}