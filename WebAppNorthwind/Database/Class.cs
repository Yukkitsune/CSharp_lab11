using Microsoft.Data.SqlClient;

namespace WebAppNorthwind.Database
{
    public class DbConnectionTest
    {
        private readonly IConfiguration _configuration;

        public DbConnectionTest(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void TestConnection()
        {
            var connectionString = _configuration.GetConnectionString("NorthwindDatabase");

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Успешное подключение к базе данных!");
                    string query = "SELECT CustomerID, CompanyName, ContactName FROM Customers";
                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"{reader["CustomerID"]} - {reader["CompanyName"]} - {reader["ContactName"]}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при подключении к базе данных: {ex.Message}");
            }
        }
    }
}