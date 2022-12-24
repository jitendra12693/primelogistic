using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InTimeCourier.Models
{
    [Table("NetworkMaster")]
    public class NetworkMaster
    {
        [Key]
        public int NetworkId { get; set; }
        public string NetworkName { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
    }
}