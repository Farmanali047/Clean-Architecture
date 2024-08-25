using Core.Interfaces;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entites;

namespace InfraStructure
{
    public class CarRegisterationRepository : ICarRegisteration
    {
        private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=booking;Integrated Security=True;";

        public async Task UpdatestatusAsync(string name)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                   await connection.OpenAsync();
                    string updateQuery = "UPDATE CarRegisteration SET Status1 = 'Accept' WHERE CarName = @CarName";
                   await connection.ExecuteAsync(updateQuery, new { CarName = name });
                }
                Console.WriteLine("Successful");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<List<CarRegisteration>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT * FROM CarRegisteration";
                return (await connection.QueryAsync<CarRegisteration>(query)).ToList();
            }
        }

        public async Task<List<CarRegisteration>> GetAll1Async()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT * FROM CarRegisteration";
                return (await connection.QueryAsync<CarRegisteration>(query)).ToList();
            }
        }

        public async Task<List<CarRegisteration>> AvailableAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT * FROM CarRegisteration";
                return (await connection.QueryAsync<CarRegisteration>(query)).ToList();
            }
        }

        public async Task<List<CarRegisteration>> BookedAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT * FROM CarRegisteration WHERE Status1 = 'Booked'";
                return (await connection.QueryAsync<CarRegisteration>(query)).ToList();
            }
        }
    }
}
