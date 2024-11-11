using CashFlowApp.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowApp.Domain.Entities
{
    public class Transaction
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("date")]
        public DateTime Date { get; set; }
        [BsonElement("amount")]
        public decimal Amount { get; set; }
        [BsonElement("type")]
        public TransactionType Type { get; set; }
        [BsonElement("description")]
        public string Description { get; set; }

        public Transaction(decimal amount, TransactionType type, string description)
        {          
            Date = DateTime.Now;
            Amount = amount;
            Type = type;
            Description = description;
        }
    }
}
