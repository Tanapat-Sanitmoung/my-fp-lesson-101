module HelloSquare

open System.Text

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

let computeCrc (data:string) :string=
    let mutable crc = 0xffffus
    let removedCrcValue = data.Substring(0, data.Length - 4)
    let bytes  = Encoding.UTF8.GetBytes(s=removedCrcValue)
    for b in bytes do
        crc <- (crc <<< 8) ^^^ CRC.table.Value[int (crc >>> 8) ^^^ int b &&& 0xff]
    crc.ToString("X4")

let printBlocks (blocks: Block list) :unit =
    blocks
    |> List.iter (fun b -> printfn "id: %s, value: %s" b.Id b.Value)

[<EntryPoint>]
let main argv =

    let qr = "0002020102113010123456789A5303718540310063041234"

    computeCrc qr
    |> printfn "CRC = %s"

    readBlock qr 0 []
    |> printBlocks

    0 // Return an integer exit code
