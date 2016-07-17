using System;
using System.Linq;
using System.Threading.Tasks;
using BudgetBuddy.Infrastructure.DependencyInjection;
using LiteDB;
using Microsoft.Extensions.Configuration;

namespace BudgetBuddy.Api.General.Storage
{
    public interface IRepository<T> : IDisposable where T : IDocument, new()
    {
        Task<T[]> GetAll();
        Task Insert(T item);
        Task Update(T item);
    }

    [Transient(typeof(IRepository<>))]
    public class Repository<T> : IRepository<T> where T : IDocument, new()
    {
        private readonly string _connectionString;
        private readonly string _collectionName;

        public Repository(IConfiguration configuration)
        {
            _connectionString = configuration["Budgets:ConnectionString"];
            _collectionName = $"{typeof(T).Name}s";
        }

        public Task<T[]> GetAll()
        {
            using (var database = CreateDb())
            {
                var all = GetCollection(database).FindAll().ToArray();
                return Task.FromResult(all);
            }
        }

        public Task Insert(T item)
        {
            using (var database = CreateDb())
            {
                GetCollection(database).Insert(item);
                database.Commit();
                return Task.CompletedTask;
            }
        }

        public Task Update(T item)
        {
            using (var database = CreateDb())
            {
                GetCollection(database).Update(item);
                database.Commit();
                return Task.CompletedTask;
            }
        }

        public void Dispose()
        {

        }

        private LiteDatabase CreateDb()
        {
            return new LiteDatabase(_connectionString);
        }

        private LiteCollection<T> GetCollection(LiteDatabase database)
        {
            return database.GetCollection<T>(_collectionName);
        }
    }
}
