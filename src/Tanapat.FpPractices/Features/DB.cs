using LanguageExt;
using static LanguageExt.Prelude;

using Microsoft.EntityFrameworkCore;

namespace Tanapat.FpPractices.Features;

public record User(string Email, string FirstName, string LastName);

public class DB : DbContext
{
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite(
            "Data Source=" + Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "fp.db")
        );

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasKey(e => e.Email);
    }

}

public static class DBMethods
{
    private static Either<ProblemDetail, DB> GetDB() 
    {
        try
        {
            return Right<ProblemDetail, DB>(new DB());
        }
        catch (Exception ex)
        {
            return Left<ProblemDetail, DB>(
                new ProblemDetail(nameof(GetDB) + "-2410220707", "001", "Failed to create DB")
            );
        }
    }

    private static Either<ProblemDetail, User> FindByEmail(DB db, string email)
    {
        try
        {
            var user = db.Users
                .Where(u => u.Email == email)
                .Single();

            return Right<ProblemDetail, User>(user);
        }
        catch (Exception ex)
        {
            return Left<ProblemDetail, User>(
                new ProblemDetail(nameof(FindByEmail) + "-2410220708", "002", "Failed to get user")
            );
        }
    }

    public static Either<ProblemDetail, User> FindUserByEmail(string email) => 
        from db in GetDB()
        from user in FindByEmail(db, email)
        select user;
}

