using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InTimeCourier.Models
{
    public partial class SourcePartyRateMapping
    {
        public int SourcePartyRateMappingId { get; set; }
        [Required(ErrorMessage = "Please enter source name.")]
        public int SourceId { get; set; }
        public int StateId { get; set; }
        public string SourceName { get; set; }
        [Required(ErrorMessage ="Please select your party")]
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        [Required(ErrorMessage = "Please enter your rate")]
        public decimal Rate { get; set; }
        [Required(ErrorMessage = "Please select courrier mode")]
        public int CourrierModeId { get; set; }
        public string CourrierModeName { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}