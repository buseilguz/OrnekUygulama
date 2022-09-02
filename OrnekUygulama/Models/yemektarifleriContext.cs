using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace OrnekUygulama.Models
{
    public partial class yemektarifleriContext : DbContext
    {
        public yemektarifleriContext()
        {
        }

        public yemektarifleriContext(DbContextOptions<yemektarifleriContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Kategoriler> Kategorilers { get; set; }
        public virtual DbSet<Kullanicilar> Kullanicilars { get; set; }
        public virtual DbSet<Menuler> Menulers { get; set; }
        public virtual DbSet<Sayfalar> Sayfalars { get; set; }
        public virtual DbSet<Yemektarifleri> Yemektarifleris { get; set; }
        public virtual DbSet<Yorumlar> Yorumlars { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseNpgsql("User ID=postgres;Password=\"123456\";Host=localhost;Port=5432;Database=yemektarifleri;Pooling=true;Connection Lifetime=0;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Turkish_Turkey.1254");

            modelBuilder.Entity<Kategoriler>(entity =>
            {
                entity.HasKey(e => e.KategoriId)
                    .HasName("kategoriler_pkey");

                entity.ToTable("kategoriler");

                entity.Property(e => e.KategoriId)
                    .HasColumnName("kategoriId")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Aktif)
                    .HasColumnType("boolean")
                    .HasColumnName("aktif");

                entity.Property(e => e.Kategoriadi)
                    .HasMaxLength(100)
                    .HasColumnName("kategoriadi");

                entity.Property(e => e.Silindi)
                    .HasColumnType("boolean")
                    .HasColumnName("silindi");
            });

            modelBuilder.Entity<Kullanicilar>(entity =>
            {
                entity.HasKey(e => e.KullaniciId)
                    .HasName("kullanicilar_pkey");

                entity.ToTable("kullanicilar");

                entity.Property(e => e.KullaniciId)
                    .HasColumnName("kullaniciId")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Adi)
                    .HasMaxLength(25)
                    .HasColumnName("adi");

                entity.Property(e => e.Aktif)
                    .HasColumnType("boolean")
                    .HasColumnName("aktif");

                entity.Property(e => e.Eposta)
                    .HasMaxLength(50)
                    .HasColumnName("eposta");

                entity.Property(e => e.Parola)
                    .HasMaxLength(40)
                    .HasColumnName("parola");

                entity.Property(e => e.Silindi)
                    .HasColumnType("boolean")
                    .HasColumnName("silindi");

                entity.Property(e => e.Soyadi)
                    .HasMaxLength(20)
                    .HasColumnName("soyadi");

                entity.Property(e => e.Telefon)
                    .HasMaxLength(15)
                    .HasColumnName("telefon");

                entity.Property(e => e.Yetki)
                    .HasColumnType("boolean")
                    .HasColumnName("yetki");
            });

            modelBuilder.Entity<Menuler>(entity =>
            {
                entity.HasKey(e => e.MenuId)
                    .HasName("menuler_pkey");

                entity.ToTable("menuler");

                entity.Property(e => e.MenuId)
                    .HasColumnName("menuId")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Aktif)
                    .HasColumnType("boolean")
                    .HasColumnName("aktif");

                entity.Property(e => e.Baslik)
                    .HasMaxLength(250)
                    .HasColumnName("baslik");

                entity.Property(e => e.Silindi)
                    .HasColumnType("boolean")
                    .HasColumnName("silindi");

                entity.Property(e => e.Sira).HasColumnName("sira");

                entity.Property(e => e.Url)
                    .HasMaxLength(500)
                    .HasColumnName("url");

                entity.Property(e => e.Ustid).HasColumnName("ustid");
            });

            modelBuilder.Entity<Sayfalar>(entity =>
            {
                entity.HasKey(e => e.SayfaId)
                    .HasName("sayfalar_pkey");

                entity.ToTable("sayfalar");

                entity.Property(e => e.SayfaId)
                    .HasColumnName("sayfaId")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Aktif)
                    .HasColumnType("boolean")
                    .HasColumnName("aktif");

                entity.Property(e => e.Baslik)
                    .HasMaxLength(250)
                    .HasColumnName("baslik");

                entity.Property(e => e.Icerik)
                    .HasMaxLength(500)
                    .HasColumnName("icerik");

                entity.Property(e => e.Silindi)
                    .HasColumnType("boolean")
                    .HasColumnName("silindi");
            });

            modelBuilder.Entity<Yemektarifleri>(entity =>
            {
                entity.HasKey(e => e.TarifId)
                    .HasName("yemektarifleri_pkey");

                entity.ToTable("yemektarifleri");

                entity.Property(e => e.TarifId)
                    .HasColumnName("tarifId")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Aktif)
                    .HasColumnType("boolean")
                    .HasColumnName("aktif");

                entity.Property(e => e.Kategoriid).HasColumnName("kategoriid");

                entity.Property(e => e.Silindi)
                    .HasColumnType("boolean")
                    .HasColumnName("silindi");

                entity.Property(e => e.Sira).HasColumnName("sira");

                entity.Property(e => e.Tarif)
                    .HasMaxLength(300)
                    .HasColumnName("tarif");

                entity.Property(e => e.Yemekadi)
                    .HasMaxLength(250)
                    .HasColumnName("yemekadi");

                entity.HasOne(d => d.Kategori)
                    .WithMany(p => p.Yemektarifleris)
                    .HasForeignKey(d => d.Kategoriid)
                    .HasConstraintName("kategori");
            });

            modelBuilder.Entity<Yorumlar>(entity =>
            {
                entity.HasKey(e => e.YorumId)
                    .HasName("yorumlar_pkey");

                entity.ToTable("yorumlar");

                entity.Property(e => e.YorumId)
                    .HasColumnName("yorumId")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Aktif)
                    .HasColumnType("boolean")
                    .HasColumnName("aktif");

                entity.Property(e => e.Eklemetarihi)
                    .HasColumnType("date")
                    .HasColumnName("eklemetarihi");

                entity.Property(e => e.Silindi)
                    .HasColumnType("boolean")
                    .HasColumnName("silindi");

                entity.Property(e => e.TarifId).HasColumnName("tarifId");

                entity.Property(e => e.KullaniciId).HasColumnName("kullaniciId");

                entity.Property(e => e.Yorum)
                    .HasMaxLength(250)
                    .HasColumnName("yorum");

                entity.HasOne(d => d.Tarif)
                    .WithMany(p => p.Yorumlars)
                    .HasForeignKey(d => d.TarifId)
                    .HasConstraintName("tarifId");

                entity.HasOne(d => d.Uye)
                    .WithMany(p => p.Yorumlars)
                    .HasForeignKey(d => d.KullaniciId)
                    .HasConstraintName("kullaniciId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
