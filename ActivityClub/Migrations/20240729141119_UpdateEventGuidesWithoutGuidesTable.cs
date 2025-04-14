using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActivityClub.Migrations
{
    public partial class UpdateEventGuidesWithoutGuidesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Guides')
                BEGIN
                    CREATE TABLE [Guides] (
                        [GuideID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        [FullName] NVARCHAR(MAX) NOT NULL,
                        [Email] NVARCHAR(MAX) NOT NULL,
                        [Password] NVARCHAR(MAX) NOT NULL,
                        [DateOfBirth] DATETIME2 NULL,
                        [JoiningDate] DATETIME2 NULL,
                        [Photo] VARBINARY(MAX) NOT NULL,
                        [Profession] NVARCHAR(MAX) NOT NULL
                    )
                END");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Lookups')
                BEGIN
                    CREATE TABLE [Lookups] (
                        [LookupID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        [Code] NVARCHAR(MAX) NOT NULL,
                        [LookupName] NVARCHAR(MAX) NOT NULL,
                        [Order] INT NOT NULL
                    )
                END");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Members')
                BEGIN
                    CREATE TABLE [Members] (
                        [MemberID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        [FullName] NVARCHAR(MAX) NOT NULL,
                        [Email] NVARCHAR(MAX) NOT NULL,
                        [MemberPassword] NVARCHAR(MAX) NOT NULL,
                        [DateOfBirth] DATETIME2 NULL,
                        [Gender] NVARCHAR(MAX) NOT NULL,
                        [JoiningDate] DATETIME2 NULL,
                        [MobileNumber] NVARCHAR(MAX) NOT NULL,
                        [EmergencyNumber] NVARCHAR(MAX) NOT NULL,
                        [Photo] VARBINARY(MAX) NOT NULL,
                        [Profession] NVARCHAR(MAX) NOT NULL,
                        [Nationality] NVARCHAR(MAX) NOT NULL
                    )
                END");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Users')
                BEGIN
                    CREATE TABLE [Users] (
                        [UserID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        [UserName] NVARCHAR(MAX) NOT NULL,
                        [DateOfBirth] DATETIME2 NULL,
                        [Gender] NVARCHAR(1) NULL,
                        [Email] NVARCHAR(MAX) NOT NULL,
                        [Pass] NVARCHAR(MAX) NOT NULL,
                        [UserRole] NVARCHAR(MAX) NOT NULL
                    )
                END");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Events')
                BEGIN
                    CREATE TABLE [Events] (
                        [EventID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        [EventName] NVARCHAR(MAX) NOT NULL,
                        [EventDes] NVARCHAR(MAX) NOT NULL,
                        [Category] INT NOT NULL,
                        [Destination] NVARCHAR(MAX) NOT NULL,
                        [DateFrom] DATETIME2 NOT NULL,
                        [DateTo] DATETIME2 NOT NULL,
                        [Cost] DECIMAL(18,2) NOT NULL,
                        [Stat] NVARCHAR(MAX) NOT NULL,
                        CONSTRAINT [FK_Events_Lookups_Category] FOREIGN KEY ([Category]) REFERENCES [Lookups] ([LookupID]) ON DELETE CASCADE
                    )
                END");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'EventGuides')
                BEGIN
                    CREATE TABLE [EventGuides] (
                        [EventID] INT NOT NULL,
                        [GuideID] INT NOT NULL,
                        [UserID] INT NULL,
                        PRIMARY KEY ([EventID], [GuideID]),
                        CONSTRAINT [FK_EventGuides_Events_EventID] FOREIGN KEY ([EventID]) REFERENCES [Events] ([EventID]) ON DELETE CASCADE,
                        CONSTRAINT [FK_EventGuides_Guides_GuideID] FOREIGN KEY ([GuideID]) REFERENCES [Guides] ([GuideID]) ON DELETE CASCADE,
                        CONSTRAINT [FK_EventGuides_Users_UserID] FOREIGN KEY ([UserID]) REFERENCES [Users] ([UserID])
                    )
                END");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'EventMembers')
                BEGIN
                    CREATE TABLE [EventMembers] (
                        [EventID] INT NOT NULL,
                        [MemberID] INT NOT NULL,
                        PRIMARY KEY ([EventID], [MemberID]),
                        CONSTRAINT [FK_EventMembers_Events_EventID] FOREIGN KEY ([EventID]) REFERENCES [Events] ([EventID]) ON DELETE CASCADE,
                        CONSTRAINT [FK_EventMembers_Users_MemberID] FOREIGN KEY ([MemberID]) REFERENCES [Users] ([UserID]) ON DELETE CASCADE
                    )
                END");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME = 'EventGuides' AND CONSTRAINT_NAME = 'IX_EventGuides_GuideID')
                BEGIN
                    CREATE INDEX [IX_EventGuides_GuideID] ON [EventGuides]([GuideID])
                END");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'EventGuides' AND COLUMN_NAME = 'UserID')
                BEGIN
                    ALTER TABLE [EventGuides] ADD [UserID] INT NULL
                END");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME = 'EventGuides' AND CONSTRAINT_NAME = 'IX_EventGuides_UserID')
                BEGIN
                    CREATE INDEX [IX_EventGuides_UserID] ON [EventGuides]([UserID])
                END");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME = 'EventMembers' AND CONSTRAINT_NAME = 'IX_EventMembers_MemberID')
                BEGIN
                    CREATE INDEX [IX_EventMembers_MemberID] ON [EventMembers]([MemberID])
                END");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME = 'Events' AND CONSTRAINT_NAME = 'IX_Events_Category')
                BEGIN
                    CREATE INDEX [IX_Events_Category] ON [Events]([Category])
                END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'EventGuides')
                BEGIN
                    DROP TABLE [EventGuides]
                END");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'EventMembers')
                BEGIN
                    DROP TABLE [EventMembers]
                END");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Members')
                BEGIN
                    DROP TABLE [Members]
                END");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Guides')
                BEGIN
                    DROP TABLE [Guides]
                END");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Events')
                BEGIN
                    DROP TABLE [Events]
                END");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Users')
                BEGIN
                    DROP TABLE [Users]
                END");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Lookups')
                BEGIN
                    DROP TABLE [Lookups]
                END");
        }
    }
}
