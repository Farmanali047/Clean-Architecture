using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Core.Interfaces;

namespace InfraStructure
{
    public class GenericRepository<TEntity> : IRepository<TEntity>
    {
        private readonly string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=booking;Integrated Security=True;";

        public async Task AddAsync(TEntity entity)
        {
            var tablename = typeof(TEntity).Name;

            var properties = typeof(TEntity).GetProperties().Where(p => p.Name != "Id" && p.Name != "CarImage" && p.Name != "ImageName");
            var columnNames = string.Join(",", properties.Select(x => x.Name));
            var parameterName = string.Join(",", properties.Select(y => "@" + y.Name));

            var query = $"insert into {tablename} ({columnNames}) values({parameterName})";

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(query, entity);
            }
        }

        public async Task UpdateAsync(TEntity entity)
        {
            var tableName = typeof(TEntity).Name;
            var primaryKey = "Id";

            var properties = typeof(TEntity).GetProperties().Where(x => x.Name != primaryKey && x.Name != "CarImage" && x.Name != "ImgUrl");
            var idProp = typeof(TEntity).GetProperties().Where(x => x.Name == primaryKey);

            var setClause = string.Join(",", properties.Select(a => $"{a.Name}=@{a.Name}"));
            int ourId = 0;

            foreach (var prop in idProp)
            {
                if (prop.Name == "Id")
                {
                    string temp = (prop.GetValue(entity).ToString());
                    ourId = int.Parse(temp);
                    break;
                }
            }

            var query = $"update {tableName} set {setClause} where {primaryKey}= {ourId}";
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(query, entity);
            }
        }

        public async Task DeleteAsync(TEntity entity)
        {
            var tablename = typeof(TEntity).Name;
            var query = $"delete from {tablename} where Id = @Id";

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(query, entity);
            }
        }

        public async Task<TEntity?> GetAsync(int id)
        {
            var tablename = typeof(TEntity).Name;
            var query = $"select * from {tablename} where id = @id";

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var result = await connection.QueryAsync<TEntity>(query, new { id });
                return result.FirstOrDefault();
            }
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            var tablename = typeof(TEntity).Name;
            var query = $"select * from {tablename}";

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var result = await connection.QueryAsync<TEntity>(query);
                return result.ToList();
            }
        }
    }
}
