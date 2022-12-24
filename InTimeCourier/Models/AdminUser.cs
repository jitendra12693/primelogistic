namespace InTimeCourier.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AdminUser")]
    public partial class AdminUser
    {
        [Key]
        public long UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string UniqueId { get; set; }

        [StringLength(150)]
        public string TokenId { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Required]
        [StringLength(10)]
        public string MobileNo { get; set; }

        [StringLength(50)]
        public string EmailId { get; set; }

        [Required]
        [StringLength(20)]
        public string Username { get; set; }

        [Required]
        [StringLength(30)]
        public string Password { get; set; }

        [StringLength(30)]
        public string ReTypePassword { get; set; }

        public bool? IsActive { get; set; }

        public int? RoleId { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDt { get; set; }

        public int? ModifyBy { get; set; }

        public DateTime? ModifyDt { get; set; }
    }
}
