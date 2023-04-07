namespace InTimeCourier.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CourrierMaster")]
    public partial class CourrierMaster
    {
        [Key]
        public long CourrierId { get; set; }

       // [Required(ErrorMessage = "Party is required")]
        public long PartyId { get; set; }

        public long? SourceId { get; set; }

        [Required(ErrorMessage = "Party is required")]
        [StringLength(50)]
        public string Distance { get; set; }

       // [Required]
        public long? DestinationId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        public decimal? Amount { get; set; }

        [Required(ErrorMessage = "Mode is required")]
        public int? CourrierModeId { get; set; }

        [Required(ErrorMessage = "AWB No is required")]
        [StringLength(15)]
        public string CNNo { get; set; }

        [Required(ErrorMessage = "Docket Date is required")]
        public DateTime DepartureDt { get; set; }

        [Required(ErrorMessage = "Weight is required")]
        public decimal Weight { get; set; }

        [StringLength(15)]
        public string TrackingNo { get; set; }

        [Required(ErrorMessage = "Rate is required")]
        public decimal? Rate { get; set; }

        public int? StatusId { get; set; }

        public DateTime? CreatedDt { get; set; }

        public int? CreatedBy { get; set; }

        public int? ModifyBy { get; set; }

        public DateTime? ModifyDate { get; set; }

        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Destination is required")]
        public string Location { get; set; }
        public decimal? ODACharges { get; set; }

        [Required(ErrorMessage = "Network is required")]
        public int NetworkModeId { get; set; }
        public decimal? Discount { get; set; }
        public int Qty { get; set; }
        public bool? isInvoiceDone { get; set; }
        public decimal? FuelCharges { get; set; }
        public int? BillId { get; set; }
    }
}
