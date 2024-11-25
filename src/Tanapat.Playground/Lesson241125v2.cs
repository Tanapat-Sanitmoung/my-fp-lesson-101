
using LanguageExt.Common;

namespace Tanapat.Playground.Lesson241125v2;

// learn from this
// https://github.com/louthy/language-ext/wiki/How-to-deal-with-side-effects

public delegate Either<Error, A> IO<Env, A>(Env env);
  
public static class Xxx
{
    public static void Run()
    {
        var inpath = "source.txt";
        var outpath = "dest.txt";

        var computation = 
            from text in ReadAllText<LiveFile>(inpath)
            from _    in WriteAllText<LiveFile>(outpath, text)
            select unit;

        var env = new LiveFile();
        computation.Run(env);
    }

    static IO<Env, string> ReadAllText<Env>(string fileName)
        where Env: FileIO
            => env => env.ReadAllText(fileName);
    
    static IO<Env, Unit> WriteAllText<Env>(string fileName, string content)
        where Env: FileIO
            => env => env.WriteAllText(fileName, content);
}

public static class IO
{
    public static IO<Env, A> Pure<Env, A>(A value) => (Env env) => value;

    public static Either<Error, A> Run<Env, A>(this IO<Env, A> ma, Env env)
    {
        try
        {
            return ma(env);
        }
        catch (Exception ex)
        {
            return Error.New("Run failed", ex);
        }
    }

    public static IO<Env, B> Select<Env, A, B>(this IO<Env, A> ma, Func<A, B> f)
        => env => ma(env).Match(
            Right: x => f(x),
            Left: Left<Error, B>);

    public static IO<Env, B> Map<Env, A, B>(this IO<Env,A> ma, Func<A, B> f)
        => Select(ma, f);

    // Keep in mind the IO<Env, A> is delegate, NOT a class

    public static IO<Env, B> SelectMany<Env, A, B>(this IO<Env, A> ma, Func<A, IO<Env, B>> f)
        => env => ma(env).Match(
            Right: x => f(x)(env),  // How can someone think like this, this is God plan for sure.
            Left: Left<Error, B>);

    public static IO<Env, B> Bind<Env, A, B>(this IO<Env, A> ma, Func<A, IO<Env, B>> f)
        => SelectMany(ma, f);

    public static IO<Env, C> SelectMany<Env, A, B, C>(this IO<Env, A> ma, 
        Func<A, IO<Env, B>> bind,
        Func<A, B, C> project) 
        => ma.SelectMany(a => bind(a).Select(b => project(a, b))); // again, How can someone can found this !!!
}

public interface FileIO
{
    string ReadAllText(string fileName);

    Unit WriteAllText(string fileName, string content);
}

public class LiveFile : FileIO
{
    public string ReadAllText(string fileName)
        => File.ReadAllText(fileName);

    public Unit WriteAllText(string fileName, string content)
    {
        File.WriteAllText(fileName, content);
        return unit;
    }
}