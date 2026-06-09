using System.Text.Json;

HttpClient http = new HttpClient();

string url = "http://127.0.0.1:5080/read";

const double MIN_TEMP = 2.0;
const double MAX_TEMP = 8.0;
const double DOOR_OPEN_LIMIT_CM = 50.0;

while (true)
{
    try
    {
        string json = await http.GetStringAsync(url);

        using JsonDocument doc = JsonDocument.Parse(json);
        JsonElement root = doc.RootElement;

        double temperature = root.GetProperty("temperature").GetDouble();
        double distanceCm = root.GetProperty("distanceCm").GetDouble();

        bool tempProblem = temperature < MIN_TEMP || temperature > MAX_TEMP;
        bool doorProblem = distanceCm > DOOR_OPEN_LIMIT_CM;

        Console.Clear();
        Console.WriteLine("=== Pharmaceutical Vault Monitor ===");
        Console.WriteLine($"Temperatura: {temperature:F2} C");
        Console.WriteLine($"Dystans drzwi: {distanceCm:F2} cm");
        Console.WriteLine();

        if (tempProblem && doorProblem)
        {
            Console.WriteLine("CRITICAL INCIDENT");
            Console.WriteLine("Temperatura poza normą i drzwi otwarte.");
        }
        else if (tempProblem || doorProblem)
        {
            Console.WriteLine("SECURITY EVENT");

            if (tempProblem)
                Console.WriteLine("Temperatura poza bezpiecznym zakresem <2°C, 8°C>.");

            if (doorProblem)
                Console.WriteLine("Drzwi są otwarte lub uchylone.");
        }
        else
        {
            Console.WriteLine("OK");
            Console.WriteLine("Warunki w normie.");
        }
    }
    catch (Exception ex)
    {
        Console.Clear();
        Console.WriteLine("Błąd klienta.");
        Console.WriteLine(ex.Message);
    }

    await Task.Delay(1000);
}