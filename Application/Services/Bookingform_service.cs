using Core.Entites;
using Core.Interfaces;

namespace Application.Services
{
    public class Bookingform_service
    {
        private readonly IBookingform _bookingform;

        public Bookingform_service(IBookingform bookingform)
        {
            _bookingform = bookingform;
        }

        public async Task AddAsync(Bookingform bookingform)
        {
            await _bookingform.AddAsync(bookingform);
        }

        public async Task UpdateStatusAsync(int id)
        {
            await _bookingform.UpdateStatusAsync(id);
        }

        public async Task<List<Bookingform>> GetAllAsync()
        {
            return await _bookingform.GetAllAsync();
        }
        public async Task<List<AspNetUsers>> GetUserAsync()
        {
            return await _bookingform.GetUserAsync();
        }

        public async Task<List<Bookingform>> GetAll1Async(string email)
        {
            return await _bookingform.GetAll1Async(email);
        }

        public async Task<List<Bookingform>> GetByNameAsync(string name)
        {
            return await _bookingform.GetByNameAsync(name);
        }
    }
}
