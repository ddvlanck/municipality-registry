namespace MunicipalityRegistry.Projections.Legacy.MunicipalityLinkedDataEventStream
{
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Runner.MigrationExtensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using MunicipalityRegistry.Infrastructure;
    using Newtonsoft.Json;
    using NodaTime;
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class MunicipalityLinkedDataEventStreamItem : MunicipalityLanguagesBase
    {
        public long Position { get; set; }

        public Guid? MunicipalityId { get; set; }
        public string? NisCode { get; set; }
        public string? ChangeType { get; set; }

        public string? NameDutch { get; set; }
        public string? NameFrench { get; set; }
        public string? NameGerman { get; set; }
        public string? NameEnglish { get; set; }

        public MunicipalityStatus? Status { get; set; }

        public DateTimeOffset EventGeneratedAtTimeAsDatetimeOffset { get; set; }

        public Instant EventGeneratedAtTime
        {
            get => Instant.FromDateTimeOffset(EventGeneratedAtTimeAsDatetimeOffset);
            set => EventGeneratedAtTimeAsDatetimeOffset = value.ToDateTimeOffset();
        }

        public bool IsComplete { get; set; }
        public string ObjectHash { get; set; }

        public MunicipalityLinkedDataEventStreamItem CloneAndApplyEventInfo(
            long newPosition,
            string eventName,
            Instant lastChangedOn,
            Action<MunicipalityLinkedDataEventStreamItem> editFunc)
        {
            var newItem = new MunicipalityLinkedDataEventStreamItem
            {
                Position = newPosition,

                MunicipalityId = MunicipalityId,
                NisCode = NisCode,
                ChangeType = eventName,

                OfficialLanguages = OfficialLanguages,
                FacilitiesLanguages = FacilitiesLanguages,

                NameDutch = NameDutch,
                NameEnglish = NameEnglish,
                NameFrench = NameFrench,
                NameGerman = NameGerman,

                Status = Status,

                EventGeneratedAtTime = lastChangedOn
            };

            editFunc(newItem);
            newItem.SetObjectHash();

            return newItem;
        }

        public void SetObjectHash()
        {
            ObjectHash = string.Empty;
            var objectString = JsonConvert.SerializeObject(this);

            using var md5Hash = MD5.Create();
            var hashBytes = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(objectString));
            ObjectHash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
        }
    }

    public class MunicipalityLinkedDataEventStreamItemConfiguration : IEntityTypeConfiguration<MunicipalityLinkedDataEventStreamItem>
    {
        private const string TableName = "Municipality";

        public void Configure(EntityTypeBuilder<MunicipalityLinkedDataEventStreamItem> b)
        {
            b.ToTable(TableName, Schema.LinkedDataEventStream)
               .HasKey(x => x.Position)
               .IsClustered();

            b.Property(x => x.Position).ValueGeneratedNever();
            b.HasIndex(x => x.Position).IsColumnStore($"CI_{TableName}_Position");

            b.Property(x => x.MunicipalityId).IsRequired();
            b.Property(x => x.NisCode);
            b.Property(x => x.ChangeType);

            b.Property(x => x.NameDutch);
            b.Property(x => x.NameFrench);
            b.Property(x => x.NameGerman);
            b.Property(x => x.NameEnglish);
            b.Property(x => x.Status);
            b.Property(x => x.IsComplete);

            b.Property(MunicipalityLanguagesBase.OfficialLanguagesBackingPropertyName)
                .HasColumnName("OfficialLanguages");

            b.Property(MunicipalityLanguagesBase.FacilitiesLanguagesBackingPropertyName)
                .HasColumnName("FacilitiesLanguages");

            b.Property(x => x.EventGeneratedAtTimeAsDatetimeOffset).HasColumnName("EventGeneratedAtTime");
            b.Property(x => x.ObjectHash).HasColumnName("ObjectIdentifier");

            b.Ignore(x => x.EventGeneratedAtTime);
            b.Ignore(x => x.OfficialLanguages);
            b.Ignore(x => x.FacilitiesLanguages);

            b.HasIndex(x => x.MunicipalityId);
        }
    }
}
