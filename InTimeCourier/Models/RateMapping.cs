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

        [Required]
        public int? PartyId { get; set; }

        [Required]
        public int? ModeId { get; set; }

        [Required]
        [RegularExpression(@"\d+(\.\d{1,3})?", ErrorMessage = "Please enter only 3 decimal point")]
        public decimal? FromWt { get; set; }

        [Required]
        [RegularExpression(@"\d+(\.\d{1,3})?", ErrorMessage = "Please enter only 3 decimal point")]
        public decimal? ToWt { get; set; }

        [Required]
        public decimal? Rate { get; set; }

        public DateTime? Date { get; set; }

        public bool? IsActive { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        [Required]
        public int NetworkModeId { get; set; }
    }
}
