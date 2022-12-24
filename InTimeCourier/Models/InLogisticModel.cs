using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace InTimeCourier.Models
{
    public partial class InLogisticModel : DbContext
    {
        public InLogisticModel()
            : base("name=InLogisticModel")
        {
        }

        public virtual DbSet<AdminUser> AdminUsers { get; set; }
        public virtual DbSet<AreaMaster> AreaMasters { get; set; }
        public virtual DbSet<BillDetails> BillDetails { get; set; }
        public virtual DbSet<CountryMaster> CountryMasters { get; set; }
        public virtual DbSet<CourrierMaster> CourrierMasters { get; set; }
        public virtual DbSet<CourrierMode> CourrierModes { get; set; }
        public virtual DbSet<PartyMaster> PartyMasters { get; set; }
        public virtual DbSet<PaymentMaster> PaymentMasters { get; set; }
        public virtual DbSet<RoleMaster> RoleMasters { get; set; }
        public virtual DbSet<SourceMaster> SourceMasters { get; set; }
        public virtual DbSet<StateMaster> StateMasters { get; set; }
        public virtual DbSet<StatusMaster> StatusMasters { get; set; }
        public virtual DbSet<RateMapping> RateMapping { get; set; }
        public virtual DbSet<ApiMailClient> ApiMailClients { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminUser>()
                .Property(e => e.UniqueId)
                .IsUnicode(false);

            modelBuilder.Entity<AdminUser>()
                .Property(e => e.TokenId)
                .IsUnicode(false);

            modelBuilder.Entity<AdminUser>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<AdminUser>()
                .Property(e => e.MobileNo)
                .IsUnicode(false);

            modelBuilder.Entity<AdminUser>()
                .Property(e => e.EmailId)
                .IsUnicode(false);

            modelBuilder.Entity<AdminUser>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<AdminUser>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<AdminUser>()
                .Property(e => e.ReTypePassword)
                .IsUnicode(false);

            modelBuilder.Entity<AreaMaster>()
                .Property(e => e.AreaName)
                .IsUnicode(false);

            modelBuilder.Entity<AreaMaster>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<BillDetails>()
                .Property(e => e.BillNo)
                .IsUnicode(false);

            modelBuilder.Entity<BillDetails>()
                .Property(e => e.TotalAmount)
                .HasPrecision(12, 2);

            modelBuilder.Entity<BillDetails>()
                .Property(e => e.FullCharges)
                .HasPrecision(12, 2);

            modelBuilder.Entity<BillDetails>()
                .Property(e => e.CGST)
                .HasPrecision(12, 2);

            modelBuilder.Entity<BillDetails>()
                .Property(e => e.SGST)
                .HasPrecision(12, 2);

            modelBuilder.Entity<BillDetails>()
                .Property(e => e.GrandTotal)
                .HasPrecision(12, 2);

            modelBuilder.Entity<CountryMaster>()
                .Property(e => e.CountryName)
                .IsUnicode(false);

            modelBuilder.Entity<CountryMaster>()
                .Property(e => e.Continent)
                .IsUnicode(false);

            modelBuilder.Entity<CourrierMaster>()
                .Property(e => e.Distance)
                .IsUnicode(false);

            modelBuilder.Entity<CourrierMaster>()
                .Property(e => e.Amount)
                .HasPrecision(12, 3);

            modelBuilder.Entity<CourrierMaster>()
                .Property(e => e.CNNo)
                .IsUnicode(false);

            modelBuilder.Entity<CourrierMaster>()
                .Property(e => e.Weight)
                .HasPrecision(12, 3);

            modelBuilder.Entity<CourrierMaster>()
                .Property(e => e.TrackingNo)
                .IsUnicode(false);

            modelBuilder.Entity<CourrierMaster>()
                .Property(e => e.Rate)
                .HasPrecision(10, 3);

            modelBuilder.Entity<CourrierMode>()
                .Property(e => e.CourrierModeName)
                .IsUnicode(false);

            modelBuilder.Entity<CourrierMode>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<PartyMaster>()
                .Property(e => e.PartyName)
                .IsUnicode(false);

            modelBuilder.Entity<PartyMaster>()
                .Property(e => e.PartyType)
                .IsUnicode(false);

            modelBuilder.Entity<PartyMaster>()
                .Property(e => e.Address1)
                .IsUnicode(false);

            modelBuilder.Entity<PartyMaster>()
                .Property(e => e.Address2)
                .IsUnicode(false);

            modelBuilder.Entity<PartyMaster>()
                .Property(e => e.EmailId)
                .IsUnicode(false);

            modelBuilder.Entity<PartyMaster>()
                .Property(e => e.MobileNo)
                .IsUnicode(false);

            modelBuilder.Entity<PartyMaster>()
                .Property(e => e.AlternateContact)
                .IsUnicode(false);

            modelBuilder.Entity<PartyMaster>()
                .Property(e => e.Ladline)
                .IsUnicode(false);

            modelBuilder.Entity<PartyMaster>()
                .Property(e => e.Landmark)
                .IsUnicode(false);

            modelBuilder.Entity<PartyMaster>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<PartyMaster>()
                .Property(e => e.GSTNumber)
                .IsUnicode(false);

            modelBuilder.Entity<PartyMaster>()
                .Property(e => e.FuelCharges)
                .HasPrecision(5, 2);

            modelBuilder.Entity<PartyMaster>()
                .Property(e => e.Discount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PartyMaster>()
                .Property(e => e.Pincode)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentMaster>()
                .Property(e => e.RecieptNo)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentMaster>()
                .Property(e => e.ChequeNo)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentMaster>()
                .Property(e => e.BankName)
                .IsUnicode(false);

            modelBuilder.Entity<RoleMaster>()
                .Property(e => e.RoleName)
                .IsUnicode(false);

            modelBuilder.Entity<SourceMaster>()
                .Property(e => e.SourceName)
                .IsUnicode(false);

            modelBuilder.Entity<SourceMaster>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<StateMaster>()
                .Property(e => e.StateName)
                .IsUnicode(false);

            modelBuilder.Entity<StateMaster>()
                .Property(e => e.StateCode)
                .IsUnicode(false);

            modelBuilder.Entity<StatusMaster>()
                .Property(e => e.StatusName)
                .IsUnicode(false);

            modelBuilder.Entity<RateMapping>()
                .Property(e => e.FromWt)
                .HasPrecision(18, 3);

            modelBuilder.Entity<RateMapping>()
                .Property(e => e.ToWt)
                .HasPrecision(18, 3);

            modelBuilder.Entity<ApiMailClient>()
                .Property(e => e.ApiUrl)
                .IsUnicode(false);

            modelBuilder.Entity<ApiMailClient>()
                .Property(e => e.ApiDomain)
                .IsUnicode(false);

            modelBuilder.Entity<ApiMailClient>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<ApiMailClient>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<ApiMailClient>()
                .Property(e => e.ApiKey)
                .IsUnicode(false);
        }
    }
}
