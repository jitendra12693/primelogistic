namespace InTimeCourier.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PartyMaster")]
    public partial class PartyMaster
    {
        [Key]
        public long PartyId { get; set; }

        [Required]
        [StringLength(200)]
        public string PartyName { get; set; }

        [StringLength(50)]
        public string PartyType { get; set; }

        [Required]
        [StringLength(200)]
        public string Address1 { get; set; }

        [StringLength(200)]
        public string Address2 { get; set; }

        [StringLength(50)]
        public string EmailId { get; set; }

        [StringLength(12)]
        public string MobileNo { get; set; }

        [StringLength(12)]
        public string AlternateContact { get; set; }

        [StringLength(12)]
        public string Ladline { get; set; }

        [StringLength(200)]
        public string Landmark { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; }

        [StringLength(20)]
        public string GSTNumber { get; set; }

        public bool? IsIGST { get; set; }

        public decimal? FuelCharges { get; set; }

        public decimal? Discount { get; set; }

        public int StateId { get; set; }

        public int CountryId { get; set; }

        [StringLength(6)]
        public string Pincode { get; set; }

        public int? CompanyId { get; set; }

        public bool? IsActive { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDt { get; set; }

        public int? ModifyBy { get; set; }

        public DateTime? ModifyDate { get; set; }

        public int? PartyNature { get; set; }
    }
}
