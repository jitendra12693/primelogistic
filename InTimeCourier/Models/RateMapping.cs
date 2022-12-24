namespace InTimeCourier.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RateMapping")]
    public partial class RateMapping
    {
        [Key]
        public int Id { get; set; }

        public int? PartyId { get; set; }

        public int? ModeId { get; set; }

        public decimal? FromWt { get; set; }

        public decimal? ToWt { get; set; }

        public decimal? Rate { get; set; }

        public DateTime? Date { get; set; }

        public bool? IsActive { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public int NetworkModeId { get; set; }
    }
}
