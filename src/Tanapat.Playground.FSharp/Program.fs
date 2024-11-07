module HelloSquare

type Block = { Id: string; Value: string }

let rec readBlock (input: string) (index: int) (accumulator: Block list) = 
    if index < input.Length then
        let id = input.Substring(index, 2)
        let len = input.Substring(index + 2, 2) |> int
        let value = input.Substring(index + 4, len)
        let index = index + 4 + len
        readBlock input index ({ Id = id; Value = value } :: accumulator)
    else
        List.rev accumulator

[<EntryPoint>]
let main argv =
    let qr = "0002020102113010123456789A5303718540310063041234"
    let blocks = readBlock qr 0 []
    blocks |> List.iter (fun b -> printfn "ID: %s, Value: %s" b.Id b.Value)
    0 // Return an integer exit code
