using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kw9.Models
{
    public class Transactions
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string ReceiverId { get; set; }

        public string SenderId { get; set; }

        public double Balance { get; set; }

        public string TransactionName { get; set; }
    }
}
