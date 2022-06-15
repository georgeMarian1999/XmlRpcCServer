using System.Data;
using Npgsql;

namespace WebService.JDBC;

public class JDBC
{
    private string connectionString = "Host=ec2-52-200-215-149.compute-1.amazonaws.com;Username=nbwavzyshzkuiy;Password=5ce75fe8818ecb2e397d77d0749ef05d630b38c732e7cbfb6eeaf7eba775a90f;Database=ds75uhlqkbe92";

    private NpgsqlConnection con;


    public JDBC()
    {
        getNewConnection();
    }
    
    public void getNewConnection()
    {
        con = new NpgsqlConnection(connectionString);
        con.Open();
        
    }

    public NpgsqlConnection getConnection()
    {
        return con;
    }
}