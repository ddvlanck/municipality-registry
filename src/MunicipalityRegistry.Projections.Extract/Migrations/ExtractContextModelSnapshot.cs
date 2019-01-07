﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MunicipalityRegistry.Projections.Extract;

namespace MunicipalityRegistry.Projections.Extract.Migrations
{
    [DbContext(typeof(ExtractContext))]
    partial class ExtractContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Be.Vlaanderen.Basisregisters.ProjectionHandling.Runner.ProjectionStates.ProjectionStateItem", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Position");

                    b.HasKey("Name")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("ProjectionStates","MunicipalityRegistryExtract");
                });

            modelBuilder.Entity("MunicipalityRegistry.Projections.Extract.MunicipalityExtractItem", b =>
                {
                    b.Property<Guid?>("MunicipalityId")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("DbaseRecord");

                    b.Property<string>("NisCode");

                    b.HasKey("MunicipalityId")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("NisCode");

                    b.ToTable("Municipality","MunicipalityRegistryExtract");
                });
#pragma warning restore 612, 618
        }
    }
}
