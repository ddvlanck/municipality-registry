namespace MunicipalityRegistry.Projections.Legacy.MunicipalityLinkedDataEventStream
{
    using Be.Vlaanderen.Basisregisters.GrAr.Provenance;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Connector;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.SqlStreamStore;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public static class MunicipalityLinkedDataEventStreamExtensions
    {
        public static async Task CreateNewMunicipalityLinkedDataEventStreamItem<T>(
            this LegacyContext context,
            Guid municipalityId,
            Envelope<T> message,
            Action<MunicipalityLinkedDataEventStreamItem> applyEventInfoOn,
            CancellationToken ct) where T : IHasProvenance
        {
            var municipalityLinkedDataEventStreamItem = await context.LatestPosition(municipalityId, ct);

            if (municipalityLinkedDataEventStreamItem == null)
                throw DatabaseItemNotFound(municipalityId);

            var provenance = message.Message.Provenance;

            var newMunicipalityLinkedDataEventStreamItem = municipalityLinkedDataEventStreamItem.CloneAndApplyEventInfo(
                message.Position,
                message.EventName,
                provenance.Timestamp,
                applyEventInfoOn);

            newMunicipalityLinkedDataEventStreamItem.SetIsComplete();

            await context
                .MunicipalityLinkedDataEventsStream
                .AddAsync(newMunicipalityLinkedDataEventStreamItem, ct);
        }

        public static async Task<MunicipalityLinkedDataEventStreamItem> LatestPosition(
            this LegacyContext context,
            Guid municipalityId,
            CancellationToken ct)
            => context
                   .MunicipalityLinkedDataEventsStream
                   .Local
                   .Where(x => x.MunicipalityId == municipalityId)
                   .OrderByDescending(x => x.Position)
                   .FirstOrDefault()
               ?? await context
                   .MunicipalityLinkedDataEventsStream
                   .Where(x => x.MunicipalityId == municipalityId)
                   .OrderByDescending(x => x.Position)
                   .FirstOrDefaultAsync(ct);

        public static void SetIsComplete(this MunicipalityLinkedDataEventStreamItem linkedDataEventStreamItem)
        {
            var hasName = false;
            if (!string.IsNullOrEmpty(linkedDataEventStreamItem.NameDutch)
                || !string.IsNullOrEmpty(linkedDataEventStreamItem.NameFrench)
                || !string.IsNullOrEmpty(linkedDataEventStreamItem.NameEnglish)
                || !string.IsNullOrEmpty(linkedDataEventStreamItem.NameGerman))
                hasName = true;

            var hasNisCode = false;
            if (!string.IsNullOrEmpty(linkedDataEventStreamItem.NisCode))
                hasNisCode = true;

            var hasLanguage = false;
            if (linkedDataEventStreamItem.OfficialLanguages.Count > 0)
                hasLanguage = true;

            var hasStatus = false;
            if (linkedDataEventStreamItem.Status.HasValue)
                hasStatus = true;

            linkedDataEventStreamItem.IsComplete = hasName && hasNisCode && hasLanguage && hasStatus;
        }

        private static ProjectionItemNotFoundException<MunicipalityLinkedDataEventStreamProjections> DatabaseItemNotFound(Guid municipalityId)
            => new ProjectionItemNotFoundException<MunicipalityLinkedDataEventStreamProjections>(municipalityId.ToString("D"));
    }
}
