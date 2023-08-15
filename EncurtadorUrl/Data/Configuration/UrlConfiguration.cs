using EncurtadorUrl.Data.Common;
using EncurtadorUrl.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EncurtadorUrl.Data.Configuration
{
    public class UrlConfiguration : IEntityTypeConfiguration<UrlModel>
    {
        public void Configure(EntityTypeBuilder<UrlModel> builder)
        {
            builder.ToTable("tb_url");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("Id").ValueGeneratedOnAdd();

            builder.Property(x => x.Hits).HasColumnName("Hits");
            builder.Property(x => x.Url).HasColumnName("Url").HasMaxLength(500).IsRequired();
            builder.Property(x => x.ShortUrl).HasColumnName("ShortUrl").HasMaxLength(500).IsRequired();
        }
    }
}
