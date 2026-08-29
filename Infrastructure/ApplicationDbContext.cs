using Domain.Entities;
using Domain.ValueObjects.Room;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Room>(builder =>
        {
            builder.ToTable("Rooms");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(RoomName.MaxLength);

            builder.Property(r => r.BaseRentalRate)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(r => r.Capacity)
                .IsRequired();
            
            builder.HasMany(r => r.Bookings)
                .WithOne()
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Booking>(builder =>
        {
            builder.ToTable("Bookings");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Start)
                .IsRequired();

            builder.Property(b => b.End)
                .IsRequired();

            builder.Property(b => b.Price)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.HasIndex(b => new { b.RoomId, b.Start, b.End });
        });

        modelBuilder.Entity<Service>(builder =>
        {
            builder.ToTable("Services");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Price)
                .HasPrecision(18, 2)
                .IsRequired();
        });

        modelBuilder.Entity<RoomService>(builder =>
        {
            builder.ToTable("RoomServices");
            builder.HasKey(rs => new { rs.RoomId, rs.ServiceId });

            builder.HasOne(rs => rs.Room)
                .WithMany(r => r.RoomServices)
                .HasForeignKey(rs => rs.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rs => rs.Service)
                .WithMany()
                .HasForeignKey(rs => rs.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<BookingService>(builder =>
        {
            builder.ToTable("BookingServices");

            builder.HasKey(bs => new { bs.BookingId, bs.ServiceId });

            builder.HasOne(bs => bs.Booking)
                .WithMany(b => b.BookingServices)
                .HasForeignKey(bs => bs.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(bs => bs.Service)
                .WithMany()
                .HasForeignKey(bs => bs.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<BookingTimeDiscount>(builder =>
        {
            builder.ToTable("BookingTimeDiscounts");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.DiscountPercentage)
                .HasPrecision(5, 4)
                .IsRequired();

            builder.Property(d => d.From)
                .IsRequired();

            builder.Property(d => d.To)
                .IsRequired();
        });
    }
}