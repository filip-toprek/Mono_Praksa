INSERT INTO "Position" ("Id", "PositionName", "DateCreated", "DateUpdated", "IsActive")
VALUES 
  ('50129bf7-1579-4ba0-a4a5-6512694f7eaf', 'Goalie', NOW(), NOW(), true),
  ('2dbea048-c4be-43ec-908d-d6a77e896970', 'Forward', NOW(), NOW(), true),
  ('afe82fd9-5731-4d13-ac6c-7d3dc4dd31f2', 'Defender', NOW(), NOW(), true),
  ('2c975b0e-6a4f-40fe-bc43-7e40c2b163e8', 'Midfielder', NOW(), NOW(), true),
  ('7d54479a-dbd0-4021-8b53-72aa8684a060', 'Fill/Any', NOW(), NOW(), true)
ON CONFLICT ("Id") DO NOTHING;
