using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BudgetBuddy.Api.Budgets.Shared.Model.Entities;
using BudgetBuddy.Api.General.Storage;
using BudgetBuddy.Infrastructure.DependencyInjection;
using BudgetBuddy.Test.Utilities;
using LiteDB;
using Microsoft.Extensions.Configuration;
using Xunit;


namespace BudgetBuddy.Api.Test.General.Storage
{

    public class RepositoryTest : IDisposable
    {
        private readonly Repository<Budget> _repository;
        private readonly string _connectionString;

        public RepositoryTest()
        {
            _connectionString = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".db");
            var builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string>("Budgets:ConnectionString", _connectionString)
            });
            var configuration = builder.Build();
            _repository = new Repository<Budget>(configuration);
        }

        [Fact]
        public async Task GetAll_ShouldGetAllDocuments()
        {
            InsertBudget(new Budget());
            InsertBudget(new Budget());
            InsertBudget(new Budget());
            var actual = await _repository.GetAll();
            Assert.Equal(3, actual.Length);
        }

        [Fact]
        public async Task Query_ShouldFilterDocuments()
        {
            InsertBudget(new Budget {StartDate = new DateTime(2016, 5, 1)});
            InsertBudget(new Budget {StartDate = new DateTime(2015, 5, 1)});
            InsertBudget(new Budget {StartDate = new DateTime(2016, 6, 1)});
            InsertBudget(new Budget {StartDate = new DateTime(2015, 6, 1)});

            var actual = await _repository.Query(b => b.StartDate > new DateTime(2015, 7, 1));
            Assert.Equal(2, actual.Length);
        }

        [Fact]
        public async Task Insert_ShouldAddDocumentToDatabase()
        {
            await _repository.Insert(new Budget());

            AssertBudgetInserted();
        }

        [Fact]
        public async Task Update_ShouldUpdateDocumentInDatabase()
        {
            var budget = new Budget();
            InsertBudget(budget);

            budget.Income = 45000;

            await _repository.Update(budget);
            AssertBudgetUpdated(budget, 45000);
        }

        [Fact]
        public void Repository_ShouldBeTransient()
        {
            var transient = _repository.GetAttribute<TransientAttribute>();
            Assert.Equal(typeof(IRepository<>), transient.InterfaceType);
        }

        public void Dispose()
        {
            _repository.Dispose();
            if (File.Exists(_connectionString))
                File.Delete(_connectionString);
        }

        private void InsertBudget(Budget budget)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                db.GetCollection<Budget>("Budgets").Insert(budget);
                db.Commit();
            }
        }

        private void AssertBudgetUpdated(Budget budget, int income)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var updatedBudget = db.GetCollection<Budget>("Budgets").FindOne(b => b.Id == budget.Id);
                Assert.Equal(income, updatedBudget.Income);
            }
        }

        private void AssertBudgetInserted()
        {
            using (var liteDatabase = new LiteDatabase(_connectionString))
            {
                var budgets = liteDatabase.GetCollection<Budget>("Budgets");
                Assert.Equal(1, budgets.FindAll().Count());
            }
        }
    }
}
