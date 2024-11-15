namespace Tanapat.Playground.Lesson241115;

public class FakeDbConnection : IDisposable
{
    public FakeDbConnection(string connectionString)
    {
        
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public T[] Select<T>(string query) => new T[]{};
    public Task<T[]> SelectAsync<T>(string query) => Task.FromResult(new T[]{});
}

public static class Usage
{
    public static async void Execute()
    {
        // More declarative
        var result3 = await GetConfigsAsync(["name", "endpoint", "key"]);
    }

    public record Config(string Name, string Value);

    public static R Using<TDisp, R>(TDisp resource, Func<TDisp, R> f) where TDisp : IDisposable
    {
        using (resource) return f(resource);
    }

    public static R Connect<R>(string connectionString, Func<FakeDbConnection, R> f)
        => Using(new FakeDbConnection(connectionString), f);

    public static Task<Config[]> GetConfigsAsync(string[] names)
        => ConnectMyDb(
            db => db.SelectAsync<Config>(
                query: BuildGetConfigQuery(names)));

    public static string BuildGetConfigQuery(string[] names)
        => $"Select * from table1 where name in ({string.Join(",", names)})";

    public static R ConnectMyDb<R>(Func<FakeDbConnection, R> f)
        => Connect("Data Source=127.0.0.1;User=sa;Password=pword123", f);
}