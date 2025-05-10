using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tigrin.Models;
using LiteDB;

namespace Tigrin.Litedb
{
    public class TransactionRepository : ITransactionRepository
    {

        private readonly LiteDatabase _db;
        private readonly string collectionName = "transactions";

        public TransactionRepository(LiteDatabase db)
        {
            _db = db;
        }
        public List<Transaction> GetAll()
        {
            return _db
                .GetCollection<Transaction>(collectionName)
                .Query()
                .OrderByDescending(i => i.Date)
                .ToList();
        }

        public void Add(Transaction transaction)
        {
            var collection = _db.GetCollection<Transaction>(collectionName);
                collection.Insert(transaction);
                collection.EnsureIndex(i => i.Date);
        }
        public void Update(Transaction transaction)
        {
            var collection = _db.GetCollection<Transaction>(collectionName);
            collection.Update(transaction);
        }

        public void Delete(Transaction transaction)
        {
            var collection = _db.GetCollection<Transaction>(collectionName);
            collection.Delete(transaction.Id);
        }
    }
}
