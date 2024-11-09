// simple types in one line
type Person = {First:string; Last:string}

// complex types in a few lines
// AKA: discriminated union type
type Employee =
  | Worker of Person
  | Manager of Employee list

// type inference
let jdoe = {First="John"; Last="Doe"}
let worker = Worker jdoe


// Co-Pilot example 
let person1 = { First = "John"; Last = "Doe" }
let person2 = { First = "Jane"; Last = "Smith" }

let worker1 = Worker person1
let worker2 = Worker person2

let manager = Manager [worker1; worker2]

let rec printEmployee (emp: Employee) =
    match emp with
    | Worker person ->
        printfn "Worker: %s %s" person.First person.Last
    | Manager employees ->
        printfn "Manager with employees:"
        employees |> List.iter printEmployee

printEmployee manager


// NOTE TO MYSELF
// for real wolrd
// code block should look like this

// access external data/infra
// business logic
// access external data/infra

let sumOfSquare n =
  [1..n] |> List.map (fun num -> num * num) |> List.sum

5 |> sumOfSquare |> printfn "sum of square := %d"

let add x y = x + y

(1, 2) ||> add |> printfn "add result := %d"

let add4nums a b c d = a + b + c + d

(1, 2, 3) |||> add4nums 4 |> printfn "add4nums result = %d"

type Foo(inName: string) =
  // private field
  let name = inName

  // public property
  [<DefaultValue>] val mutable Age : int

  // public function
  member me.increaseAgeByOne = 
    me.Age <- me.Age + 1
    me.printAge

  member me.printAge =
    printfn "%s is now %d years" name me.Age

// extension
type Foo with 
  member me.decreaseAgeByOne =
    me.Age <- me.Age - 1
    me.printAge

// Usage ...

let f1 = new Foo("John")
let f2 = new Foo("Jane")

printfn "Foo1 is %d" f1.Age

f1.Age <- 30

printfn "Foo1 is %d" f1.Age

f1.increaseAgeByOne
f1.decreaseAgeByOne
f1.printAge
f2.increaseAgeByOne