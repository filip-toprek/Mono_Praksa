CREATE TABLE "User" (
  "Id" uuid PRIMARY KEY,
  "FirstName" varchar(50) NOT NULL,
  "LastName" varchar(50) NOT NULL,
  "Username" varchar(15) UNIQUE NOT NULL,
  "PasswordHash" text NOT NULL,
  "RoleId" uuid NOT NULL,
  "CreatedBy" uuid NOT NULL,
  "UpdatedBy" uuid NOT NULL,
  "DateCreated" timestamp NOT NULL,
  "DateUpdated" timestamp NOT NULL,
  "IsActive" bool NOT NULL
);

CREATE TABLE "Role" (
  "Id" uuid PRIMARY KEY,
  "RoleName" varchar(15) NOT NULL,
  "DateCreated" timestamp NOT NULL,
  "DateUpdated" timestamp NOT NULL,
  "IsActive" bool NOT NULL
);

CREATE TABLE "SportCategory" (
  "Id" uuid PRIMARY KEY,
  "CategoryName" varchar(10) NOT NULL,
  "DateCreated" timestamp NOT NULL,
  "DateUpdated" timestamp NOT NULL,
  "IsActive" bool NOT NULL
);

CREATE TABLE "Position" (
  "Id" uuid PRIMARY KEY,
  "PositionName" varchar(10) NOT NULL,
  "DateCreated" timestamp NOT NULL,
  "DateUpdated" timestamp NOT NULL,
  "IsActive" bool NOT NULL
);

CREATE TABLE "County" (
  "Id" uuid PRIMARY KEY,
  "CountyName" varchar(50) NOT NULL,
  "DateCreated" timestamp NOT NULL,
  "DateUpdated" timestamp NOT NULL,
  "IsActive" bool NOT NULL
);

CREATE TABLE "InviteStatus" (
  "Id" uuid PRIMARY KEY,
  "StatusName" varchar(10) NOT NULL,
  "DateCreated" timestamp NOT NULL,
  "DateUpdated" timestamp NOT NULL,
  "IsActive" bool NOT NULL
);

CREATE TABLE "Player" (
  "Id" uuid PRIMARY KEY,
  "UserId" uuid NOT NULL,
  "TeamId" uuid,
  "Height" decimal(5,2),
  "Weight" decimal(5,2),
  "PreferredPositionId" uuid,
  "DOB" date NOT NULL,
  "CountyId" uuid NOT NULL,
  "SportCategoryId" uuid NOT NULL,
  "Description" text,
  "CreatedBy" uuid NOT NULL,
  "UpdatedBy" uuid NOT NULL,
  "DateCreated" timestamp NOT NULL,
  "DateUpdated" timestamp NOT NULL,
  "IsActive" bool NOT NULL
);

CREATE TABLE "TeamLeader" (
  "Id" uuid PRIMARY KEY,
  "UserId" uuid NOT NULL,
  "SportCategoryId" uuid NOT NULL,
  "Description" text,
  "IsAccepted" bool NOT NULL DEFAULT false,
  "CreatedBy" uuid NOT NULL,
  "UpdatedBy" uuid NOT NULL,
  "DateCreated" timestamp NOT NULL,
  "DateUpdated" timestamp NOT NULL,
  "IsActive" bool NOT NULL
);

CREATE TABLE "Team" (
  "Id" uuid PRIMARY KEY,
  "TeamLeaderId" uuid NOT NULL,
  "Name" text NOT NULL,
  "Description" text,
  "CreatedBy" uuid NOT NULL,
  "UpdatedBy" uuid NOT NULL,
  "DateCreated" timestamp NOT NULL,
  "DateUpdated" timestamp NOT NULL,
  "IsActive" bool NOT NULL
);

CREATE TABLE "Tournament" (
  "Id" uuid PRIMARY KEY,
  "Name" varchar(50) NOT NULL,
  "Description" text,
  "NumberOfTeams" integer NOT NULL,
  "Location" varchar(255) NOT NULL,
  "Date" timestamp NOT NULL,
  "CreatedBy" uuid NOT NULL,
  "UpdatedBy" uuid NOT NULL,
  "DateCreated" timestamp NOT NULL,
  "DateUpdated" timestamp NOT NULL,
  "IsActive" bool NOT NULL
);

