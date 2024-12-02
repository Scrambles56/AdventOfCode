
open System
open System.Collections.Generic
open System.Globalization
open System.IO
open System.Net.Http
open CsvHelper
open CsvHelper.Configuration
open FSharp.Control


type Input = { 
    A: int
    B: int
}

let envSession = Environment.GetEnvironmentVariable "AOC_SESSION"

let session = 
    if String.IsNullOrWhiteSpace envSession then
        Console.WriteLine "Please set the AOC_SESSION environment variable"
        Console.ReadLine()
    else
        envSession

let url = "https://adventofcode.com/2024/day/1/input"
let req = new HttpRequestMessage(HttpMethod.Get, url)
req.Headers.Add("Cookie", $"session={session}")

let client = new HttpClient()
let content =
    async {
        let! response = client.SendAsync(req) |> Async.AwaitTask
        let! awaitTask = response.Content.ReadAsStringAsync() |> Async.AwaitTask
        
        return awaitTask
    } |> Async.RunSynchronously
    
let readInputs =
    taskSeq {        
        let configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
        configuration.HasHeaderRecord <- false
        configuration.Delimiter <- "   "
        
        let reader = new CsvReader(new StringReader(content), configuration)
        let records = reader.GetRecords<Input>()
        
        yield! records
    } |> TaskSeq.toSeq

let colA = readInputs |> Seq.map _.A |> Seq.sort
let colB = readInputs |> Seq.map _.B

let dict = new Dictionary<int, int>()
colB
    |> Seq.groupBy id
    |> Seq.map (fun (k, v) -> k, Seq.length v)
    |> Seq.iter (fun (k, v) -> dict.Add(k, v))
    
    
let result =
    colA
        |> Seq.map (fun a ->
                let b = dict.GetValueOrDefault(a, 0)
                if b > 0 then
                    a * b
                else
                    b
            )
        |> Seq.sum

Console.WriteLine result
        
        



    
