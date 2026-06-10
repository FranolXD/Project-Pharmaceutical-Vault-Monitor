using System.Net.Sockets;
using System.Text;

string host = "127.0.0.1";
int serialPort = 4000;

string lastLine = "{\"temperature\":3.00,\"distanceCm\":5.00}";
// Domyślny payload testowy.
// Gdy działa połączenie Wokwi -> Bridge, ta wartość jest nadpisywana danymi z Serial.
DateTime lastUpdateUtc = DateTime.MinValue;
string lastError = "";

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://127.0.0.1:5080");

var app = builder.Build();

_ = Task.Run(async () =>
{
    while (true)
    {
        try
        {
            using TcpClient client = new TcpClient();
            await client.ConnectAsync(host, serialPort);

            using NetworkStream stream = client.GetStream();
            using StreamReader reader = new StreamReader(stream, Encoding.UTF8);

            while (true)
            {
                string? line = await reader.ReadLineAsync();

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (line.StartsWith("{") && line.EndsWith("}"))
                {
                    lastLine = line;
                    lastUpdateUtc = DateTime.UtcNow;
                    lastError = "";
                }
                else
                {
                    lastError = line;
                }
            }
        }
        catch (Exception ex)
        {
            lastError = ex.Message;
            await Task.Delay(1000);
        }
    }
});

app.MapGet("/health", () =>
{
    return Results.Json(new
    {
        status = string.IsNullOrWhiteSpace(lastLine) ? "waiting" : "ok",
        lastUpdateUtc,
        lastError
    });
});

app.MapGet("/read", () =>
{
    if (string.IsNullOrWhiteSpace(lastLine))
    {
        return Results.Problem(
            title: "No data",
            detail: lastError,
            statusCode: 503
        );
    }

    return Results.Content(lastLine, "application/json");
});

app.Run();