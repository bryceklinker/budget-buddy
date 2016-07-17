using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using BudgetBuddy.Api.General.Storage;

namespace BudgetBuddy.Test.Utilities.Stubs.General
{
    public class InMemoryRepository<T> : IRepository<T> where T : IDocument, new()
    {
        public bool IsDisposed { get; set; }

        public List<T> Entities { get; } = new List<T>();

        public List<T> UpdatedEntities { get; } = new List<T>();

        public void Dispose()
        {
            IsDisposed = true;
        }

        public Task<T[]> GetAll()
        {
            return Task.FromResult(Entities.ToArray());
        }

        public Task Insert(T item)
        {
            Entities.Add(item);
            item.Id = Guid.NewGuid();
            return Task.CompletedTask;
        }

        public Task Update(T item)
        {
            UpdatedEntities.Add(item);
            return Task.CompletedTask;;
        }
    }
}
