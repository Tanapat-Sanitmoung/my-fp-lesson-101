using LanguageExt;
using static LanguageExt.Prelude;

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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
    private static Either<Exception, DB> GetDB() 
    {
        try
        {
            return Right<Exception, DB>(new DB());
        }
        catch (Exception ex)
        {
            return Left<Exception, DB>(ex);
        }
    }

    private static Either<Exception, User> FindByEmail(DB db, string email)
    {
        try
        {
            var user = db.Users
                .Where(u => u.Email == email)
                .Single();

            return Right<Exception, User>(user);
        }
        catch (Exception ex)
        {
            return Left<Exception, User>(ex);
        }
    }

    public static Either<Exception, User> FindUserByEmail(string email) => 
        from db in GetDB()
        from user in FindByEmail(db, email)
        select user;
}

