namespace MunicipalityRegistry.Infrastructure.Modules
{
    using Autofac;
    using Municipality;
    using Repositories;

    public class RepositoriesModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            // We could just scan the assembly for classes using Repository<> and registering them against the only interface they implement
            containerBuilder
                .RegisterType<Municipalities>()
                .As<IMunicipalities>();
        }
    }
}
