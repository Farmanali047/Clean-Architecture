using Core.Entites;

namespace Core.Interfaces
{
    public interface ICarRegisteration
    {
        Task UpdatestatusAsync(string name);

        Task<List<CarRegisteration>> GetAllAsync();

        Task<List<CarRegisteration>> GetAll1Async();

        Task<List<CarRegisteration>> AvailableAsync();

        Task<List<CarRegisteration>> BookedAsync();
    }
}
