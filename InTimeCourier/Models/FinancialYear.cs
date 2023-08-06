using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InTimeCourier.Models
{
    [Table("FinancialYear")]
    public class FinancialYear
    {
        [Key]
        public int FYearId { get; set; }
        public string FYear { get; set; }
        public bool IsActive { get; set; }
    }
}