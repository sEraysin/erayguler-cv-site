CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    PRIMARY KEY (`MigrationId`)
);

START TRANSACTION;
CREATE TABLE `AspNetRoles` (
    `Id` varchar(255) NOT NULL,
    `Name` varchar(256) NULL,
    `NormalizedName` varchar(256) NULL,
    `ConcurrencyStamp` longtext NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `AspNetUsers` (
    `Id` varchar(255) NOT NULL,
    `FullName` longtext NULL,
    `UserName` varchar(256) NULL,
    `NormalizedUserName` varchar(256) NULL,
    `Email` varchar(256) NULL,
    `NormalizedEmail` varchar(256) NULL,
    `EmailConfirmed` tinyint(1) NOT NULL,
    `PasswordHash` longtext NULL,
    `SecurityStamp` longtext NULL,
    `ConcurrencyStamp` longtext NULL,
    `PhoneNumber` longtext NULL,
    `PhoneNumberConfirmed` tinyint(1) NOT NULL,
    `TwoFactorEnabled` tinyint(1) NOT NULL,
    `LockoutEnd` datetime NULL,
    `LockoutEnabled` tinyint(1) NOT NULL,
    `AccessFailedCount` int NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `BlogPosts` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Title` varchar(180) NOT NULL,
    `Slug` varchar(220) NOT NULL,
    `Summary` varchar(500) NOT NULL,
    `Content` varchar(12000) NOT NULL,
    `IsPublished` tinyint(1) NOT NULL,
    `CreatedAtUtc` datetime(6) NOT NULL,
    `UpdatedAtUtc` datetime(6) NULL,
    `PublishedAtUtc` datetime(6) NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `Certificates` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(220) NOT NULL,
    `Issuer` varchar(100) NOT NULL,
    `DateText` varchar(80) NOT NULL,
    `DisplayOrder` int NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `Educations` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `School` varchar(160) NOT NULL,
    `Degree` varchar(160) NOT NULL,
    `Field` varchar(160) NOT NULL,
    `Grade` varchar(40) NOT NULL,
    `DateRange` varchar(80) NOT NULL,
    `Description` varchar(1500) NOT NULL,
    `DisplayOrder` int NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `Experiences` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Title` varchar(140) NOT NULL,
    `Company` varchar(160) NOT NULL,
    `DateRange` varchar(80) NOT NULL,
    `Description` varchar(3000) NOT NULL,
    `DisplayOrder` int NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `Messages` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `FullName` varchar(120) NOT NULL,
    `Email` varchar(160) NOT NULL,
    `Subject` varchar(160) NOT NULL,
    `Body` varchar(4000) NOT NULL,
    `CreatedAtUtc` datetime(6) NOT NULL,
    `IsRead` tinyint(1) NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `Projects` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(160) NOT NULL,
    `Technologies` varchar(240) NOT NULL,
    `Description` varchar(2000) NOT NULL,
    `Url` varchar(240) NULL,
    `DisplayOrder` int NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `SiteProfiles` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `FirstName` varchar(80) NOT NULL,
    `LastName` varchar(80) NOT NULL,
    `Title` varchar(120) NOT NULL,
    `Location` varchar(180) NOT NULL,
    `Phone` varchar(30) NOT NULL,
    `Email` varchar(120) NOT NULL,
    `Language` varchar(120) NOT NULL,
    `About` varchar(3000) NOT NULL,
    `ImageUrl` varchar(240) NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `Skills` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(80) NOT NULL,
    `Category` varchar(80) NOT NULL,
    `Level` int NOT NULL,
    `DisplayOrder` int NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `SocialLinks` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(60) NOT NULL,
    `Url` varchar(240) NOT NULL,
    `IconClass` varchar(80) NOT NULL,
    `IsActive` tinyint(1) NOT NULL,
    `DisplayOrder` int NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `AspNetRoleClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `RoleId` varchar(255) NOT NULL,
    `ClaimType` longtext NULL,
    `ClaimValue` longtext NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AspNetUserClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` varchar(255) NOT NULL,
    `ClaimType` longtext NULL,
    `ClaimValue` longtext NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AspNetUserLogins` (
    `LoginProvider` varchar(255) NOT NULL,
    `ProviderKey` varchar(255) NOT NULL,
    `ProviderDisplayName` longtext NULL,
    `UserId` varchar(255) NOT NULL,
    PRIMARY KEY (`LoginProvider`, `ProviderKey`),
    CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AspNetUserRoles` (
    `UserId` varchar(255) NOT NULL,
    `RoleId` varchar(255) NOT NULL,
    PRIMARY KEY (`UserId`, `RoleId`),
    CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AspNetUserTokens` (
    `UserId` varchar(255) NOT NULL,
    `LoginProvider` varchar(255) NOT NULL,
    `Name` varchar(255) NOT NULL,
    `Value` longtext NULL,
    PRIMARY KEY (`UserId`, `LoginProvider`, `Name`),
    CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
);

CREATE INDEX `IX_AspNetRoleClaims_RoleId` ON `AspNetRoleClaims` (`RoleId`);

CREATE UNIQUE INDEX `RoleNameIndex` ON `AspNetRoles` (`NormalizedName`);

CREATE INDEX `IX_AspNetUserClaims_UserId` ON `AspNetUserClaims` (`UserId`);

CREATE INDEX `IX_AspNetUserLogins_UserId` ON `AspNetUserLogins` (`UserId`);

CREATE INDEX `IX_AspNetUserRoles_RoleId` ON `AspNetUserRoles` (`RoleId`);

CREATE INDEX `EmailIndex` ON `AspNetUsers` (`NormalizedEmail`);

CREATE UNIQUE INDEX `UserNameIndex` ON `AspNetUsers` (`NormalizedUserName`);

CREATE UNIQUE INDEX `IX_BlogPosts_Slug` ON `BlogPosts` (`Slug`);

CREATE INDEX `IX_Messages_CreatedAtUtc` ON `Messages` (`CreatedAtUtc`);

CREATE INDEX `IX_SocialLinks_Name` ON `SocialLinks` (`Name`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20260622213309_InitialMySqlCodeFirst', '10.0.7');

COMMIT;

