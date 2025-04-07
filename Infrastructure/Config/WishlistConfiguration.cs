using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
{
    public void Configure(EntityTypeBuilder<Wishlist> builder)
    {
        builder.HasKey(w => w.Id);

        builder.Property(w => w.BuyerEmail)
            .IsRequired();

        builder.Property(w => w.BookId)
            .IsRequired();

        builder.HasOne(w => w.Book)
            .WithMany()
            .HasForeignKey(w => w.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(w => new { w.BuyerEmail, w.BookId }).IsUnique();
    }
}