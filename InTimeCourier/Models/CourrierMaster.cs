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

        [Required]
        public long PartyId { get; set; }

        public long? SourceId { get; set; }

        [StringLength(50)]
        public string Distance { get; set; }

        [Required]
        public long? DestinationId { get; set; }

        [Required]
        public decimal? Amount { get; set; }

        [Required]
        public int? CourrierModeId { get; set; }

        [Required]
        [StringLength(15)]
        public string CNNo { get; set; }

        [Required]
        public DateTime DepartureDt { get; set; }

        [Required]
        public decimal Weight { get; set; }

        [StringLength(15)]
        public string TrackingNo { get; set; }

        [Required]
        public decimal? Rate { get; set; }

        public int? StatusId { get; set; }

        public DateTime? CreatedDt { get; set; }

        public int? CreatedBy { get; set; }

        public int? ModifyBy { get; set; }

        public DateTime? ModifyDate { get; set; }

        public bool? IsActive { get; set; }

        //[Required]
        public string Location { get; set; }
        public decimal? ODACharges { get; set; }

        [Required]
        public int NetworkModeId { get; set; }
        public decimal? Discount { get; set; }
        public int Qty { get; set; }
        public bool isInvoiceDone { get; set; }
    }
}