CREATE TABLE "Match" (
  "Id" uuid PRIMARY KEY,
  "TournamentId" uuid NOT NULL,
  "TeamAwayId" uuid NOT NULL,
  "TeamHomeId" uuid NOT NULL,
  "Result" varchar(50),
  "DateCreated" timestamp NOT NULL,
  "DateUpdated" timestamp NOT NULL,
  "IsActive" bool NOT NULL
);

CREATE TABLE "Comment" (
  "Id" uuid PRIMARY KEY,
  "PlayerId" uuid NOT NULL,
  "Text" text NOT NULL,
  "TournamentId" uuid NOT NULL,
  "CreatedBy" uuid NOT NULL,
  "UpdatedBy" uuid NOT NULL,
  "DateCreated" timestamp NOT NULL,
  "DateUpdated" timestamp NOT NULL,
  "IsActive" bool NOT NULL
);

CREATE TABLE "Attend" (
  "Id" uuid PRIMARY KEY,
  "TournamentId" uuid NOT NULL,
  "TeamId" uuid,
  "IsApproved" bool NOT NULL DEFAULT false,
  "CreatedBy" uuid NOT NULL,
  "UpdatedBy" uuid NOT NULL,
  "DateCreated" timestamp NOT NULL,
  "DateUpdated" timestamp NOT NULL,
  "IsActive" bool NOT NULL
);

CREATE TABLE "Invite" (
  "Id" uuid PRIMARY KEY,
  "PlayerId" uuid NOT NULL,
  "TeamId" uuid,
  "InviteStatus" uuid NOT NULL,
  "CreatedBy" uuid NOT NULL,
  "UpdatedBy" uuid NOT NULL,
  "DateCreated" timestamp NOT NULL,
  "DateUpdated" timestamp NOT NULL,
  "IsActive" bool NOT NULL
);

ALTER TABLE "User" ADD FOREIGN KEY ("RoleId") REFERENCES "Role" ("Id");

ALTER TABLE "Player" ADD FOREIGN KEY ("UserId") REFERENCES "User" ("Id");

ALTER TABLE "Player" ADD FOREIGN KEY ("TeamId") REFERENCES "Team" ("Id");

ALTER TABLE "Player" ADD FOREIGN KEY ("PreferredPositionId") REFERENCES "Position" ("Id");

ALTER TABLE "Player" ADD FOREIGN KEY ("CountyId") REFERENCES "County" ("Id");

ALTER TABLE "Player" ADD FOREIGN KEY ("SportCategoryId") REFERENCES "SportCategory" ("Id");

ALTER TABLE "TeamLeader" ADD FOREIGN KEY ("UserId") REFERENCES "User" ("Id");

ALTER TABLE "TeamLeader" ADD FOREIGN KEY ("SportCategoryId") REFERENCES "SportCategory" ("Id");

ALTER TABLE "Team" ADD FOREIGN KEY ("TeamLeaderId") REFERENCES "TeamLeader" ("Id");

ALTER TABLE "Match" ADD FOREIGN KEY ("TournamentId") REFERENCES "Tournament" ("Id");

ALTER TABLE "Match" ADD FOREIGN KEY ("TeamAwayId") REFERENCES "Team" ("Id");

ALTER TABLE "Match" ADD FOREIGN KEY ("TeamHomeId") REFERENCES "Team" ("Id");

ALTER TABLE "Comment" ADD FOREIGN KEY ("PlayerId") REFERENCES "Player" ("Id");

ALTER TABLE "Comment" ADD FOREIGN KEY ("TournamentId") REFERENCES "Tournament" ("Id");

ALTER TABLE "Attend" ADD FOREIGN KEY ("TournamentId") REFERENCES "Tournament" ("Id");

ALTER TABLE "Attend" ADD FOREIGN KEY ("TeamId") REFERENCES "Team" ("Id");

ALTER TABLE "Invite" ADD FOREIGN KEY ("PlayerId") REFERENCES "Player" ("Id");

ALTER TABLE "Invite" ADD FOREIGN KEY ("TeamId") REFERENCES "Team" ("Id");

ALTER TABLE "Invite" ADD FOREIGN KEY ("InviteStatus") REFERENCES "InviteStatus" ("Id");