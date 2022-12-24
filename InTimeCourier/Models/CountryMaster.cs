namespace InTimeCourier.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CountryMaster")]
    public partial class CountryMaster
    {
        [Key]
        public int CountryId { get; set; }

        [StringLength(150)]
        public string CountryName { get; set; }

        [StringLength(150)]
        public string Continent { get; set; }

        public DateTime? CreatedDt { get; set; }

        public DateTime? UpdatedDt { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        public bool? IsActive { get; set; }
    }
}
