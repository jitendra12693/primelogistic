namespace InTimeCourier.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ApiMailClient")]
    public partial class ApiMailClient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ApiID { get; set; }

        public Guid? UniqueID { get; set; }

        [StringLength(50)]
        public string ApiUrl { get; set; }

        [StringLength(50)]
        public string ApiDomain { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        [StringLength(200)]
        public string ApiKey { get; set; }
    }
}
