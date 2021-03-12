namespace MunicipalityRegistry.Api.Legacy.Municipality.Query
{
    using Be.Vlaanderen.Basisregisters.Api.Search;
    using Be.Vlaanderen.Basisregisters.Api.Search.Filtering;
    using Be.Vlaanderen.Basisregisters.Api.Search.Sorting;
    using Microsoft.EntityFrameworkCore;
    using MunicipalityRegistry.Projections.Legacy;
    using MunicipalityRegistry.Projections.Legacy.MunicipalityLinkedDataEventStream;
    using NodaTime;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public class MunicipalityLinkedDataEventStreamQueryResult
    {
        public long Position { get; }
        public string ChangeType { get; }
        public Instant GeneratedAtTime { get; }
        public string NisCode { get; }

        public string? NameDutch { get; }
        public string? NameFrench { get; }
        public string? NameEnglish { get; }
        public string? NameGerman { get; }

        public MunicipalityStatus Status { get; }

        public IEnumerable<Language> OfficialLanguages { get; } = new List<Language>();
        public IEnumerable<Language>? FacilitiesLanguages { get; } = new List<Language>();

        public MunicipalityLinkedDataEventStreamQueryResult(
            long position,
            string changeType,
            Instant generatedAtTime,
            string nisCode,
            MunicipalityStatus status,
            IEnumerable<Language> officialLanguages)
        {
            Position = position;
            ChangeType = changeType;
            GeneratedAtTime = generatedAtTime;
            NisCode = nisCode;

            Status = status;
            OfficialLanguages = officialLanguages;
        }

        public MunicipalityLinkedDataEventStreamQueryResult(
            long position,
            string changeType,
            Instant generatedAtTime,
            string nisCode,
            MunicipalityStatus status,
            IEnumerable<Language> officialLanguages,
            IEnumerable<Language>? facilitiesLanguages,
            string? nameDutch,
            string? nameFrench,
            string? nameEnglish,
            string? nameGerman) : this(
                position,
                changeType,
                generatedAtTime,
                nisCode,
                status,
                officialLanguages)
        {
            FacilitiesLanguages = facilitiesLanguages;

            NameDutch = nameDutch;
            NameFrench = nameFrench;
            NameEnglish = nameEnglish;
            NameGerman = nameGerman;
        }
    }

    public class MunicipalityLinkedDataEventStreamQuery : Query<MunicipalityLinkedDataEventStreamItem, MunicipalityLinkedDataEventStreamFilter, MunicipalityLinkedDataEventStreamQueryResult>
    {
        private readonly LegacyContext _context;

        public MunicipalityLinkedDataEventStreamQuery(LegacyContext context)
            => _context = context;

        protected override ISorting Sorting => new MunicipalityLinkedDataEventStreamSorting();

        protected override Expression<Func<MunicipalityLinkedDataEventStreamItem, MunicipalityLinkedDataEventStreamQueryResult>> Transformation
        {
            get
            {
                return linkedDataEventstreamItem => new MunicipalityLinkedDataEventStreamQueryResult(
                    linkedDataEventstreamItem.Position,
                    linkedDataEventstreamItem.ChangeType,
                    linkedDataEventstreamItem.EventGeneratedAtTime,
                    linkedDataEventstreamItem.NisCode,
                    (MunicipalityStatus)linkedDataEventstreamItem.Status,
                    linkedDataEventstreamItem.OfficialLanguages,
                    linkedDataEventstreamItem.FacilitiesLanguages,
                    linkedDataEventstreamItem.NameDutch,
                    linkedDataEventstreamItem.NameFrench,
                    linkedDataEventstreamItem.NameEnglish,
                    linkedDataEventstreamItem.NameGerman);
            }
        }

        protected override IQueryable<MunicipalityLinkedDataEventStreamItem> Filter(FilteringHeader<MunicipalityLinkedDataEventStreamFilter> filtering)
            =>_context
            .MunicipalityLinkedDataEventsStream
            .Where(x => x.IsComplete == true)
            .OrderBy(x => x.Position)
            .AsNoTracking();
    }

    internal class MunicipalityLinkedDataEventStreamSorting: ISorting
    {
        public IEnumerable<string> SortableFields { get; } = new[]
        {
            nameof(MunicipalityLinkedDataEventStreamItem.Position)
        };

        public SortingHeader DefaultSortingHeader { get; } = new SortingHeader(nameof(MunicipalityLinkedDataEventStreamItem.Position), SortOrder.Ascending);
    }

    public class MunicipalityLinkedDataEventStreamFilter
    {
        public int PageNumber { get; set; }
    }
}
