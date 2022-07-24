using Microsoft.EntityFrameworkCore;
using TestTask.Models;

namespace TestTask.Context;
public class ApplicationContext : DbContext
{
    public DbSet<Transaction> Transaction { get; set; }


    /*public static async Task<SqlConnection> GetConnecton(string connectionString)
    {
        SqlConnection sqlConnection = new SqlConnection(connectionString);
        await sqlConnection.OpenAsync();
        SqlCommand cmd = sqlConnection.CreateCommand();
        SqlDataReader rdr = cmd.ExecuteReader();
        return sqlConnection;
    }*/
    public ApplicationContext()
    {
        /*_connectionString = connectionString;*/
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=TestTask;Trusted_Connection=True;");
    }
}
