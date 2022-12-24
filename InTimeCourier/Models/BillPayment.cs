using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTimeCourier.Models
{
    public class BillPayment
    {
        public long PaymentId { get; set; }
        public long BillId { get; set; }
        public string BillNo { get; set; }
        public int PartyId { get; set; }
        public decimal BillAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal PaymentAmount { get; set; }
        public string ChequeNo { get; set; }
        public string BankName { get; set; }
        public  DateTime? ChequeDate { get; set; }
        public int? PaymentTypeId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public string PartyName { get; set; }
        public DateTime BillDate { get; set; }
    }
}