using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PCBuilder.Repository.Model;

public partial class PcBuildingContext : DbContext
{
    public PcBuildingContext()
    {
    }

    public PcBuildingContext(DbContextOptions<PcBuildingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Compatibility> Compatibilities { get; set; }

    public virtual DbSet<Component> Components { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Pc> Pcs { get; set; }

    public virtual DbSet<PcComponent> PcComponents { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=ec2-3-84-219-144.compute-1.amazonaws.com;Initial Catalog=PcBuilding;Persist Security Info=True;User ID=sa;Password=swp12345@;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Brand__3214EC07B586B789");

            entity.ToTable("Brand");

            entity.Property(e => e.Logo)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Origin)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC07204175EB");

            entity.ToTable("Category");

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Brand).WithMany(p => p.Categories)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK__Category__BrandI__4BAC3F29");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK__Category__Parent__4AB81AF0");
        });

        modelBuilder.Entity<Compatibility>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Compatib__3214EC0774F09A5B");

            entity.ToTable("Compatibility");

            entity.Property(e => e.Category01Id).HasColumnName("Category01_ID");
            entity.Property(e => e.Category02Id).HasColumnName("Category02_ID");

            entity.HasOne(d => d.Category01).WithMany(p => p.CompatibilityCategory01s)
                .HasForeignKey(d => d.Category01Id)
                .HasConstraintName("FK__Compatibi__Categ__4E88ABD4");

            entity.HasOne(d => d.Category02).WithMany(p => p.CompatibilityCategory02s)
                .HasForeignKey(d => d.Category02Id)
                .HasConstraintName("FK__Compatibi__Categ__4F7CD00D");
        });

        modelBuilder.Entity<Component>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Componen__3214EC07F34AFB05");

            entity.ToTable("Component");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Image)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Brand).WithMany(p => p.Components)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Component__Brand__52593CB8");

            entity.HasOne(d => d.Category).WithMany(p => p.Components)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Component__Categ__534D60F1");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3214EC07BE9EB17D");

            entity.ToTable("Order");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.OrderDate).HasColumnType("date");
            entity.Property(e => e.StatusId)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Payment).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentId)
                .HasConstraintName("FK__Order__PaymentId__45F365D3");

            entity.HasOne(d => d.Pc).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PcId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__PcId__440B1D61");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__UserId__44FF419A");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payment__3214EC0759DA2C7B");

            entity.ToTable("Payment");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.PaymentMode)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.PaymentTime)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Pc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PC__3214EC07F188AB29");

            entity.ToTable("PC");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Discount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Image)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TemplateId).HasColumnName("TemplateID");

            entity.HasOne(d => d.DesignByNavigation).WithMany(p => p.Pcs)
                .HasForeignKey(d => d.DesignBy)
                .HasConstraintName("FK__PC__DesignBy__3D5E1FD2");

            entity.HasOne(d => d.Template).WithMany(p => p.InverseTemplate)
                .HasForeignKey(d => d.TemplateId)
                .HasConstraintName("FK__PC__TemplateID__3C69FB99");
        });

        modelBuilder.Entity<PcComponent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PC_Compo__3214EC07C3825B15");

            entity.ToTable("PC_Component");

            entity.HasOne(d => d.Component).WithMany(p => p.PcComponents)
                .HasForeignKey(d => d.ComponentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PC_Compon__Compo__571DF1D5");

            entity.HasOne(d => d.Pc).WithMany(p => p.PcComponents)
                .HasForeignKey(d => d.PcId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PC_Compone__PcId__5629CD9C");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC0717850A15");

            entity.ToTable("Role");

            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC0788ABF565");

            entity.ToTable("User");

            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.Avatar)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Fullname).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__RoleID__398D8EEE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
