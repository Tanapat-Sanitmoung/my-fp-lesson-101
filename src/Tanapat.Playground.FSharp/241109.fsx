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

type Faa() =
  let mutable num = 0

  member me.count = 
    num <- num + 1 
    printfn "Count :%d" num

let fa1 = Faa()
fa1.count

let fa2 = Faa()
fa2.count
fa2.count

let fa3 = new Faa()
fa3.count

type MyType(?x: int, ?y: int) =
    member val X = defaultArg x 0 with get, set
    member val Y = defaultArg y 0 with get, set

let instance1 = MyType()        // Calls constructor with no arguments
let instance2 = new MyType(5)   // Calls constructor with one argument
let instance3 = new MyType(5, 10) // Calls constructor with two arguments
let instance4 = MyType(5)
let instance5 = MyType(5, 10)

printfn "Instance1: X=%d, Y=%d" instance1.X instance1.Y
printfn "Instance2: X=%d, Y=%d" instance2.X instance2.Y
printfn "Instance3: X=%d, Y=%d" instance3.X instance3.Y
printfn "Instance4: X=%d, Y=%d" instance4.X instance4.Y
printfn "Instance5: X=%d, Y=%d" instance5.X instance5.Y

type MyClass (v: int) =
  // Default constructor
  new() = 
    MyClass(0)

  member this.Value = v

let my1 = MyClass()       // Calls default constructor
let my2 = new MyClass(5)  // Calls constructor with integer parameter

printfn "Instance1 Value: %d" my1.Value
printfn "Instance2 Value: %d" my2.Value


type MyClassV2() =
  // Private mutable field to hold the value
  let mutable value = 0

  // Secondary constructor with an integer parameter
  new(v: int) as me =
      MyClassV2()
      then
        me.SetValue v
        printfn "Constructing with value: %d" v
  member private me.SetValue v=
    value <- v

  member me.Value = value

let mv21 = MyClassV2()
let mv22 = MyClassV2(2)

printfn "Instance1 Value: %d" mv21.Value
printfn "Instance2 Value: %d" mv22.Value