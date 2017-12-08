using System;
using MySql.Data.MySqlClient;
public class DAO
{

    protected static string connectionString = "server=127.0.0.1;"+
        "uid=root;" +
        "pwd=3616;"+
        "database=universus;";

    public DAO()
	{

	}

    protected static MySqlConnection getConnect()
    {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = myConnectionString;
            
    }
}
