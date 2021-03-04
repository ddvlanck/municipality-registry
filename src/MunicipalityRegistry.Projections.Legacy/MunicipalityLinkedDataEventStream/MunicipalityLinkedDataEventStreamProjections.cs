namespace MunicipalityRegistry.Projections.Legacy.MunicipalityLinkedDataEventStream
{
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Connector;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.SqlStreamStore;
    using MunicipalityRegistry.Municipality.Events;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MunicipalityLinkedDataEventStreamProjections : ConnectedProjection<LegacyContext>
    {
        public MunicipalityLinkedDataEventStreamProjections()
        {
            When<Envelope<MunicipalityWasRegistered>>(async (context, message, ct) =>
            {
                var newMunicipalityLinkedDataEventStreamItem = new MunicipalityLinkedDataEventStreamItem
                {
                    Position = message.Position,
                    MunicipalityId = message.Message.MunicipalityId,
                    NisCode = message.Message.NisCode,
                    EventGeneratedAtTime = message.Message.Provenance.Timestamp,
                    ChangeType = message.EventName
                };

                newMunicipalityLinkedDataEventStreamItem.SetIsComplete();

                await context
                    .MunicipalityLinkedDataEventsStream
                    .AddAsync(newMunicipalityLinkedDataEventStreamItem, ct);
            });

            When<Envelope<MunicipalityNisCodeWasDefined>>(async (context, message, ct) =>
            {
                await context.CreateNewMunicipalityLinkedDataEventStreamItem(
                    message.Message.MunicipalityId,
                    message,
                    x => x.NisCode = message.Message.NisCode,
                    ct);
            });

            When<Envelope<MunicipalityNisCodeWasCorrected>>(async (context, message, ct) =>
            {
                await context.CreateNewMunicipalityLinkedDataEventStreamItem(
                    message.Message.MunicipalityId,
                    message,
                    x => x.NisCode = message.Message.NisCode,
                    ct);
            });

            When<Envelope<MunicipalityWasNamed>>(async (context, message, ct) =>
            {
                await context.CreateNewMunicipalityLinkedDataEventStreamItem(
                    message.Message.MunicipalityId,
                    message,
                    x => UpdateNameByLanguage(x, message.Message.Language, message.Message.Name),
                    ct);
            });

            When<Envelope<MunicipalityNameWasCorrected>>(async (context, message, ct) =>
            {
                await context.CreateNewMunicipalityLinkedDataEventStreamItem(
                    message.Message.MunicipalityId,
                    message,
                    x => UpdateNameByLanguage(x, message.Message.Language, message.Message.Name),
                    ct);
            });

            When<Envelope<MunicipalityNameWasCleared>>(async (context, message, ct) =>
            {
                await context.CreateNewMunicipalityLinkedDataEventStreamItem(
                    message.Message.MunicipalityId,
                    message,
                    x => UpdateNameByLanguage(x, message.Message.Language, null),
                    ct);
            });

            When<Envelope<MunicipalityNameWasCorrectedToCleared>>(async (context, message, ct) =>
            {
                await context.CreateNewMunicipalityLinkedDataEventStreamItem(
                    message.Message.MunicipalityId,
                    message,
                    x => UpdateNameByLanguage(x, message.Message.Language, null),
                    ct);
            });

            When<Envelope<MunicipalityOfficialLanguageWasAdded>>(async (context, message, ct) =>
            {
                await context.CreateNewMunicipalityLinkedDataEventStreamItem(
                    message.Message.MunicipalityId,
                    message,
                    x => x.AddOfficialLanguage(message.Message.Language),
                    ct);
            });

            When<Envelope<MunicipalityOfficialLanguageWasRemoved>>(async (context, message, ct) =>
            {
                await context.CreateNewMunicipalityLinkedDataEventStreamItem(
                    message.Message.MunicipalityId,
                    message,
                    x => x.RemoveOfficialLanguage(message.Message.Language),
                    ct);
            });

            When<Envelope<MunicipalityFacilityLanguageWasAdded>>(async (context, message, ct) =>
            {
                await context.CreateNewMunicipalityLinkedDataEventStreamItem(
                    message.Message.MunicipalityId,
                    message,
                    x => x.AddFacilitiesLanguage(message.Message.Language),
                    ct);
            });

            When<Envelope<MunicipalityFacilitiesLanguageWasRemoved>>(async (context, message, ct) =>
            {
                await context.CreateNewMunicipalityLinkedDataEventStreamItem(
                    message.Message.MunicipalityId,
                    message,
                    x => x.RemoveFacilitiesLanguage(message.Message.Language),
                    ct);
            });

            When<Envelope<MunicipalityBecameCurrent>>(async (context, message, ct) =>
            {
                await context.CreateNewMunicipalityLinkedDataEventStreamItem(
                    message.Message.MunicipalityId,
                    message,
                    x => x.Status = MunicipalityStatus.Current,
                    ct);
            });

            When<Envelope<MunicipalityWasCorrectedToCurrent>>(async (context, message, ct) =>
            {
                await context.CreateNewMunicipalityLinkedDataEventStreamItem(
                    message.Message.MunicipalityId,
                    message,
                    x => x.Status = MunicipalityStatus.Current,
                    ct);
            });

            When<Envelope<MunicipalityWasRetired>>(async (context, message, ct) =>
            {
                await context.CreateNewMunicipalityLinkedDataEventStreamItem(
                    message.Message.MunicipalityId,
                    message,
                    x => x.Status = MunicipalityStatus.Retired,
                    ct);
            });

            When<Envelope<MunicipalityWasCorrectedToRetired>>(async (context, message, ct) =>
            {
                await context.CreateNewMunicipalityLinkedDataEventStreamItem(
                    message.Message.MunicipalityId,
                    message,
                    x => x.Status = MunicipalityStatus.Retired,
                    ct);
            });
        }

        private static void UpdateNameByLanguage(MunicipalityLinkedDataEventStreamItem municipalityLinkedDataEventStreamItem, Language? language, string name)
        {
            switch (language)
            {
                case Language.Dutch:
                    municipalityLinkedDataEventStreamItem.NameDutch = name;
                    break;

                case Language.French:
                    municipalityLinkedDataEventStreamItem.NameFrench = name;
                    break;

                case Language.German:
                    municipalityLinkedDataEventStreamItem.NameGerman = name;
                    break;

                case Language.English:
                    municipalityLinkedDataEventStreamItem.NameEnglish = name;
                    break;
            }
        }
    }
}
