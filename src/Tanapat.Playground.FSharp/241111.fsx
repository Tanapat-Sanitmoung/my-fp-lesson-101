let days : string array =
    [|"MON"; "TUE"; "WED"; "THU"; "FRI"; "SAT"; "SUN"|]

let (|BetweenInclusive|_|) (low:int) (high:int) (x:int) :option<unit>=
    if x >= low && x <= high then Some() else None


let lookupDay (num: int) :string =
    match num with
        | BetweenInclusive 1 7-> days[num - 1]
        | _ -> invalidArg (nameof num) (sprintf "Value passed in was %d." num)

let (|BetweenInclusiveV2|_|) (low:int) (high:int) (x:int) :option<int>=
    if x >= low && x <= high then Some(x) else None

// in patterh match combine with Active pattern
// we supply "num" in match
// compiler will use "num" as last parameter of BetweenInclusiveV2
// we need to partially apply 2 parameters to create partial function of BetweenInclusiveV2
// BetweenInclusiveV2 x y z: val int -> (int -> int) -> (int -> in) -> option<int>
// BetweenInclusiveV2 x y : val int -> option<int>
// so, 1 will apply to low, 7 will apply to high, num will apply to x
// then z will use to bind result of BetweenInclusiveV2 which is int in option<int>
let lookupDayV2 (num: int) :string =
    match num with 
        | BetweenInclusiveV2 1 7 z-> days[z - 1] // type of z is 'a depend on :option<'a>
        | _ -> invalidArg (nameof num) (sprintf "Value passed in was %d." num)

lookupDay 1 |> printfn "%s"
lookupDay 2 |> printfn "%s"

// this try block will return string after return type of lookupDay
try
    lookupDay 10
with
    // this line will be forced to return string after to lookupDay as well
    | ex -> sprintf "%s" ex.Message
|> printfn "%s"

// Note to myself:
// sprintf is used to format string and return value without display in console
