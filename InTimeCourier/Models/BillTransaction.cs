using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InTimeCourier.Models
{
    [Table("BillTransaction")]
    public class BillTransaction
    {
        [Key]
        public long TransactionId { get; set; }
        public int PartyId { get; set; }
        public string FinancialYear { get; set;}
        public string InvoiceNo { get; set; }
        public string TransactionType { get; set; }
        public string VoucherType { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Narration { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set;}
        public DateTime UpdatedDate { get; set; }
    }
}