using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InTimeCourier.Models
{
    [Table("BillDetails")]
    public partial class BillDetails
    {
        [Key]
        public long BillId { get; set; }
        public string BillNo { get; set; }
        public long PartyId { get; set; }
        public string PartyName { get; set; }
        public string GSTINNumber { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public string BillDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public string PeriodFrom { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public string PeriodTo { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal FullCharges { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal GrandTotal { get; set; }
        public DateTime CreatedDt { get; set; }
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDt { get; set; }
        public string SearchTerm { get; set; }
        public decimal PaidAmount { get; set; }
        public string ChequeNo { get; set; }
    }
}