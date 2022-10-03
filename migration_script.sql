CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;

CREATE TABLE "Todos" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Todos" PRIMARY KEY AUTOINCREMENT,
    "Title" TEXT NULL,
    "Done" INTEGER NOT NULL
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20211013154057_InicitialCreation', '5.0.11');

COMMIT;

BEGIN TRANSACTION;

ALTER TABLE "Todos" ADD "Date" TEXT NOT NULL DEFAULT '0001-01-01 00:00:00';

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20211013154545_AddedDateTime', '5.0.11');

COMMIT;

