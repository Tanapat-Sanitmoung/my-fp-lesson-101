open System

printf "What is you name:"

let add x y = x + y

// Anonymous type
let db =
    {| Student = [
        {| Id=1; Name="Tanapat"|}
        {| Id=2; Name="Lawansiri"|}
    ]|}

let z = 
    query {
        for student in db.Student do
        where (student.Id = 1)
        select student
    }

let add42 x = x + 42

let double x = x * 2

let sqaure x = x * x

let mix1 x= 
    add42(double(sqaure(x)))

let mix2 x = 
    x 
    |> sqaure 
    |> double 
    |> add42

add 10 20
    |> sprintf "result=%i"

let doSomething formatter x = 
    let y = formatter(x + 1)
    "hello " + y

[<Struct>]
type Thing = { Id:int; Name:string }

type Thang = { Id:int; Name:string }


let a = { Id=1; Name="Tanapat" }

let b : Thing = { Id=2; Name="Tanaput" }

let c = { b with Id = 3 }

type PrimaryColor = Red | Blue | Green

type RGB = { R: int; G: int; B: int; }

type Color = {
    PrimaryColor : PrimaryColor
    RGB : RGB
    Named : string
}