using System;
namespace InTimeCourier.Models
{
    public class DailyCourierManifesto
    {
        public string NetworkName { get; set; }
        public string CourierMode { get; set; }
        public string PartyName { get; set; }
        public string CNNo { get; set; }
        public DateTime BookingDate { get; set; }
        public string Location { get; set; }
        public decimal Weight { get; set; }
        public int Quantity { get; set; }
        //public decimal Rate { get; set; }
        //public decimal DiscountAmount { get; set; }
        //public decimal ODACharges { get; set; }
        public decimal Amount { get; set; }
    }
}