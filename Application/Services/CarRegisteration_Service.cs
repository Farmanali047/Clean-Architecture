using Core.Entites;
using Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CarRegisteration_Service
    {
        private readonly ICarRegisteration _carRegisteration;

        public CarRegisteration_Service(ICarRegisteration carRegisteration)
        {
            _carRegisteration = carRegisteration;
        }

        public async Task UpdatestatusAsync(string name)
        {
            await _carRegisteration.UpdatestatusAsync(name);
        }

        public async Task<List<CarRegisteration>> GetAllAsync()
        {
            return await _carRegisteration.GetAllAsync();
        }

        public async Task<List<CarRegisteration>> GetAll1Async()
        {
            return await _carRegisteration.GetAll1Async();
        }

        public async Task<List<CarRegisteration>> AvailableAsync()
        {
            return await _carRegisteration.AvailableAsync();
        }

        public async Task<List<CarRegisteration>> BookedAsync()
        {
            return await _carRegisteration.BookedAsync();
        }
    }
}
