
namespace Tanapat.Playground.Lesson241113;

public class Usages
{

    // TIn -> TOut
    public static TOut
        Map<TIn, TOut>(
            TIn @input, 
            Func<TIn, TOut> fn) => fn(@input);

    public void UseCase1()
    {
       Map(
        @input: new { a = 1, b = 2 }, 
        fn: (s) => s.a + s.b);
    }

    public static void UseCase2()
    {
        var result = 
            new int[]{ 1, 2, 3, 4, 5 }
                .MyWhere(num => num % 2 == 1)
                .ToList();
    }

    public static void UseCase3()
    {
        var result = GetOrCreate("id1", () => 1);
    }

    public static TIn Store<TKey, TIn>(TKey key, TIn value) => value;

    public static TOut? Get<TKey, TOut>(TKey key) => default;

    public static TOut GetOrCreate<TKey, TOut>(TKey key, Func<TOut> createFn)
        => Store(key, value: Get<TKey, TOut>(key) ?? createFn());

    public static void UseCase4()
    {
        Func<int, bool> CanDividedBy(int former) => (int latter) => latter % former == 0; 
        Func<int, int>  MultiplyBy(int a) => (int b) => a * b;

        var result = 
            new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }
                .Where(CanDividedBy(2))
                .Select(MultiplyBy(5));
    }

    public static void  UseCase5()
    {
        // Design
        /*

        var result = SetupTearDown(
            connectionString: "xxx", 
            execute: () => "Select * from xxx where a = @a", 
            @params: new { a = "123" });


        */
    }
}

public static class Extensions
{
    public static IEnumerable<TIn> MyWhere<TIn>(this IEnumerable<TIn> inputs, Func<TIn, bool> predicate)
        {
            foreach (var item in inputs)
                if (predicate(item))
                    yield return item;
        }

}