namespace InTimeCourier.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SourceMaster")]
    public partial class SourceMaster
    {
        [Key]
        public long SourceId { get; set; }

        [Required]
        [StringLength(150)]
        public string SourceName { get; set; }

        public int StateId { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public bool? IsActive { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDt { get; set; }

        public int? ModifyBy { get; set; }

        public DateTime? ModifyDt { get; set; }
    }
}
