using TradeStreamCommonData.DatabaseConnector;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddXmlDataContractSerializerFormatters();
builder.Services.AddMemoryCache();

// Trying the database connection
DatabaseConnector.GetInstance();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllerRoute(
name: "/api/",
pattern: "{symbol}/SimpleMovingAverage/{numberOfDataPoints}/{timePeriod}/{startDateTime?}"
);

app.MapControllers();

app.Run();
