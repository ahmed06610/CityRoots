using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CityRoots.Core.Models;
using CityRoots.Core.Models.Recommendations;
using CityRoots.Core.Seeding;

namespace CityRoots.EF.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets for each entity
        public DbSet<Farmer> Farmers { get; set; }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<Investor> Investors { get; set; }
        public DbSet<Farm> Farms { get; set; }
        public DbSet<LandParcel> LandParcels { get; set; }
        public DbSet<Crop> Crops { get; set; }
        public DbSet<Cycle> Cycles { get; set; }
        public DbSet<OpenInvestmentCycle> OpenInvestmentCycles { get; set; }
        public DbSet<InvestmentRequest> InvestmentRequests { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<CycleUpdate> CycleUpdates { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Harvest> Harvests { get; set; }
        public DbSet<PurchaseRequest> PurchaseRequests { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<FeedBack> feedBacks { get; set; }
        public DbSet<CropType> CropTypes { get; set; }
        public DbSet<AiPredict> AiPredicts { get; set; }
        public DbSet<CycleNotificationLog> cycleNotificationLogs { get; set; }
        public DbSet<HarvestNotificationLog> HarvestNotificationLogs { get; set; }
        public DbSet<ScheduleNotificationLog> scheduleNotificationLogs { get; set; }
        public DbSet<InteractionOfInvestor> interactionOfInvestors { get; set; }
        public DbSet<InteractionOfMerchant> interactionOfMerchants { get; set; }
        public DbSet<FavoriteFarmers> favoriteFarmers { get; set; }
        public DbSet<Rate> Rates { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure entity relationships and constraints

            // Define a one-to-one relationship between ApplicationUser and Farmer
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(a => a.Farmer)
                .WithOne(f => f.ApplicationUser)
                .HasForeignKey<Farmer>(f => f.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Define a one-to-one relationship between ApplicationUser and Investor
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(a => a.Investor)
                .WithOne(i => i.ApplicationUser)
                .HasForeignKey<Investor>(i => i.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Define a one-to-one relationship between ApplicationUser and Merchant
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(a => a.Merchant)
                .WithOne(m => m.ApplicationUser)
                .HasForeignKey<Merchant>(m => m.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Farmer-Farm (one-to-many relationship)
            modelBuilder.Entity<Farm>()
                .HasOne(f => f.Farmer)
                .WithMany(fr => fr.Farms)
                .HasForeignKey(f => f.FarmerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Farm-LandParcel (one-to-many relationship)
            modelBuilder.Entity<LandParcel>()
                .HasOne(lp => lp.Farm)
                .WithMany(f => f.LandParcels)
                .HasForeignKey(lp => lp.FarmId)
                .OnDelete(DeleteBehavior.Cascade);

            // LandParcel-Cycle and Crop-Cycle (many-to-one relationships)
            modelBuilder.Entity<Cycle>()
                .HasOne(c => c.LandParcel)
                .WithMany(lp => lp.Cycles)
                .HasForeignKey(c => c.ParcelId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cycle>()
                .HasOne(c => c.Crop)
                .WithMany(crop => crop.Cycles)
                .HasForeignKey(c => c.CropId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cycle-OpenInvestmentCycle (one-to-one relationship)
            modelBuilder.Entity<Cycle>()
                .HasOne(c => c.OpenInvestmentCycle)
                .WithOne(oic => oic.Cycle)
                .HasForeignKey<OpenInvestmentCycle>(oic => oic.CycleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cycle-InvestmentRequest (one-to-many relationship)
            modelBuilder.Entity<InvestmentRequest>()
                .HasOne(ir => ir.Cycle)
                .WithMany(c => c.InvestmentRequests)
                .HasForeignKey(ir => ir.CycleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Investor-InvestmentRequest (one-to-many relationship)
            modelBuilder.Entity<InvestmentRequest>()
                .HasOne(ir => ir.Investor)
                .WithMany(i => i.InvestmentRequests)
                .HasForeignKey(ir => ir.InvestorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cycle-Schedule and Cycle-CycleUpdate (one-to-many relationships)
            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Cycle)
                .WithMany(c => c.Schedules)
                .HasForeignKey(s => s.CycleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CycleUpdate>()
                .HasOne(cu => cu.Cycle)
                .WithMany(c => c.CycleUpdates)
                .HasForeignKey(cu => cu.CycleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Payment - Payer relationship
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Payer)
                .WithMany(u => u.Payments)
                .HasForeignKey(p => p.PayerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Payment - Payee relationship
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Payee)
                .WithMany()
                .HasForeignKey(p => p.PayeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Harvest-Crop and Harvest-Purchase (one-to-many relationships)
            modelBuilder.Entity<Harvest>()
                .HasOne(h => h.Crop)
                .WithMany(c => c.Harvests)
                .HasForeignKey(h => h.CropId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PurchaseRequest>()
                .HasOne(p => p.Harvest)
                .WithMany(h => h.Purchases)
                .HasForeignKey(p => p.HarvestId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PurchaseRequest>()
                .HasOne(p => p.Merchant)
                .WithMany(m => m.Purchases)
                .HasForeignKey(p => p.MerchantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Chat - Sender relationship
            modelBuilder.Entity<Chat>()
                .HasOne(c => c.Sender)
                .WithMany(u => u.SentChats)
                .HasForeignKey(c => c.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Chat - Receiver relationship
            modelBuilder.Entity<Chat>()
                .HasOne(c => c.Receiver)
                .WithMany(u => u.ReceivedChats)
                .HasForeignKey(c => c.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            // User-Notification (one-to-many relationship)
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FeedBack>()
                .HasOne(p => p.User)
                .WithMany(p => p.FeedBacks)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Crop>()
                .HasOne(x => x.CropType)
                .WithMany(x => x.crops)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<FavoriteFarmers>().HasKey(x => new { x.userId, x.FarmerId });
            modelBuilder.Entity<Payment>()
        .HasIndex(p => p.PaypalOrderId)
        .IsUnique();
            modelBuilder.Entity<Rate>().HasKey(x => new { x.UserId, x.FarmerId });

            // Seed data
            modelBuilder.Entity<AiPredict>().HasData(AiPredictSeeder.GetSeedData());
            modelBuilder.Entity<Crop>().HasData(CropSeeder.GetSeedData());
        }
    }
    /*  private static List<CropType> SeedCropType()
  {
      return new List<CropType>() {
      new CropType{CropTypeId=1,Name="حبوب"},
      new CropType{CropTypeId=2,Name="فاكهه" },
      new CropType{CropTypeId=3,Name="خضار"}
      };
}*/
}
