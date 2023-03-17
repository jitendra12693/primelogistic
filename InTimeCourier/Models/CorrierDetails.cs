using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InTimeCourier.Models
{
    public partial class CorrierDetails
    {
        public List<CorrierDetails> CorrierDetails1 { get; set; }
        public List<CorrierDetails> CorrierDetails2 { get; set; }

    }
    public partial class CorrierDetails
    {
        public long CourrierId { get; set; }
        public string TrackingNo { get; set; }
        public long PartyId { get; set; }
        public string CNNo { get; set; }
        public string PartyName { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.000}")]
        public decimal? Weight { get; set; }
        public string BookingDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal? Amount { get; set; }

        public DateTime? CreatedDt { get; set; }

        public int? CreatedBy { get; set; }

        public int? ModifyBy { get; set; }

        public DateTime? ModifyDate { get; set; }

        public bool? IsActive { get; set; }

        public int? StatusId { get; set; }
        public List<SelectListItem> SelectParty { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.000}")]
        public decimal Rate { get; set; }
        public int CourrierModeId { get; set; }
        public int NetworkModeId { get; set; }
        public int Quantity { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal? DiscountAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal? ODACharges { get; set; }
        public string Location { get; set; }
        public string NetworkName { get; set; }
        public string CourierMode { get; set; }
        public int RowRank { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal CalculatedAmount { get; set; }
    }
}