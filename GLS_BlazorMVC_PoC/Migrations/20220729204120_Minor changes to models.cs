using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GLS_BlazorMVC_PoC.Migrations
{
    public partial class Minorchangestomodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "HashedPassword",
                value: "ALEmOSQB+K8f4I2RYgKRFtENhjJWs1xPuLGJVwf1NrWsvBZsB5Y/zqWmde/joDHPUg==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "HashedPassword",
                value: "AFIWJVAUZ3pHU2QNEL62jVBYA5wvJQVg7S2L+f3dyg8ryeQ3x1cHvAhXKL9C0gVSCw==");
        }
    }
}
