namespace InTimeCourier.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RateMappingDetails
    {
        [Key]
        public int Id { get; set; }

        public int? PartyId { get; set; }

        public int? CourrierModeId { get; set; }
        public string NetworkModeName { get; set; }

        public decimal? FromWt { get; set; }

        public decimal? ToWt { get; set; }

        public decimal? Rate { get; set; }

        public DateTime? Date { get; set; }

        public string CourrierModeName { get; set; }
        public string PartyName { get; set; }


    }
}
