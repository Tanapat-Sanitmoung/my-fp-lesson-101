namespace Tanapat.Playground.Lesson241121;

using Microsoft.VisualBasic;
using static Console;

public static class Practices
{

    public static async Task Take01()
    {
        var func1 = async () => await Task.FromResult(1);
        var func2 = async () => await Task.FromResult(2);
        var func3 = async () => await Task.FromResult(3);

        var result =  from a in func1()
            from b in func2()
            from c in func3()
            select a + b + c;

        WriteLine($"Result is {await result}");
    }

    public static async Task Take02()
    {
        var func1 = async () => await Task.FromResult(1);
        var func2 = async () => await Task.FromResult(2);
        var func3 = async () => await Task.FromResult(3);

        var result = await (from a in func1()
            from b in func2()
            from c in func3()
            select a + b + c);

        WriteLine($"Result is {result}");
    }

    public static async Task Take03()
    {
        var func1 = async () => {  WriteLine("Task 1 ..."); await Task.Delay(2_000); return 1; };
        var func2 = () => { WriteLine("Task 2 ..."); return Task.FromResult(2); };
        var func3 = () => { WriteLine("Task 3 ..."); return Task.FromResult(3); };

        var task = from a in func1()
            from b in func2()
            from c in func3()
            select a + b + c;

        var result = await task;


        WriteLine($"Result is {result}");

    }

    public static async Task Take04()
    {
        var func1 = async () => {  WriteLine("Task 1 ..."); await Task.Delay(2_000); return 1; };
        var func2 = () => { WriteLine("Task 2 ..."); throw new Exception("Task 2 error"); return Task.FromResult(2);};
        var func3 = () => { WriteLine("Task 3 ..."); return Task.FromResult(3); };

        try
        {
            var task = from a in func1()
                from b in func2()
                from c in func3()
                select a + b + c;

            var result = await task;

            WriteLine($"Result is {result}");
        }
        catch (Exception ex)
        {
            WriteLine($"Error := {ex.Message}");
        }

        //output :
        /*
        Task 1 ...
        Task 2 ...
        Exception thrown: 'System.Exception' in Tanapat.Playground.dll
        Exception thrown: 'System.Exception' in System.Private.CoreLib.dll
        Error := Task 2 error
        */

    }


    public static async Task Take05()
    {

        try
        {

            var func1 = async () => {  WriteLine("Task 1 ..."); await Task.Delay(2_000); return 1; };
            var func2 = () => { WriteLine("Task 2 ..."); throw new Exception("Task 2 error"); return Task.FromResult(2);};
            var func3 = () => { WriteLine("Task 3 ..."); return Task.FromResult(3); };
            
            var task = from a in func1()
                from b in func2()
                from c in func3()
                select a + b + c;

            var result = await task;

            WriteLine($"Result is {result}");
        }
        catch (Exception ex)
        {
            WriteLine($"Error := {ex.Message}");
        }

        //output :
        /*
        Task 1 ...
        Task 2 ...
        Exception thrown: 'System.Exception' in Tanapat.Playground.dll
        Exception thrown: 'System.Exception' in System.Private.CoreLib.dll
        Error := Task 2 error
        */

    }

    public static void Take06()
    {
        var num1 = Some(10);
        var num2 = Option<int>.None;
        var num3 = Some(10);

        var result = 
            from a in num1
            from b in num2
            from c in num3
            select a + b + c;
        // use pattern matching instead of Match
        var some = result.Case switch 
        {
            int n => n,
            _ => 0
        };
    }

    public static void Take07()
    {
        // Map: (A<T>, F<T, R>) -> A<R>
        ISet<int> a = new ISet<int>(10);
        ISet<string> b = a.Map(i => i.ToString());
    }

    public static void Take08()
    {
        var people = new Dictionary<string, Employee>();

        var wp = GetWorkPermit(people, "12345");

        var avg = AverageYearsWorkedAtTheCompany(people.Values.ToList());

    }

    
    static Option<Employee> Lookup(Dictionary<string, Employee> people, string employeeId)
        => people.TryGetValue(employeeId, out var emp) ? emp : None;

    static bool IsExpired(WorkPermit workPermit) => workPermit.Expiry < DateTime.Now;

    static bool Not(bool value) => !value;

    static bool NotExpired(WorkPermit workPermit) => Not(IsExpired(workPermit));

    static  Option<WorkPermit> GetWorkPermit(Dictionary<string, Employee> people, string employeeId) 
                // Lookup: (Diction<string, Employee>, string) -> Option<Employee>
             => Lookup(people, employeeId) 
                // Bind: (Option<Employee>, (Employee -> Option<WorkPermit>)) -> Option<WorkPermit>
                .Bind(a => a.WorkPermit);

    static Option<WorkPermit> GetValidWorkPermit(Dictionary<string, Employee> people, string employeeId)
        => GetWorkPermit(people, employeeId)
            .Where(NotExpired);

    static double YearsBetween(DateTime start, DateTime end)
         => start.Subtract(end).Duration().TotalDays / 365d;

    static double AverageYearsWorkedAtTheCompany(List<Employee> employees)
          => employees.Bind(
                            //Map (Option<DateTime>, ((DateTime, DateTime) -> double)) -> Option<double>
                e => e.LeftOn.Map(leftOn => YearsBetween(leftOn, e.JoinedOn)))
            .Average();
}

public record struct ISet<T>
{
    public T Value { get; }

    public ISet(T value) => Value = value;

    public ISet<R> Map<R>(Func<T, R> func)
        => new ISet<R>(func(Value));

}

public struct Employee
{
    public string Id { get; set; }
    public Option<WorkPermit> WorkPermit { get; set; }
    public DateTime JoinedOn { get; }
    public Option<DateTime> LeftOn { get; }
}

public struct WorkPermit
{
    public string Number { get; set; }
    public DateTime Expiry { get; set; }
}