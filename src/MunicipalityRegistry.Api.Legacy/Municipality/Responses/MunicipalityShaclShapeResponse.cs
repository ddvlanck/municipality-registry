namespace MunicipalityRegistry.Api.Legacy.Municipality.Responses
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;

    [DataContract(Name = "MunicipalityShaclShape", Namespace = "")]
    public class MunicipalityShaclShapeResponse
    {
        [DataMember(Name = "@context", Order = 1)]
        public readonly MunicipalityShaclContext Context = new MunicipalityShaclContext();

        [DataMember(Name = "@id", Order = 2)]
        public Uri Id { get; set; }

        [DataMember(Name = "@type", Order = 3)]
        public readonly string Type = "sh:NodeShape";

        [DataMember(Name = "sh:property", Order = 4)]
        public readonly List<MunicipalityShaclProperty> Shape = new MunicipalityShaclShape().Properties;
    }

    public class MunicipalityShaclShape
    {
        public readonly List<MunicipalityShaclProperty> Properties = new List<MunicipalityShaclProperty>()
        {
            new MunicipalityShaclProperty
            {
                PropertyPath = "dct:isVersionOf",
                NodeKind = "sh:IRI",
                MinimumCount = 1,
                MaximumCount = 1
            },
            new MunicipalityShaclProperty
            {
                PropertyPath = "prov:generatedAtTime",
                DataType = "xsd:string",
                MinimumCount = 1,
                MaximumCount = 1
            },
            new MunicipalityShaclProperty
            {
                PropertyPath = "adms:versionNotes",
                DataType = "xsd:string",
                MinimumCount = 1,
                MaximumCount = 1
            },
            new MunicipalityShaclProperty
            {
                PropertyPath = "rdf:label",
                DataType = "rdf:langString",
                MinimumCount = 1
            },
            new MunicipalityShaclProperty
            {
                PropertyPath = "br:Gemeente.officieleTaal",
                DataType = "xsd:string",
                MinimumCount = 1
            },
            new MunicipalityShaclProperty
            {
                PropertyPath = "br:Gemeente.faciliteitenTaal",
                DataType = "xsd:string"
            },
            new MunicipalityShaclProperty
            {
                PropertyPath = "br:Gemeente.status",
                DataType = "skos:Concept",
                MinimumCount = 1,
                MaximumCount = 1
            }
        };
    }

    public class MunicipalityShaclProperty
    {
        [JsonProperty("sh:path")]
        public string PropertyPath { get; set; }

        [JsonProperty("sh:datatype", NullValueHandling = NullValueHandling.Ignore)]
        public string? DataType { get; set; }

        [JsonProperty("sh:nodeKind", NullValueHandling = NullValueHandling.Ignore)]
        public string? NodeKind { get; set; }

        [JsonProperty("sh:minimumCount", NullValueHandling = NullValueHandling.Ignore)]
        public int? MinimumCount { get; set; }

        [JsonProperty("sh:maximumCount", NullValueHandling = NullValueHandling.Ignore)]
        public int? MaximumCount { get; set; }
    }
}
