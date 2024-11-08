module HelloSquare

open System.Text

type Block = { Id: string; Value: string }

module CRC = 
    let table = lazy (
        let output = Array.zeroCreate<uint16> 256
        let poly = 0x1021us
        for i in 0 .. output.Length - 1 do
            let mutable temp = 0us
            let mutable a = uint16 (i <<< 8)
            for _ in 0 .. 7 do
                if (temp ^^^ a) &&& 0x8000us <> 0us then
                    temp <- (temp <<< 1) ^^^ poly
                else
                    temp <- temp <<< 1
                a <- a <<< 1
            output[i] <- temp
        output 
    )

    let compute (data:string) :string=
        let mutable crc = 0xffffus
        let removedCrcValue = data.Substring(0, data.Length - 4)
        let bytes  = Encoding.UTF8.GetBytes(s=removedCrcValue)
        for b in bytes do
            crc <- (crc <<< 8) ^^^ table.Value[int (crc >>> 8) ^^^ int b &&& 0xff]
        crc.ToString("X4")

//rec = recursive
// :: operator is concat list
let rec readBlocks (input: string) (index: int) (accumulator: Block list) = 
    if index < input.Length then
        let id = input.Substring(index, 2)
        let len = input.Substring(index + 2, 2) |> int
        let value = input.Substring(index + 4, len)
        let index = index + 4 + len
        readBlocks input index ({ Id = id; Value = value } :: accumulator)
    else
        List.rev accumulator

let printOutput (content:string) :string =
    content
    |> printfn "%s"
    content

open System.IO

let writeOutput content =
    task {
        use file = File.Create("test.log")
        let data = System.Text.Encoding.UTF8.GetBytes(s=content)
        do! file.WriteAsync data
    }

let buildOutput blocks =
    let sb = new StringBuilder()
    blocks 
    |> List.iter (
        fun b -> 
            sb.AppendLine($"ID: {b.Id}, Value: {b.Value}") 
            |> ignore
        )
    sb.ToString()

[<EntryPoint>]
let main argv =

    let qr = "0002020102113010123456789A5303718540310063041234"

    qr
    |> CRC.compute 
    |> printfn "calculate CRC = %s"

    readBlocks qr 0 []
    |> buildOutput 
    |> printOutput
    |> writeOutput 
    |> Async.AwaitTask
    |> Async.RunSynchronously

    0 // Return an integer exit code
