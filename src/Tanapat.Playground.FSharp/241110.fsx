
open System.Net.Http
open System

let fetch (url: string) =
    async {
        let http = new HttpClient()
        http.Timeout <- TimeSpan.FromSeconds(5)
        printfn "Fetch url := %s" url
        try
            let response = 
                http.GetStringAsync(url) 
                |> Async.AwaitTask
                |> Async.RunSynchronously

            return response
        with 
            ex -> return ex.Message
    }

printfn "Start .."

let url = "https://jsonplaceholder.typicode.com/posts/1" 

// Run the async fetch function and wait for the result
fetch url 
    |> Async.RunSynchronously
    |> printfn "Content : %s"

printfn "Ending..."

type FamilyDetails = {SinceYear:int}
type Person = { Name: string; Age: int}
type Family = { Name: string; Members: Person list; Details:FamilyDetails}

[
        {Name="Tanapat"; Age=30;};
        {Name="Van"; Age=33;};
        {Name="Joe"; Age=100_000};
        {Name="Tanapat"; Age=33};
]
    |> List.distinctBy _.Name
    |> List.sumBy _.Age
    |> printfn "Result is %d"

let fam = { 
    Name="Black"; 
    Members=[
        {Name="Jane"; Age=30;};
        {Name="Joe"; Age=33;};
    ];
    Details={SinceYear=2001}}

let copyFam = 
    {
        fam with Details.SinceYear = 2010     
    }

printfn "fam = %A" fam
printfn "copy fam = %A" copyFam

let days includeWeekend = 
    seq {
        "MON"
        "TUE"
        "WED"
        "THU"
        "FRI"
        if includeWeekend then
            "SAT"
            "SUN"
    }

days true
|> Seq.iter (fun s -> printfn "%s" s)

let classifyAge a =
    match a with
    | con1 when con1 < 18 -> "Not adult"
    | _ -> "Adult"

let age = 15
age
|> classifyAge
|> printfn "%s"

let (|Even|Odd|) x = if x % 2 = 0 then Even else Odd

let TestNumber input =
   match input with
   | Even -> printfn "%d is even" input
   | Odd -> printfn "%d is odd" input

TestNumber 7
TestNumber 11
TestNumber 32

let isAdult a = a > 17

let (|Adult|_|) x 
    = if isAdult x then Some(Adult) else None

let testRate x =
    match x with
    | Adult a ->  printfn "%d Adult rating" x
    | _ -> printfn "%d Not adult" x

[10;18;30;12] |> List.map testRate

// this amaze me : https://learn.microsoft.com/en-us/dotnet/fsharp/language-reference/active-patterns

