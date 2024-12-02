using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

var sessionCookie = Environment.GetEnvironmentVariable("AOC_SESSION");
if (string.IsNullOrEmpty(sessionCookie))
{
    Console.WriteLine("Enter session cookie:");
    sessionCookie = Console.ReadLine();
}

if (string.IsNullOrEmpty(sessionCookie))
{
    Console.Error.WriteLine("Session cookie is required");
    return 1;
}

using var client = new HttpClient();

var request = new HttpRequestMessage(HttpMethod.Get, "https://adventofcode.com/2024/day/1/input");
request.Headers.Add("Cookie", $"session={sessionCookie}");

var response = await client.SendAsync(request);
if (!response.IsSuccessStatusCode)
{
    Console.Error.WriteLine("Failed to download input");
    return 1;
}

var input = await response.Content.ReadAsStringAsync();

using var reader = new StringReader(input);
using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
{
    Delimiter = "   ",
    HasHeaderRecord = false
});

var records = csv.GetRecords<Input>().ToList();

var firstColumn = records.Select(r => r.A).Order().ToList();
var secondColumn = records.Select(r => r.B).Order().ToList();

var result = firstColumn.Zip(secondColumn, (a, b) => Math.Abs(a - b)).Sum();

Console.WriteLine(result);
return 0;

internal record struct Input(int A, int B);