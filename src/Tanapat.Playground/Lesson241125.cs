
using LanguageExt.Common;

namespace Tanapat.Playground.Lesson241125;

// learn from this
// https://github.com/louthy/language-ext/wiki/How-to-deal-with-side-effects

public delegate Either<Error, A> IO<A>();
  
public static class Xxx
{
    public static void Run()
    {
        var inpath = "source.txt";
        var outpath = "dest.txt";

        var computation1 = 
            from text in ReadAllText(inpath)
            // this will not be wrapped
            let uppr = text.ToUpper()
            from _    in WriteAllText(outpath, uppr)
            select unit;

        // Run will put all the thing in side try/catch block
        computation1.Run().Match(
            s => Console.WriteLine("Success"),
            e => Console.WriteLine("Error = " + e.Message)
        );

        var computation2 = 
            from text in ReadAllText(inpath)
            // this will be wrapped and lift UPPER case text into IO
            from uppr in IO.Pure(text.ToUpper()) 
            from _    in WriteAllText(outpath, uppr)
            select unit;

        // Run will put all the thing in side try/catch block
        computation2.Run().Match(
            s => Console.WriteLine("Success"),
            e => Console.WriteLine("Error = " + e.Message)
        );
    }

    static IO<string> ReadAllText(string path) =>
        () => File.ReadAllText(path);

    static IO<Unit> WriteAllText(string path, string text) =>
        () => { File.WriteAllText(path, text); return unit; };
}

public static class IO
{
    // Allows us to lift pure values into the IO domain
    public static IO<A> Pure<A>(A value) => 
        () => value;

    // Wrap up the error handling
    public static Either<Error, A> Run<A>(this IO<A> ma)
    {
        try
        {
            return ma();
        }
        catch(Exception e)
        {
            return Error.New("IO error", e);
        }
    }

    // Functor map
    public static IO<B> Select<A, B>(this IO<A> ma, Func<A, B> f) => () =>
        ma().Match(
            Right: x => f(x),
            Left:  Left<Error, B>);

    // Functor map
    public static IO<B> Map<A, B>(this IO<A> ma, Func<A, B> f) => 
        ma.Select(f);

    // Monadic bind
    public static IO<B> SelectMany<A, B>(this IO<A> ma, Func<A, IO<B>> f) => () =>
        ma().Match(
            Right: x => f(x)(),
            Left:  Left<Error, B>);

    // Monadic bind
    public static IO<B> Bind<A, B>(this IO<A> ma, Func<A, IO<B>> f) =>
        SelectMany(ma, f);

    // Monadic bind + projection
    public static IO<C> SelectMany<A, B, C>(
        this IO<A> ma, 
        Func<A, IO<B>> bind, 
        Func<A, B, C> project) => 
        ma.SelectMany(a => bind(a).Select(b => project(a, b)));    
}