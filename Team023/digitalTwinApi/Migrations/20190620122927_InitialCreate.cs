using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace digitalTwinApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sensors",
                columns: table => new
                {
                    HardwareId = table.Column<string>(nullable: false),
                    MachineNaam = table.Column<string>(nullable: true),
                    DataType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensors", x => x.HardwareId);
                });

            migrationBuilder.CreateTable(
                name: "Simulations",
                columns: table => new
                {
                    Key = table.Column<string>(nullable: false),
                    ProductieStraat = table.Column<string>(nullable: true),
                    KlantId = table.Column<string>(nullable: true),
                    Bovenwaarde = table.Column<int>(nullable: false),
                    Onderwaarde = table.Column<int>(nullable: false),
                    Datum = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Simulations", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "SensorDatas",
                columns: table => new
                {
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    SensorHardwareId = table.Column<string>(nullable: true),
                    SimulationKey = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorDatas", x => x.TimeStamp);
                    table.ForeignKey(
                        name: "FK_SensorDatas_Sensors_SensorHardwareId",
                        column: x => x.SensorHardwareId,
                        principalTable: "Sensors",
                        principalColumn: "HardwareId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SensorDatas_Simulations_SimulationKey",
                        column: x => x.SimulationKey,
                        principalTable: "Simulations",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SensorDatas_SensorHardwareId",
                table: "SensorDatas",
                column: "SensorHardwareId");

            migrationBuilder.CreateIndex(
                name: "IX_SensorDatas_SimulationKey",
                table: "SensorDatas",
                column: "SimulationKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SensorDatas");

            migrationBuilder.DropTable(
                name: "Sensors");

            migrationBuilder.DropTable(
                name: "Simulations");
        }
    }
}
