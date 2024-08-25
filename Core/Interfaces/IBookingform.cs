using Core.Entites;

namespace Core.Interfaces
{
    public interface IBookingform
    {
        Task AddAsync(Bookingform bookingform);

        Task UpdateStatusAsync(int id);

        Task<List<Bookingform>> GetAll1Async(string email);

        Task<List<Bookingform>> GetAllAsync();
        Task<List<AspNetUsers>> GetUserAsync();
        Task<List<Bookingform>> GetByNameAsync(string name);
    }
}
