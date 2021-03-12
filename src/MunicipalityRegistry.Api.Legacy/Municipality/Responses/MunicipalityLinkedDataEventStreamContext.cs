namespace MunicipalityRegistry.Api.Legacy.Municipality.Responses
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;

    [DataContract(Name = "MunicipalityLinkedDataEventStreamContext", Namespace = "")]
    public class MunicipalityLinkedDataEventStreamContext
    {
        [DataMember(Name = "tree")]
        public readonly Uri HypermediaSpecificationUri = new Uri("https://w3id.org/tree#");

        [DataMember(Name = "skos")]
        public readonly Uri CodelistsUri = new Uri("http://www.w3.org/2004/02/skos/core#");

        [DataMember(Name = "rdf")]
        public readonly Uri RdfUri = new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#");

        [DataMember(Name = "xsd")]
        public readonly Uri XmlSchemaUri = new Uri("http://www.w3.org/2001/XMLSchema#");

        [DataMember(Name = "prov")]
        public readonly Uri ProvenanceUri = new Uri("http://www.w3.org/ns/prov#");

        [DataMember(Name = "dct")]
        public readonly Uri MetadataTermsUri = new Uri("http://purl.org/dc/terms/");

        [DataMember(Name = "adms")]
        public readonly Uri AssetDescription = new Uri("http://www.w3.org/ns/adms#");

        [DataMember(Name = "adres")]
        public readonly Uri OsloAddressUri = new Uri("https://data.vlaanderen.be/ns/adres#");

        [DataMember(Name = "br")]
        public readonly Uri BaseRegistryImplementationModelUri = new Uri("https://basisregisters.vlaanderen.be/ns/adres#");

        [DataMember(Name = "items")]
        public readonly string ItemsDefinitionUri = "tree:member";

        [DataMember(Name = "viewOf")]
        public readonly TreeCollectionRelationContext ViewOf = new TreeCollectionRelationContext();

        [DataMember(Name = "generatedAtTime")]
        public readonly ProvenanceContext Provenance = new ProvenanceContext();

        [DataMember(Name = "eventName")]
        public readonly string EventNameUri = "adms:versionNotes";

        [DataMember(Name = "isVersionOf")]
        public readonly ParentInformationContext IsVersionOf = new ParentInformationContext();

        [DataMember(Name = "Gemeente")]
        [JsonProperty(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        public readonly Uri MunicipalityType = new Uri("https://data.vlaanderen.be/ns/generiek#Gemeente");

        [DataMember(Name = "status")]
        public readonly PropertyOverride MunicipalityStatus = new PropertyOverride
        {
            Id = "br:Gemeente.status",
            Type = "skos:concept"
        };

        [DataMember(Name = "gemeentenaam")]
        public readonly PropertyOverride MunicipalityName = new PropertyOverride
        {
            Id = "rdf:label",
            Type = "rdf:langString"
        };

        [DataMember(Name = "officieleTaal")]
        public readonly PropertyOverride OfficialLanguage = new PropertyOverride
        {
            Id = "br:Gemeente.officieleTaal",
            Type = "xsd:string"
        };

        [DataMember(Name = "faciliteitenTaal")]
        public readonly PropertyOverride FacilityLanguage = new PropertyOverride
        {
            Id = "br:Gemeente.faciliteitenTaal",
            Type = "xsd:string"
        };
    }

    public class TreeCollectionRelationContext
    {
        [JsonProperty("@reverse")]
        public readonly string ReverseRelation = "tree:view";

        [JsonProperty("@type")]
        public readonly string Type = "@id";
    }

    public class ProvenanceContext
    {
        [JsonProperty("@id")]
        public readonly Uri Id = new Uri("prov:generatedAtTime");

        [JsonProperty("@type")]
        public readonly string Type = "xsd:dateTime";
    }

    public class ParentInformationContext
    {
        [JsonProperty("@id")]
        public readonly Uri Id = new Uri("dct:isVersionOf");

        [JsonProperty("@type")]
        public readonly string Type = "@id";
    }

    public class PropertyOverride
    {
        [JsonProperty("@id", NullValueHandling = NullValueHandling.Ignore)]
        public string? Id { get; set; }

        [JsonProperty("@type")]
        public string Type { get; set; }
    }

    public class MunicipalityShaclContext
    {
        [JsonProperty("sh")]
        public readonly Uri ShaclUri = new Uri("https://www.w3.org/ns/shacl#");

        [JsonProperty("rdf")]
        public readonly Uri RdfUri = new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#");

        [JsonProperty("xsd")]
        public readonly Uri XmlSchemaUri = new Uri("http://www.w3.org/2001/XMLSchema#");

        [JsonProperty("skos")]
        public readonly Uri CodelistUri = new Uri("http://www.w3.org/2004/02/skos/core#");

        [JsonProperty("prov")]
        public readonly Uri ProvenanceUri = new Uri("http://www.w3.org/ns/prov#");

        [JsonProperty("dct")]
        public readonly Uri MetadataTermsUri = new Uri("http://purl.org/dc/terms/");

        [JsonProperty("adms")]
        public readonly Uri AssetDescription = new Uri("http://www.w3.org/ns/adms#");

        [JsonProperty("adres")]
        public readonly Uri OsloAddressUri = new Uri("https://data.vlaanderen.be/ns/adres#");

        [JsonProperty("br")]
        public readonly Uri BaseRegistryImplementationModelUri = new Uri("https://basisregisters.vlaanderen.be/ns/adres#");

        [JsonProperty("sh:datatype")]
        public readonly ShaclPropertyExtension DataTypeExtended = new ShaclPropertyExtension();

        [JsonProperty("sh:nodeKind")]
        public readonly ShaclPropertyExtension NodeKindExnteded = new ShaclPropertyExtension();

        [JsonProperty("sh:path")]
        public readonly ShaclPropertyExtension PathExtended = new ShaclPropertyExtension();
    }

    public class ShaclPropertyExtension
    {
        [JsonProperty("@type")]
        private readonly string Type = "@id";
    }
}
