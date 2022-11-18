using Data.Auth;
using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class FinalAssignmentContext : IdentityDbContext<ApplicationUser>
    {
        public FinalAssignmentContext(DbContextOptions<FinalAssignmentContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Category

            modelBuilder.Entity<Category>()
                       .ToTable("Category")//ten bang trong sql
                       .HasKey(cat => cat.CategoryId);//khoa chinh

            modelBuilder.Entity<Category>()
                            .Property(cat => cat.CategoryId)
                            .HasColumnName("CategoryId")
                            .HasColumnType("uniqueidentifier")
                            .IsRequired();

            modelBuilder.Entity<Category>()
                            .Property(cat => cat.CategoryName)
                            .HasColumnName("CategoryName")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(50);

            modelBuilder.Entity<Category>()
                            .Property(cat => cat.CategoryCode)
                            .HasColumnName("CategoryCode")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(50);

            #endregion

            #region Asset

            modelBuilder.Entity<Asset>()
                           .ToTable("Asset")
                           .HasKey(pro => pro.AssetId);

            modelBuilder.Entity<Asset>()
                            .HasOne<Category>(s => s.Category)//trong asset lay category 
                            .WithMany(g => g.Assets)//1 category nay ket noi nhieu asset
                            .HasForeignKey(s => s.CategoryForeignId);

            modelBuilder.Entity<Asset>()
                            .Property(pro => pro.AssetId)
                            .HasColumnName("AssetId")
                            .HasColumnType("uniqueidentifier")
                            .IsRequired();

            modelBuilder.Entity<Asset>()
                            .Property(pro => pro.AssetCode)
                            .HasColumnName("AssetCode")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(50);

            modelBuilder.Entity<Asset>()
                            .Property(pro => pro.CategoryForeignId)
                            .HasColumnName("CategoryForeignId")
                            .HasColumnType("uniqueidentifier")
                            .HasMaxLength(50);

            modelBuilder.Entity<Asset>()
                            .Property(pro => pro.CategoryName)
                            .HasColumnName("CategoryName")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(50);

            modelBuilder.Entity<Asset>()
                            .Property(pro => pro.AssetStatus)
                            .HasColumnName("AssetStatus")
                            .HasColumnType("int");

            modelBuilder.Entity<Asset>()
                            .Property(pro => pro.Specification)
                            .HasColumnName("Specification")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(100);

            modelBuilder.Entity<Asset>()
                            .Property(pro => pro.AssetName)
                            .HasColumnName("AssetName")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(50);

            modelBuilder.Entity<Asset>()
                            .Property(pro => pro.InstalledDate)
                            .HasColumnName("InstalledDate")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(50);

            modelBuilder.Entity<Asset>()
                            .Property(pro => pro.Location)
                            .HasColumnName("Location")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(50);

            #endregion

            #region Assignment

            modelBuilder.Entity<Assignment>()
                           .ToTable("Assignment")
                           .HasKey(pro => pro.AssignmentId);

            modelBuilder.Entity<Assignment>()
                            .Property(pro => pro.AssignmentId)
                            .HasColumnName("AssignmentId")
                            .HasColumnType("uniqueidentifier")
                            .HasMaxLength(50);
            
            modelBuilder.Entity<Assignment>()
                            .Property(pro => pro.AssignedTo)
                            .HasColumnName("AssignedTo")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(50);

            modelBuilder.Entity<Assignment>()
                            .Property(pro => pro.AssignedBy)
                            .HasColumnName("AssignedBy")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(50);

            modelBuilder.Entity<Assignment>()
                            .Property(pro => pro.AcceptedBy)
                            .HasColumnName("AcceptedBy")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(50);

            modelBuilder.Entity<Assignment>()
                            .Property(pro => pro.ReturnDate)
                            .HasColumnName("ReturnDate")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(50);
            
            modelBuilder.Entity<Assignment>()
                            .Property(pro => pro.Note)
                            .HasColumnName("Note")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(100);

            modelBuilder.Entity<Assignment>()
                            .Property(pro => pro.AssignmentState)
                            .HasColumnName("AssignmentState")
                            .HasColumnType("int")
                            .HasMaxLength(50);

            modelBuilder.Entity<Assignment>()
                            .Property(pro => pro.RequestBy)
                            .HasColumnName("RequestBy")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(50);
            
            modelBuilder.Entity<Assignment>()
                            .Property(pro => pro.AssetCode)
                            .HasColumnName("ReturnDate")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(50);
            
            modelBuilder.Entity<Assignment>()
                            .Property(pro => pro.AssetName)
                            .HasColumnName("AssetName")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(50);

            modelBuilder.Entity<Assignment>()
                            .Property(pro => pro.Specification)
                            .HasColumnName("Specification")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(50);

            #endregion

            #region RequestReturning

            modelBuilder.Entity<RequestReturning>()
                           .ToTable("RequestReturning")
                           .HasKey(pro => pro.RequestReturningId);

            modelBuilder.Entity<RequestReturning>()
                            .Property(pro => pro.RequestReturningId)
                            .HasColumnName("RequestReturningId")
                            .HasColumnType("uniqueidentifier")
                            .HasMaxLength(50);
            
            modelBuilder.Entity<RequestReturning>()
                            .Property(pro => pro.AssetCode)
                            .HasColumnName("AssetCode")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(50);

            modelBuilder.Entity<RequestReturning>()
                            .Property(pro => pro.AssignedDate)
                            .HasColumnName("AssignedDate")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(50);

            modelBuilder.Entity<RequestReturning>()
                            .Property(pro => pro.AssignmentState)
                            .HasColumnName("AssignmentState")
                            .HasColumnType("int")
                            .HasMaxLength(50);

            modelBuilder.Entity<RequestReturning>()
                            .Property(pro => pro.Note)
                            .HasColumnName("Note")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(100);

            modelBuilder.Entity<RequestReturning>()
                            .Property(pro => pro.ReturnDate)
                            .HasColumnName("ReturnDate")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(50);

            modelBuilder.Entity<RequestReturning>()
                            .Property(pro => pro.AssignedBy)
                            .HasColumnName("AssignedBy")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(50);

            modelBuilder.Entity<RequestReturning>()
                            .Property(pro => pro.RequestedBy)
                            .HasColumnName("RequestedBy")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(50);
            
            modelBuilder.Entity<RequestReturning>()
                            .Property(pro => pro.AcceptedBy)
                            .HasColumnName("AcceptedBy")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(50);

            #endregion

            #region Report

            modelBuilder.Entity<Report>()
                           .ToTable("Report")
                           .HasKey(pro => pro.ReportId);

            modelBuilder.Entity<Report>()
                            .Property(pro => pro.ReportId)
                            .HasColumnName("ReportId")
                            .HasColumnType("uniqueidentifier")
                            .HasMaxLength(50);

            modelBuilder.Entity<Report>()
                            .Property(pro => pro.CategoryName)
                            .HasColumnName("CategoryName")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(50);

            modelBuilder.Entity<Report>()
                            .Property(pro => pro.Total)
                            .HasColumnName("Total")
                            .HasColumnType("int")
                            .HasMaxLength(50);
            
            modelBuilder.Entity<Report>()
                            .Property(pro => pro.Assigned)
                            .HasColumnName("Assigned")
                            .HasColumnType("int")
                            .HasMaxLength(50);
            
            modelBuilder.Entity<Report>()
                            .Property(pro => pro.Available)
                            .HasColumnName("Available")
                            .HasColumnType("int")
                            .HasMaxLength(50);

            modelBuilder.Entity<Report>()
                            .Property(pro => pro.NotAvailable)
                            .HasColumnName("NotAvailable")
                            .HasColumnType("int")
                            .HasMaxLength(50);

            modelBuilder.Entity<Report>()
                            .Property(pro => pro.WaitingForRecycling)
                            .HasColumnName("WaitingForRecycling")
                            .HasColumnType("int")
                            .HasMaxLength(50);
            
            modelBuilder.Entity<Report>()
                            .Property(pro => pro.Recycled)
                            .HasColumnName("Recycled")
                            .HasColumnType("int")
                            .HasMaxLength(50);

            #endregion

        }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Asset>? Assets { get; set; }
        public DbSet<Assignment>? Assignments { get; set; }
        public DbSet<Report>? Reports { get; set; }
        public DbSet<RequestReturning>? RequestReturnings { get; set; }
    }
}
