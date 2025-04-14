using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActivityClub.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Events_Lookups_Category')
                ALTER TABLE Events DROP CONSTRAINT FK_Events_Lookups_Category;
            ");

            migrationBuilder.Sql("IF COL_LENGTH('Events', 'Category') IS NOT NULL EXEC sp_rename 'Events.Category', 'CategoryLookupID', 'COLUMN';");

            migrationBuilder.Sql("IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Events_Category') EXEC sp_rename N'Events.IX_Events_Category', N'IX_Events_CategoryLookupID', N'INDEX';");

            migrationBuilder.AlterColumn<string>(
                name: "Profession",
                table: "Guides",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Photo",
                table: "Guides",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.Sql(@"
                IF COL_LENGTH('Events', 'LookupID') IS NULL
                ALTER TABLE Events ADD LookupID int NULL;
            ");

            migrationBuilder.Sql(@"
                IF COL_LENGTH('EventGuides', 'UserID1') IS NULL
                ALTER TABLE EventGuides ADD UserID1 int NULL;
            ");

            migrationBuilder.CreateIndex(
                name: "IX_Events_LookupID",
                table: "Events",
                column: "LookupID");

            migrationBuilder.CreateIndex(
                name: "IX_EventGuides_UserID1",
                table: "EventGuides",
                column: "UserID1");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Events_Lookups_CategoryLookupID')
                ALTER TABLE Events ADD CONSTRAINT FK_Events_Lookups_CategoryLookupID FOREIGN KEY (CategoryLookupID) REFERENCES Lookups (LookupID) ON DELETE CASCADE;
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Events_Lookups_LookupID')
                ALTER TABLE Events ADD CONSTRAINT FK_Events_Lookups_LookupID FOREIGN KEY (LookupID) REFERENCES Lookups (LookupID);
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_EventGuides_Users_UserID1')
                ALTER TABLE EventGuides ADD CONSTRAINT FK_EventGuides_Users_UserID1 FOREIGN KEY (UserID1) REFERENCES Users (UserID);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventGuides_Users_UserID1",
                table: "EventGuides");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Lookups_CategoryLookupID",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Lookups_LookupID",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_LookupID",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_EventGuides_UserID1",
                table: "EventGuides");

            migrationBuilder.Sql(@"
                IF COL_LENGTH('Events', 'LookupID') IS NOT NULL
                ALTER TABLE Events DROP COLUMN LookupID;
            ");

            migrationBuilder.Sql(@"
                IF COL_LENGTH('EventGuides', 'UserID1') IS NOT NULL
                ALTER TABLE EventGuides DROP COLUMN UserID1;
            ");

            migrationBuilder.Sql("IF COL_LENGTH('Events', 'CategoryLookupID') IS NOT NULL EXEC sp_rename 'Events.CategoryLookupID', 'Category', 'COLUMN';");

            migrationBuilder.Sql("IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Events_CategoryLookupID') EXEC sp_rename N'Events.IX_Events_CategoryLookupID', N'IX_Events_Category', N'INDEX';");

            migrationBuilder.AlterColumn<string>(
                name: "Profession",
                table: "Guides",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "Photo",
                table: "Guides",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Lookups_Category",
                table: "Events",
                column: "Category",
                principalTable: "Lookups",
                principalColumn: "LookupID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
