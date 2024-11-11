using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowApp.Domain.ValueObjects
{
    public class Amount
    {
        public decimal Value { get; }

        public Amount(decimal value)
        {
            
            Value = value;
        }

        public static implicit operator decimal(Amount amount) => amount.Value;
    }
}
