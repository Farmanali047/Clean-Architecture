using Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class Generic_Service<TEntity>
    {
        private readonly IRepository<TEntity> _repository;

        public Generic_Service(IRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _repository.AddAsync(entity);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            await _repository.DeleteAsync(entity);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            await _repository.UpdateAsync(entity);
        }

        public async Task<TEntity> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
    }
}
