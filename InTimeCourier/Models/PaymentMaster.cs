using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InTimeCourier.Models
{
    [Table("PaymentMaster")]
    public class PaymentMaster
    {
        [Key]
        public long PaymentId { get; set; }

        [StringLength(50)]
        public string RecieptNo { get; set; }

        public int PartyId { get; set; }

        public long BillId { get; set; }

        public decimal BillAmount { get; set; }

        public decimal BalanceAmount { get; set; }

        public decimal? PaidAmount { get; set; }

        public decimal PaymentAmount { get; set; }

        [StringLength(10)]
        public string ChequeNo { get; set; }

        [StringLength(50)]
        public string BankName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ChequeDate { get; set; }

        public int? PaymentTypeId { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? UpdateBy { get; set; }
    }
}