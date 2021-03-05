namespace MunicipalityRegistry.Api.Legacy.Municipality.Responses
{
    using Be.Vlaanderen.Basisregisters.GrAr.Common;
    using MunicipalityRegistry.Api.Legacy.Infrastructure;
    using Newtonsoft.Json;
    using NodaTime;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;

    [DataContract(Name = "MunicipalityLinkedDataEventStream", Namespace = "")]
    public class MunicipalityLinkedDataEventStreamResponse
    {
        [DataMember(Name = "@context")]
        public readonly object Context = new MunicipalityLinkedDataEventStreamContext();

        [DataMember(Name = "@id")]
        public Uri Id { get; set; }

        [DataMember(Name = "@type")]
        public readonly string Type = "tree:Node";

        [DataMember(Name = "viewOf")]
        public Uri CollectionLink { get; set; }

        [DataMember(Name = "tree:shape")]
        public Uri MunicipalityShape { get; set; }

        [DataMember(Name = "tree:relation")]
        [JsonProperty(Required = Required.AllowNull, NullValueHandling = NullValueHandling.Ignore)]
        public List<HypermediaControl>? HypermediaControls { get; set; }

        [DataMember(Name = "items")]
        public List<MunicipalityVersionObject> Municipalities { get; set; }
    }

    [DataContract(Name = "MunicipalityVersionObject", Namespace = "")]
    public class MunicipalityVersionObject
    {
        [DataMember(Name = "@id", Order = 1)]
        public Uri Id { get; set; }

        [DataMember(Name = "@type", Order = 2)]
        public readonly string Type = "Gemeente";

        [DataMember(Name = "isVersionOf", Order = 3)]
        public Uri IsVersionOf { get; set; }

        [DataMember(Name = "generatedAtTime", Order = 4)]
        public DateTimeOffset GeneratedAtTime { get; set; }

        [DataMember(Name = "eventName", Order = 5)]
        public string ChangeType { get; set; }

        [DataMember(Name = "gemeentenaam", Order = 6)]
        public List<LanguageString> MunicipalityNames { get; set; }

        [DataMember(Name = "officieleTaal", Order = 7)]
        public List<string> OfficialLanguages { get; set; }

        [DataMember(Name = "faciliteitenTaal", Order = 8)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string>? FacilityLanguages { get; set; }

        [DataMember(Name = "status", Order = 9)]
        public Uri Status { get; set; }

        [IgnoreDataMember]
        private LinkedDataEventStreamConfiguration Configuration { get; set; }

        public MunicipalityVersionObject(
            LinkedDataEventStreamConfiguration configuration,
            long position,
            string changeType,
            Instant generatedAtTime,
            string nisCode,
            MunicipalityStatus status,
            IEnumerable<Language> officialLanguages)
        {
            Configuration = configuration;

            ChangeType = changeType;
            GeneratedAtTime = generatedAtTime.ToBelgianDateTimeOffset();

            Id = CreateVersionUri(position);
            IsVersionOf = GetPersistentUri(nisCode);
            Status = GetStatus(status);

            OfficialLanguages = GetLanguageCodes(officialLanguages);
        }

        public MunicipalityVersionObject(
            LinkedDataEventStreamConfiguration configuration,
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
                configuration,
                position,
                changeType,
                generatedAtTime,
                nisCode,
                status,
                officialLanguages)
        {
            FacilityLanguages = GetLanguageCodes(facilitiesLanguages);
            MunicipalityNames = GetMunicipalityNames(nameDutch, nameFrench, nameGerman, nameEnglish);
        }

        private Uri CreateVersionUri(long position)
            => new Uri($"{Configuration.ApiEndpoint}#{position}");

        private Uri GetPersistentUri(string nisCode)
            => new Uri($"{Configuration.DataVlaanderenNamespace}/{nisCode}");

        private Uri GetStatus(MunicipalityStatus status)
        {
            switch (status)
            {
                case MunicipalityStatus.Current:
                    return new Uri("https://data.vlaanderen.be/id/concept/gemeentestatus/inGebruik");

                case MunicipalityStatus.Retired:
                    return new Uri("https://data.vlaanderen.be/id/concept/gemeentestatus/gehistoreerd");

                default:
                    throw new Exception($"Status {status} can not be translated to a URI.");
            }
        }

        private List<LanguageString> GetMunicipalityNames(string? nameDutch, string? nameFrench, string? nameGerman, string? nameEnglish)
        {
            List<LanguageString> municipalityNames = new List<LanguageString>();

            if (!string.IsNullOrEmpty(nameDutch))
                municipalityNames.Add(new LanguageString
                {
                    Value = nameDutch,
                    Language = "nl"
                });

            if (!string.IsNullOrEmpty(nameFrench))
                municipalityNames.Add(new LanguageString
                {
                    Value = nameFrench,
                    Language = "fr"
                });

            if (!string.IsNullOrEmpty(nameGerman))
                municipalityNames.Add(new LanguageString
                {
                    Value = nameGerman,
                    Language = "de"
                });

            if (!string.IsNullOrEmpty(nameEnglish))
                municipalityNames.Add(new LanguageString
                {
                    Value = nameEnglish,
                    Language = "en"
                });

            if (municipalityNames.Count == 0)
                throw new Exception("Municipality should have at least one name");

            return municipalityNames;
        }

        private List<string>? GetLanguageCodes(IEnumerable<Language> languages)
        {
            List<string> languageCodes = new List<string>();

            foreach(var language in languages)
            {
                switch (language)
                {
                    case Language.Dutch:
                        languageCodes.Add("nl");
                        break;

                    case Language.French:
                        languageCodes.Add("fr");
                        break;

                    case Language.German:
                        languageCodes.Add("de");
                        break;

                    case Language.English:
                        languageCodes.Add("en");
                        break;
                }
            }

            return languageCodes.Count > 0 ? languageCodes : null;
        }
    }

    public class LanguageString
    {
        [JsonProperty("@value")]
        public string Value { get; set; }

        [JsonProperty("@language")]
        public string Language { get; set; }
    }
}
