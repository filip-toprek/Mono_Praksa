INSERT INTO "Role" ("Id", "RoleName", "DateCreated", "DateUpdated", "IsActive")
SELECT
    '7428ffe8-724a-47d7-86e5-b1a728714ba7' AS "Id",
    'Admin' AS "RoleName",
    NOW() AS "DateCreated",
    NOW() AS "DateUpdated",
    true AS "IsActive"
WHERE NOT EXISTS (
    SELECT 1
    FROM "Role"
    WHERE "RoleName" = 'Admin'
);

INSERT INTO "Role" ("Id", "RoleName", "DateCreated", "DateUpdated", "IsActive")
SELECT
    '4e14ad66-9744-4d5d-8573-704b9ee54512' AS "Id",
    'Player' AS "RoleName",
    NOW() AS "DateCreated",
    NOW() AS "DateUpdated",
    true AS "IsActive"
WHERE NOT EXISTS (
    SELECT 1
    FROM "Role"
    WHERE "RoleName" = 'Player'
);

INSERT INTO "Role" ("Id", "RoleName", "DateCreated", "DateUpdated", "IsActive")
SELECT
    'e44e39a7-f1a6-4ae5-8f95-d7a92ae36fe3' AS "Id",
    'Teamleader' AS "RoleName",
    NOW() AS "DateCreated",
    NOW() AS "DateUpdated",
    true AS "IsActive"
WHERE NOT EXISTS (
    SELECT 1
    FROM "Role"
    WHERE "RoleName" = 'Teamleader'
);