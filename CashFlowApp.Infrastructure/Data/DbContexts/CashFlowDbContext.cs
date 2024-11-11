using CashFlowApp.Domain.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowApp.Infrastructure.Data.DbContexts
{
    public class CashFlowDbContext
    {
        private readonly IMongoDatabase _database;

        public CashFlowDbContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<Transaction> Transactions => _database.GetCollection<Transaction>("Transactions");
        public IMongoCollection<DailyBalance> DailyBalances => _database.GetCollection<DailyBalance>("DailyBalances");
    }
}
