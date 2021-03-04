using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MunicipalityRegistry.Projections.Legacy.Migrations
{
    public partial class AddTableForLinkedDataEventStream : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MunicipalityLinkedDataEventStream",
                schema: "MunicipalityRegistryLegacy",
                columns: table => new
                {
                    Position = table.Column<long>(type: "bigint", nullable: false),
                    MunicipalityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NisCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameDutch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameFrench = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameGerman = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    EventGeneratedAtTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false),
                    FacilitiesLanguages = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfficialLanguages = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MunicipalityLinkedDataEventStream", x => x.Position)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateIndex(
                name: "CI_MunicipalityLinkedDataEventStream_Position",
                schema: "MunicipalityRegistryLegacy",
                table: "MunicipalityLinkedDataEventStream",
                column: "Position");

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalityLinkedDataEventStream_MunicipalityId",
                schema: "MunicipalityRegistryLegacy",
                table: "MunicipalityLinkedDataEventStream",
                column: "MunicipalityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MunicipalityLinkedDataEventStream",
                schema: "MunicipalityRegistryLegacy");
        }
    }
}
