namespace MunicipalityRegistry.Importer.Crab
{
    using System;

    public static class MapLogging
    {
        public static Action<string> Log { get; set; }

        static MapLogging() => Log = s => { };
    }
}
