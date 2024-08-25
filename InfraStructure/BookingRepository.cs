using Core.Interfaces;
using System.Data.SqlClient;
using Dapper;
using Core.Entites;

namespace InfraStructure
{
    public class BookingRepository : IBookingform
    {
        private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=booking;Integrated Security=True";

        public async Task AddAsync(Bookingform bookingform)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Bookingform (PersonName, CarName, Cnic, PhoneNumber, Email, CarPlateNumber, CarColor, PickupLocation, PickUpDate, ReturnDate, CarType, SomeInformation, Quantity) VALUES (@PersonName, @CarName, @Cnic, @PhoneNumber, @Email, @CarPlateNumber, @CarColor, @PickupLocation, @PickUpDate, @ReturnDate, @CarType, @SomeInformation, @Quantity)";
                     connection.Execute(query, bookingform);
                }
                Console.WriteLine("Successful");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task UpdateStatusAsync(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                     connection.Open();
                    string query = "UPDATE Bookingform SET Status = 'Accept' WHERE Id = @Id";
                     connection.Execute(query, new { Id = id });
                }
                Console.WriteLine("Successful");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<List<Bookingform>> GetAll1Async(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT * FROM Bookingform WHERE Email = @Email";
                return (await connection.QueryAsync<Bookingform>(query, new { Email = email })).ToList();
            }
        }

        public async Task<List<Bookingform>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT * FROM Bookingform";
                return (await connection.QueryAsync<Bookingform>(query)).ToList();
            }
        }

        public async Task<List<AspNetUsers>> GetUserAsync()
        {
            var connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=User;Integrated Security=True;");  
            {
                await connection.OpenAsync();
                string query = "SELECT * FROM AspNetUsers";
                return (await connection.QueryAsync<AspNetUsers>(query)).ToList();
            }
        }
        public async Task<List<Bookingform>> GetByNameAsync(string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT * FROM Bookingform WHERE PersonName = @Name";
                return (await connection.QueryAsync<Bookingform>(query, new { Name = name })).ToList();
            }
        }
    }
}
