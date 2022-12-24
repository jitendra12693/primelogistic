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

        public long PartyId { get; set; }

        public long? SourceId { get; set; }

        [StringLength(50)]
        public string Distance { get; set; }

        public long? DestinationId { get; set; }

        public decimal? Amount { get; set; }

        public int? CourrierModeId { get; set; }

        [Required]
        [StringLength(15)]
        public string CNNo { get; set; }

        public DateTime DepartureDt { get; set; }

        public decimal Weight { get; set; }

        [StringLength(15)]
        public string TrackingNo { get; set; }

        public decimal? Rate { get; set; }

        public int? StatusId { get; set; }

        public DateTime? CreatedDt { get; set; }

        public int? CreatedBy { get; set; }

        public int? ModifyBy { get; set; }

        public DateTime? ModifyDate { get; set; }

        public bool? IsActive { get; set; }

        public string Location { get; set; }
        public decimal? ODACharges { get; set; }
        public int NetworkModeId { get; set; }
        public decimal? Discount { get; set; }
    }
}
