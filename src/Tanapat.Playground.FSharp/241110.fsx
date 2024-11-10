
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