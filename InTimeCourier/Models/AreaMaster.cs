namespace InTimeCourier.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AreaMaster")]
    public partial class AreaMaster
    {
        [Key]
        public long AreaId { get; set; }

        [Required]
        [StringLength(100)]
        public string AreaName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public int StateId { get; set; }

        public int CountryId { get; set; }

        public bool? IsActive { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDt { get; set; }

        public int? ModifyBy { get; set; }

        public DateTime? ModifyDate { get; set; }
    }
}
