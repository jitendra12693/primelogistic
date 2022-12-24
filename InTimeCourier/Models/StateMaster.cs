namespace InTimeCourier.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StateMaster")]
    public partial class StateMaster
    {
        [Key]
        public long StateId { get; set; }

        public int? CountryId { get; set; }

        [StringLength(150)]
        public string StateName { get; set; }

        public bool? IsActive { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDt { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDt { get; set; }

        [StringLength(5)]
        public string StateCode { get; set; }
    }
}
