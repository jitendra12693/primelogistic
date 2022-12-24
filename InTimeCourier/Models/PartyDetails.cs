using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InTimeCourier.Models
{
    public class PartyDetails
    {
        [Key]
        public long PartyId { get; set; }

        public string PartyName { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Landmark { get; set; }

        public string City { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }

        public bool? IsActive { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDt { get; set; }

        public int? ModifyBy { get; set; }

        public DateTime? ModifyDate { get; set; }
        
        public string Pincode { get; set; }
       
        public string EmailId { get; set; }
        
        public string MobileNo { get; set; }
        
        public string AlternateContact { get; set; }
      
        public string Ladline { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        public string ActivationMode { get; set; }
        public string GSTNumber { get; set; }
    }
}