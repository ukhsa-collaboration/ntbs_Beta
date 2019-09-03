using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class PopulateHospitalTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Hospital",
                columns: new[] { "HospitalId", "CountryCode", "Name", "TBServiceCode" },
                values: new object[,]
                {
                    { new Guid("37104a55-4db0-4964-8443-2bc9119c5753"), "W92000004", "ABERAERON HOSPITAL", "TBS0001" },
                    { new Guid("20120fac-8e26-49e2-83cd-0c1ec364fa9e"), "E92000001", "ROYAL HOSPITAL (WOLVERHAMPTON)", "TBS0195" },
                    { new Guid("f788e479-e3de-4372-87f6-00ad90df8132"), "E92000001", "ROYAL HAMPSHIRE COUNTY HOSPITAL", "TBS0194" },
                    { new Guid("9278a04d-c67a-47a6-963d-25048b4fd834"), "E92000001", "ROYAL HALLAMSHIRE HOSPITAL", "TBS0194" },
                    { new Guid("e3e38a75-0cd0-4f91-bd71-653752d7f209"), "W92000004", "ROYAL GWENT HOSPITAL", "TBS0193" },
                    { new Guid("2c36399e-8aba-4be8-99d9-de1a01341f9e"), "W92000004", "ROYAL GLAMORGAN HOSPITAL", "TBS0193" },
                    { new Guid("f6801336-866e-4767-9019-b4b576acb729"), "E92000001", "ROYAL FREE HOSPITAL", "TBS0192" },
                    { new Guid("f5406aa8-f218-4ec3-a457-e2f73789d5c9"), "N92000002", "ROYAL HOSPITAL FOR SICK CHILDREN [BELFAST]", "TBS0195" },
                    { new Guid("18b6e8b8-c327-4f37-9143-c215bad7faf3"), "E92000001", "ROYAL EYE INFIRMARY", "TBS0191" },
                    { new Guid("0a5a2531-1f6c-438b-90d1-427d14b35632"), "E92000001", "ROYAL CORNWALL HOSPITAL (TRELISKE)", "TBS0189" },
                    { new Guid("eaad3369-bcc6-4f59-9e05-a9f8503322da"), "E92000001", "ROYAL BUCKINGHAMSHIRE HOSPITAL", "TBS0188" },
                    { new Guid("aaebaac0-3ace-4ab2-b473-8a09749c7787"), "E92000001", "ROYAL BROMPTON HOSPITAL", "TBS0187" },
                    { new Guid("4f7ba3f9-c2c7-453a-8775-1283615f61ff"), "E92000001", "ROYAL BOURNEMOUTH HOSPITAL", "TBS0186" },
                    { new Guid("c0754abf-09ff-429f-a464-fc9d00834b26"), "E92000001", "ROYAL BOURNEMOUTH GENERAL HOSPITAL", "TBS0185" },
                    { new Guid("978fbf0b-9c22-4c29-bd4d-c3b613429519"), "E92000001", "ROYAL BOURNEMOUTH GENERAL HOSPITAL", "TBS0184" },
                    { new Guid("6bae1060-fac0-4515-8ce5-d09dd5b6eefe"), "E92000001", "ROYAL DEVON & EXETER HOSPITAL (WONFORD)", "TBS0190" },
                    { new Guid("baeb671b-1b0d-4064-b637-87c2dbbd97a3"), "E92000001", "ROYAL BOLTON HOSPITAL", "TBS0183" },
                    { new Guid("f8e5aac4-adb0-4ecf-87d1-851f32559c76"), "E92000001", "ROYAL HOSPITAL HASLAR", "TBS0195" },
                    { new Guid("16a5bc66-9d12-4156-8db9-2b75292bfceb"), "E92000001", "ROYAL LANCASTER INFIRMARY", "TBS0196" },
                    { new Guid("e4d073fa-9f5a-48d4-a0fd-10d4e667a634"), "E92000001", "ROYAL PRESTON HOSPITAL", "TBS0205" },
                    { new Guid("1001ba58-bb01-4745-a1d2-b15ff4070ef0"), "E92000001", "ROYAL ORTHOPAEDIC HOSPITAL", "TBS0204" },
                    { new Guid("2b1d1d3d-1ce0-4a95-9d5b-fbad03e4b75c"), "E92000001", "ROYAL OLDHAM HOSPITAL", "TBS0203" },
                    { new Guid("11b304c6-f0bc-498f-925b-1fc8e1c7031a"), "E92000001", "ROYAL NATIONAL THROAT, NOSE & EAR HOSPITAL", "TBS0202" },
                    { new Guid("a90ee26b-ec10-4047-bf21-ddbff9735e6d"), "E92000001", "ROYAL NATIONAL ORTHOPAEDIC HOSPITAL (STANMORE)", "TBS0201" },
                    { new Guid("c109b0b2-33f3-4d29-8d37-e25623d9a2f6"), "E92000001", "ROYAL NATIONAL ORTHOPAEDIC HOSPITAL (BOLSOVER STREET)", "TBS0200" },
                    { new Guid("64a797c5-6852-4095-97ae-3bd45beb357f"), "E92000001", "ROYAL INFIRMARY OF EDINBURGH", "TBS0196" },
                    { new Guid("79236fd5-e97d-45e7-9a5b-5ee9b623c6d3"), "E92000001", "ROYAL NATIONAL HOSPITAL FOR RHEUMATIC DISEASES", "TBS0200" },
                    { new Guid("ad0e1247-6c1b-4056-bafc-071f67c97550"), "E92000001", "ROYAL MARSDEN HOSPITAL (LONDON)", "TBS0198" },
                    { new Guid("e97e21fb-2810-4664-a21c-cd845b44d4f8"), "E92000001", "ROYAL MANCHESTER CHILDREN'S HOSPITAL", "TBS0197" },
                    { new Guid("dc46aa79-983d-4c10-84c9-1181fddd2f4c"), "E92000001", "ROYAL LONDON HOSPITAL", "TBS0196" },
                    { new Guid("323f047e-5341-424a-886c-0324ab669fe7"), "E92000001", "ROYAL LONDON HOMEOPATHIC HOSPITAL", "TBS0196" },
                    { new Guid("858f0d2c-e3c5-4cff-bd82-c311d8f376c9"), "E92000001", "ROYAL LIVERPOOL UNIVERSITY HOSPITAL", "TBS0196" },
                    { new Guid("c1a0da74-9e02-43fc-beec-14589057478c"), "E92000001", "ROYAL LEAMINGTON SPA REHABILITATION HOSPITAL", "TBS0196" },
                    { new Guid("a9820956-84cd-4373-8e93-04eb01c45c87"), "E92000001", "ROYAL MARSDEN HOSPITAL (SURREY)", "TBS0199" },
                    { new Guid("54f67f43-a30e-4a7d-8248-783119f1a360"), "E92000001", "ROYAL SHREWSBURY HOSPITAL", "TBS0206" },
                    { new Guid("850c1f6b-5bef-4660-9a74-7656f784d86c"), "E92000001", "ROYAL BLACKBURN HOSPITAL", "TBS0182" },
                    { new Guid("2a474e42-4634-4990-84ab-b0ee3ae8327e"), "W92000004", "ROYAL ALEXANDRA HOSPITAL [RHYL]", "TBS0181" },
                    { new Guid("2082a678-403f-4969-8b07-557ce2240409"), "E92000001", "RIVERS HOSPITAL", "TBS0168" },
                    { new Guid("bd77d4f6-5a7e-48e4-a4fb-6c2e6bb41a60"), "E92000001", "RIPON AND DISTRICT COMMUNITY HOSPITAL", "TBS0167" },
                    { new Guid("251ca4b4-7699-4927-a36c-f2d53f737d1f"), "E92000001", "RIDLEY DAY HOSPITAL", "TBS0166" },
                    { new Guid("f0dd939f-2d7c-407c-9573-34a4fa543670"), "E92000001", "RIDGEWAY HOSPITAL", "TBS0165" },
                    { new Guid("31c14107-8e42-43d6-8dfe-bbe5b19f1d4a"), "E92000001", "RIDGE LEA HOSPITAL", "TBS0164" },
                    { new Guid("57e673cc-ed70-442f-bd96-0f261fc88c8c"), "E92000001", "RICHARDSON HOSPITAL", "TBS0163" },
                    { new Guid("805559be-9862-4194-9a37-9a9d93b73be6"), "E92000001", "ROBERT JONES & AGNES HUNT", "TBS0169" },
                    { new Guid("88b3881c-9401-480c-8f95-53fa0ad345c2"), "E92000001", "RIBBLETON HOSPITAL", "TBS0162" },
                    { new Guid("cebbe2c3-f6e8-49a9-adfc-7264fe6f0539"), "E92000001", "RENACRES HOSPITAL", "TBS0160" },
                    { new Guid("2f7deab6-80f3-4f2a-811c-c096bacb650b"), "W92000004", "REDWOOD MEMORIAL HOSPITAL", "TBS0160" },
                    { new Guid("a4da4b5d-3907-4962-88c7-04b387471131"), "E92000001", "REDCLIFFE DAY HOSPITAL", "TBS0160" },
                    { new Guid("7c5a090e-b20b-4dd0-938d-03aabc914eb2"), "E92000001", "RAVENSCOURT PARK HOSPITAL", "TBS0160" },
                    { new Guid("d907b8c5-ea96-4ded-a696-5e906b2537bc"), "E92000001", "RATHBONE HOSPITAL", "TBS0160" },
                    { new Guid("acf1a783-686a-4ef2-a3ca-0e87a7006541"), "E92000001", "RAMSBOTTOM COTTAGE HOSPITAL", "TBS0160" },
                    { new Guid("076c7e3b-c9a2-43d0-942d-558a9d6371e3"), "E92000001", "RETFORD HOSPITAL", "TBS0161" },
                    { new Guid("b8aa918d-233f-4c41-b9ae-be8a8dc8be7a"), "E92000001", "ROYAL BERKSHIRE HOSPITAL", "TBS0182" },
                    { new Guid("8b3b8368-fcf4-4612-b78a-e7a00e7fa826"), "E92000001", "ROBOROUGH DAY HOSPITAL", "TBS0170" },
                    { new Guid("171806a0-98a7-4ae6-bac2-ffcb4f3fc092"), "E92000001", "ROCHFORD COMMUNITY HOSPITAL", "TBS0172" },
                    { new Guid("6a4403f4-4e9d-44d5-b78e-b330547bb781"), "E92000001", "ROYAL ALEXANDRA CHILDREN'S HOSPITAL [BRIGHTON]", "TBS0181" },
                    { new Guid("3cd116f8-c660-46a1-96a0-ef6ce8fb532e"), "E92000001", "ROYAL ALBERT EDWARD INFIRMARY", "TBS0181" },
                    { new Guid("459820f1-9c30-4998-a207-9dbada17108c"), "E92000001", "ROXBOURNE HOSPITAL", "TBS0181" },
                    { new Guid("b95f2b70-0a48-4470-bf16-8ba6fc1a2264"), "E92000001", "ROWLEY REGIS HOSPITAL", "TBS0181" },
                    { new Guid("d7d5c158-2e6f-4133-a601-14d729efb1f5"), "E92000001", "ROWLEY HOSPITAL", "TBS0180" },
                    { new Guid("6a4c0d9b-fcbe-44b5-900f-558b10ed9ecc"), "E92000001", "ROUNDWAY HOSPITAL", "TBS0179" },
                    { new Guid("4c6d94b4-e66c-46f5-8fad-4167a858a9d6"), "E92000001", "ROCHDALE INFIRMARY", "TBS0171" },
                    { new Guid("c8088eb9-9205-42a4-9e5e-92679f36123b"), "E92000001", "ROTHERHAM DISTRICT GENERAL HOSPITAL", "TBS0178" },
                    { new Guid("52399c6d-9d4a-4286-963f-7ec91adf946e"), "E92000001", "ROSSENDALE HOSPITAL", "TBS0176" },
                    { new Guid("abe80c99-0bbf-4e48-933b-874b65358eca"), "E92000001", "ROSS COMMUNITY HOSPITAL", "TBS0176" },
                    { new Guid("39ef6af3-6c00-4d52-83fe-39f398809e46"), "E92000001", "ROSIE HOSPITAL", "TBS0176" },
                    { new Guid("c499b778-7933-49dc-b944-155184fdf522"), "W92000004", "ROOKWOOD HOSPITAL", "TBS0175" },
                    { new Guid("fd8337ea-cb3f-46b0-8586-fdd9dbd64862"), "E92000001", "ROMSEY HOSPITAL", "TBS0174" },
                    { new Guid("af1a9b2b-5a89-4b4b-a434-d58e123afc32"), "E92000001", "ROCKWELL DAY HOSPITAL", "TBS0173" },
                    { new Guid("ba9b7f24-7e91-42ca-abfe-ce530f26aa36"), "E92000001", "ROTHBURY COMMUNITY HOSPITAL", "TBS0177" },
                    { new Guid("179b430d-5d8c-4f21-9ba8-c6a652df0d62"), "E92000001", "RAMPTON HOSPITAL", "TBS0160" },
                    { new Guid("3be91589-f3e1-4200-a956-7973459ded35"), "E92000001", "ROYAL SOUTH HANTS HOSPITAL", "TBS0207" },
                    { new Guid("3522c2e0-f45e-4716-b1c3-de9c70cafee0"), "E92000001", "ROYAL SUSSEX COUNTY HOSPITAL", "TBS0209" },
                    { new Guid("31b6677d-f5df-4101-950d-cd47878edf55"), "E92000001", "SKIPTON GENERAL HOSPITAL", "TBS0250" },
                    { new Guid("ac84273e-bbe7-4720-b731-5892e4ee1b2c"), "E92000001", "SKEGNESS & DISTRICT GENERAL HOSPITAL", "TBS0249" },
                    { new Guid("485bf2da-6079-4160-bd84-8eb30559c63c"), "E92000001", "SITTINGBOURNE MEMORIAL HOSPITAL", "TBS0248" },
                    { new Guid("0d92e7ff-aba9-4e80-a048-a3ed4e0c44f0"), "E92000001", "SIR G B HUNTER MEMORIAL HOSPITAL", "TBS0247" },
                    { new Guid("4485e161-a44c-43fd-b43f-a038f3beb012"), "E92000001", "SIR ALFRED JONES MEMORIAL HOSPITAL (ACUTE)", "TBS0246" },
                    { new Guid("9878d735-1022-4001-86ba-7e53f576f850"), "W92000004", "SINGLETON HOSPITAL", "TBS0245" },
                    { new Guid("98ac8f80-56be-43bc-a433-7e92e0cabeb1"), "E92000001", "SLOANE HOSPITAL", "TBS0251" },
                    { new Guid("4c04567b-5ad0-4660-9186-e6c36734dfcf"), "E92000001", "SHOTLEY BRIDGE HOSPITAL", "TBS0244" },
                    { new Guid("6f637c51-b8aa-4fff-a796-cf69bdaa49f7"), "E92000001", "SHIREHILL HOSPITAL", "TBS0242" },
                    { new Guid("67956308-b9df-4107-b50f-3f6c911005ae"), "E92000001", "SHIPLEY HOSPITAL", "TBS0241" },
                    { new Guid("f1484bb2-5277-45f9-8549-0c4ea1255aff"), "E92000001", "SHEPTON MALLET COMMUNITY HOSPITAL", "TBS0239" },
                    { new Guid("5d3afb26-c8a6-44f6-a00a-f50d23fb220e"), "E92000001", "SHEPPEY COMMUNITY HOSPITAL", "TBS0239" },
                    { new Guid("5e4cfca5-f1da-4233-afe8-f41f9e34392c"), "E92000001", "SHELTON HOSPITAL", "TBS0238" },
                    { new Guid("d20f8b13-6717-47ca-a235-f772a8d16514"), "E92000001", "SHELBURNE HOSPITAL", "TBS0237" },
                    { new Guid("910d3ae1-0c2c-4ed6-baac-0e1e69260969"), "E92000001", "SHIRLEY OAKS HOSPITAL", "TBS0243" },
                    { new Guid("457925d3-db5b-459a-8130-a4d0d367f4ae"), "E92000001", "SHEFFIELD CHILDREN'S HOSPITAL", "TBS0237" },
                    { new Guid("4ca759bf-600e-4e4d-be99-0f5b09f8ad4c"), "E92000001", "SOLIHULL HOSPITAL", "TBS0252" },
                    { new Guid("4cdaac14-f923-4735-91cd-ee695752fee3"), "E92000001", "SOUTH CHESHIRE PRIVATE HOSPITAL", "TBS0253" },
                    { new Guid("6344cd67-0f25-4288-9026-6af917df5790"), "E92000001", "SOUTHPORT GENERAL INFIRMARY", "TBS0262" },
                    { new Guid("d04eede9-ba6f-4190-9eaf-e93398312f5c"), "E92000001", "SOUTHPORT & FORMBY DISTRICT GENERAL HOSPITAL", "TBS0262" },
                    { new Guid("bcfc88e8-ead4-4e40-9d7e-be7896adbd4a"), "E92000001", "SOUTHMEAD HOSPITAL", "TBS0262" },
                    { new Guid("84576348-2dca-4487-b0a9-46268f222fa6"), "E92000001", "SOUTHLANDS HOSPITAL", "TBS0262" },
                    { new Guid("33e41bc7-1a80-4fb3-a1b8-1ae4e9543f4e"), "E92000001", "SOUTHEND HOSPITAL", "TBS0262" },
                    { new Guid("00bddb9c-07f5-46d0-afb0-12f66bb85ab9"), "E92000001", "SOUTHAMPTON GENERAL HOSPITAL", "TBS0261" },
                    { new Guid("d1a119d9-92bd-4863-92b5-a03536c18cb1"), "E92000001", "SOMERFIELD HOSPITAL", "TBS0253" },
                    { new Guid("b9b71847-9616-4993-a609-93801703ae02"), "N92000002", "SOUTH TYRONE HOSPITAL", "TBS0260" },
                    { new Guid("90cd195c-ca0b-41fe-a04b-70d2c90c15b7"), "E92000001", "SOUTH SHORE HOSPITAL", "TBS0259" },
                    { new Guid("f13b200d-fd21-4078-a951-fc5fc331718d"), "E92000001", "SOUTH PETHERTON HOSPITAL", "TBS0258" },
                    { new Guid("766271cd-8bb8-4b14-9c4d-50424a30f6bc"), "W92000004", "SOUTH PEMBROKESHIRE HOSPITAL HEALTH & SOCIAL CARE RESOURCE CENTRE", "TBS0257" },
                    { new Guid("5b41832a-3656-4a4c-b3a7-e6d3d67d76d8"), "E92000001", "SOUTH MOOR HOSPITAL", "TBS0256" },
                    { new Guid("4970ea1f-b07d-4290-98c7-1cbfc055829e"), "E92000001", "SOUTH MOLTON HOSPITAL", "TBS0255" },
                    { new Guid("a22dda9e-0c7d-475f-ba10-6b76d549f920"), "E92000001", "SOUTH HAMS HOSPITAL", "TBS0254" },
                    { new Guid("d3bf3541-5f1e-4bdb-b779-0225fef56501"), "E92000001", "SOUTH TYNESIDE DISTRICT HOSPITAL", "TBS0259" },
                    { new Guid("deda56b0-32e1-409b-8109-27f96c7308f7"), "E92000001", "ROYAL SURREY COUNTY HOSPITAL", "TBS0208" },
                    { new Guid("37187ac2-5da3-46a1-b6b1-2dff7c8c9720"), "E92000001", "SEVENOAKS HOSPITAL", "TBS0237" },
                    { new Guid("d27fe1ae-3758-48db-9da9-7c0ea3c6c5ca"), "E92000001", "SELBY & DISTRICT WAR MEMORIAL HOSPITAL", "TBS0235" },
                    { new Guid("af29e41e-2f82-41ac-983e-ff1df2294e58"), "E92000001", "RYE MEMORIAL HOSPITAL", "TBS0223" },
                    { new Guid("f128c767-8664-40b9-a086-fd929ae133a2"), "E92000001", "RUTSON HOSPITAL", "TBS0222" },
                    { new Guid("03f4894f-0ef3-4c23-8701-245f83d9e66c"), "W92000004", "RUTHIN COMMUNITY HOSPITAL", "TBS0221" },
                    { new Guid("29d4ded5-bcfb-4cfd-b6da-1f3ef98d3a64"), "E92000001", "RUTH LANCASTER JAMES HOSPITAL (ALSTON MATERNITY)", "TBS0220" },
                    { new Guid("2284b28d-6722-4d27-b30b-8ec34e413985"), "E92000001", "RUSSELLS HALL HOSPITAL", "TBS0219" },
                    { new Guid("13958bea-c1bc-4ccf-ad03-e9bbfa016502"), "E92000001", "RUSHDEN MEMORIAL CLINIC", "TBS0218" },
                    { new Guid("d8705a5e-3662-406d-a540-afa447aabca3"), "E92000001", "RYHOPE GENERAL HOSPITAL", "TBS0224" },
                    { new Guid("50757c7c-705d-44e7-968e-c3189b9c94a1"), "E92000001", "RUSHDEN HOSPITAL", "TBS0217" },
                    { new Guid("18f24540-166d-4d59-84d7-61aa1ccb46c0"), "E92000001", "RUNNEYMEDE HOSPITAL", "TBS0215" },
                    { new Guid("768011c9-e0c4-4b1b-84a3-317c4bf2f402"), "E92000001", "ROYSTON HOSPITAL", "TBS0214" },
                    { new Guid("213920fc-bf88-43e1-858f-bf29f08c4ae1"), "E92000001", "ROYAL VICTORIA INFIRMARY [NEWCASTLE]", "TBS0213" },
                    { new Guid("1b5ef984-a883-4248-9285-7e02be2db1c3"), "E92000001", "ROYAL VICTORIA HOSPITAL [FOLKESTONE]", "TBS0212" },
                    { new Guid("92908614-3504-420f-866f-8f53d2d796ae"), "N92000002", "ROYAL VICTORIA HOSPITAL [BELFAST]", "TBS0211" },
                    { new Guid("c1e0fe37-5d3f-436a-bbcf-adf64a269f37"), "E92000001", "ROYAL UNITED HOSPITAL", "TBS0210" },
                    { new Guid("25bf770e-faa6-4e5e-b412-a125ddec94ed"), "E92000001", "RUNWELL HOSPITAL", "TBS0216" },
                    { new Guid("7d3350c0-9637-43b9-b866-00562796ebf1"), "E92000001", "SELLY OAK HOSPITAL", "TBS0236" },
                    { new Guid("becd2e23-8d27-42b8-9b03-d96571023580"), "E92000001", "SAFFRON WALDEN COMMUNITY HOSPITAL", "TBS0225" },
                    { new Guid("61fcc8a0-34d4-418c-a77d-55328773dbb2"), "E92000001", "SALISBURY DISTRICT HOSPITAL", "TBS0227" },
                    { new Guid("1fde9ae3-9a76-406b-93d6-e3bea0cb2250"), "E92000001", "SEDGEFIELD COMMUNITY HOSPITAL", "TBS0234" },
                    { new Guid("4a8aa3dc-1da9-42cd-b9ae-4e882255d86a"), "E92000001", "SEAFORD DAY HOSPITAL", "TBS0234" },
                    { new Guid("52d019a7-3fa9-46e4-bef9-a645db4c783f"), "E92000001", "SEACROFT HOSPITAL", "TBS0233" },
                    { new Guid("c7ce583f-996d-4116-afca-71e1efe96efe"), "E92000001", "SCUNTHORPE GENERAL HOSPITAL", "TBS0233" },
                    { new Guid("f376d84f-37b1-4013-bb85-209788983603"), "E92000001", "SCOTT HOSPITAL", "TBS0232" },
                    { new Guid("3fe6c7bf-c0bd-4b9f-87f5-1bac12969a91"), "E92000001", "SCARSDALE HOSPITAL", "TBS0232" },
                    { new Guid("3ecac202-c204-4384-b3f9-0d3ff412dc36"), "E92000001", "SALFORD ROYAL", "TBS0226" },
                    { new Guid("003ade77-4099-4aff-afc9-564757f54ecb"), "E92000001", "SCARBOROUGH GENERAL HOSPITAL", "TBS0232" },
                    { new Guid("e798fcd0-c648-4a84-aa52-4274711438e1"), "E92000001", "SAXON CLINIC", "TBS0232" },
                    { new Guid("d43a3585-c851-48ae-9ffd-8cf9045befc2"), "E92000001", "SAVERNAKE HOSPITAL", "TBS0232" },
                    { new Guid("2a6fa966-d70f-48f0-a72a-782827ee1974"), "E92000001", "SARUM ROAD HOSPITAL", "TBS0231" },
                    { new Guid("54279a3a-dbbd-41a3-a317-fa565d1b6e3b"), "E92000001", "SANDWELL GENERAL HOSPITAL", "TBS0230" },
                    { new Guid("e9241b59-41a5-463a-8933-2abe381d91b6"), "E92000001", "SANDRINGHAM HOSPITAL", "TBS0229" },
                    { new Guid("64150436-2aac-45af-b6c0-5f948dc3cbf3"), "E92000001", "SAMARITAN HOSPITAL FOR WOMEN", "TBS0228" },
                    { new Guid("ead4dc97-a84a-4deb-a1ce-73cf295a0095"), "E92000001", "SCALEBOR PARK HOSPITAL", "TBS0232" },
                    { new Guid("2b0c8ab0-4909-4c9c-9441-fc510b936d41"), "E92000001", "RADFORD HEALTH CENTRE", "TBS0159" },
                    { new Guid("2b4a2cf9-2f58-4b45-8828-cb955be2161f"), "E92000001", "RADCLIFFE INFIRMARY", "TBS0158" },
                    { new Guid("dd610c20-10eb-49ff-aed0-062780823320"), "E92000001", "QUEENS MEDICAL CENTRE [NOTTS]", "TBS0157" },
                    { new Guid("f99825d4-8949-42bd-af1c-3ad4eb9b14e1"), "E92000001", "NUFFIELD HEALTH TEES HOSPITAL", "TBS0109" },
                    { new Guid("aa778a08-5ad9-4268-b8c0-f457c8b591ab"), "E92000001", "NUFFIELD HEALTH TAUNTON HOSPITAL", "TBS0109" },
                    { new Guid("20494aba-dd68-401a-bef2-1b5fa0315828"), "E92000001", "NUFFIELD HEALTH SHREWSBURY HOSPITAL", "TBS0108" },
                    { new Guid("a4eaac12-239b-4e65-ac79-e7858080a94e"), "E92000001", "NUFFIELD HEALTH PLYMOUTH HOSPITAL", "TBS0108" },
                    { new Guid("0dcae460-f8cc-44af-b6c1-8b86b0a725d5"), "E92000001", "NUFFIELD HEALTH NORTH STAFFORDSHIRE HOSPITAL", "TBS0108" },
                    { new Guid("c267a057-4558-4f6a-aa42-3a034e868d64"), "E92000001", "NUFFIELD HEALTH NEWCASTLE-UPON-TYNE HOSPITAL", "TBS0108" },
                    { new Guid("2c222cd0-880d-4b83-9e61-31b820c01626"), "E92000001", "NUFFIELD HEALTH THE GROSVENOR HOSPITAL CHESTER", "TBS0109" },
                    { new Guid("9de16b29-e316-4d88-ad2a-e47a0b413a3d"), "E92000001", "NUFFIELD HEALTH LEICESTER HOSPITAL", "TBS0107" },
                    { new Guid("c88a545c-fe4a-4bff-ab44-0f00f40e721f"), "E92000001", "NUFFIELD HEALTH IPSWICH HOSPITAL", "TBS0107" },
                    { new Guid("92dce1f7-c2aa-4371-a2e6-1a8f24b22b4c"), "E92000001", "NUFFIELD HEALTH HEREFORD HOSPITAL", "TBS0107" },
                    { new Guid("5baced7b-0109-482a-bc8f-799aea68b010"), "E92000001", "NUFFIELD HEALTH HAYWARDS HEATH HOSPITAL", "TBS0107" },
                    { new Guid("1e2e25df-bd19-4647-82f0-bd3b3af5ec19"), "E92000001", "NUFFIELD HEALTH HAMPSHIRE HOSPITAL", "TBS0107" },
                    { new Guid("227d2a1e-42aa-4d34-98b6-db4b489533d5"), "E92000001", "NUFFIELD HEALTH GUILDFORD HOSPITAL", "TBS0106" },
                    { new Guid("41230103-eafc-4e94-8a18-00a4233ae90d"), "E92000001", "NUFFIELD HEALTH EXETER HOSPITAL", "TBS0106" },
                    { new Guid("fbcc8132-cdd9-4a07-9305-12ed8755ca17"), "E92000001", "NUFFIELD HEALTH LEEDS HOSPITAL", "TBS0107" },
                    { new Guid("5f5233ee-f540-466f-b022-04c1da465ef3"), "E92000001", "NUFFIELD HEALTH DERBY HOSPITAL", "TBS0106" },
                    { new Guid("06749b63-ae06-4cc6-9d66-9dd5c4a0245e"), "E92000001", "NUFFIELD HEALTH THE MANOR HOSPITAL OXFORD", "TBS0109" },
                    { new Guid("cb07a530-0aef-4909-a8d2-a364f9f8183b"), "E92000001", "NUFFIELD HEALTH WARWICKSHIRE HOSPITAL", "TBS0109" },
                    { new Guid("c62715cd-d9c2-460c-a2fb-7ded14087b63"), "W92000004", "OVERMONNOW DAY HOSPITAL", "TBS0119" },
                    { new Guid("8a702ecd-96fd-457f-a931-4b367f12277a"), "E92000001", "OTTERY ST MARY HOSPITAL", "TBS0118" },
                    { new Guid("5334cfd4-c2d9-48ad-b83b-edb432894f4c"), "E92000001", "ORSETT HOSPITAL", "TBS0117" },
                    { new Guid("23290947-b60d-4e69-b014-b77a443d4caf"), "E92000001", "ORPINGTON HOSPITAL", "TBS0116" },
                    { new Guid("5d39f71c-ede7-44bc-a1f8-5376f59666ea"), "E92000001", "ORMSKIRK & DISTRICT GENERAL HOSPITAL", "TBS0115" },
                    { new Guid("3c6b423a-2d73-4bf4-a10f-28b98014ac0c"), "E92000001", "OLDCHURCH HOSPITAL", "TBS0114" },
                    { new Guid("9b5ee9bb-78d0-4207-91c3-7c8437f54b2d"), "E92000001", "NUFFIELD HEALTH TUNBRIDGE WELLS HOSPITAL", "TBS0109" },
                    { new Guid("ac1f4e4a-6f25-4349-b7a3-8966bd6ac2db"), "E92000001", "OLD COTTAGE HOSPITAL", "TBS0113" },
                    { new Guid("54a4a2b1-6b88-43e6-8c8c-299526dd9afb"), "E92000001", "OAKLANDS HOSPITAL", "TBS0111" },
                    { new Guid("fdab0616-a8a2-4a2d-9cdb-e664f3820b43"), "W92000004", "OAKDALE HOSPITAL", "TBS0110" },
                    { new Guid("f1fbf3a6-4119-40c4-84fd-c7f06b45aacb"), "E92000001", "NUFFIELD ORTHOPAEDIC CENTRE", "TBS0110" },
                    { new Guid("6f797a5c-078a-48f8-b27f-2e3d67203175"), "E92000001", "NUFFIELD HEALTH YORK HOSPITAL", "TBS0109" },
                    { new Guid("7c6e024a-27a7-4763-b1a7-30f442aea7bf"), "E92000001", "NUFFIELD HEALTH WOLVERHAMPTON HOSPITAL", "TBS0109" },
                    { new Guid("b0406779-891a-4ba2-9c79-46b78b4966eb"), "E92000001", "NUFFIELD HEALTH WOKING HOSPITAL", "TBS0109" },
                    { new Guid("11081272-a36b-459d-9e6b-7e8c486fdc9f"), "E92000001", "OKEHAMPTON COMMUNITY HOSPITAL", "TBS0112" },
                    { new Guid("f688b1b4-7f21-42ef-b23e-9d69ea97cd05"), "E92000001", "OXFORD CLINIC", "TBS0119" },
                    { new Guid("318b6d3c-4854-4ed2-871d-056e56b8d120"), "E92000001", "NUFFIELD HEALTH CHICHESTER HOSPITAL", "TBS0105" },
                    { new Guid("0aefd55a-7dd0-4b80-abec-3ecf44a104b9"), "E92000001", "NUFFIELD HEALTH CAMBRIDGE HOSPITAL", "TBS0103" },
                    { new Guid("bd7f3cf7-8686-45eb-932e-a4b0d1853149"), "E92000001", "NORTH HAMPSHIRE HOSPITAL", "TBS0089" },
                    { new Guid("52b6a55a-6ff7-4101-aa17-0b82367f5b8e"), "E92000001", "NORTH DOWNS HOSPITAL", "TBS0088" },
                    { new Guid("c4f4d541-f5ec-4f12-b04d-ee2d1dbdedff"), "E92000001", "NORTH DEVON DISTRICT HOSPITAL", "TBS0087" },
                    { new Guid("4c68430e-9088-4351-814d-a17cae6e55fe"), "E92000001", "NORTH CAMBRIDGESHIRE HOSPITAL", "TBS0086" },
                    { new Guid("83fab0e7-b142-4326-9897-e73394885d2c"), "E92000001", "NORFOLK & NORWICH UNIVERSITY HOSPITAL", "TBS0086" },
                    { new Guid("3845346d-169d-47b0-903b-49cec8824b89"), "E92000001", "NEWTON COMMUNITY HOSPITAL", "TBS0086" },
                    { new Guid("eac084df-b5e4-449c-aa97-df6d3cbac41f"), "E92000001", "NORTH MANCHESTER GENERAL HOSPITAL", "TBS0089" },
                    { new Guid("0111f8e8-dabf-4d6a-9a00-c5878d8d9366"), "E92000001", "NEWTON ABBOT HOSPITAL", "TBS0086" },
                    { new Guid("f862ce99-cca8-46f1-baa9-e7407c7093d6"), "W92000004", "NEWPORT INTERIM DAY HOSPITAL", "TBS0084" },
                    { new Guid("5ff90321-280c-4cc1-87d3-58b8012224e7"), "E92000001", "NEWMARKET HOSPITAL", "TBS0083" },
                    { new Guid("73119e11-1702-460c-b0f6-fb14ce31f0b0"), "E92000001", "NEWHAVEN HILLRISE DAY HOSPITAL", "TBS0082" },
                    { new Guid("34b5922b-b889-4135-b628-45ca2a15a8a0"), "E92000001", "NEWHAM GENERAL HOSPITAL", "TBS0081" },
                    { new Guid("7a0af4b6-241e-4dde-bcbb-e7aa7908ff3e"), "E92000001", "NEWCASTLE GENERAL HOSPITAL", "TBS0080" },
                    { new Guid("ed4c6fc7-cde2-4352-8829-d6d5516f6be9"), "E92000001", "NEWARK HOSPITAL", "TBS0079" },
                    { new Guid("b3055ec0-0a3f-45d1-bba1-4bbd43aae087"), "E92000001", "NEWQUAY HOSPITAL", "TBS0085" },
                    { new Guid("391ed44b-76fb-4cd1-8eb8-00ad07336412"), "E92000001", "NUFFIELD HEALTH CHELTENHAM HOSPITAL", "TBS0104" },
                    { new Guid("88ddf0f9-1f3c-450d-a541-6f253bb83ae6"), "E92000001", "NORTH MIDDLESEX HOSPITAL", "TBS0090" },
                    { new Guid("c3d08dd5-1216-4448-b62c-a0d2ca781294"), "E92000001", "NORTH WALSHAM COTTAGE HOSPITAL", "TBS0092" },
                    { new Guid("b6d8ea7c-1d31-4102-baf2-a799480ca577"), "E92000001", "NUFFIELD HEALTH BRISTOL HOSPITAL", "TBS0102" },
                    { new Guid("b1b3cdb9-8e54-4e1b-b94c-9cbe63d3d86a"), "E92000001", "NUFFIELD HEALTH BRIGHTON HOSPITAL", "TBS0101" },
                    { new Guid("7b5935b4-c2e1-4b8c-a8cf-a48ee9cc8617"), "E92000001", "NUFFIELD HEALTH BRENTWOOD HOSPITAL", "TBS0100" },
                    { new Guid("b3b6cb43-1e6e-4857-bc35-b271b9bd42b7"), "E92000001", "NUFFIELD HEALTH BOURNEMOUTH HOSPITAL", "TBS0100" },
                    { new Guid("4ee171e5-a4ae-4838-8cba-c4f493508145"), "E92000001", "NUFFIELD DIAGNOSTIC CENTRE", "TBS0100" },
                    { new Guid("27988285-43f2-41d6-a40b-a98fe588da6e"), "E92000001", "NOTTINGHAM WOODTHORPE HOSPITAL", "TBS0099" },
                    { new Guid("973ce72a-b4de-46ba-98e3-3e03df0e5594"), "E92000001", "NORTH TYNESIDE GENERAL HOSPITAL", "TBS0091" },
                    { new Guid("d7deb78d-a28f-488d-b31a-58fab992083d"), "E92000001", "NOTTINGHAM UNIVERSITY HOSPITALS NHS TRUST", "TBS0098" },
                    { new Guid("a8853a77-64b4-4eea-b3a1-017d2309d8b0"), "E92000001", "NORWICH COMMUNITY HOSPITAL", "TBS0096" },
                    { new Guid("54d734b4-327a-4595-96ef-2f6633735c60"), "E92000001", "NORTHWICK PARK HOSPITAL", "TBS0095" },
                    { new Guid("27bb6d5a-ffb2-448c-8583-19dd3d471c02"), "E92000001", "NORTHGATE HOSPITAL [MORPETH]", "TBS0095" },
                    { new Guid("f98655e6-a6cc-4759-8e6f-5bb39dd74403"), "E92000001", "NORTHGATE HOSPITAL [GREAT YARMOUTH]", "TBS0095" },
                    { new Guid("a10905be-4797-4e7d-9911-b1be4534f6d8"), "E92000001", "NORTHERN GENERAL HOSPITAL", "TBS0094" },
                    { new Guid("2c74c36c-6227-4a4b-9b9c-bcf57f70443d"), "E92000001", "NORTHAMPTON GENERAL HOSPITAL", "TBS0093" },
                    { new Guid("0b2bfb8d-0e9e-4138-8751-1dc1b2581ae5"), "E92000001", "NOTTINGHAM CITY HOSPITAL", "TBS0097" },
                    { new Guid("aeb507d7-b0fc-466c-a26a-c2b4165bd01a"), "E92000001", "OXTED & LIMPSFIELD HOSPITAL", "TBS0119" },
                    { new Guid("d21489d6-7d1e-46fb-9d11-c4b536285347"), "E92000001", "PADDOCKS CLINIC", "TBS0119" },
                    { new Guid("69bddd08-ed80-4885-869c-4714dc02ce06"), "E92000001", "PAIGNTON HOSPITAL", "TBS0119" },
                    { new Guid("bc5a7dc6-be8a-47f3-b02e-5b2efe6a379e"), "E92000001", "PRUDHOE HOSPITAL", "TBS0142" },
                    { new Guid("aa2806f9-ea36-48b1-a5b2-04fee3f81a0d"), "E92000001", "PROSPECT PARK HOSPITAL", "TBS0142" },
                    { new Guid("6a203fa2-8f39-404f-af66-d28e23925625"), "E92000001", "PRIORY HOSPITAL", "TBS0141" },
                    { new Guid("4b055366-6301-456f-855d-555f5aa7c73f"), "E92000001", "PRINCESS ROYAL UNIVERSITY HOSPITAL", "TBS0140" },
                    { new Guid("a977da6b-3014-449e-8fc9-477f50d7d502"), "E92000001", "PRINCESS ROYAL HOSPITAL [WEST SUSSEX]", "TBS0140" },
                    { new Guid("86a7ae93-f6ed-414d-a980-d279d583dae2"), "E92000001", "PRINCESS ROYAL HOSPITAL [TELFORD]", "TBS0140" },
                    { new Guid("2b73ae84-1c40-4098-9679-86f512999682"), "E92000001", "PURLEY WAR MEMORIAL HOSPITAL", "TBS0142" },
                    { new Guid("f3083498-736f-421a-a001-feb4cf951422"), "E92000001", "PRINCESS ROYAL HOSPITAL [HULL]", "TBS0140" },
                    { new Guid("f11294d0-1f00-4582-9d24-3707bf5280d6"), "E92000001", "PRINCESS OF WALES COMMUNITY HOSPITAL", "TBS0139" },
                    { new Guid("0c144e14-ed87-4980-b962-6480bb945afd"), "E92000001", "PRINCESS MARINA HOSPITAL", "TBS0139" },
                    { new Guid("9a6d0dfa-73f2-4df5-be61-95e4834cf564"), "E92000001", "PRINCESS MARGARET HOSPITAL", "TBS0138" },
                    { new Guid("0a77a26e-c6cf-4f2d-8501-8d94ea136d1d"), "E92000001", "PRINCESS LOUISE KENSINGTON HOSPITAL", "TBS0138" },
                    { new Guid("5740e112-41b7-4b5a-aec0-1d15fdb6d997"), "E92000001", "PRINCESS GRACE HOSPITAL", "TBS0137" },
                    { new Guid("16aba4bf-274c-465e-9bfd-ff3309266e97"), "E92000001", "PRINCESS ANNE HOSPITAL", "TBS0137" },
                    { new Guid("df4bf235-5604-4256-ab72-a9ccf68988ad"), "W92000004", "PRINCESS OF WALES HOSPITAL", "TBS0140" },
                    { new Guid("3550401a-119c-48c3-883e-46977a8c6032"), "E92000001", "PRINCESS ALEXANDRA HOSPITAL", "TBS0137" },
                    { new Guid("181d832c-0210-42a9-9f8a-370aa4816196"), "W92000004", "PWLLHELI DAY HOSPITAL", "TBS0143" },
                    { new Guid("b1ce8793-4601-44ff-b71b-fdd3ccec7cfd"), "E92000001", "QUEEN CHARLOTTE'S HOSPITAL", "TBS0145" },
                    { new Guid("41ba2e29-1862-47bc-bf15-56599ad510e1"), "E92000001", "QUEEN'S HOSPITAL [ROMFORD]", "TBS0157" },
                    { new Guid("778e92af-2f03-47f3-b692-dc1b20635544"), "E92000001", "QUEENS HOSPITAL [CROYDON]", "TBS0157" },
                    { new Guid("027f7fd2-36f8-4504-8683-41dd4510d339"), "E92000001", "QUEEN'S HOSPITAL [BURTON UPON TRENT]", "TBS0157" },
                    { new Guid("48fc350e-6fd5-4087-9b3d-a5b2c43c2b45"), "E92000001", "QUEEN VICTORIA MEMORIAL HOSPITAL", "TBS0156" },
                    { new Guid("96ec72d1-fdcf-4988-b73a-e3fc5a83f49d"), "E92000001", "QUEEN VICTORIA HOSPITAL [MORECAMBE]", "TBS0155" },
                    { new Guid("a33f56be-8bc2-4b50-b64e-a533a2e495d6"), "E92000001", "QUEEN VICTORIA HOSPITAL [EAST GRINSTEAD]", "TBS0154" },
                    { new Guid("e57e2006-33bd-4432-acab-36b0d3237e81"), "E92000001", "QUEEN ALEXANDRA HOSPITAL", "TBS0144" },
                    { new Guid("745f7bed-f6be-42c9-ba43-537954cdd284"), "E92000001", "QUEEN MARY'S HOSPITAL [SIDCUP]", "TBS0153" },
                    { new Guid("73d6dd94-75db-4ec8-a497-ea04af0c8bea"), "E92000001", "QUEEN ELIZABETH THE QUEEN MOTHER HOSPITAL", "TBS0151" },
                    { new Guid("fcbb64dd-c63a-4f4c-9925-617cdbf3ff51"), "E92000001", "QUEEN ELIZABETH II HOSPITAL [WELWYN GARDEN CITY]", "TBS0150" },
                    { new Guid("e5984fde-45f2-45fa-9d8a-bf17a8313d61"), "E92000001", "QUEEN ELIZABETH HOSPITAL [LONDON]", "TBS0149" },
                    { new Guid("13c912bf-3127-4dd2-908b-6bbcaa3bc984"), "E92000001", "QUEEN ELIZABETH HOSPITAL [KING'S LYNN]", "TBS0148" },
                    { new Guid("65d18006-fe12-4b5b-b781-96a8a8ad877c"), "E92000001", "QUEEN ELIZABETH HOSPITAL [GATESHEAD]", "TBS0147" },
                    { new Guid("cd292c05-fcdb-44a1-9648-7b7521836236"), "E92000001", "QUEEN ELIZABETH HOSPITAL [BIRMINGHAM]", "TBS0146" },
                    { new Guid("48c263ce-33a1-4016-91d8-1beb5d08c99b"), "E92000001", "QUEEN MARY'S HOSPITAL [LONDON]", "TBS0152" },
                    { new Guid("fd423059-a4c8-4eab-bc72-9da422524e12"), "W92000004", "PRINCE PHILIP HOSPITAL", "TBS0137" },
                    { new Guid("cea254e5-b953-4bf2-82f1-8304337ff73c"), "W92000004", "PRINCE CHARLES HOSPITAL", "TBS0137" },
                    { new Guid("4836a4e5-d0a3-43de-b8e7-d4bfe81b7e9c"), "E92000001", "PRIMROSE HILL HOSPITAL", "TBS0137" },
                    { new Guid("39530369-9873-483e-a218-114c3bcc6240"), "E92000001", "PAULTON MEMORIAL HOSPITAL", "TBS0123" },
                    { new Guid("f5326f7f-d87f-4f89-b7c2-36220872fcaf"), "E92000001", "PATRICK STEAD HOSPITAL", "TBS0123" },
                    { new Guid("4fcd15ef-14c7-454b-aae4-4ecdb49f9d6e"), "E92000001", "PARKWOOD HOSPITAL", "TBS0123" },
                    { new Guid("eaeb2563-5cff-44f4-91b2-e4ef6d28d6cc"), "E92000001", "PARKSIDE HOSPITAL", "TBS0122" },
                    { new Guid("89a79fd3-e59f-4255-9e7a-2b536dbb5075"), "E92000001", "PARKLANDS HOSPITAL", "TBS0121" },
                    { new Guid("14f099af-044d-4a30-ae8d-6aef04dbd1a1"), "E92000001", "PARK VIEW DAY HOSPITAL", "TBS0120" },
                    { new Guid("70693b46-61f4-4321-a896-a9913d0aa600"), "E92000001", "PEMBERTON CLINIC", "TBS0124" },
                    { new Guid("4340fe30-9786-4feb-9d90-e1c82f340113"), "E92000001", "PARK VIEW CLINIC", "TBS0119" },
                    { new Guid("9670dd65-0d74-423c-ad51-cd1d4c638d03"), "E92000001", "PARK LANE MEDICAL CENTRE", "TBS0119" },
                    { new Guid("8e2e428a-341c-4f76-9633-124d5ca342b5"), "E92000001", "PARK HOSPITAL [OXFORD]", "TBS0119" },
                    { new Guid("8a7d861d-5086-4fba-a813-20fb895c8412"), "E92000001", "PARK HOSPITAL [NOTTINGHAM]", "TBS0119" },
                    { new Guid("5524785c-7823-41df-9341-6b4a65aa9c19"), "E92000001", "PARK HILL HOSPITAL", "TBS0119" },
                    { new Guid("eddf6efc-738a-4235-9c80-54d7506556d2"), "E92000001", "PAPWORTH HOSPITAL", "TBS0119" },
                    { new Guid("ae74328a-9f61-4d1e-9247-4895ae7045da"), "E92000001", "PALMER COMMUNITY HOSPITAL", "TBS0119" },
                    { new Guid("790391e9-6c41-4d28-8341-c1b8811036de"), "W92000004", "PARK SQUARE DAY HOSPITAL", "TBS0119" },
                    { new Guid("86aac5eb-db9d-496e-929d-9d7e34609e58"), "E92000001", "PEMBURY HOSPITAL", "TBS0124" },
                    { new Guid("b6148321-d7b1-4d3f-905d-258ff2711655"), "E92000001", "PENDLE COMMUNITY HOSPITAL", "TBS0125" },
                    { new Guid("4220440e-b0e6-4f2f-827c-539552ff32ad"), "W92000004", "PENLEY HOSPITAL", "TBS0126" },
                    { new Guid("28cbee25-66f4-4a20-bf3d-f26077792ef5"), "E92000001", "PRESTON HALL HOSPITAL", "TBS0137" },
                    { new Guid("27911115-f3ea-4f56-b343-4e9e52b0a84c"), "W92000004", "PRESTATYN COMMUNITY HOSPITAL", "TBS0137" },
                    { new Guid("12759488-b4db-4f7c-ab2e-f13bd8c6e73d"), "E92000001", "POTTERS BAR COMMUNITY HOSPITAL", "TBS0137" },
                    { new Guid("ac9539f5-8879-4127-90d9-5511cbec1645"), "E92000001", "PORTLAND HOSPITAL", "TBS0137" },
                    { new Guid("8a42fca1-8e62-4805-990f-ee19f85a6a4f"), "E92000001", "POOLE GENERAL HOSPITAL", "TBS0136" },
                    { new Guid("5543c2b5-bde5-4c41-b856-e1469fa11df9"), "W92000004", "PONTYPRIDD & DISTRICT HOSPITAL", "TBS0135" },
                    { new Guid("bdb0bc8f-01de-4c50-bd28-f9536cb91355"), "E92000001", "PONTEFRACT GENERAL INFIRMARY", "TBS0134" },
                    { new Guid("9a4f657a-01e7-4ca6-b8d5-b9e033401eb6"), "E92000001", "PINEHILL HOSPITAL", "TBS0133" },
                    { new Guid("31b2d7b3-2fb4-42a7-8251-ce84ea728fd5"), "E92000001", "PINDERFIELDS GENERAL HOSPITAL", "TBS0132" },
                    { new Guid("59755880-6b5d-4ce2-8b12-5125a7ddf832"), "E92000001", "PILGRIM HOSPITAL", "TBS0131" },
                    { new Guid("a6ee57cb-dda6-4d29-852c-62b762d147a1"), "E92000001", "PHOENIX DAY HOSPITAL", "TBS0131" },
                    { new Guid("4de9449e-65f5-40c0-9c32-953d189f20df"), "E92000001", "PETERSFIELD COMMUNITY HOSPITAL", "TBS0130" },
                    { new Guid("7306f844-2521-48d0-a765-2c1e45621d88"), "E92000001", "PETERLEE COMMUNITY HOSPITAL", "TBS0129" },
                    { new Guid("9fd2fb99-4a9b-463f-8b23-a4ad8961de06"), "E92000001", "PETERBOROUGH CITY HOSPITAL", "TBS0128" },
                    { new Guid("d2558cd1-71ca-4c7f-802d-98498b2eaf90"), "E92000001", "PENRITH HOSPITAL", "TBS0127" },
                    { new Guid("733fd6d6-be41-42ed-b105-68acd32b678f"), "E92000001", "SOUTHWOLD HOSPITAL", "TBS0263" },
                    { new Guid("b0ff6cdd-9485-4e2f-b7cb-40c5161d631b"), "E92000001", "NEW VICTORIA HOSPITAL", "TBS0078" },
                    { new Guid("a4cba359-68e2-4188-91a7-0991b72b2013"), "E92000001", "SPIRE ALEXANDRA HOSPITAL", "TBS0263" },
                    { new Guid("e251a3ab-ebcf-4128-a93d-aefb37b39fda"), "E92000001", "SPIRE BRISTOL HEALTH CLINIC", "TBS0263" },
                    { new Guid("a6f34849-54f7-44e7-8fce-265a9a8b604f"), "E92000001", "WARLEY HOSPITAL", "TBS0108" },
                    { new Guid("29c3bf29-502c-40fa-92a0-e91455b1ed5c"), "E92000001", "WARDERS MEDICAL CENTRE", "TBS0108" },
                    { new Guid("61ed6301-8b4a-4ed0-93ba-2fa07b5c8796"), "E92000001", "WANTAGE COMMUNITY HOSPITAL", "TBS0108" },
                    { new Guid("335fe746-12f2-4c42-9f28-7f21a3535ad4"), "E92000001", "WANSTEAD HOSPITAL", "TBS0108" },
                    { new Guid("c2e40f80-03ff-47f4-b092-a5176085bf35"), "E92000001", "WANSBECK GENERAL HOSPITAL", "TBS0107" },
                    { new Guid("ca29f4a2-7b4e-4d06-aeb3-a08ff41d2978"), "E92000001", "WALTON HOSPITAL", "TBS0107" },
                    { new Guid("57725cc0-5c4d-4ac9-8de6-5355cd8abe66"), "E92000001", "WARMINSTER COMMUNITY HOSPITAL", "TBS0109" },
                    { new Guid("1219b1c8-850c-483f-bf3b-16dc9090dc7b"), "E92000001", "WALTON COMMUNITY HOSPITAL", "TBS0107" },
                    { new Guid("e5afd154-c762-434b-ac16-a54d71737d31"), "E92000001", "WALNUT TREE HOSPITAL", "TBS0107" },
                    { new Guid("1be6ea32-ab5d-4efe-ac25-371d7b2525bd"), "E92000001", "WALLINGFORD COMMUNITY HOSPITAL", "TBS0107" },
                    { new Guid("22e73828-676d-4c3e-bc25-092cb00c0ae0"), "E92000001", "WALKERGATE PARK HOSPITAL", "TBS0106" },
                    { new Guid("6b96e2f9-75a6-4c49-923d-98d448b2a266"), "W92000004", "VICTORIA MEMORIAL HOSPITAL [POWYS]", "TBS0106" },
                    { new Guid("6a0db87b-8a6e-48ab-964a-2e785cc3d6d3"), "E92000001", "VICTORIA INFIRMARY [CHESHIRE]", "TBS0106" },
                    { new Guid("9623526c-0c4b-4ff2-a99d-4b8ed508a0ce"), "E92000001", "VICTORIA HOSPITAL [SIDMOUTH]", "TBS0105" },
                    { new Guid("5fa85ad6-b12c-4b32-a3be-71697bfe26e9"), "E92000001", "WALTON CENTRE FOR NEUROLOGY & NEUROSURGERY", "TBS0107" },
                    { new Guid("2d14f397-3474-45b5-a8ef-995666fb7563"), "E92000001", "VICTORIA HOSPITAL [ROMFORD]", "TBS0104" },
                    { new Guid("d5b0e5e7-051f-4f72-9980-288125b680f1"), "E92000001", "WARNINGLID DAY HOSPITAL", "TBS0109" },
                    { new Guid("9aa7ca5d-bac1-4efa-a393-db5636d183b1"), "E92000001", "WARWICK HOSPITAL", "TBS0109" },
                    { new Guid("77f47444-1ab3-4245-ae3f-4472ea7e4ade"), "E92000001", "WEST CUMBERLAND HOSPITAL", "TBS0117" },
                    { new Guid("834b99ca-f6cc-47aa-bf9a-318726e89dc1"), "E92000001", "WEST CORNWALL HOSPITAL (PENZANCE)", "TBS0116" },
                    { new Guid("60e405a2-4d47-4a49-a739-dbedfb7bf9b3"), "E92000001", "WEST BERKSHIRE COMMUNITY HOSPITAL", "TBS0115" },
                    { new Guid("042fa0e4-791a-47a0-9f5f-2c1348954954"), "E92000001", "WESHAM PARK HOSPITAL", "TBS0114" },
                    { new Guid("d3717548-d4e0-499a-9ed0-f933624773af"), "W92000004", "WERNDALE HOSPITAL", "TBS0113" },
                    { new Guid("a53921b3-05d5-485c-9dcf-9cd7371d5a33"), "E92000001", "WEMBLEY HOSPITAL", "TBS0112" },
                    { new Guid("3cdda2a9-ab20-4b83-8caf-68e7ecac9cf5"), "E92000001", "WARRINGTON HOSPITAL", "TBS0109" },
                    { new Guid("ecad63ff-94da-48ff-927a-db56bff7b4e7"), "E92000001", "WELLINGTON HOSPITAL", "TBS0111" },
                    { new Guid("7e1071f0-410c-4f32-8dd0-3763d23605e3"), "E92000001", "WELLAND HOSPITAL", "TBS0110" },
                    { new Guid("eeaeea6e-08c1-4096-9866-588752a91764"), "E92000001", "WEALD DAY HOSPITAL", "TBS0109" },
                    { new Guid("93ccb810-7dfd-44a5-afe4-69c9d9c6d314"), "E92000001", "WATHWOOD HOSPITAL", "TBS0109" },
                    { new Guid("5f9ceddf-54d4-4279-b306-fa5382fbb4b4"), "E92000001", "WATFORD GENERAL HOSPITAL", "TBS0109" },
                    { new Guid("91c4fcb5-6ca9-468f-9085-55d267414263"), "E92000001", "WATERLOO DAY HOSPITAL", "TBS0109" },
                    { new Guid("264f90dc-2c62-4e92-8f64-69f1085fe3e2"), "E92000001", "WATER EATON HEALTH CENTRE", "TBS0109" },
                    { new Guid("80018752-3ab1-4285-b26f-34655f5c4ed7"), "E92000001", "WELLINGTON & DISTRICT COTTAGE HOSPITAL", "TBS0110" },
                    { new Guid("cd24e785-e620-4163-a001-feac1082711c"), "E92000001", "WEST HEATH HOSPITAL", "TBS0118" },
                    { new Guid("67320253-12f2-4c75-ba77-a916db405cb3"), "E92000001", "VICTORIA HOSPITAL [LICHFIELD]", "TBS0103" },
                    { new Guid("3725a52c-1e6f-4a30-a55a-5fb4aa983973"), "E92000001", "VICTORIA HEALTH CENTRE", "TBS0101" },
                    { new Guid("2a9c7e60-af14-4929-8233-79deb17855bd"), "E92000001", "UCKFIELD COMMUNITY HOSPITAL", "TBS0087" },
                    { new Guid("05e4acfc-593c-42b5-984e-98a54187e0d3"), "W92000004", "TYWYN & DISTRICT DAY HOSPITAL", "TBS0086" },
                    { new Guid("71179d01-f00f-4579-b87a-d622e9dfbb36"), "N92000002", "TYRONE COUNTY HOSPITAL", "TBS0086" },
                    { new Guid("77b00623-0efd-4e7e-9cfd-b6a017cc9738"), "E92000001", "TYRELL HOSPITAL", "TBS0086" },
                    { new Guid("609bbae6-b46b-4e9f-bcc4-44bd5b01599a"), "E92000001", "TYNDALE CENTRE DAY HOSPITAL", "TBS0086" },
                    { new Guid("9dd22d58-44dc-4a73-9d52-e36cf488e570"), "E92000001", "TROWBRIDGE COMMUNITY HOSPITAL", "TBS0085" },
                    { new Guid("0f6d1869-c347-4a32-be45-ceea4df5f537"), "N92000002", "ULSTER HOSPITAL", "TBS0088" },
                    { new Guid("6b91313f-05c5-4556-848f-0d4bd77b5b11"), "W92000004", "TREGARON HOSPITAL", "TBS0084" },
                    { new Guid("3d2b0065-bb83-4e0e-9b23-d007500e703f"), "E92000001", "TRAFFORD GENERAL HOSPITAL", "TBS0082" },
                    { new Guid("5afb1591-0296-42f0-8e1a-4abe24fcfb7a"), "E92000001", "TOWNLANDS HOSPITAL", "TBS0081" },
                    { new Guid("f572f90a-512c-4271-825b-ef6c82c28610"), "E92000001", "TOWERS HOSPITAL", "TBS0080" },
                    { new Guid("3a2b45af-6c2c-4420-9b93-c54476743324"), "E92000001", "TOTNES COMMUNITY HOSPITAL", "TBS0079" },
                    { new Guid("3b110ed9-3faf-458e-a188-e6c6d719707c"), "E92000001", "TORRINGTON HOSPITAL", "TBS0078" },
                    { new Guid("930f0749-18d8-47f3-b29e-424c352209be"), "E92000001", "TORBAY DISTRICT GENERAL HOSPITAL", "TBS0077" },
                    { new Guid("0e757d5c-57b8-41f1-b2ae-a75ab69c4854"), "W92000004", "TREDEGAR GENERAL HOSPITAL", "TBS0083" },
                    { new Guid("d66633d4-3d4a-435c-a8ae-90845e79a6e7"), "E92000001", "VICTORIA HOSPITAL [DEAL]", "TBS0102" },
                    { new Guid("7021e407-fb6a-462f-87f7-25ef32a85915"), "E92000001", "UNIVERSITY COLLEGE HOSPITAL", "TBS0089" },
                    { new Guid("1ee330e7-3759-473b-93eb-a65a09dee01d"), "E92000001", "UNIVERSITY HOSPITAL COVENTRY", "TBS0090" },
                    { new Guid("38872531-c7ec-4f84-b3c2-3d3f0c923c18"), "E92000001", "VICTORIA COTTAGE HOSPITAL [MARYPORT]", "TBS0100" },
                    { new Guid("a0e8bd6c-90eb-4c96-a868-834b41d3390b"), "E92000001", "VICTORIA CENTRAL HOSPITAL [WALLASEY]", "TBS0100" },
                    { new Guid("4b6fa7ab-b480-4651-8831-abc0f78755af"), "E92000001", "VERRINGTON HOSPITAL", "TBS0100" },
                    { new Guid("fc865a59-a750-4924-84bf-75d222287f4d"), "W92000004", "VELINDRE HOSPITAL", "TBS0099" },
                    { new Guid("b2a38a25-3fd9-4274-8ee5-f85744670ef1"), "E92000001", "UPTON HOUSE DAY HOSPITAL [NORTHAMPTON]", "TBS0098" },
                    { new Guid("f606a608-5d17-4226-8fbf-92956ef34c27"), "E92000001", "UPTON HOSPITAL [SLOUGH]", "TBS0097" },
                    { new Guid("7eb349c6-db5a-423c-b996-a793b1e4db1b"), "E92000001", "UNIVERSITY HOSPITAL AINTREE", "TBS0089" },
                    { new Guid("6a939b10-83d6-4ecd-97df-c3f732f3f77f"), "E92000001", "UPTON DAY HOSPITAL [KENT]", "TBS0096" },
                    { new Guid("f778894d-48c3-4f1d-99ed-7755507682fa"), "W92000004", "UNIVERSITY HOSPITAL OF WALES", "TBS0095" },
                    { new Guid("e96c269c-e38f-4abc-8d92-02ab29ded69c"), "E92000001", "UNIVERSITY HOSPITAL OF NORTH TEES", "TBS0095" },
                    { new Guid("1acefe30-cb48-432c-9bbc-10b371aa0a59"), "E92000001", "UNIVERSITY HOSPITAL OF NORTH STAFFORDSHIRE", "TBS0094" },
                    { new Guid("ea3f028d-3b47-4241-938c-e8c3ddf15e54"), "E92000001", "UNIVERSITY HOSPITAL OF NORTH DURHAM", "TBS0093" },
                    { new Guid("d9348c9d-2b43-4439-bb05-3c1736a13e02"), "E92000001", "UNIVERSITY HOSPITAL OF HARTLEPOOL", "TBS0092" },
                    { new Guid("07186db5-833f-4d9d-a0aa-e1374dd64fb8"), "E92000001", "UNIVERSITY HOSPITAL LEWISHAM", "TBS0091" },
                    { new Guid("79f9b55e-5bd5-4862-a8fc-e5bff1a957f9"), "E92000001", "UNIVERSITY HOSPITALS OF LEICESTER NHS TRUST", "TBS0095" },
                    { new Guid("88fcd776-b1fb-4e1e-9e8c-31ea47ab719a"), "W92000004", "TONNA DAY HOSPITAL", "TBS0077" },
                    { new Guid("f8eeccf0-529d-4a86-ac0a-8795a315b33a"), "E92000001", "WEST LANE HOSPITAL", "TBS0119" },
                    { new Guid("a12c1fd7-a6c9-4e31-87a5-1cc213aebdd6"), "E92000001", "WEST MIDDLESEX UNIVERSITY HOSPITAL", "TBS0119" },
                    { new Guid("1d6e8331-f3c2-4b5d-8415-e98dc82f72de"), "E92000001", "WOODLANDS HOSPITAL [DARLINGTON]", "TBS0140" },
                    { new Guid("2dfe6f2d-1a1e-45ca-82aa-cd19e85fdaee"), "E92000001", "WOODLAND HOSPITAL [KETTERING]", "TBS0140" },
                    { new Guid("f41ce433-dee7-416c-b1ed-e723e740f5f8"), "E92000001", "WOLVERHAMPTON & MIDLAND EYE INFIRMARY", "TBS0140" },
                    { new Guid("53737ff9-2b2e-4800-9851-5967dacd26d9"), "E92000001", "WOKINGHAM HOSPITAL", "TBS0140" },
                    { new Guid("2599dd55-6274-41e1-ad21-3dbc0d2b8349"), "E92000001", "WOKING COMMUNITY HOSPITAL", "TBS0140" },
                    { new Guid("1ff7f5f2-1f35-47f0-8e3f-891eb4fdd2bd"), "E92000001", "WITNEY COMMUNITY HOSPITAL", "TBS0139" },
                    { new Guid("6a9e42da-c4ba-48f9-a6ac-acc852027a87"), "E92000001", "WOODS HOSPITAL", "TBS0141" },
                    { new Guid("f6484b85-6324-42ae-b650-5bbcc8e41950"), "W92000004", "WITHYBUSH GENERAL HOSPITAL", "TBS0139" },
                    { new Guid("30b7756e-486a-40c7-a24d-ff5ca4860db9"), "E92000001", "WITHERNSEA HOSPITAL", "TBS0138" },
                    { new Guid("69c94cd1-0423-4df6-a64d-aab6e96a569c"), "E92000001", "WINTERBOURNE HOSPITAL", "TBS0137" },
                    { new Guid("fa8fcdc3-4477-4b51-afc6-1deb64bf99fe"), "E92000001", "WINFIELD HOSPITAL", "TBS0137" },
                    { new Guid("3846f91b-107f-430a-badf-49fdf130079a"), "E92000001", "WINCHCOMBE HOSPITAL", "TBS0137" },
                    { new Guid("1895e8b7-ab2c-4307-adf3-f36a119a32a4"), "E92000001", "WIMBOURNE COMMUNITY HOSPITAL", "TBS0137" },
                    { new Guid("3e5952e3-4d49-4c01-8ac4-e91928af25a3"), "E92000001", "WILSON HOSPITAL", "TBS0137" },
                    { new Guid("1d024fdf-bde9-4b5d-8e85-7682186cff14"), "E92000001", "WITHINGTON HOSPITAL", "TBS0138" },
                    { new Guid("e1e68b14-02cc-40a2-b7db-0f3ec16ecc89"), "E92000001", "WILLOWBANK DAY HOSPITAL", "TBS0137" },
                    { new Guid("af0562fd-363c-4f51-832f-41a133fe8147"), "E92000001", "WORCESTER ROYAL INFIRMARY", "TBS0142" },
                    { new Guid("15e5950d-fc36-439d-aab0-ca520e0b617c"), "E92000001", "WORKINGTON COMMUNITY HOSPITAL", "TBS0142" },
                    { new Guid("c5a3dc21-16c3-4d47-a945-331d08f42f89"), "W92000004", "YSTRAD MYNACH HOSPITAL", "TBS0156" },
                    { new Guid("dd4fbf70-7485-4afd-94a2-4e2f2fa550b7"), "W92000004", "YSBYTY'R TRI CHWM", "TBS0155" },
                    { new Guid("a5594f9e-9eec-4626-b62a-21f724be2117"), "W92000004", "YSBYTY PENRHOS STANLEY", "TBS0154" },
                    { new Guid("4caded10-fff2-4e38-9b28-cb45cee309aa"), "W92000004", "YSBYTY GWYNEDD", "TBS0153" },
                    { new Guid("66f7ed14-a941-4c1e-b1e9-c72d15671816"), "W92000004", "YSBYTY GEORGE THOMAS", "TBS0152" },
                    { new Guid("c860f05d-6460-402e-a6f9-cacf8e963396"), "E92000001", "YORK DISTRICT HOSPITAL", "TBS0151" },
                    { new Guid("aa1463dd-93e9-4315-a039-1dfd867fe5f5"), "E92000001", "WORCESTERSHIRE ROYAL HOSPITAL", "TBS0142" },
                    { new Guid("915fe2f1-3327-467b-a2e9-295ec96c7c5e"), "E92000001", "YEOVIL DISTRICT HOSPITAL", "TBS0150" },
                    { new Guid("a3c6922e-3566-4f31-8b7b-9bf6f1b55708"), "E92000001", "WYTHENSHAWE HOSPITAL", "TBS0148" },
                    { new Guid("3ba986ff-d53a-43f3-bf55-361061559204"), "E92000001", "WYCOMBE HOSPITAL", "TBS0147" },
                    { new Guid("b748f1d2-0fd3-4155-99a9-93da8be2c960"), "E92000001", "WRIGHTINGTON HOSPITAL", "TBS0146" },
                    { new Guid("475fb009-c4b1-4b28-b526-96f06b922db6"), "W92000004", "WREXHAM MAELOR HOSPITAL", "TBS0145" },
                    { new Guid("9583590e-1925-4f46-9025-aeb6d5a04b07"), "E92000001", "WREXHAM CHEST CLINIC", "TBS0144" },
                    { new Guid("9d17ed6f-dea4-4c69-8228-ebbe708e3148"), "E92000001", "WORTHING HOSPITAL", "TBS0143" },
                    { new Guid("34cc991a-c066-4dc9-a272-dadbddd46d0f"), "E92000001", "YEATMAN HOSPITAL", "TBS0149" },
                    { new Guid("148130af-457a-4509-946d-f6e6736f22ff"), "E92000001", "WEST MENDIP COMMUNITY HOSPITAL", "TBS0119" },
                    { new Guid("397448bf-759f-4f96-bf78-a885710a2450"), "E92000001", "WILLITON HOSPITAL", "TBS0137" },
                    { new Guid("348e403c-7f80-4b6b-b5f3-0b4c62134e2b"), "E92000001", "WILLIAM HARVEY HOSPITAL", "TBS0137" },
                    { new Guid("cad2c172-1c1b-4194-81ee-582deef55f14"), "E92000001", "WESTON PARK HOSPITAL", "TBS0123" },
                    { new Guid("0ac033ab-9a11-4fa6-aa1a-1fca71180c2f"), "E92000001", "WESTON GENERAL HOSPITAL", "TBS0122" },
                    { new Guid("334d0057-4c0a-47a0-a833-66f0184770cd"), "E92000001", "WESTMORLAND GENERAL HOSPITAL", "TBS0121" },
                    { new Guid("db5ec265-0e69-4987-9f3b-8632840fe10e"), "E92000001", "WESTMINSTER MEMORIAL HOSPITAL", "TBS0120" },
                    { new Guid("fa1d124f-8a8e-4555-941c-8db2a0964659"), "W92000004", "WESTFA DAY HOSPITAL", "TBS0119" },
                    { new Guid("629bfd57-7c1d-438e-9cdf-c1524f885090"), "E92000001", "WESTERN EYE HOSPITAL", "TBS0119" },
                    { new Guid("356f6b1a-ab11-44bf-8db0-1afc5d455fee"), "E92000001", "WEXHAM PARK HOSPITAL", "TBS0123" },
                    { new Guid("4d3c8bfa-4dd5-48b9-8e52-6215b93e1132"), "E92000001", "WESTERN COMMUNITY HOSPITAL", "TBS0119" },
                    { new Guid("bf02e2de-09bf-4f0f-bd63-6e8dd1dc5c8e"), "W92000004", "WEST WALES GENERAL HOSPITAL", "TBS0119" },
                    { new Guid("5a723464-db3e-4387-ad11-ac66c0da2653"), "E92000001", "WEST SUFFOLK HOSPITAL", "TBS0119" },
                    { new Guid("b52f9c45-7719-43f3-bc40-d1b140fe0f5c"), "E92000001", "WEST PARK HOSPITAL [WOLVERHAMPTON]", "TBS0119" },
                    { new Guid("d9968ce6-7359-494e-8848-71d5f97c2446"), "E92000001", "WEST PARK HOSPITAL [EPSOM]", "TBS0119" },
                    { new Guid("4424dc08-b730-4a64-b252-0b138dd52699"), "E92000001", "WEST PARK HOSPITAL [DARLINGTON]", "TBS0119" },
                    { new Guid("dbf3318e-ce56-4ff7-b3d5-ef84c846b9eb"), "E92000001", "WEST MIDLANDS HOSPITAL", "TBS0119" },
                    { new Guid("f6af9dae-a163-419f-ade9-d4231621e518"), "E92000001", "WESTBURY COMMUNITY HOSPITAL", "TBS0119" },
                    { new Guid("e36442ed-35cb-40e0-882c-1c5d41ba3f68"), "E92000001", "WILLIAM JULIEN COURTAULD HOSPITAL", "TBS0137" },
                    { new Guid("8c00f2ef-4a63-4f7c-a264-552e5ea5c1f2"), "E92000001", "WEYBRIDGE COMMUNITY HOSPITAL", "TBS0123" },
                    { new Guid("a8ea4caa-1e57-4a50-9817-c718dad73539"), "E92000001", "WHALLEY DRIVE CLINIC", "TBS0124" },
                    { new Guid("172b8133-4b56-4858-982a-a2b8cb4cd46d"), "E92000001", "WILLESDEN HOSPITAL", "TBS0137" },
                    { new Guid("f937f259-3e02-4207-b62f-5528bc469424"), "E92000001", "WIGTON HOSPITAL", "TBS0136" },
                    { new Guid("ee749b92-c917-46da-adc3-b2a67faa98ed"), "E92000001", "WHITWORTH HOSPITAL", "TBS0135" },
                    { new Guid("47159ea7-a205-4c4f-a93f-8d2d4498c39f"), "E92000001", "WHITTINGTON HOSPITAL", "TBS0134" },
                    { new Guid("7eda8526-ce1a-4bba-98d3-32e6e3ca816e"), "E92000001", "WHITSTABLE & TANKERTON HOSPITAL", "TBS0133" },
                    { new Guid("78b6d05e-e916-4536-b091-3552019e326c"), "N92000002", "WHITEABBEY HOSPITAL", "TBS0132" },
                    { new Guid("3dbcf14f-400a-46b0-b179-1aa84e656724"), "E92000001", "WEYMOUTH COMMUNITY HOSPITAL", "TBS0124" },
                    { new Guid("19835e7e-2d97-4ee5-833e-62a603ba2685"), "E92000001", "WHITE CROSS REHABILITATION HOSPITAL", "TBS0131" },
                    { new Guid("13967c5c-8172-44f0-bdab-b206c8bdd8d2"), "W92000004", "WHITCHURCH HOSPITAL [CARDIFF]", "TBS0130" },
                    { new Guid("e78dc003-3cb5-4af6-bac6-822748137109"), "E92000001", "WHITBY COMMUNITY HOSPITAL", "TBS0129" },
                    { new Guid("87eed220-c1a8-4f17-9bbb-59c9e8f90e6c"), "E92000001", "WHISTON HOSPITAL", "TBS0128" },
                    { new Guid("5b13ce84-37b9-43ad-afa0-3407e277bfe2"), "E92000001", "WHIPPS CROSS UNIVERSITY HOSPITAL", "TBS0127" },
                    { new Guid("2e96d1a5-91d4-4f82-9b37-e090dbe091be"), "E92000001", "WHELLEY HOSPITAL", "TBS0126" },
                    { new Guid("631c5c01-a844-49eb-8cdf-7555416cab1f"), "E92000001", "WHARFEDALE GENERAL HOSPITAL", "TBS0125" },
                    { new Guid("9dbbac50-5607-4e9b-b21c-a1fafdd1c36d"), "E92000001", "WHITCHURCH HOSPITAL [SHROPSHIRE]", "TBS0131" },
                    { new Guid("c2e2f1fe-7a7e-4f18-8c45-869a53dd6124"), "E92000001", "TONBRIDGE COTTAGE HOSPITAL", "TBS0077" },
                    { new Guid("a5f820dd-37d5-4ef6-88d1-d779183c306b"), "E92000001", "TOLWORTH HOSPITAL", "TBS0077" },
                    { new Guid("35d8d223-8e22-4601-9abf-32b86bb8bc0c"), "E92000001", "TIVERTON AND DISTRICT HOSPITAL", "TBS0076" },
                    { new Guid("18bb0905-d268-4f33-9c94-b6a15791c7f4"), "W92000004", "ST CADOCS HOSPITAL", "TBS0027" },
                    { new Guid("b400850e-8fc9-46f0-9018-c6bd26460b44"), "E92000001", "ST BARTHOLOMEWS HOSPITAL [ROCHESTER]", "TBS0026" },
                    { new Guid("ee8f217b-e6e3-4838-be3a-9916b24be160"), "E92000001", "ST BARTHOLOMEW'S HOSPITAL [LONDON]", "TBS0025" },
                    { new Guid("e0fd498c-85f1-49d2-a283-0dcc0dc367b6"), "E92000001", "ST BARTHOLOMEWS DAY HOSPITAL [LIVERPOOL]", "TBS0025" },
                    { new Guid("28dcb0dc-31d4-4b7a-ad44-3e27cb37016c"), "E92000001", "ST BARNABAS HOSPITAL", "TBS0025" },
                    { new Guid("31140ae9-27d7-4860-833b-673dab14fd0e"), "E92000001", "ST AUSTELL COMMUNITY HOSPITAL", "TBS0025" },
                    { new Guid("11e3674c-17d3-4c4e-8f2c-a4af70dd6fb0"), "E92000001", "ST CATHERINES HOSPITAL", "TBS0028" },
                    { new Guid("9b11d0f4-3c0e-4da3-b11c-bd90c0e4d65a"), "E92000001", "ST ANTHONYS HOSPITAL", "TBS0024" },
                    { new Guid("7545fc82-4a58-4c7c-aab4-3e169c0dfc13"), "E92000001", "ST ANNS HOSPITAL [LONDON]", "TBS0022" },
                    { new Guid("fd883e81-6f40-4d86-a7b0-776a1b39bc75"), "E92000001", "ST ANNE'S HOSPITAL [ALTRINCHAM]", "TBS0021" },
                    { new Guid("d719e975-644b-4fda-b001-c77ab132a672"), "E92000001", "ST ANDREW'S HOSPITAL [LONDON]", "TBS0020" },
                    { new Guid("e26d81f7-1557-4193-bc39-0edb829e0f37"), "E92000001", "ST ANDREWS [WELLS]", "TBS0019" },
                    { new Guid("e1dda57d-81ab-4c5e-bffc-c631204ed830"), "E92000001", "ST ALBANS CITY HOSPITAL", "TBS0019" },
                    { new Guid("7c1c7f89-d326-42ec-8edd-d67cd3e52488"), "E92000001", "SPRINGFIELD HOSPITAL", "TBS0019" },
                    { new Guid("dc37346d-f34c-451b-90f0-35b5d69aae46"), "E92000001", "ST ANN'S HOSPITAL [POOLE]", "TBS0023" },
                    { new Guid("63fb8f7e-54b8-4900-9e90-d171a1763fc7"), "W92000004", "SPIRE YALE HOSPITAL", "TBS0019" },
                    { new Guid("4745febe-293f-40ec-bc10-8490a4b245cd"), "E92000001", "ST CHARLES HOSPITAL", "TBS0028" },
                    { new Guid("cd6e1eb8-0a2c-470d-8c2f-c8a6089a7e02"), "W92000004", "ST DAVID'S COMMUNITY HOSPITAL [CARDIFF]", "TBS0028" },
                    { new Guid("cfcedb91-8957-4483-98c8-a796b2d36508"), "E92000001", "ST JOHN & ST ELIZABETH HOSPITAL", "TBS0038" },
                    { new Guid("efc96ca7-099f-4d15-9420-de1e381833de"), "E92000001", "ST JAMES'S UNIVERSITY HOSPITAL [LEEDS]", "TBS0037" },
                    { new Guid("e2dda559-e27c-4e94-99eb-814e4f9fd476"), "E92000001", "ST JAMES HOSPITAL [SOUTHSEA]", "TBS0036" },
                    { new Guid("97b68a41-f8c9-49b5-be79-0f646c3e3ba1"), "E92000001", "ST HELIER HOSPITAL", "TBS0036" },
                    { new Guid("0e76ada3-8ccd-4426-a6d0-4b08663c38a5"), "E92000001", "ST HELENS REHABILITATION HOSPITAL [YORK]", "TBS0036" },
                    { new Guid("459f4747-a065-4288-8184-2536237f016b"), "E92000001", "ST HELENS HOSPITAL [MERSEYSIDE]", "TBS0035" },
                    { new Guid("9ba3ecfd-157a-418d-9fe4-5fc83add48b0"), "E92000001", "ST CHRISTOPHER'S HOSPITAL", "TBS0028" },
                    { new Guid("ecb3de7f-012b-4f8c-a56e-6d5813cc01ad"), "E92000001", "ST GEORGE'S HOSPITAL [TOOTING]", "TBS0034" },
                    { new Guid("9d01552f-d274-4cbc-a4a6-a15e4d34a72c"), "E92000001", "ST GEORGES HOSPITAL [MORPETH]", "TBS0032" },
                    { new Guid("db2877fd-2b4b-402e-9964-af271f1cd9ae"), "E92000001", "ST GEORGES HOSPITAL [LINCOLN]", "TBS0031" },
                    { new Guid("a54f2a19-1469-40d9-8913-799107453654"), "E92000001", "ST GEMMA'S HOSPICE", "TBS0030" },
                    { new Guid("b30f30f7-51e8-4805-b1d3-6d719a0b5b9a"), "E92000001", "ST EDMUND'S HOSPITAL [NORTHAMPTON]", "TBS0029" },
                    { new Guid("40bcdeef-7d37-4632-9893-70f454a50045"), "E92000001", "ST EDMUNDS HOSPITAL [BURY]", "TBS0028" },
                    { new Guid("3f83592d-30eb-4f87-a13e-94b62308cc5c"), "W92000004", "ST DAVIDS HOSPITAL [CARMARTHEN]", "TBS0028" },
                    { new Guid("acc36813-9c02-46fd-88a6-3358b3bfe182"), "E92000001", "ST GEORGE'S HOSPITAL [STAFFORD]", "TBS0033" },
                    { new Guid("dc26d176-1277-4ce2-a1a9-4ccadd053e27"), "E92000001", "ST JOHNS HOSPITAL", "TBS0039" },
                    { new Guid("339e7a48-b7cb-4947-99d2-e5943533f2cc"), "E92000001", "SPIRE WELLESLEY HOSPITAL", "TBS0019" },
                    { new Guid("0838cd26-5690-461e-bcf4-11de5e20d606"), "E92000001", "SPIRE TUNBRIDGE WELLS HOSPITAL", "TBS0019" },
                    { new Guid("053ae50b-3b39-44b7-89ec-1bce415bd846"), "E92000001", "SPIRE LEEDS HOSPITAL", "TBS0006" },
                    { new Guid("c9c50578-f0c3-4ff0-8438-409c9a17a7a0"), "E92000001", "SPIRE LEA CAMBRIDGE HOSPITAL", "TBS0006" },
                    { new Guid("d0602bd5-530d-474e-bf74-532bfbfeb6d8"), "E92000001", "SPIRE HULL AND EAST RIDING HOSPITAL", "TBS0006" },
                    { new Guid("f72c4f52-85c3-4ef8-9abf-b636b293b417"), "E92000001", "SPIRE HARTSWOOD HOSPITAL", "TBS0006" },
                    { new Guid("4e5b013c-66d5-4098-ad3f-78d03587c653"), "E92000001", "SPIRE HARPENDEN HOSPITAL", "TBS0006" },
                    { new Guid("f81364f2-4002-4d20-8728-3997c24a62b4"), "E92000001", "SPIRE GATWICK PARK HOSPITAL", "TBS0005" },
                    { new Guid("8fa5e547-b7c7-45cb-8912-8609d6c16f1d"), "E92000001", "SPIRE LEICESTER HOSPITAL", "TBS0007" },
                    { new Guid("26b15340-f812-4993-87d8-1a6e32c8115c"), "E92000001", "SPIRE FYLDE COAST HOSPITAL", "TBS0004" },
                    { new Guid("6a7f7bd2-5667-4cf8-a857-e8ae47871898"), "E92000001", "SPIRE DUNEDIN HOSPITAL", "TBS0002" },
                    { new Guid("fcaec936-3aff-4b19-a1a0-048ea2a06ae0"), "E92000001", "SPIRE CLARE PARK HOSPITAL", "TBS0001" },
                    { new Guid("266014b2-0bc5-4daf-9bff-219f1c77db68"), "E92000001", "SPIRE CHESHIRE HOSPITAL", "TBS0265" },
                    { new Guid("8f3159bc-446d-4e05-ad90-b26b5a983c6c"), "W92000004", "SPIRE CARDIFF HOSPITAL", "TBS0264" },
                    { new Guid("5c5cbc05-7e6a-4b10-add0-7f7638ec1918"), "E92000001", "SPIRE BUSHEY HOSPITAL", "TBS0263" },
                    { new Guid("2f262b76-8fef-47f8-b650-eeda51d76200"), "E92000001", "SPIRE BRISTOL HOSPITAL", "TBS0263" },
                    { new Guid("34a57071-c138-4f11-b5e8-15bbc4b5462d"), "E92000001", "SPIRE ELLAND HOSPITAL", "TBS0003" },
                    { new Guid("2c216f57-6c1a-4e50-811a-5f8a5b8568b3"), "E92000001", "SPIRE WASHINGTON HOSPITAL", "TBS0019" },
                    { new Guid("6e03ad18-2623-42cf-a39a-1a78dcef8714"), "E92000001", "SPIRE LITTLE ASTON HOSPITAL", "TBS0008" },
                    { new Guid("6db4a299-9431-4e71-a0d9-c97b186ce043"), "E92000001", "SPIRE LIVINGSTON CLINIC", "TBS0010" },
                    { new Guid("292d7a2b-1b87-4644-ad43-83f1301a0c94"), "E92000001", "SPIRE THAMES VALLEY HOSPITAL", "TBS0019" },
                    { new Guid("2332b4f9-b6c0-42a5-86d6-eca9837d40a7"), "E92000001", "SPIRE SUSSEX HOSPITAL", "TBS0019" },
                    { new Guid("21f28284-c7b7-4cf3-9b55-03b80ba14477"), "E92000001", "SPIRE ST SAVIOUR'S HOSPITAL", "TBS0019" },
                    { new Guid("671be757-50e5-4636-912e-707241af5149"), "E92000001", "SPIRE SOUTHAMPTON HOSPITAL", "TBS0019" },
                    { new Guid("02c4ada5-6853-448e-ae72-33e1395f7cf3"), "E92000001", "SPIRE SOUTH BANK HOSPITAL", "TBS0018" },
                    { new Guid("4d5c6fdb-dc53-44d3-8800-6a6babc9ea35"), "E92000001", "SPIRE RODING HOSPITAL", "TBS0018" },
                    { new Guid("90546964-e70f-4518-a11c-e912c7812587"), "E92000001", "SPIRE LIVERPOOL HOSPITAL", "TBS0009" },
                    { new Guid("0e7e5809-d38e-40d7-9c70-b69660c244c7"), "E92000001", "SPIRE REGENCY HOSPITAL", "TBS0017" },
                    { new Guid("8d98d3c3-2705-40c3-9c9c-c22730bd711e"), "E92000001", "SPIRE PARKWAY HOSPITAL", "TBS0015" },
                    { new Guid("c72ae942-1baa-4136-a21f-39438bf52f71"), "E92000001", "SPIRE NORWICH HOSPITAL", "TBS0014" },
                    { new Guid("42047d4a-7837-419c-87b0-ec879f751745"), "E92000001", "SPIRE MURRAYFIELD HOSPITAL", "TBS0013" },
                    { new Guid("48b7a7b0-ed79-45af-92cf-25445c376102"), "E92000001", "SPIRE METHLEY PARK HOSPITAL", "TBS0012" },
                    { new Guid("c29f5a18-29e6-45ca-9f70-f436002750ae"), "E92000001", "SPIRE MANCHESTER HOSPITAL", "TBS0012" },
                    { new Guid("d473c823-ec99-43f8-ae8f-23eb332b1b29"), "E92000001", "SPIRE LONGLANDS CONSULTING ROOMS?", "TBS0011" },
                    { new Guid("d383e248-a391-4cad-8394-85871b372412"), "E92000001", "SPIRE PORTSMOUTH HOSPITAL", "TBS0016" },
                    { new Guid("b568b5fb-ff20-4cd5-bb76-47375e796653"), "W92000004", "ST JOSEPH'S HOSPITAL", "TBS0040" },
                    { new Guid("b9262d3f-b00c-47e4-a6fb-7891523fce51"), "E92000001", "ST LEONARDS HOSPITAL [RINGWOOD]", "TBS0040" },
                    { new Guid("0370a142-f5ed-4c1e-991c-5d92b2aacb40"), "E92000001", "ST LEONARDS HOSPITAL [SUDBURY]", "TBS0041" },
                    { new Guid("de3ccd70-38df-460b-9a20-e0857c55c292"), "E92000001", "SUTTON HOSPITAL", "TBS0064" },
                    { new Guid("04dd6731-97a7-4fad-bfde-d95c98cd04c5"), "E92000001", "SURBITON HOSPITAL", "TBS0064" },
                    { new Guid("0cb2dff3-5ab3-46b2-ad45-e70716d8b853"), "E92000001", "SUNDERLAND ROYAL HOSPITAL", "TBS0064" },
                    { new Guid("cc66f1f9-f212-4007-9797-bc61d23eb847"), "E92000001", "SUNDERLAND EYE INFIRMARY", "TBS0063" },
                    { new Guid("9a08a374-5112-4386-9ea1-100819754d8b"), "E92000001", "STROUD MATERNITY HOSPITAL", "TBS0063" },
                    { new Guid("a5aa2617-09b7-4889-a863-2dfa6d254236"), "E92000001", "STROUD GENERAL HOSPITAL", "TBS0062" },
                    { new Guid("20f1a936-225b-4b47-9ecd-cedc4fb2fd45"), "E92000001", "SWANAGE COMMUNITY HOSPITAL", "TBS0064" },
                    { new Guid("db845686-5165-43fd-9848-2751e2e5d27e"), "E92000001", "STRETFORD MEMORIAL HOSPITAL", "TBS0062" },
                    { new Guid("d5e9268f-e45e-48cd-aa4d-35a846fecb45"), "E92000001", "STRATFORD HOSPITAL", "TBS0061" },
                    { new Guid("3fa1136d-a326-49e0-907f-8e12db7f8429"), "E92000001", "STONEBURY DAY HOSPITAL", "TBS0060" },
                    { new Guid("458809a5-5ae8-4d9d-8af3-d4249d14a5de"), "E92000001", "STONE HOUSE HOSPITAL", "TBS0060" },
                    { new Guid("b6333773-5b85-45d6-aa19-a60bb101b659"), "E92000001", "STOKE MANDEVILLE HOSPITAL", "TBS0059" },
                    { new Guid("71259dbf-b9b8-46ed-a7bd-9c256bdf848b"), "E92000001", "STEWART DAY HOSPITAL", "TBS0059" },
                    { new Guid("f6f75ed0-c2ba-4af1-b62a-28559b3f34ef"), "E92000001", "STEPPING HILL HOSPITAL", "TBS0058" },
                    { new Guid("4e9c7a1d-a42d-414a-9d14-4e2986c939f0"), "E92000001", "STRATTON HOSPITAL", "TBS0062" },
                    { new Guid("26c2e669-97e7-4e78-97e2-b87af2729ea7"), "E92000001", "STEAD PRIMARY CARE HOSPITAL", "TBS0057" },
                    { new Guid("d5a39d4b-bfa0-4ff0-b6b5-d1b75d75d10c"), "W92000004", "SWN-Y-GWYNT DAY HOSPITAL", "TBS0064" },
                    { new Guid("bcf74c38-ce96-4cbf-8656-eb25021d23b0"), "E92000001", "TAMESIDE GENERAL HOSPITAL", "TBS0065" }
                });

            migrationBuilder.InsertData(
                table: "Hospital",
                columns: new[] { "HospitalId", "CountryCode", "Name", "TBServiceCode" },
                values: new object[,]
                {
                    { new Guid("fd9c1d05-d245-4da8-b447-34b0f59244c6"), "E92000001", "TICKHILL ROAD HOSPITAL", "TBS0076" },
                    { new Guid("26b4b173-8990-41c6-8099-405d26f158d2"), "E92000001", "THURROCK COMMUNITY HOSPITAL", "TBS0075" },
                    { new Guid("04248670-4d1f-4026-bf22-e913208e672e"), "E92000001", "THREE SHIRES HOSPITAL", "TBS0074" },
                    { new Guid("533f62df-234b-41a9-8735-27b0849fa018"), "E92000001", "THORNBURY HOSPITAL [SHEFFIELD]", "TBS0073" },
                    { new Guid("b787c819-9215-4757-80e6-5349c10d1564"), "E92000001", "THORNBURY HOSPITAL [BRISTOL]", "TBS0072" },
                    { new Guid("775cb1aa-73b9-480b-9edd-23108f1f05e7"), "E92000001", "THOMAS LINACRE CENTRE", "TBS0071" },
                    { new Guid("391fbb68-4531-4f6c-9e8d-b79b8ca93608"), "E92000001", "SYLVAN HOSPITAL", "TBS0064" },
                    { new Guid("2e04b26e-1e81-4022-8d4e-f3817c3fbe47"), "E92000001", "THAME COMMUNITY HOSPITAL", "TBS0070" },
                    { new Guid("d4d54135-d9e7-4664-b6f2-38d91b3da119"), "W92000004", "TENBY COTTAGE HOSPITAL", "TBS0068" },
                    { new Guid("2a1aede9-90b4-4967-84c7-e2788fde1eec"), "E92000001", "TENBURY COMMUNITY HOSPITAL", "TBS0067" },
                    { new Guid("0a8ab02a-c64b-459e-8ff8-1f96bb8a0159"), "E92000001", "TEIGNMOUTH HOSPITAL", "TBS0066" },
                    { new Guid("e310a5bd-842e-4298-a162-667abb8e9dd2"), "E92000001", "TEDDINGTON MEMORIAL HOSPITAL", "TBS0065" },
                    { new Guid("f934b137-6e22-4dc8-bcc4-272f75d7f6df"), "E92000001", "TAVISTOCK HOSPITAL", "TBS0065" },
                    { new Guid("4f24dd69-5bc8-4f1c-99f5-2260817643a6"), "E92000001", "TARPORLEY WAR MEMORIAL HOSPITAL", "TBS0065" },
                    { new Guid("dbd45055-c1c6-46f5-828f-392e9c9ba04b"), "E92000001", "TEWKESBURY GENERAL HOSPITAL", "TBS0069" },
                    { new Guid("818340c5-52a3-44c5-be80-957033a3dcda"), "E92000001", "STANDISH HOSPITAL", "TBS0057" },
                    { new Guid("fa7f0d01-6347-41c3-b183-e504eb17cc6c"), "E92000001", "STAMFORD & RUTLAND HOSPITAL", "TBS0056" },
                    { new Guid("10ade1d8-e91f-44e1-b6fb-6fdbcf38edca"), "E92000001", "STAFFORDSHIRE GENERAL HOSPITAL", "TBS0056" },
                    { new Guid("98f82c09-d243-4c39-8260-4874c71a0433"), "E92000001", "ST MARY'S HOSPITAL [MELTON MOWBRAY]", "TBS0051" },
                    { new Guid("7d3100cc-0a92-4256-8988-4f9101ee8f45"), "E92000001", "ST MARY'S HOSPITAL [MANCHESTER]", "TBS0051" },
                    { new Guid("5911c2f6-5441-430b-98f4-369132ce4513"), "E92000001", "ST MARY'S HOSPITAL [LONDON]", "TBS0050" },
                    { new Guid("a2c4eace-1bef-4773-bdbc-d6fad216867b"), "E92000001", "ST MARY'S HOSPITAL [LEEDS]", "TBS0049" },
                    { new Guid("cd65d55f-1eb0-43c2-a2f0-f08790090ac8"), "E92000001", "ST MARY'S HOSPITAL [KETTERING]", "TBS0048" },
                    { new Guid("1e017501-e18e-471c-9a41-433367097fba"), "E92000001", "ST MARY'S HOSPITAL [ISLE OF WIGHT]", "TBS0047" },
                    { new Guid("fc02f232-e6c9-477a-bc1c-28b800038857"), "E92000001", "ST MARY'S HOSPITAL [PORTSMOUTH]", "TBS0051" },
                    { new Guid("9fa43296-0946-4e57-bb3c-1e599b6d611b"), "E92000001", "ST MARYS [GLOUCESTER]", "TBS0046" },
                    { new Guid("fc53ef0a-c36f-46d9-b6f0-f8f86cd9cb00"), "E92000001", "ST MARTINS HOSPITAL [BATH]", "TBS0044" },
                    { new Guid("23aa321d-33d9-4b4f-ac38-f585a004b1d2"), "E92000001", "ST MARKS HOSPITAL [MAIDENHEAD]", "TBS0044" },
                    { new Guid("795bb9c1-8d85-4b38-ad40-d77344139fc1"), "E92000001", "ST MARK'S HOSPITAL [HARROW]", "TBS0043" },
                    { new Guid("e83fcec2-0045-4e25-b86d-95451479d08b"), "E92000001", "ST LUKE'S HOSPITAL [MIDDLESBROUGH]", "TBS0043" },
                    { new Guid("07c51540-3fb0-44d6-8c40-c98a4ff59ae2"), "E92000001", "ST LUKE'S HOSPITAL [HUDDERSFIELD]", "TBS0043" },
                    { new Guid("46ddbeee-a422-4794-9640-0c114e1f6ac9"), "E92000001", "ST LUKE'S HOSPITAL [BRADFORD]", "TBS0042" },
                    { new Guid("36910905-2275-4316-beb5-241396e8b893"), "E92000001", "ST MARTINS HOSPITAL [CANTERBURY]", "TBS0045" },
                    { new Guid("1ca2bb07-2fb7-4e0c-a853-0568d7452ba0"), "E92000001", "ST MARY'S HOSPITAL [SCARBOROUGH]", "TBS0051" },
                    { new Guid("695d7449-a7bf-46dd-9e7e-6d49f05c24d1"), "E92000001", "ST MARY'S HOSPITAL [ST MARY'S]", "TBS0052" },
                    { new Guid("f424ce2a-d643-4cb6-99dd-98126cb2c2ce"), "E92000001", "ST MICHAEL'S HOSPITAL [BRAINTREE]", "TBS0053" },
                    { new Guid("c1adb3e8-8d55-4a30-86e1-8834c328e6f0"), "E92000001", "ST. MARGARET'S HOSPITAL", "TBS0056" },
                    { new Guid("ed102eed-92f5-449e-8a9a-ac26b587c0a5"), "W92000004", "ST WOOLOS", "TBS0055" },
                    { new Guid("fa6a9204-08c5-41ae-911e-d3510ff9b9ba"), "W92000004", "ST TYDFILS HOSPITAL", "TBS0054" },
                    { new Guid("fbf5eed1-36ed-49ef-8205-89fc822b3f28"), "E92000001", "ST THOMAS HOSPITAL [STOCKPORT]", "TBS0054" },
                    { new Guid("0f2aa7de-6615-400e-bb14-ec960d42994d"), "E92000001", "ST THOMAS' HOSPITAL [LONDON]", "TBS0054" },
                    { new Guid("2ae06413-a942-4c89-8de1-1fa212fcd64b"), "E92000001", "ST RICHARD'S HOSPITAL", "TBS0054" },
                    { new Guid("c58dfe16-bcf6-4380-97e2-a2250c63b41d"), "E92000001", "ST PETER'S HOSPITAL [MALDON]", "TBS0054" },
                    { new Guid("6e1a2fb2-f33d-407e-b3df-8f01115d9d47"), "E92000001", "ST PETER'S HOSPITAL [CHERTSEY]", "TBS0054" },
                    { new Guid("5d75da70-d99e-46d6-a066-97347f6ee5c4"), "E92000001", "ST PAUL'S HOSPITAL", "TBS0054" },
                    { new Guid("266fa510-3c54-4aad-92bf-f30bda272d68"), "E92000001", "ST PANCRAS HOSPITAL", "TBS0053" },
                    { new Guid("c0185c82-7014-4a10-85a5-a644cbf4572b"), "E92000001", "ST OSWALDS HOSPITAL", "TBS0053" },
                    { new Guid("9e2bd90d-dd34-4355-af2a-2917065629f0"), "E92000001", "ST NICHOLAS HOSPITAL", "TBS0053" },
                    { new Guid("768cfc30-fc07-420f-86ce-d6f91501d253"), "E92000001", "ST MONICAS HOSPITAL", "TBS0053" },
                    { new Guid("a661eb9f-262c-49e8-bc53-1010973021ab"), "E92000001", "ST MICHAEL'S HOSPITAL [HAYLE]", "TBS0053" },
                    { new Guid("1cfd833f-76ca-41e5-b151-65045df09358"), "E92000001", "ST MICHAEL'S HOSPITAL [BRISTOL]", "TBS0053" },
                    { new Guid("b50b66ed-8182-4ef8-9b7d-848f14cb3e91"), "E92000001", "SPIRE BARNSLEY CONSULTING ROOMS", "TBS0263" },
                    { new Guid("653f8e76-c913-4894-8957-f4d812593f1b"), "W92000004", "YSTRADGYNLAIS COMMUNITY HOSPITAL", "TBS0157" },
                    { new Guid("4a93fc07-d694-46be-bd34-fb31de211c57"), "E92000001", "NEW HALL HOSPITAL", "TBS0077" },
                    { new Guid("447a23ce-6449-47c3-90ea-081681d6c5fd"), "E92000001", "NEW CROSS HOSPITAL", "TBS0077" },
                    { new Guid("2e32407c-b568-447f-8c4a-052dd3a1fd67"), "E92000001", "CHIPPING NORTON HOSPITAL", "TBS0109" },
                    { new Guid("f0d93c98-3a4d-4efd-90b6-70d7cfb25617"), "E92000001", "CHIPPENHAM COMMUNITY HOSPITAL", "TBS0109" },
                    { new Guid("4513e509-0e43-4c2f-b4b4-9abd4d81a856"), "E92000001", "CHINGFORD HOSPITAL", "TBS0109" },
                    { new Guid("225fad12-be15-414d-8e4c-178f7c6aa763"), "E92000001", "CHILTERN HOSPITAL", "TBS0109" },
                    { new Guid("4fc9ec5f-e93e-4447-9cdd-62a8f8e8170f"), "E92000001", "CHESTERFIELD ROYAL HOSPITAL", "TBS0109" },
                    { new Guid("938e9578-51e7-4bc6-8760-97069a6cf535"), "E92000001", "CHESTER LE STREET HOSPITAL", "TBS0108" },
                    { new Guid("842f158f-9354-4222-81c5-e4416e37255e"), "W92000004", "CHIRK COMMUNITY HOSPITAL", "TBS0109" },
                    { new Guid("c2e5388f-a002-4e74-b924-ae34eb7fd6b0"), "E92000001", "CHESHUNT COMMUNITY HOSPITAL", "TBS0108" },
                    { new Guid("121c9756-16e7-4186-a1d4-e36b51b57dfb"), "E92000001", "CHERRY KNOWLE HOSPITAL", "TBS0108" },
                    { new Guid("20c6e25b-ca1f-46f4-a52c-27036a7858d4"), "W92000004", "CHEPSTOW COMMUNITY HOSPITAL", "TBS0107" },
                    { new Guid("81f824fb-01f7-4941-bbd6-17a7e9a7a77f"), "E92000001", "CHELTENHAM GENERAL HOSPITAL", "TBS0107" },
                    { new Guid("590b226a-d7f6-4f1c-a30e-f94a0a1ba53f"), "E92000001", "CHELSFIELD PARK HOSPITAL", "TBS0107" },
                    { new Guid("5b733263-b172-492b-9fa2-81d5cd867d6d"), "E92000001", "CHELSEA & WESTMINSTER HOSPITAL", "TBS0107" },
                    { new Guid("379d9d0f-a629-4d21-bdbb-1702f2960ee8"), "E92000001", "CHELMSFORD & ESSEX HOSPITAL", "TBS0107" },
                    { new Guid("9e12885c-7511-4266-8eb9-19175f116167"), "E92000001", "CHERRY TREE HOSPITAL", "TBS0108" },
                    { new Guid("60593be6-cc9f-4a86-bbc0-f65da4b79b51"), "E92000001", "CHEADLE ROYAL HOSPITAL", "TBS0107" },
                    { new Guid("2ff75d5a-0145-4912-afa5-9d52c63bcd68"), "E92000001", "CHORLEY & SOUTH RIBBLE HOSPITAL", "TBS0109" },
                    { new Guid("545e51b4-30f1-4a2b-bc99-a4103e00b963"), "E92000001", "CHRISTIE HOSPITAL", "TBS0109" },
                    { new Guid("3f1725c7-758a-42a4-ab5d-21ee6ad2c8d5"), "E92000001", "COCKERMOUTH COMMUNITY HOSPITAL", "TBS0119" },
                    { new Guid("5d9e6668-df0b-4235-a9f7-e047a5eed8a3"), "W92000004", "CLYDACH WAR MEMORIAL HOSPITAL", "TBS0119" },
                    { new Guid("68806afc-e4a9-472b-8899-ba0426b1ebe5"), "E92000001", "CLITHEROE COMMUNITY HOSPITAL", "TBS0119" },
                    { new Guid("312b9df1-b85b-4b11-94e8-4b2034125b3f"), "E92000001", "CLIFTON HOSPITAL", "TBS0119" },
                    { new Guid("fe6a5f2d-0389-45de-8391-56d5b0f65374"), "E92000001", "CLEVEDON HOSPITAL", "TBS0118" },
                    { new Guid("74bf9182-ddd1-4bf5-87f0-d3195a79f0a9"), "E92000001", "CLEMENTINE CHURCHILL HOSPITAL", "TBS0117" },
                    { new Guid("d804bebd-f872-4b4a-868e-6879a0bf4bce"), "E92000001", "CHRISTCHURCH HOSPITAL", "TBS0109" },
                    { new Guid("17340cde-67c7-4203-9957-afa0c3275895"), "E92000001", "CLAYTON HOSPITAL", "TBS0116" },
                    { new Guid("22ddd848-f889-4a26-8b48-1b70d5d77719"), "E92000001", "CLACTON AND DISTRICT HOSPITAL", "TBS0114" },
                    { new Guid("2a4b6534-5b02-47bf-8c3d-c7fdcf8eaf61"), "E92000001", "CITY HOSPITAL [BIRMINGHAM]", "TBS0113" },
                    { new Guid("65f565da-c32c-4dc3-a357-08d4022f334b"), "E92000001", "CITY GENERAL HOSPITAL [S-O-T]", "TBS0112" },
                    { new Guid("eda85b84-4400-49fe-a011-cc06ea57aee3"), "E92000001", "CIRENCESTER HOSPITAL", "TBS0111" },
                    { new Guid("fe42ede2-95f5-4a2b-a6f9-6c7b4fd66bf4"), "E92000001", "CHURCHILL HOSPITAL", "TBS0110" },
                    { new Guid("ed941380-859c-4c53-aed0-1f4655a8da3e"), "E92000001", "CHURCH HILL HOUSE HOSPITAL", "TBS0110" },
                    { new Guid("51f9c113-1d15-4a02-b1a8-3d7edb3fefdb"), "E92000001", "CLATTERBRIDGE HOSPITAL", "TBS0115" },
                    { new Guid("0eee2ec2-1f3e-4175-be90-85aa33f0686c"), "E92000001", "COLCHESTER GENERAL HOSPITAL", "TBS0119" },
                    { new Guid("dd30aad5-4955-43fc-806e-3700ed14257e"), "E92000001", "CHEADLE HOSPITAL- NORTH STAFFS COMBINED HEALTHCARE", "TBS0106" },
                    { new Guid("5ab01d61-cf4b-46ec-8792-44632e8a7e4c"), "E92000001", "CHATSWORTH SUITE", "TBS0106" },
                    { new Guid("470649c0-3f2f-4f13-a23d-fcb1d6b9b6e4"), "E92000001", "CASTLE HILL HOSPITAL", "TBS0091" },
                    { new Guid("023f1000-b4d1-4887-b601-333a87bb6514"), "E92000001", "CARLTON HEALTH CLINIC", "TBS0090" },
                    { new Guid("9c8a6802-5175-45b4-8a62-82d19c5377ce"), "W92000004", "CARDIGAN & DISTRICT MEMORIAL HOSPITAL", "TBS0089" },
                    { new Guid("2906e828-41e8-47a6-9fd4-3da2e1ff9a06"), "W92000004", "CARDIFF ROYAL INFIRMARY", "TBS0089" },
                    { new Guid("383386f0-421f-46d6-b037-b73b44be95ad"), "E92000001", "CAPIO RIVERS HOSPITAL", "TBS0088" },
                    { new Guid("c07e2713-e938-4f52-92ff-ef19624eb235"), "E92000001", "CAPIO READING HOSPITAL", "TBS0087" },
                    { new Guid("3ed63450-486c-4ade-a188-07f6c9e8139d"), "E92000001", "CASTLEBERG HOSPITAL", "TBS0092" },
                    { new Guid("6385964d-19af-4fb0-ac9f-122a64305752"), "E92000001", "CAPIO OAKS HOSPITAL", "TBS0086" },
                    { new Guid("b481c80c-fead-4b41-9eb7-5bcc7dca7d7b"), "E92000001", "CAMBORNE REDRUTH COMMUNITY HOSPITAL", "TBS0086" },
                    { new Guid("265ba139-52f9-4207-b10a-feedb861cf85"), "E92000001", "CALDERSTONES HOSPITAL", "TBS0086" },
                    { new Guid("bfaa15e7-93f6-4b1a-822d-7a6bf5e1ecb6"), "E92000001", "CALDERDALE ROYAL HOSPITAL", "TBS0085" },
                    { new Guid("85d14bcc-8351-47f8-bfb9-000534f66834"), "W92000004", "CAERPHILLY DISTRICT MINERS HOSPITAL", "TBS0084" },
                    { new Guid("ab3317d3-f91e-408f-a639-d6f322b6fa0f"), "W92000004", "CAERNARFON COTTAGE HOSPITAL", "TBS0083" },
                    { new Guid("d059dd3f-7ee1-4fd3-bdd1-56034e08e99f"), "E92000001", "BUXTON HOSPITAL", "TBS0082" },
                    { new Guid("8e3a276f-e332-4bbf-955a-16ec59556460"), "E92000001", "CANNOCK CHASE HOSPITAL", "TBS0086" },
                    { new Guid("3d450887-56ad-48f6-9599-fdf5625126d4"), "E92000001", "CHAUCER HOSPITAL", "TBS0106" },
                    { new Guid("a4ddff83-2c02-4377-a034-0304e5e3c374"), "E92000001", "CASTLEFORD & NORMANTON DISTRICT HOSPITAL", "TBS0093" },
                    { new Guid("160b52ad-9881-4c07-930c-933315dab76e"), "N92000002", "CAUSEWAY HOSPITAL", "TBS0095" },
                    { new Guid("f7d4b02a-494c-466e-b7d6-9a11cb09267a"), "E92000001", "CHASE HOSPITAL", "TBS0105" },
                    { new Guid("514246ae-1e23-41c2-a6db-2bd6e4a9df4b"), "E92000001", "CHASE FARM HOSPITAL", "TBS0104" },
                    { new Guid("23d82b05-d327-4aca-b01f-afc856fb21a4"), "E92000001", "CHARTER MEDICAL CENTRE", "TBS0103" },
                    { new Guid("ac2a5bd8-b96b-4c0c-9540-6e9539fafd52"), "E92000001", "CHARING CROSS HOSPITAL", "TBS0102" },
                    { new Guid("e7cf1925-02b2-4761-a526-98733667e908"), "E92000001", "CHARD & DISTRICT HOSPITAL", "TBS0101" },
                    { new Guid("1a498852-ef37-4a94-9a87-44fba3869b70"), "E92000001", "CHAPEL ALLERTON HOSPITAL", "TBS0100" },
                    { new Guid("7e12c815-5491-4cf9-aa03-5f4528aed0e4"), "E92000001", "CATERHAM DENE HOSPITAL", "TBS0094" },
                    { new Guid("97e4ac81-920c-4877-9151-96b8aef40c36"), "E92000001", "CHANTRY HOUSE DAY HOSPITAL", "TBS0100" },
                    { new Guid("b0dd2ccd-a351-4621-92e7-8adacb8e55b4"), "E92000001", "CHADWELL HEATH HOSPITAL", "TBS0099" },
                    { new Guid("a01ef32a-f363-4276-b02f-256f6acdfe58"), "E92000001", "CENTRAL MIDDLESEX HOSPITAL", "TBS0098" },
                    { new Guid("36029668-0d6e-4ad8-a578-d6b0baace5a2"), "W92000004", "CEFNI HOSPITAL", "TBS0097" },
                    { new Guid("ae531d23-0411-47ee-b5a4-29e7074b7e21"), "W92000004", "CEFN COED DAY HOSPITAL", "TBS0096" },
                    { new Guid("9a45c03f-ab96-45a7-8f11-36c4bb210f60"), "E92000001", "CAVENDISH HOSPITAL", "TBS0095" },
                    { new Guid("6ea6858c-cf45-4143-8359-02fe6294ccda"), "E92000001", "CAVELL HOSPITAL", "TBS0095" },
                    { new Guid("214ec28c-2a69-4940-89ef-136bc462dfbe"), "E92000001", "CHALFONT'S & GERRARDS CROSS HOSPITAL", "TBS0100" },
                    { new Guid("d6bf4347-d6c1-49c2-9eac-f3cb57e082cc"), "E92000001", "BUTLEIGH HOSPITAL", "TBS0081" },
                    { new Guid("6614eacb-155f-4a13-aba1-c585877bc9a6"), "N92000002", "COLERAINE HOSPITAL", "TBS0119" },
                    { new Guid("1e1a4fb3-b8e4-4162-ab5f-913ceb70e007"), "E92000001", "COMPTON HOSPICE", "TBS0119" },
                    { new Guid("7337c2ec-97e0-44b9-b72c-1a7f5408884c"), "E92000001", "DORSET COUNTY HOSPITAL", "TBS0143" },
                    { new Guid("4a1177f9-6a35-43e9-b106-c309bd5313dd"), "E92000001", "DORKING GENERAL HOSPITAL", "TBS0142" },
                    { new Guid("344852e4-4684-4297-b44c-20de42c8234f"), "E92000001", "DONCASTER ROYAL INFIRMARY", "TBS0142" },
                    { new Guid("98bce02c-7f69-4cee-9f1f-8ecb6667fa7f"), "E92000001", "DONCASTER GATE HOSPITAL", "TBS0142" },
                    { new Guid("bd6dc2e0-43d6-42d7-8f78-d424abeaef2b"), "W92000004", "DOLGELLAU & BARMOUTH DISTRICT HOSPITAL", "TBS0141" },
                    { new Guid("db51d970-e418-4755-b7cd-beedeb36137f"), "E92000001", "DILKE MEMORIAL HOSPITAL", "TBS0140" },
                    { new Guid("086a670b-bbed-49f5-879e-6f402a43309c"), "E92000001", "DOUGLAS MACMILLAN HOSPICE", "TBS0144" },
                    { new Guid("e88b61ba-4d65-4a7c-b7b2-833a86472322"), "E92000001", "DIDCOT COMMUNITY HOSPITAL", "TBS0140" },
                    { new Guid("41e962f8-d31b-445d-a58d-6d91b74bf966"), "E92000001", "DEWSBURY & DISTRICT HOSPITAL", "TBS0140" },
                    { new Guid("9324dfba-f970-4d80-89a5-4af86ab62ed5"), "W92000004", "DEWI SANT DAY HOSPITAL", "TBS0140" },
                    { new Guid("1ebcb983-55d2-4365-ae0f-c91d1d19ef86"), "E92000001", "DEVONSHIRE ROAD HOSPITAL", "TBS0139" },
                    { new Guid("868dbe9d-8a26-4e06-ae43-9abeb6202aec"), "E92000001", "DEVIZES COMMUNITY HOSPITAL", "TBS0139" },
                    { new Guid("d32fe9db-0260-4107-a1f0-2f1e2c7615a4"), "E92000001", "DERRIFORD HOSPITAL", "TBS0138" },
                    { new Guid("29f693f4-4738-4163-a6a5-71005d9601bb"), "E92000001", "DEREHAM HOSPITAL", "TBS0138" },
                    { new Guid("ccef02e8-9448-4762-84c3-36e16f0bf43e"), "E92000001", "DIANA PRINCESS OF WALES HOSPITAL", "TBS0140" },
                    { new Guid("90554de3-8d9a-4332-bfa6-ac3c174a00dd"), "E92000001", "DERBYSHIRE ROYAL INFIRMARY", "TBS0137" },
                    { new Guid("80ffe3cc-baba-4785-963c-81b6a30a217d"), "E92000001", "DOVE DAY HOSPITAL", "TBS0145" },
                    { new Guid("be016df2-0002-46b3-afa2-eb4be27cda7d"), "E92000001", "DROITWICH SPA HOSPITAL", "TBS0147" },
                    { new Guid("13b6c143-587d-46f8-ba7b-5341482066f4"), "E92000001", "EDITH CAVELL HOSPITAL", "TBS0158" },
                    { new Guid("891688fd-d707-4413-bef4-d6692e1582d0"), "E92000001", "EDGWARE HOSPITAL", "TBS0157" },
                    { new Guid("de747d91-aa94-411e-a7c0-55873c96ad2c"), "E92000001", "EDGBASTON HOSPITAL", "TBS0157" },
                    { new Guid("b700b31e-6d6d-4ee5-90c7-5680b8cf34d7"), "E92000001", "EDENBRIDGE WAR MEMORIAL HOSPITAL", "TBS0157" },
                    { new Guid("f835d1ab-0e33-4291-b567-60aac42f8c6e"), "E92000001", "ECH - EAST CLEVELAND HOSPITAL", "TBS0157" },
                    { new Guid("a08d156f-0224-486c-850e-785408b7489a"), "E92000001", "EASTBOURNE DISTRICT GENERAL HOSPITAL", "TBS0156" },
                    { new Guid("4bbf4924-b27a-4ec3-94ae-df1004ec415c"), "N92000002", "DOWNE HOSPITAL", "TBS0146" },
                    { new Guid("d6f1b12c-0200-42c7-b8e9-7d2bdde46969"), "E92000001", "EAST SURREY HOSPITAL", "TBS0155" },
                    { new Guid("b2efe4a8-51f0-452b-9a9b-6b6d4ce1ca69"), "E92000001", "DURHAM COMMUNITY HOSPITAL", "TBS0153" },
                    { new Guid("3be2648f-d5b5-4e8d-880c-ab4dbeb989a6"), "E92000001", "DUNSTON HILL HOSPITAL", "TBS0152" },
                    { new Guid("3074f823-eb92-47b5-b434-75b860c5d7cc"), "E92000001", "DUNEDIN HOSPITAL", "TBS0151" },
                    { new Guid("131ccafa-9147-4bd3-93f4-157c70b56b4b"), "E92000001", "DUCHY HOSPITAL", "TBS0150" },
                    { new Guid("2a3b7e0e-7e95-4090-acff-d4759996a3cd"), "E92000001", "DUCHESS OF KENT HOSPITAL", "TBS0149" },
                    { new Guid("24766cb2-0f78-4c8f-a188-57f1e3fe9a30"), "E92000001", "DRYDEN ROAD DAY HOSPITAL", "TBS0148" },
                    { new Guid("0ef08231-4b51-4887-91cb-316408c02657"), "E92000001", "EALING HOSPITAL", "TBS0154" },
                    { new Guid("5e32e34a-16cd-4287-9788-d243793f1ba8"), "W92000004", "COLWYN BAY COMMUNITY HOSPITAL", "TBS0119" },
                    { new Guid("ecbc8d01-bd24-4f9f-85a7-cf3d3f4d4f22"), "E92000001", "DERBY CITY GENERAL HOSPITAL", "TBS0137" },
                    { new Guid("1087f367-ce99-4bcd-b7e1-704cbfd9105d"), "W92000004", "DENBIGH COMMUNITY HOSPITAL", "TBS0137" },
                    { new Guid("3871aa2d-b501-4e53-8d6e-346beebe9167"), "E92000001", "CRANLEIGH HOSPITAL", "TBS0125" },
                    { new Guid("c5db5f25-9997-4df8-8c5f-9307c5005843"), "N92000002", "CRAIGAVON AREA HOSPITAL", "TBS0124" },
                    { new Guid("fa8f6ee9-81a3-4d37-ba59-2399036597e9"), "E92000001", "COVENTRY & WARWICKSHIRE HOSPITAL", "TBS0124" },
                    { new Guid("f40b47db-0d19-40a3-80b1-55458b7ca68b"), "W92000004", "COUNTY HOSPITAL [PONTYPOOL]", "TBS0123" },
                    { new Guid("39395bd2-5f8c-4f1a-9245-e4337817e278"), "E92000001", "COUNTY HOSPITAL [DURHAM]", "TBS0123" },
                    { new Guid("7bcb517f-6cf3-4396-9d17-fa116d8ece59"), "E92000001", "COUNTESS OF CHESTER HOSPITAL", "TBS0123" },
                    { new Guid("62be61a4-7d43-415b-baae-92e83e521140"), "E92000001", "CRANLEIGH VILLAGE HOSPITAL", "TBS0126" },
                    { new Guid("85364a25-a67d-4194-83f0-d48e848b3fac"), "E92000001", "COSSHAM HOSPITAL", "TBS0122" },
                    { new Guid("a6c371f6-a52e-4821-bb68-c59e7f0a3b1e"), "E92000001", "CORBY COMMUNITY HOSPITAL", "TBS0120" },
                    { new Guid("7b4ce341-b369-4fa4-8410-15fd7e5a7499"), "E92000001", "CORBETT HOSPITAL", "TBS0119" },
                    { new Guid("3b194a7c-455b-4dbd-a9c7-95acc378e68e"), "E92000001", "COQUETDALE COTTAGE HOSPITAL", "TBS0119" },
                    { new Guid("778a2fae-8eb6-440b-b2c6-056b637b94e7"), "E92000001", "COOKRIDGE HOSPITAL", "TBS0119" },
                    { new Guid("4b90f8da-261c-44af-b4ad-d91fbe6915a4"), "E92000001", "CONQUEST HOSPITAL", "TBS0119" },
                    { new Guid("2b5f6287-4b9f-4eb6-97c3-abbbfbfeb84e"), "E92000001", "CONGLETON WAR MEMORIAL HOSPITAL", "TBS0119" },
                    { new Guid("75a8d82b-93a2-411f-b082-b26fde40217d"), "E92000001", "CORONATION HOSPITAL", "TBS0121" },
                    { new Guid("1a2ad18f-94ef-4c63-ab09-40ac192343c7"), "E92000001", "DENMARK ROAD DAY HOSPITAL", "TBS0137" },
                    { new Guid("6e27a0ed-58d1-4a1a-93bb-d757594c6a1b"), "E92000001", "CRAWLEY HOSPITAL", "TBS0127" },
                    { new Guid("87f31e3d-d28f-4f61-8b8c-17ffef38e149"), "E92000001", "CREWKERNE HOSPITAL", "TBS0129" },
                    { new Guid("ff098a16-6549-4b31-ae3f-e23f5d68713e"), "E92000001", "DELLWOOD HOSPITAL", "TBS0137" },
                    { new Guid("ded7a769-fded-46ff-864d-f0d687db5b61"), "E92000001", "DELANCEY HOSPITAL", "TBS0137" },
                    { new Guid("63824924-492d-4e99-92f9-92fcef210ff0"), "W92000004", "DEESIDE COMMUNITY HOSPITAL", "TBS0137" },
                    { new Guid("c562c7b6-2cf8-4ec1-8cb2-2fd0d40169aa"), "E92000001", "DAWLISH HOSPITAL", "TBS0137" },
                    { new Guid("db50fe9a-3561-4848-8aa7-f04ef2f69fa2"), "E92000001", "DARTMOUTH HOSPITAL", "TBS0137" },
                    { new Guid("337014ab-d284-4e98-8ae6-65d00232fc8b"), "E92000001", "DARLINGTON MEMORIAL HOSPITAL", "TBS0137" },
                    { new Guid("d2207ecd-576b-43b9-9a26-bbe647c96c0a"), "E92000001", "CREDITON HOSPITAL", "TBS0128" },
                    { new Guid("21c98b1a-bce5-482d-bcdd-a66090acdd31"), "E92000001", "DARENT VALLEY HOSPITAL", "TBS0136" },
                    { new Guid("4c86a46c-f9c9-45ea-8f85-409731b2d9e2"), "N92000002", "DAISY HILL HOSPITAL", "TBS0134" },
                    { new Guid("96f950fb-2331-413c-966e-5b7cf4c1c4a3"), "W92000004", "CYMLA DAY HOSPITAL", "TBS0133" },
                    { new Guid("9718b416-b72a-4fe0-b394-c59f0add5d7e"), "E92000001", "CUMBERLAND INFIRMARY", "TBS0132" },
                    { new Guid("9e4fd059-9f62-4e75-b049-33bf60d7e594"), "E92000001", "CROWBOROUGH WAR MEMORIAL HOSPITAL", "TBS0131" },
                    { new Guid("e78d3e47-1f78-4873-bef0-105912a4a28a"), "E92000001", "CROSS LANE HOSPITAL", "TBS0131" },
                    { new Guid("c910f3d0-3549-4ae8-be9b-3a1ee795b22e"), "E92000001", "CROMER HOSPITAL", "TBS0130" },
                    { new Guid("ae1bc8c1-6025-455b-900d-9476965e1904"), "E92000001", "DANETRE HOSPITAL", "TBS0135" },
                    { new Guid("94259dec-cb66-438a-9d2b-2d0e7f707fca"), "E92000001", "BURY GENERAL HOSPITAL", "TBS0080" },
                    { new Guid("aa31ed2b-808a-4919-8f7c-bca57aa9b2e1"), "E92000001", "BURNLEY GENERAL HOSPITAL", "TBS0079" },
                    { new Guid("4b61f76e-b9a2-4aef-a403-95a0e922b603"), "E92000001", "BURNHAM ON SEA WAR MEMORIAL HOSPITAL", "TBS0078" },
                    { new Guid("919633a7-e62d-490f-8e4b-84339da0ab9b"), "E92000001", "BARROW HOSPITAL", "TBS0028" },
                    { new Guid("a204ba25-78a5-409c-8e47-1f0de979d15f"), "E92000001", "BARNSLEY HOSPITAL", "TBS0028" },
                    { new Guid("2da3c93c-0df4-46d1-8370-b64c3ca8acf7"), "E92000001", "BARNET HOSPITAL", "TBS0028" },
                    { new Guid("16908b7a-58f8-4e17-a8e8-91888d86f52f"), "E92000001", "BARKING HOSPITAL", "TBS0028" },
                    { new Guid("769e9162-2ae1-4689-9721-a65912933bf6"), "N92000002", "BANGOR COMMUNITY HOSPITAL", "TBS0028" },
                    { new Guid("b2b3ca4a-fa44-4040-8ee5-44b05a56628c"), "E92000001", "AXMINSTER HOSPITAL", "TBS0027" },
                    { new Guid("db5d0848-c1cd-4e30-a28c-a6199621174c"), "E92000001", "BARROWBY HOUSE", "TBS0028" },
                    { new Guid("bd24c256-a61d-4c8e-b656-0f90bd2cff3d"), "E92000001", "AVENUE DAY HOSPITAL", "TBS0026" },
                    { new Guid("87ac9a14-5333-4de8-bac9-cbc1d5bf86a6"), "E92000001", "ASTLEY HOSPITAL", "TBS0025" },
                    { new Guid("b5cee958-0e64-41a4-9f66-bc008c7a59e9"), "E92000001", "ASHWORTH HOSPITAL", "TBS0025" },
                    { new Guid("5d9f3d17-74f1-412e-9b7f-70d6debd7a12"), "E92000001", "ASHTON HOUSE HOSPITAL", "TBS0025" },
                    { new Guid("c1c90529-f7d0-40d0-b644-9c15e3046c25"), "E92000001", "ASHTEAD HOSPITAL", "TBS0024" },
                    { new Guid("51f4291e-dddd-497d-bd04-158763e1a131"), "E92000001", "ASHINGTON HOSPITAL", "TBS0023" },
                    { new Guid("868e426f-b11d-45a3-bf2c-e0c31bed2c44"), "E92000001", "ASHFORD HOSPITAL", "TBS0022" },
                    { new Guid("c4460d63-44e4-412c-8de9-b052514e41b5"), "E92000001", "AUCKLAND PARK HOSPITAL", "TBS0025" },
                    { new Guid("c89248df-265c-4881-a9ae-2aa39b6a7a5c"), "E92000001", "ASHFIELD COMMUNITY HOSPITAL", "TBS0021" },
                    { new Guid("adc02fa9-c7dd-484f-bab0-830241d8dd43"), "W92000004", "BARRY HOSPITAL", "TBS0029" },
                    { new Guid("4fcc9e17-fee4-42e9-b4ea-151de79540ad"), "E92000001", "BASINGSTOKE AND NORTH HAMPSHIRE HOSPITAL", "TBS0031" },
                    { new Guid("490806c6-db6b-40ca-a528-84f5df075231"), "E92000001", "BENENDEN HOSPITAL", "TBS0042" },
                    { new Guid("b840b161-3fb0-470c-91be-a8107c1c90e4"), "N92000002", "BELFAST CITY HOSPITAL", "TBS0041" },
                    { new Guid("b033556e-8bd5-4277-bf79-9875774dc403"), "E92000001", "BEIGHTON COMMUNITY HOSPITAL (THE CHILD & FAMILY THERAPY TEAM)", "TBS0040" },
                    { new Guid("41036c83-c26e-4590-91a1-cf167e32fd1d"), "E92000001", "BEDFORD HOSPITAL", "TBS0040" },
                    { new Guid("fc9372fa-0d90-4257-9b2f-e4a80aaee992"), "E92000001", "BECKENHAM HOSPITAL", "TBS0039" },
                    { new Guid("1387c5d9-b653-43bd-93fc-20ce75edd420"), "E92000001", "BECCLES & DISTRICT HOSPITAL", "TBS0038" },
                    { new Guid("f4bf1e54-d0b4-421c-aa7e-eaa31101eff0"), "E92000001", "BASILDON & THURROCK HOSPITAL", "TBS0030" },
                    { new Guid("60d1768d-6d86-4a22-adc9-220b6a075308"), "E92000001", "BEAUMONT HOSPITAL", "TBS0037" },
                    { new Guid("d016bc16-ed2e-43cd-837d-8867a4435c99"), "E92000001", "BEACON DAY HOSPITAL", "TBS0036" },
                    { new Guid("3daf070a-73e3-4449-867a-c69dfc90c476"), "E92000001", "BATTLE HOSPITAL", "TBS0036" },
                    { new Guid("b74e4e2f-f51f-4fd3-8167-9ad2c8e588eb"), "E92000001", "BATH ROAD DAY HOSPITAL", "TBS0035" },
                    { new Guid("33ee2791-894f-45b2-880c-e9b83854fd07"), "E92000001", "BATH MINERAL HOSPITAL", "TBS0034" },
                    { new Guid("c1fba9f4-1d9f-45fe-b27c-71bd354dc286"), "E92000001", "BATH CLINIC", "TBS0033" },
                    { new Guid("fb9ed9e8-6779-427e-bafb-8c5c9cb2a119"), "E92000001", "BASSETLAW HOSPITAL", "TBS0032" },
                    { new Guid("54b0bd76-51e1-484a-8ad8-deeaa419d564"), "E92000001", "BEARDWOOD HOSPITAL", "TBS0036" },
                    { new Guid("a72c02db-34ae-4be2-96eb-7ed3f992499d"), "E92000001", "BENSHAM HOSPITAL", "TBS0043" },
                    { new Guid("18b079d8-ba76-4145-930b-547d4c3b623f"), "E92000001", "ASHBURTON AND BUCKFASTLEIGH HOSPITAL", "TBS0020" },
                    { new Guid("bfa31f50-c61d-4ff9-b92a-193242ebf89e"), "E92000001", "ARUNDAL HOSPITAL LODGE", "TBS0019" },
                    { new Guid("f5d814fc-d3a5-4b3b-af84-2895a05815f1"), "E92000001", "ALDEBURGH HOSPITAL", "TBS0011" },
                    { new Guid("a594c0c8-227b-414c-83c2-f069b3089f7c"), "E92000001", "AIREDALE GENERAL HOSPITAL", "TBS0010" },
                    { new Guid("2a4164c7-4487-4e33-acb1-7cd51f7ad158"), "E92000001", "AINTREE HOSPITALS - OPD", "TBS0009" },
                    { new Guid("00c14073-322b-433b-aec1-f1c0698d5489"), "E92000001", "AILSA CRAIG MEDICAL PRACTICE", "TBS0008" },
                    { new Guid("2dd2fe7c-62a8-45d9-b787-5ad15808ecc3"), "E92000001", "ADDENBROOKE'S HOSPITAL", "TBS0007" },
                    { new Guid("eea9a534-f4b5-457b-a8bb-73a2d40330b6"), "E92000001", "ADAMS DAY HOSPITAL", "TBS0006" },
                    { new Guid("489189b5-f8b5-465b-9b6a-7573d21cc238"), "E92000001", "ALDER HEY CHILDREN'S HOSPITAL", "TBS0012" },
                    { new Guid("a82e9405-d427-4db3-9a4b-5292eb76f31e"), "E92000001", "ACRE DAY HOSPITAL", "TBS0006" },
                    { new Guid("93fa0a6c-474d-4ae8-af23-952076f96336"), "E92000001", "ABINGDON COMMUNITY HOSPITAL", "TBS0006" },
                    { new Guid("b868aa30-5b13-44db-9d2d-8bd163e9bc7b"), "W92000004", "ABERYSTWYTH HEALTH CLINIC", "TBS0006" },
                    { new Guid("306eb541-8669-4c0a-a65a-6413efa8b96a"), "W92000004", "ABERTILLERY DISTRICT HOSPITAL", "TBS0005" },
                    { new Guid("884e400b-1b68-4234-96f4-7e1a2029b15d"), "W92000004", "ABERGELE HOSPITAL", "TBS0004" },
                    { new Guid("15e4f8fb-09bc-45e2-8d7c-d5f654465680"), "W92000004", "ABERDARE GENERAL HOSPITAL", "TBS0003" },
                    { new Guid("f9509f7f-1858-49cf-9bdb-0af54a20afdd"), "W92000004", "ABERBARGOED & DISTRICT HOSPITAL", "TBS0002" },
                    { new Guid("1200824a-cce6-491d-bd93-33e44f0b383b"), "E92000001", "ACCRINGTON VICTORIA HOSPITAL", "TBS0006" },
                    { new Guid("80608b56-dc0e-4205-ad07-5fbbc878d679"), "E92000001", "ASH ETON", "TBS0019" },
                    { new Guid("e9ca976c-fa94-4525-9eaa-f8f4f550249e"), "E92000001", "ALDERNEY HOSPITAL", "TBS0012" },
                    { new Guid("506c8e5f-467d-4ebf-8fca-66cf0a8b5e30"), "E92000001", "ALEXANDRA HOSPITAL [CHEADLE]", "TBS0014" },
                    { new Guid("6057e81e-a0ad-46f2-8ae9-dee253f2c600"), "E92000001", "ARROWE PARK HOSPITAL", "TBS0019" },
                    { new Guid("597979fe-6365-4b9f-8043-83d9897f90e6"), "N92000002", "ARDS HOSPITAL", "TBS0019" },
                    { new Guid("f0698af5-6f64-4a2e-b364-95779b438955"), "N92000002", "ANTRIM AREA HOSPITAL", "TBS0019" },
                    { new Guid("2773dd7b-6184-4569-8a6f-a1df9a9d23d3"), "E92000001", "ANDOVER WAR MEMORIAL HOSPITAL", "TBS0019" },
                    { new Guid("7fd94f8b-83fd-46bc-ad21-9cf5b0fda3d2"), "W92000004", "AMY EVANS MEMORIAL HOSPITAL", "TBS0019" },
                    { new Guid("db023ca7-b477-4825-ba71-35046ee0337f"), "W92000004", "AMMAN VALLEY HOSPITAL", "TBS0019" },
                    { new Guid("5786ff70-02fe-4d20-bad7-affd201fe419"), "E92000001", "ALDRINGTON DAY HOSPITAL", "TBS0013" },
                    { new Guid("b5788a2e-a238-49dd-813d-d1ce921258c9"), "E92000001", "AMERSHAM HOSPITAL", "TBS0019" },
                    { new Guid("38b840bd-8862-46fb-9120-0088e0b4fdf8"), "E92000001", "ALTRINCHAM GENERAL HOSPITAL", "TBS0019" },
                    { new Guid("f370618c-e33a-4663-90fc-7c986e4053ae"), "E92000001", "ALTON COMMUNITY HOSPITAL", "TBS0018" },
                    { new Guid("84e57dd3-4cb6-495e-92ae-f09810a6743f"), "N92000002", "ALTNAGELVIN AREA HOSPITAL", "TBS0018" },
                    { new Guid("14334461-c8de-477e-b881-04df0c0e7c32"), "E92000001", "ALNWICK INFIRMARY", "TBS0017" },
                    { new Guid("8bac3ff5-fd44-4989-b4e7-0b1846686eb3"), "E92000001", "ALFRED BEAN HOSPITAL", "TBS0016" },
                    { new Guid("b2574388-808a-4a1b-98b5-af09e9caac28"), "E92000001", "ALEXANDRA HOSPITAL [REDDITCH]", "TBS0015" },
                    { new Guid("7877b857-c94d-47c1-9636-e0ef6005ef89"), "E92000001", "AMBERSTONE HOSPITAL", "TBS0019" },
                    { new Guid("aa0aa029-25ab-41ee-9414-46b1c6d6c238"), "E92000001", "BERKELEY HOSPITAL", "TBS0043" },
                    { new Guid("6e2b2f97-c1f4-4ecb-b40f-9080bebdc965"), "E92000001", "BERKSHIRE INDEPENDENT HOSPITAL", "TBS0043" },
                    { new Guid("42c35589-dd1e-41e1-bc7c-e2b965e2d7ad"), "E92000001", "BERWICK INFIRMARY", "TBS0044" },
                    { new Guid("6fe27200-3464-48af-99f3-745c6a868089"), "E92000001", "BRIXHAM HOSPITAL", "TBS0065" },
                    { new Guid("f026fdcd-7baf-4c96-994c-20e436cc8c59"), "E92000001", "BRISTOL ROYAL INFIRMARY", "TBS0065" },
                    { new Guid("5ac5c52f-75ff-4787-9fcb-655e7020b3f9"), "E92000001", "BRISTOL ROYAL HOSPITAL FOR CHILDREN", "TBS0064" },
                    { new Guid("cf060c72-425b-47ac-a892-891658b59481"), "E92000001", "BRISTOL HOMOEOPATHIC HOSPITAL", "TBS0064" },
                    { new Guid("303e9e42-6d29-43a2-858c-e67554ba4ed9"), "E92000001", "BRISTOL GENERAL HOSPITAL", "TBS0064" },
                    { new Guid("6a87886f-97b7-40ab-813a-f063c8222a94"), "E92000001", "BRISTOL EYE HOSPITAL", "TBS0064" },
                    { new Guid("3d46d50f-22d7-44fa-bc9c-358c9a00dcfa"), "W92000004", "BRO DDYFI COMMUNITY HOSPITAL", "TBS0065" },
                    { new Guid("02ecfb85-cdab-4a30-b29d-18277190de8f"), "E92000001", "BRIGHTON GENERAL HOSPITAL", "TBS0064" },
                    { new Guid("3e39aae9-008b-4cd9-89e4-40d94fcb49dd"), "E92000001", "BRIDLINGTON & DISTRICT HOSPITAL", "TBS0063" },
                    { new Guid("097ea09c-71a4-4178-bf54-c2858ad9d493"), "E92000001", "BRIDGWATER HOSPITAL", "TBS0063" },
                    { new Guid("5a943494-26bd-4c4e-8277-5ed74d4d55d5"), "E92000001", "BRIDGNORTH HOSPITAL", "TBS0062" },
                    { new Guid("9df0c003-65cb-4c9b-a667-8eba40d5fe62"), "E92000001", "BRIDGEWAYS DAY HOSPITAL", "TBS0062" },
                    { new Guid("037bd8f1-701f-4dfc-a685-7cea2327c806"), "E92000001", "BRENTWOOD COMMUNITY HOSPITAL", "TBS0062" },
                    { new Guid("c9235f5d-775f-40de-aa3b-c9dec5678d73"), "E92000001", "BRENTFORD HOSPITAL", "TBS0061" },
                    { new Guid("bb4484b3-b0ca-4e63-88f6-cb84192835b2"), "E92000001", "BRIDPORT COMMUNITY HOSPITAL", "TBS0064" },
                    { new Guid("476fec49-f090-42f4-9c7c-058e1c10e226"), "E92000001", "BRAMPTON WAR MEMORIAL HOSPITAL", "TBS0060" },
                    { new Guid("3a356b79-9fce-4193-8f6a-b43de3d74d7b"), "E92000001", "BROADGREEN HOSPITAL", "TBS0065" },
                    { new Guid("bc985eea-5f97-4e02-8bea-c7987fcfa7dd"), "W92000004", "BRON Y GARTH HOSPITAL", "TBS0067" },
                    { new Guid("fcecc812-b678-42b4-9273-d7766014e515"), "E92000001", "BURDEN NEUROLOGICAL HOSPITAL", "TBS0077" },
                    { new Guid("65e05417-0c38-40a8-bc03-a60ac083cdd8"), "W92000004", "BUILTH WELLS COTTAGE HOSPITAL", "TBS0077" },
                    { new Guid("c39362be-0a0c-440a-a471-5daa4d41b33a"), "E92000001", "BUDOCK HOSPITAL", "TBS0077" },
                    { new Guid("87ec205d-9067-4c55-b3b2-0595705af675"), "E92000001", "BUCKNALL HOSPITAL", "TBS0077" },
                    { new Guid("82b70e77-fb46-44f1-85df-4809167bef8b"), "E92000001", "BUCKLAND HOSPITAL", "TBS0076" },
                    { new Guid("2fefbf18-8fed-467a-86af-cfc54d14964d"), "E92000001", "BUCKINGHAM HOSPITAL", "TBS0076" },
                    { new Guid("eeb163a4-245b-403d-838c-f03b4e195b5c"), "E92000001", "BROMYARD COMMUNITY HOSPITAL", "TBS0066" },
                    { new Guid("7fe7d00a-f53a-4069-a4ec-0065f14c2339"), "W92000004", "BRYNSEIONT HOSPITAL", "TBS0075" },
                    { new Guid("8f58e133-4d63-4527-a5c8-29a4eb1a726e"), "W92000004", "BRYN Y NEUADD DAY HOSPITAL", "TBS0073" },
                    { new Guid("37545212-7893-400a-a413-6e75f79500ac"), "W92000004", "BRYN BERYL HOSPITAL", "TBS0072" },
                    { new Guid("d9863306-f367-4ffb-92fa-90301ddabf2a"), "E92000001", "BROOMFIELD HOSPITAL", "TBS0071" },
                    { new Guid("2c8ba49a-64a7-43fa-81eb-a072b2a0253b"), "E92000001", "BROOKLANDS HOSPITAL", "TBS0070" },
                    { new Guid("8b46b848-3543-4dca-98b7-e6148aa6afb3"), "W92000004", "BRONLLYS HOSPITAL", "TBS0069" },
                    { new Guid("1f801293-d20c-41c7-8f65-808f8092543b"), "W92000004", "BRONGLAIS GENERAL HOSPITAL", "TBS0068" },
                    { new Guid("dfe60214-36ba-43cb-b863-401a259747eb"), "W92000004", "BRYNMAWR INTERIM DAY HOSPITAL", "TBS0074" },
                    { new Guid("1bdbc2b9-1971-4af8-ac2a-d3a8f06b8399"), "E92000001", "BRAMCOTE HOSPITAL", "TBS0060" },
                    { new Guid("5c0068e6-fa8e-43de-ad7e-6d2b7c497e10"), "E92000001", "BRADWELL HOSPITAL", "TBS0059" },
                    { new Guid("69870407-443d-458e-9190-565f8e08127a"), "E92000001", "BRADFORD ROYAL INFIRMARY", "TBS0059" },
                    { new Guid("c80bb3a7-eef4-48a6-aeed-a3e6ea055b17"), "E92000001", "BISHOPS WOOD HOSPITAL", "TBS0053" },
                    { new Guid("56b105cc-0f86-4260-b421-7f93628879dd"), "E92000001", "BISHOP AUCKLAND GENERAL HOSPITAL", "TBS0053" },
                    { new Guid("f6558ad2-1df5-4d80-8f5d-17fa2235b97b"), "E92000001", "BIRMINGHAM WOMENS HOSPITAL", "TBS0052" },
                    { new Guid("98f248ae-6791-40a6-b241-5753041596db"), "E92000001", "BIRMINGHAM CHILDREN'S HOSPITAL", "TBS0051" },
                    { new Guid("716c4d1e-34fc-4eb7-ac79-e65b3b5af255"), "E92000001", "BIRMINGHAM CHEST CLINIC", "TBS0051" },
                    { new Guid("a13d2e29-6f40-488f-8fc4-ba84101cfe0f"), "E92000001", "BIRKDALE CLINIC (CROSBY)", "TBS0051" },
                    { new Guid("e3792b8f-19f7-4ae1-ad55-43655ae27012"), "E92000001", "BLACKBERRY HILL HOSPITAL", "TBS0053" },
                    { new Guid("4e872775-ef55-4ad9-b176-27c9cf4e61ef"), "E92000001", "BIRCH HILL HOSPITAL", "TBS0051" },
                    { new Guid("8d01039e-986e-4e26-ad3e-2e30729da071"), "E92000001", "BILLINGE HOSPITAL", "TBS0049" },
                    { new Guid("6050e0bd-f829-403f-83f2-6ff2865b1f2d"), "E92000001", "BIDEFORD AND DISTRICT HOSPITAL", "TBS0048" },
                    { new Guid("4987e982-7774-42ac-b22f-a24bdf5e2c22"), "E92000001", "BICESTER COTTAGE HOSPITAL", "TBS0047" },
                    { new Guid("d2563ac7-9d6e-428f-8b4e-6cd1254f2612"), "E92000001", "BEXHILL COMMUNITY HOSPITAL", "TBS0046" },
                    { new Guid("177f8d11-d5b6-48cc-bcf4-2e49519949e4"), "E92000001", "BEVERLEY WESTWOOD HOSPITAL", "TBS0045" },
                    { new Guid("be836647-61ad-4068-824d-06198752c7e0"), "E92000001", "BETHLEM ROYAL HOSPITAL", "TBS0044" },
                    { new Guid("86101239-4962-40a4-90f3-48d6fd2bf8e2"), "E92000001", "BINGLEY HOSPITAL", "TBS0050" },
                    { new Guid("91d21969-295e-4a6e-a936-723ebd16e496"), "E92000001", "BLACKHEATH HOSPITAL", "TBS0053" },
                    { new Guid("a5804acc-75eb-42ac-ba4d-66867cf52964"), "E92000001", "BLACKPOOL VICTORIA HOSPITAL", "TBS0053" },
                    { new Guid("35ce13f9-a65c-424f-8309-b0fbdf769557"), "W92000004", "BLAINA & DISTRICT HOSPITAL", "TBS0053" },
                    { new Guid("cbcc7e03-0e5c-40b8-9dd2-e1e4ca2f2145"), "E92000001", "BRADFORD ON AVON COMMUNITY HOSPITAL", "TBS0058" },
                    { new Guid("1c39b75b-ba22-4574-afc0-459c0009d4b7"), "E92000001", "BRACKLEY COTTAGE HOSPITAL", "TBS0057" },
                    { new Guid("70f2cca0-2289-4482-9ebf-0be8e031203f"), "E92000001", "BOWOOD DAY HOSPITAL", "TBS0057" },
                    { new Guid("9857ff70-d992-4e8d-b97d-d174b70dfb74"), "E92000001", "BOVEY TRACEY HOSPITAL", "TBS0056" },
                    { new Guid("e8324d8b-03ab-47c6-8385-514fac6dca6a"), "E92000001", "BOURNE HEALTH CLINIC", "TBS0056" },
                    { new Guid("d0424f82-f884-450e-b259-821f08f1c29f"), "E92000001", "BOSCOMBE COMMUNITY HOSPITAL", "TBS0056" },
                    { new Guid("cdfa84d5-e31d-4920-bff0-53cbc78142f4"), "E92000001", "BOOTHAM PARK HOSPITAL", "TBS0055" },
                    { new Guid("1199d23b-65f8-49a9-aee1-eafeba2ddcae"), "E92000001", "BOOTH HALL HOSPITAL", "TBS0054" },
                    { new Guid("e738594b-19ef-4a88-9dbf-a5a7a5ff443a"), "E92000001", "BOLITHO HOSPITAL", "TBS0054" },
                    { new Guid("1b917fe6-3fb7-4fba-b5f5-744c69f9ca60"), "E92000001", "BOLINGBROKE HOSPITAL", "TBS0054" },
                    { new Guid("23c0ea5a-a196-4a80-a923-055631fd6866"), "E92000001", "BOGNOR REGIS WAR MEMORIAL HOSPITAL", "TBS0054" },
                    { new Guid("a63548af-fe08-47cf-acd0-d9ea702b4e0c"), "E92000001", "BODMIN HOSPITAL", "TBS0054" },
                    { new Guid("3ef911af-689d-41ae-8125-dcbb121c7c66"), "E92000001", "BLYTH COMMUNITY HOSPITAL", "TBS0054" },
                    { new Guid("49ae657f-2a66-4674-8aba-060bc6388a08"), "E92000001", "BLT PRIVATE HOSPITALS", "TBS0054" },
                    { new Guid("904ac274-f26b-4b8a-9fe3-8ab27cca3e9f"), "E92000001", "BLANDFORD COMMUNITY HOSPITAL", "TBS0053" },
                    { new Guid("80948edb-30fa-4d22-8f2c-c9bcc886c6ee"), "E92000001", "EDWARD HAIN HOSPITAL", "TBS0159" },
                    { new Guid("1393efe2-9921-45d4-a384-c3ee9400c5b5"), "E92000001", "NEW EPSOM & EWELL COTTAGE HOSPITAL", "TBS0077" },
                    { new Guid("17f93ffc-01e5-48e1-b1fd-10fc7308f9ec"), "E92000001", "ELDERLY DAY HOSPITAL", "TBS0160" },
                    { new Guid("14476a1d-6db0-49ae-bf4d-c0d95b51af21"), "E92000001", "ELLEN BADGER HOSPITAL", "TBS0160" },
                    { new Guid("d4bd1125-6c1e-4cfa-8f86-bc0ab6fc7216"), "W92000004", "LLANRHAEDR MEDICAL CLINIC", "TBS0028" },
                    { new Guid("54e4d207-1a85-46b9-9bfe-225b6ce450e5"), "W92000004", "LLANGOLLEN COMMUNITY HOSPITAL", "TBS0028" },
                    { new Guid("d6a781cb-700f-490d-b2c9-009b772be2dc"), "W92000004", "LLANFRECHFA GRANGE HOSPITAL", "TBS0028" },
                    { new Guid("ce450a3a-d9b5-4db8-9472-d1a36936528c"), "W92000004", "LLANDUDNO GENERAL HOSPITAL", "TBS0027" },
                    { new Guid("634f5af4-8a00-48b1-bbf4-cd68cbd1338d"), "W92000004", "LLANDOVERY HOSPITAL", "TBS0026" },
                    { new Guid("4cc99e95-6993-466a-8c7a-1bfce7159bb0"), "W92000004", "LLANDOUGH HOSPITAL", "TBS0025" },
                    { new Guid("8680a1b4-8271-4004-8b8c-b906ce6b8b4a"), "W92000004", "LLANRWST HEALTH CENTRE", "TBS0028" },
                    { new Guid("2d19c818-ba9d-4c38-baf4-42cf14844619"), "E92000001", "LIVERPOOL WOMANS HOSPITAL", "TBS0025" },
                    { new Guid("c2769933-d4a2-4571-90a3-d624c0ab6c70"), "E92000001", "LITTLE COURT DAY HOSPITAL", "TBS0025" },
                    { new Guid("13f42cb8-4430-487a-b995-f076dc4a26e0"), "E92000001", "LITTLE BROOK (APU) HOSPITAL", "TBS0024" },
                    { new Guid("8105af2a-2934-4b33-bd4c-faca0759208d"), "E92000001", "LISTER HOSPITAL", "TBS0023" },
                    { new Guid("0391ed46-0661-4cbe-924f-af0f5959ec19"), "E92000001", "LISKEARD COMMUNITY HOSPITAL", "TBS0022" },
                    { new Guid("47d83ca2-4ece-4f3c-8771-c8c24564245e"), "E92000001", "LINGS BAR HOSPITAL", "TBS0021" },
                    { new Guid("7b0d2fa1-757d-4fed-940d-b697572e6315"), "E92000001", "LINCOLN HOSPITAL", "TBS0020" },
                    { new Guid("93add9f7-2d74-4fd4-ba27-36fc483cb9eb"), "E92000001", "LIVERPOOL HEART AND CHEST HOSPITAL", "TBS0025" },
                    { new Guid("e1fc8251-56c4-47fb-8019-2bc0b22c6966"), "E92000001", "LINCOLN COUNTY HOSPITAL", "TBS0019" },
                    { new Guid("ed46f84f-98ba-444f-a27d-f0e27427cb1b"), "W92000004", "LLUESTY HOSPITAL", "TBS0028" },
                    { new Guid("ada8c647-dce1-44b9-8d6a-003927478fc1"), "W92000004", "LLWYNYPIA HOSPITAL", "TBS0029" },
                    { new Guid("40c7827b-9556-492f-af7b-f2c4a5a3f1f2"), "E92000001", "LYTHAM HOSPITAL", "TBS0040" },
                    { new Guid("98eed1ac-5537-4254-b0ff-7b590bc7fbb0"), "E92000001", "LYMINGTON HOSPITAL (PERIPHERAL CLINIC)", "TBS0040" },
                    { new Guid("f16b7c49-8180-438f-a114-9a2e848cac52"), "E92000001", "LYDNEY & DISTRICT HOSPITAL", "TBS0039" },
                    { new Guid("aead5943-bc92-402f-8687-9e6db1203f04"), "E92000001", "LUTON & DUNSTABLE HOSPITAL", "TBS0038" },
                    { new Guid("506123fe-47b0-4fbe-b7ec-c6644b184702"), "E92000001", "LUDLOW HOSPITAL", "TBS0037" },
                    { new Guid("5f17d1c6-6964-447a-aaa5-791e8895b8b0"), "E92000001", "LOWESTOFT & NORTH SUFFOLK HOSPITAL", "TBS0036" },
                    { new Guid("6576fd71-1dfd-414d-b743-deda97b6af0b"), "W92000004", "LLWYNERYR HOSPITAL", "TBS0028" },
                    { new Guid("67396be1-9139-4766-b08f-05a4c4c1bcff"), "E92000001", "LOWER PRIORY HALL DAY HOSPITAL", "TBS0036" },
                    { new Guid("d79cea01-9e36-48c2-8b0c-782e5d2bdd2a"), "E92000001", "LOURDES HOSPITAL", "TBS0035" },
                    { new Guid("94e6eba6-4e24-4a85-b3ba-c71ba0cde4b1"), "E92000001", "LOUGHBOROUGH HOSPITAL", "TBS0034" },
                    { new Guid("cb7c01c0-f60e-4071-b83f-3520279d6c4c"), "E92000001", "LONGTON HOSPITAL", "TBS0033" },
                    { new Guid("b284d100-3ee7-4f46-873d-3f75cf0f1540"), "E92000001", "LONGRIDGE COMMUNITY HOSPITAL", "TBS0032" },
                    { new Guid("b5a9c2bf-f95b-4d72-8d50-c213d2dd7d19"), "E92000001", "LONDON INDEPENDENT HOSPITAL", "TBS0031" },
                    { new Guid("072c1d97-ac4f-459b-b27f-f5a0b9d2abb8"), "E92000001", "LONDON CHEST HOSPITAL", "TBS0030" },
                    { new Guid("f4d45e6f-c8ef-4542-8dc5-a0c680d47e71"), "E92000001", "LOUTH COUNTY HOSPITAL", "TBS0036" },
                    { new Guid("fd8ca1cc-a143-4cf5-a03d-ca1e02014964"), "E92000001", "MACCLESFIELD DISTRICT GENERAL HOSPITAL", "TBS0041" },
                    { new Guid("29bb0ca3-3329-4c44-a7d1-f5d0f77c3308"), "E92000001", "LEWES VICTORIA HOSPITAL", "TBS0019" },
                    { new Guid("ced155c4-7eff-4c5e-8b38-1123d717d0bd"), "E92000001", "LEMINGTON HOSPITAL", "TBS0019" },
                    { new Guid("ff62a052-f283-4f0b-804a-bf5d744ef777"), "E92000001", "KING'S MILL HOSPITAL", "TBS0009" },
                    { new Guid("0ba53cca-6c30-4743-a21e-0664294e64d3"), "E92000001", "KING'S COLLEGE HOSPITAL (DULWICH)", "TBS0008" },
                    { new Guid("df433bc5-840a-42b2-b329-04df152de40e"), "E92000001", "KING'S COLLEGE HOSPITAL (DENMARK HILL)", "TBS0007" },
                    { new Guid("b3997026-533f-4e18-ae41-4d24f0a0f3d5"), "E92000001", "KING GEORGE HOSPITAL", "TBS0006" },
                    { new Guid("cef2d5ad-29fa-4d9d-9dee-549c7802a5f0"), "E92000001", "KING EDWARD VII HOSPITAL [WINDSOR]", "TBS0006" },
                    { new Guid("177e56f2-dd72-45e4-94c1-34244f00ede9"), "E92000001", "KING EDWARD VII HOSPITAL [MIDHURST]", "TBS0006" },
                    { new Guid("71251e92-d6f6-48c2-946a-47b310243215"), "E92000001", "KINGS OAK HOSPITAL", "TBS0010" },
                    { new Guid("f025f44d-515c-4e9c-aabe-e914d94e6694"), "E92000001", "KILLINGBECK HOSPITAL", "TBS0006" },
                    { new Guid("00caa2cd-0224-4b6d-beeb-caedc8a6dd2a"), "E92000001", "KEYNSHAM HOSPITAL", "TBS0005" },
                    { new Guid("ceb2aa32-3740-43a3-aa8a-a153da3cc82f"), "E92000001", "KETTERING GENERAL HOSPITAL", "TBS0004" },
                    { new Guid("811b7df1-6b75-46d7-8bfe-04513996f7e0"), "E92000001", "KENT & SUSSEX HOSPITAL", "TBS0003" },
                    { new Guid("f8c58840-0164-47e2-af08-9913cd994f64"), "E92000001", "KENT & CANTERBURY HOSPITAL", "TBS0002" },
                    { new Guid("13737a9f-f4aa-4831-b151-edd051eecf68"), "E92000001", "JOYCE GREEN HOSPITAL", "TBS0001" },
                    { new Guid("b7a95499-6115-4c97-ba9f-8f2fbef86169"), "E92000001", "JOHNSON HOSPITAL", "TBS0265" },
                    { new Guid("642b2ea5-16c2-4642-b811-b7920074a656"), "E92000001", "KIDDERMINSTER HOSPITAL", "TBS0006" },
                    { new Guid("82321cab-3092-4fbf-8b4a-b82ba7ef341d"), "E92000001", "LEOMINSTER COMMUNITY HOSPITAL", "TBS0019" },
                    { new Guid("b5cc1c63-8ab5-4c8a-b181-1488c29d59a7"), "E92000001", "KINGS PARK HOSPITAL", "TBS0011" },
                    { new Guid("cf398c9f-2fb9-4f22-a5a7-21687b670fb0"), "E92000001", "KINGTON COTTAGE HOSPITAL", "TBS0012" },
                    { new Guid("4d23d7b3-bbb8-4642-a218-b0063a833a14"), "E92000001", "LEIGHTON HOSPITAL", "TBS0019" },
                    { new Guid("3ba14e3a-67a5-4488-a15c-1075e07e9d32"), "E92000001", "LEIGH INFIRMARY", "TBS0019" },
                    { new Guid("482e828f-3d1f-46fa-9269-0ce594879246"), "E92000001", "LEICESTER ROYAL INFIRMARY", "TBS0019" },
                    { new Guid("bfaf326c-6452-4ce0-84e9-18fb607d0c2f"), "E92000001", "LEICESTER GENERAL HOSPITAL", "TBS0019" },
                    { new Guid("0df7bebb-e589-4bfb-a2bb-352ac1ccd2e1"), "E92000001", "LEICESTER FRITH HOSPITAL", "TBS0019" },
                    { new Guid("3e806307-573a-45f8-b295-06f95884ffc9"), "E92000001", "LEEK MOORLANDS HOSPITAL", "TBS0019" },
                    { new Guid("8e7f80d2-ecba-4b5a-b878-6587d94da1c5"), "E92000001", "KINGSTON HOSPITAL", "TBS0012" },
                    { new Guid("7e9c715d-0248-4d97-8f67-1134fc133588"), "E92000001", "LEEDS GENERAL INFIRMARY", "TBS0019" },
                    { new Guid("4eecfcad-b8e1-4cbe-860b-6f7d42c06da0"), "E92000001", "LEATHERHEAD HOSPITAL", "TBS0018" },
                    { new Guid("a7311eba-644f-436f-ba95-dbdd124d999a"), "E92000001", "LAUNCESTON HOSPITAL", "TBS0017" },
                    { new Guid("33468af8-31bf-40bf-a40c-a733879bf5e2"), "E92000001", "LANCASTER HOSPITAL", "TBS0016" },
                    { new Guid("b7a478b2-102b-4f92-870e-4ed979c162e4"), "E92000001", "LAMBETH HOSPITAL", "TBS0015" },
                    { new Guid("98b4f15b-dc64-440d-9df4-9e16c47c5b64"), "N92000002", "LAGAN VALLEY HOSPITAL", "TBS0014" },
                    { new Guid("e883218f-e410-4256-aa98-47be80158122"), "E92000001", "KNUTSFORD & DISTRICT COMMUNITY HOSPITAL", "TBS0013" },
                    { new Guid("1280ecc8-ff7d-473a-83d7-02e7fb2bafdb"), "E92000001", "LEDBURY COTTAGE HOSPITAL", "TBS0018" },
                    { new Guid("8ec2b877-6d80-4d09-8dd0-314964f81a16"), "E92000001", "JOHN RADCLIFFE HOSPITAL", "TBS0264" },
                    { new Guid("8d879b88-5e60-4a5e-91f0-83ecc4967eea"), "W92000004", "MAESGWYN HOSPITAL", "TBS0042" },
                    { new Guid("555f7e63-2756-4765-a9eb-1de1acdc9c72"), "E92000001", "MAIDSTONE DISTRICT GENERAL HOSPITAL", "TBS0043" },
                    { new Guid("12ab2e21-d3b9-4120-b623-8f786467e5af"), "E92000001", "MORETON IN MARSH HOSPITAL", "TBS0064" },
                    { new Guid("7502a3d3-de66-4140-a6d6-960ef7c7917f"), "E92000001", "MOORGREEN HOSPITAL", "TBS0064" },
                    { new Guid("95563a29-52ea-4d9d-b22f-c644462cebf6"), "E92000001", "MOORFIELDS EYE HOSPITAL (CITY ROAD)", "TBS0064" },
                    { new Guid("3df82031-1575-465b-a075-415e457bf7f5"), "E92000001", "MOORE HOUSE", "TBS0064" },
                    { new Guid("55f81174-38cc-43b4-911a-1d1246db6f5c"), "E92000001", "MOORE COTTAGE HOSPITAL", "TBS0064" },
                    { new Guid("0275af55-727a-4cac-8a96-2285ef484b50"), "W92000004", "MONTGOMERY COUNTY INFIRMARY", "TBS0063" },
                    { new Guid("f0ac5cc9-186e-4854-b103-98b515e91eac"), "E92000001", "MORPETH COTTAGE HOSPITAL", "TBS0064" },
                    { new Guid("c1ce55db-1f5b-4ad1-92ee-2c636716110d"), "E92000001", "MONTAGUE HEALTH CENTRE", "TBS0063" },
                    { new Guid("9d9c3480-5023-48d8-8850-8ad2e57de397"), "W92000004", "MONMOUTH GENERAL HOSPITAL", "TBS0062" },
                    { new Guid("bc50c79a-5248-4b84-b4b7-027b2c59371d"), "E92000001", "MONKWEARMOUTH HOSPITAL", "TBS0062" },
                    { new Guid("66a284f5-97a0-4962-876d-f33f0750a34b"), "E92000001", "MONKTON HALL HOSPITAL", "TBS0061" },
                    { new Guid("bd42b32e-820f-4961-a732-5e3bcb953f3f"), "E92000001", "MOLESEY HOSPITAL", "TBS0060" },
                    { new Guid("58395bde-7579-47c3-ade3-854acb2e2c5d"), "W92000004", "MOLD COMMUNITY HOSPITAL", "TBS0060" },
                    { new Guid("0e4b9e79-b47c-45de-89b9-58ffe3c3b2b4"), "W92000004", "MINFORDD HOSPITAL", "TBS0059" },
                    { new Guid("c9256947-12e1-4cca-bce3-50124b7fd273"), "E92000001", "MONTAGU HOSPITAL", "TBS0062" },
                    { new Guid("6d393c1b-2af0-4be6-8450-fad7d03d4de1"), "E92000001", "MINEHEAD HOSPITAL", "TBS0059" },
                    { new Guid("42f9a891-2dec-40c6-8475-d0eb28b51d51"), "W92000004", "MORRISTON HOSPITAL", "TBS0065" },
                    { new Guid("1fdcb443-7459-4af7-8980-007e62ef2e90"), "E92000001", "MOUNT ALVERNIA HOSPITAL", "TBS0065" },
                    { new Guid("65225bbb-bd5a-431c-9b0f-46e6349c03c2"), "E92000001", "NEVILL HOSPITAL", "TBS0077" },
                    { new Guid("09deb24a-ce0d-4add-8bf9-59b831f9caa4"), "W92000004", "NEVILL HALL HOSPITAL", "TBS0076" },
                    { new Guid("4ad9271b-a7b6-41b3-b8cb-e5b9a1d0c10d"), "E92000001", "NELSON HOSPITAL", "TBS0076" },
                    { new Guid("bbe36c0a-6070-475a-9db8-d75dbbdbf425"), "W92000004", "NEATH PORT TALBOT HOSPITAL", "TBS0075" },
                    { new Guid("01c3b0dc-ff1e-4a09-a59d-647862562b9d"), "E92000001", "NATIONAL HOSPITAL FOR NEUROLOGY & NEUROSCIENCES - QUEEN SQUARE", "TBS0074" },
                    { new Guid("099481b6-ff46-4247-85ef-d04d8d0524cc"), "W92000004", "NANT-Y-GLYN DAY HOSPITAL", "TBS0073" },
                    { new Guid("77ba7c6e-6aba-4575-8468-e8a522d5c478"), "E92000001", "MOSSLEY HILL HOSPITAL", "TBS0065" },
                    { new Guid("a8b004aa-1c5c-4ec3-83b4-8a932bb8117c"), "W92000004", "MYNYDD MAWR HOSPITAL", "TBS0072" },
                    { new Guid("52afafbe-de61-449d-9d8a-29d54aae6e35"), "N92000002", "MUSGRAVE PARK HOSPITAL [NORTHERN IRELAND]", "TBS0070" },
                    { new Guid("4f16fca0-be6e-4edd-9b2c-5ca70511e480"), "W92000004", "MOUNTAIN ASH GENERAL HOSPITAL", "TBS0069" },
                    { new Guid("f768b1f6-dfc3-43bd-ab29-f03277c812b3"), "E92000001", "MOUNT VERNON HOSPITAL", "TBS0068" },
                    { new Guid("f26b852b-1da4-443b-a062-44696556e5f3"), "E92000001", "MOUNT STUART HOSPITAL", "TBS0067" },
                    { new Guid("c6b597dd-0f08-47b1-95a9-2aba06df4db9"), "E92000001", "MOUNT HOSPITAL", "TBS0066" },
                    { new Guid("11d5a99a-ebbb-43aa-8643-07a3317cfea5"), "E92000001", "MOUNT GOULD HOSPITAL", "TBS0065" },
                    { new Guid("455db200-3f89-4780-bf6c-e970fe8dc06c"), "E92000001", "MUSGROVE PARK HOSPITAL [SOMERSET]", "TBS0071" },
                    { new Guid("b11da708-86fc-4ada-ba28-623aaffd1732"), "W92000004", "MAESTEG GENERAL HOSPITAL", "TBS0043" },
                    { new Guid("195baaa9-0008-47f5-84f5-ac75b7127a1b"), "E92000001", "MILTON KEYNES GENERAL HOSPITAL", "TBS0058" },
                    { new Guid("11fe3038-e071-491a-a50b-53ce2933ce55"), "E92000001", "MILL VIEW HOSPITAL", "TBS0057" },
                    { new Guid("c62075a8-2590-4623-a907-43d3109f6139"), "E92000001", "MARKET DRAYTON CLINIC", "TBS0052" },
                    { new Guid("42c12a49-b7ef-4539-bdce-ef314ae9e460"), "E92000001", "MANSFIELD COMMUNITY HOSPITAL", "TBS0051" },
                    { new Guid("2dc2d71a-b79f-4ef9-8b84-6bbf325bb9cf"), "E92000001", "MANOR PARK HOSPITAL", "TBS0051" },
                    { new Guid("6fb3bb96-51c0-4ada-9c9c-8ac06beed41c"), "E92000001", "MANOR HOSPITAL [WALSALL]", "TBS0051" },
                    { new Guid("05eab19b-68c9-43b1-9f38-c87bc1ddc0da"), "E92000001", "MANOR HOSPITAL [NUNEATON]", "TBS0051" },
                    { new Guid("388884ed-1946-40a2-ab98-21c0cddeb2ad"), "E92000001", "MANOR HOSPITAL [BEDFORD]", "TBS0050" },
                    { new Guid("93bf8408-fe62-4958-b031-0aabfcfe32e4"), "E92000001", "MARLOW HOSPITAL", "TBS0053" },
                    { new Guid("edce7d04-977d-424a-80a8-3782dafd1475"), "E92000001", "MANCHESTER ROYAL INFIRMARY", "TBS0049" },
                    { new Guid("656a1449-4e9b-4dee-bf0f-8c6d0070df2f"), "E92000001", "MANCHESTER LIFESTYLE HOSPITAL", "TBS0047" },
                    { new Guid("33464912-e5b1-4998-afca-083c3ae65a80"), "E92000001", "MALVERN COMMUNITY HOSPITAL", "TBS0046" },
                    { new Guid("d30ce642-60ee-4c35-943e-3a5ca2075b72"), "E92000001", "MALTON COMMUNITY HOSPITAL", "TBS0045" },
                    { new Guid("c6f1ace8-1305-4cd6-a1ad-db34fca5105f"), "E92000001", "MALMESBURY COMMUNITY HOSPITAL", "TBS0044" },
                    { new Guid("9f8eb5c4-0fc0-4c84-bfdb-eed9b1d9cc94"), "E92000001", "MALHAM HOUSE DAY HOSPITAL", "TBS0044" },
                    { new Guid("463ee869-7763-46a5-9319-5dc314b0f488"), "W92000004", "MAINDIFF COURT HOSPITAL", "TBS0043" },
                    { new Guid("d19da815-0dcb-469b-aae0-08b73255e1fd"), "E92000001", "MANCHESTER ROYAL EYE HOSPITAL", "TBS0048" },
                    { new Guid("dd42651a-effa-460a-b5bc-3401bf75a628"), "E92000001", "MILLOM HOSPITAL", "TBS0057" },
                    { new Guid("a219bc91-317d-4d34-9b1f-2aebd842ff67"), "E92000001", "MARY HEWETSON COTTAGE HOSPITAL (KESWICK)", "TBS0053" },
                    { new Guid("ccb52a7d-4fa7-40e9-a737-67122b16f974"), "E92000001", "MAUDSLEY HOSPITAL", "TBS0053" },
                    { new Guid("13b336fe-92de-474f-a896-2d859d233c3d"), "E92000001", "MILFORD ON SEA WAR MEMORIAL HOSPITAL", "TBS0056" },
                    { new Guid("8d630c33-b77d-439a-b68a-a1bf4410c573"), "E92000001", "MILFORD HOSPITAL", "TBS0056" },
                    { new Guid("9c62e25a-f43f-4483-bdf9-f2000aff61d4"), "E92000001", "MILE END HOSPITAL", "TBS0056" },
                    { new Guid("66f8bd41-18fb-4c36-a7df-f82b02c31541"), "N92000002", "MID-ULSTER HOSPITAL", "TBS0055" },
                    { new Guid("679bc917-497f-49dd-8b91-ba5accf9be86"), "E92000001", "MIDHURST COMMUNITY HOSPITAL", "TBS0054" },
                    { new Guid("6d3e4698-d919-4d98-bd8e-2e98d6473bc4"), "E92000001", "MIDDLESEX HOSPITAL", "TBS0054" },
                    { new Guid("18695d7f-b64d-4600-8f73-3c890cc3a3ce"), "N92000002", "MATER INFIRMORUM HOSPITAL", "TBS0053" },
                    { new Guid("6edfc88e-fa8e-490c-bad6-f54f483ab7aa"), "E92000001", "MERIDEN HOSPITAL", "TBS0054" },
                    { new Guid("37c40954-7727-4726-804a-c7582f5c61d5"), "E92000001", "MELKSHAM COMMUNITY HOSPITAL", "TBS0054" },
                    { new Guid("0d6e8617-02cc-4491-b29f-72a902139d1b"), "E92000001", "MEDWAY MARITIME HOSPITAL", "TBS0054" },
                    { new Guid("4c35309f-7645-48a3-9acd-b461687404e1"), "E92000001", "MEADOWBANK DAY HOSPITAL", "TBS0054" },
                    { new Guid("462368bc-c8db-4369-83a3-6b8addcc6246"), "E92000001", "MCINDOE SURGICAL CENTRE", "TBS0053" },
                    { new Guid("6fdcb0f8-4f98-41bf-a57e-e7f73f2f725d"), "E92000001", "MAYFAIR DAY HOSPITAL", "TBS0053" },
                    { new Guid("bfcbe8f5-4e7c-44e2-880f-95a96e738ee6"), "E92000001", "MAYDAY UNIVERSITY HOSPITAL", "TBS0053" },
                    { new Guid("b1880968-3dad-446f-a8e3-65cdb87bc2c1"), "E92000001", "MEMORIAL HOSPITAL", "TBS0054" },
                    { new Guid("9dc201fb-9fe3-4960-92d7-8b8db7f3a0ec"), "E92000001", "JOHN COUPLAND HOSPITAL", "TBS0263" },
                    { new Guid("293604af-6d7e-43fe-8354-37e810d3be14"), "E92000001", "JAMES PAGET HOSPITAL", "TBS0263" },
                    { new Guid("b2ad12e9-2017-461f-8938-c6d04129bc04"), "E92000001", "JAMES COOK UNIVERSITY HOSPITAL", "TBS0263" },
                    { new Guid("e0e07649-eeda-4df2-baea-ddb6836d1ac4"), "W92000004", "GLAN CLWYD GENERAL HOSPITAL", "TBS0195" },
                    { new Guid("df8427d8-4e0e-45d4-ab5c-7bd9fdbb6a78"), "E92000001", "GEORGE ELIOT HOSPITAL", "TBS0195" },
                    { new Guid("49c09825-1bec-45d2-a2e9-d6219c5b02eb"), "E92000001", "GENERAL LYING-IN HOSPITAL [LONDON]", "TBS0195" },
                    { new Guid("2529c035-1cad-4bd7-8b3a-e02227a12e05"), "W92000004", "GELLINUDD HOSPITAL", "TBS0194" },
                    { new Guid("b302d0dd-7b4b-4a20-925f-ba0d951ca21e"), "W92000004", "GARNGOCH HOSPITAL", "TBS0194" },
                    { new Guid("c3bf9ab5-a70f-425d-9c61-4d270ae5a80c"), "E92000001", "GARDEN HOSPITAL", "TBS0193" },
                    { new Guid("f5fb7f72-8df9-4b9a-b5b3-680b9c19d11f"), "W92000004", "GLANRHYD HOSPITAL", "TBS0196" },
                    { new Guid("d788a508-76be-494d-98b1-69319c71cff8"), "E92000001", "FURNESS GENERAL HOSPITAL", "TBS0193" },
                    { new Guid("1cf07cb2-b6b4-44f0-9161-8e4349a7fbee"), "E92000001", "FRYATT HOSPITAL AND MAYFLOWER MEDICAL CENTRE", "TBS0191" },
                    { new Guid("2c18e17d-0e80-4342-8c85-4839f71c45ef"), "E92000001", "FROME VICTORIA HOSPITAL", "TBS0190" },
                    { new Guid("44c3608f-231e-4dd7-963c-4492d804e894"), "E92000001", "FRIMLEY PARK HOSPITAL", "TBS0189" },
                    { new Guid("f025fbe2-464a-4f53-aa78-6e02c5c116b3"), "E92000001", "FRIARY HOSPITAL", "TBS0188" },
                    { new Guid("76e51312-1e1b-42c2-8059-6ffb0dae1eae"), "E92000001", "FRIARAGE HOSPITAL", "TBS0187" },
                    { new Guid("7c5c250b-619d-4bda-9dfb-24b7c580c76c"), "E92000001", "FRENCHAY HOSPITAL", "TBS0186" },
                    { new Guid("51e00361-b228-4e21-8efd-06edd9cbb42c"), "E92000001", "FULWOOD HALL HOSPITAL", "TBS0192" },
                    { new Guid("802123fa-680b-4d03-bf0a-8d4d53c38067"), "E92000001", "FREEMAN HOSPITAL", "TBS0185" },
                    { new Guid("b77e9ffa-06f8-48d1-9fdc-668d0d13f772"), "E92000001", "GLENFIELD HOSPITAL", "TBS0196" },
                    { new Guid("3a1e6946-5300-4de1-93af-de7452f7a809"), "E92000001", "GLOUCESTER HOUSE/ DORIAN DAY HOSPITAL", "TBS0196" },
                    { new Guid("085d3235-827e-430e-b70d-b889b042af97"), "E92000001", "GREAT WESTERN HOSPITAL", "TBS0207" },
                    { new Guid("3e7b18d4-4e67-44bd-9c49-4d8e75f18607"), "E92000001", "GREAT ORMOND STREET HOSPITAL CENTRAL LONDON SITE", "TBS0206" },
                    { new Guid("7640dec2-6863-47bb-933b-b43df54b0866"), "E92000001", "GRAVESEND AND NORTH KENT HOSPITAL", "TBS0205" },
                    { new Guid("698353ff-870c-4c0d-9f6b-011bd25f3787"), "E92000001", "GRANTHAM & DISTRICT HOSPITAL", "TBS0204" },
                    { new Guid("d1de4314-6221-4270-a11f-4c230da0c71d"), "E92000001", "GOSPORT WAR MEMORIAL HOSPITAL", "TBS0203" },
                    { new Guid("511fedce-81a6-44da-ab0a-7950ae3762fc"), "E92000001", "GOSCOTE HOSPITAL", "TBS0202" },
                    { new Guid("a74eda3a-bf03-49f6-a1bb-4ba277d6336b"), "E92000001", "GLENSIDE HOSPITAL", "TBS0196" },
                    { new Guid("6b3e8c14-2249-4d1b-b95b-2459cd4d86cb"), "W92000004", "GORSEINON HOSPITAL", "TBS0201" },
                    { new Guid("698f063b-feaa-4921-b3b1-db97668615d3"), "E92000001", "GORING HALL HOSPITAL", "TBS0200" },
                    { new Guid("bfe242b1-00bb-4e30-a0a8-9614bbd2fd52"), "E92000001", "GORDON HOSPITAL", "TBS0199" },
                    { new Guid("392a986c-a664-46c9-9c1b-d7f7f5c30b06"), "E92000001", "GOOLE DISTRICT HOSPITAL", "TBS0198" },
                    { new Guid("1e558266-7db6-445f-915d-42510a23e737"), "E92000001", "GOOD HOPE HOSPITAL", "TBS0197" },
                    { new Guid("de919ba5-357f-4440-9e13-962762f90535"), "E92000001", "GLOUCESTERSHIRE ROYAL HOSPITAL", "TBS0196" },
                    { new Guid("628e70b0-af6b-4fe6-b63a-36b3c116bebf"), "E92000001", "GLOUCESTERSHIRE HOSPITALS NHS FOUNDATION TRUST", "TBS0196" },
                    { new Guid("ea38954b-b569-46b4-ac8f-e36a236eb4d2"), "E92000001", "GORSE HILL HOSPITAL", "TBS0200" },
                    { new Guid("9a60d410-2ce3-4611-82cf-b4a0196ff5e1"), "E92000001", "GREATER MANCHESTER WEST MENTAL HEALTH NHS TRUST", "TBS0208" },
                    { new Guid("96fa60e4-55df-43a9-bb6e-86e303cd2d67"), "E92000001", "FRANKLYN COMMUNITY HOSPITAL", "TBS0184" },
                    { new Guid("12f92666-3a55-49ea-b298-a996411385cf"), "W92000004", "FORGLEN DAY HOSPITAL", "TBS0182" }
                });

            migrationBuilder.InsertData(
                table: "Hospital",
                columns: new[] { "HospitalId", "CountryCode", "Name", "TBServiceCode" },
                values: new object[,]
                {
                    { new Guid("f07e8eb9-d4ae-4591-b27e-d858be34fba2"), "E92000001", "FAIRFIELD GENERAL HOSPITAL", "TBS0170" },
                    { new Guid("dc90bc15-6ebe-4d24-b9d1-d1d2b2ace8eb"), "E92000001", "EXMOUTH HOSPITAL", "TBS0169" },
                    { new Guid("2c136d6d-d644-4cf2-a995-b2f9e5218a4d"), "E92000001", "EVESHAM COMMUNITY HOSPITAL", "TBS0168" },
                    { new Guid("7ade1b25-109f-4b96-8eed-7fc6c7b32d6b"), "E92000001", "EUXTON HALL HOSPITAL", "TBS0167" },
                    { new Guid("26b1f5b9-cfc1-46ed-9984-dfacdf41544d"), "E92000001", "ESSEX COUNTY HOSPITAL", "TBS0166" },
                    { new Guid("509797da-05a6-4c9e-8168-d46a7f22d867"), "E92000001", "ESPERANCE PRIVATE HOSPITAL", "TBS0165" },
                    { new Guid("e59daa5f-b8f2-441d-b116-9a5eb1ba05cd"), "E92000001", "FAIRFORD HOSPITAL", "TBS0171" },
                    { new Guid("126bc5ef-cde0-4747-96aa-02aa804865ff"), "E92000001", "ESPERANCE HOSPITAL", "TBS0164" },
                    { new Guid("84ef9b5f-cd49-4c74-9cd3-a2e613a86674"), "N92000002", "ERNE HOSPITAL", "TBS0162" },
                    { new Guid("66290930-5cfe-40e1-96c0-d918f90f4191"), "E92000001", "ERITH & DISTRICT HOSPITAL", "TBS0161" },
                    { new Guid("bc9d011b-b150-4d67-843f-d52fcc7c026a"), "E92000001", "EPSOM GENERAL HOSPITAL", "TBS0160" },
                    { new Guid("f8d1338d-9db8-4866-b060-c3ca4a7a3d08"), "E92000001", "EMSWORTH HOSPITAL", "TBS0160" },
                    { new Guid("8218a639-27b1-4a8c-8f44-3ca722294c4f"), "E92000001", "ELMS DAY HOSPITAL", "TBS0160" },
                    { new Guid("7aaec4ba-fee4-4db5-8498-df3987bb620f"), "E92000001", "ELLESMERE PORT HOSPITAL", "TBS0160" },
                    { new Guid("6fe59de3-1917-4f36-bf38-610259f71378"), "W92000004", "ERYRI HOSPITAL", "TBS0163" },
                    { new Guid("73a5df50-ee94-4dd2-b0b0-e00bef38fff9"), "E92000001", "FOSCOTE HOSPITAL", "TBS0183" },
                    { new Guid("65937cd7-855c-4e0b-afbb-354d10942588"), "W92000004", "FAIRWOOD HOSPITAL", "TBS0172" },
                    { new Guid("4f27a05e-b05b-4a76-8ae7-358d947c59c9"), "E92000001", "FARNHAM HOSPITAL", "TBS0174" },
                    { new Guid("6ac374a6-4151-40a4-943e-4e9f42d30b36"), "E92000001", "FORDINGBRIDGE HOSPITAL", "TBS0182" },
                    { new Guid("df9a25f3-dade-4fcc-acde-82e8ae84b5fb"), "W92000004", "FLINT COMMUNITY HOSPITAL", "TBS0181" },
                    { new Guid("1ee2b39a-428f-44c7-b4bb-000649636591"), "E92000001", "FLEETWOOD HOSPITAL", "TBS0181" },
                    { new Guid("f9454382-9fbd-4524-8b65-04c1b449469c"), "E92000001", "FLEET HOSPITAL", "TBS0181" },
                    { new Guid("025dcd69-efb7-45f4-9b02-8c260cfc77f4"), "E92000001", "FITZWILLIAM HOSPITAL", "TBS0181" },
                    { new Guid("06e43793-c2a7-45db-91f1-7a32882c7437"), "E92000001", "FISHERMEAD MEDICAL CENTRE", "TBS0181" },
                    { new Guid("101c3e99-befb-48c9-bcd1-7c716197fa11"), "E92000001", "FALMOUTH HOSPITAL", "TBS0173" },
                    { new Guid("cf00498b-dbb6-48f1-83ec-aae4572daad4"), "E92000001", "FINCHLEY MEMORIAL HOSPITAL", "TBS0180" },
                    { new Guid("4770940a-f28c-4179-bac6-9f99d56aa7a5"), "W92000004", "FFESTINIOG MEMORIAL HOSPITAL", "TBS0178" },
                    { new Guid("cc2a4a36-92dd-4ac1-97ee-9648940a0dea"), "E92000001", "FENWICK HOSPITAL (PERIPHERAL CLINIC)", "TBS0177" },
                    { new Guid("a147d206-4a86-4665-bc81-21c651334073"), "E92000001", "FELIXSTOWE HOSPITAL", "TBS0176" },
                    { new Guid("200add54-aeae-4ae8-83fb-c17f4086b86f"), "E92000001", "FAWKHAM MANOR HOSPITAL", "TBS0176" },
                    { new Guid("7a96bb0c-38d0-4b16-bf21-4f49df83d86f"), "E92000001", "FAVERSHAM COTTAGE HOSPITAL", "TBS0176" },
                    { new Guid("6dc8b7ba-3315-49ae-aa00-9f7649998706"), "E92000001", "FARNHAM ROAD HOSPITAL", "TBS0175" },
                    { new Guid("a7ecceeb-ea05-42e8-a6ef-0119e309c5a6"), "E92000001", "FIELDHEAD HOSPITAL", "TBS0179" },
                    { new Guid("b4e9975d-9bf3-4e8c-82ba-a7a777dcd912"), "W92000004", "GROESWEN HOSPITAL", "TBS0209" },
                    { new Guid("c717489d-8ca6-4da9-91fa-f22bc9cd7427"), "E92000001", "GROVE CONVALESCENT HOSPITAL", "TBS0210" },
                    { new Guid("d586409a-1bf3-4b39-8d0e-7ca0537d1cef"), "E92000001", "GROVE HOSPITAL", "TBS0211" },
                    { new Guid("01037c28-d804-437c-a7d8-7240bac7e32c"), "E92000001", "HOMEOPATHIC HOSPITAL", "TBS0253" },
                    { new Guid("1a69d10b-f4c7-4061-abed-58f6131d45f7"), "E92000001", "HOMELANDS HOSPITAL", "TBS0252" },
                    { new Guid("3e855da9-6a91-49d8-b94a-000863127f59"), "W92000004", "HOLYWELL COMMUNITY HOSPITAL", "TBS0251" },
                    { new Guid("21049817-2eed-40a9-bcca-0e3b5f6185c1"), "E92000001", "HOLSWORTHY HOSPITAL", "TBS0250" },
                    { new Guid("3c55dd08-0747-470d-8a17-ba1deb200662"), "E92000001", "HOLMEVALLEY MEMORIAL HOSPITAL", "TBS0249" },
                    { new Guid("08b374a6-3d0d-462d-b39c-818dbbb17ec1"), "E92000001", "HOLLY HOUSE HOSPITAL", "TBS0248" },
                    { new Guid("3af604c4-15d8-4408-be3c-4afed220c84a"), "E92000001", "HOMERTON UNIVERSITY HOSPITAL", "TBS0253" },
                    { new Guid("48614141-0cee-4337-803c-9ae4e0b12ae3"), "E92000001", "HOLLINS PARK HOSPITAL GERIATRIC DAYCARE", "TBS0247" },
                    { new Guid("8c3a922e-4f05-4e07-8143-37ec157db294"), "E92000001", "HINCKLEY & DISTRICT HOSPITAL", "TBS0245" },
                    { new Guid("67da876c-ac6f-4b1b-8083-385f6abb7fd1"), "E92000001", "HINCHINGBROOKE HOSPITAL", "TBS0244" },
                    { new Guid("2cdcb5e9-4d13-4cb7-941c-5f6b8a45e7c2"), "E92000001", "HILLINGDON HOSPITAL", "TBS0243" },
                    { new Guid("5154dc63-f7cb-40c9-b612-3abec95e54ed"), "W92000004", "HILL HOUSE DAY HOSPITAL", "TBS0242" },
                    { new Guid("9cadcafd-3526-4afc-8a4b-d1f1185c6e60"), "E92000001", "HIGHWOOD HOSPITAL", "TBS0241" },
                    { new Guid("7292e6aa-d4ee-41fc-a09e-cf9365b38c09"), "E92000001", "HIGHFIELD HOSPITAL", "TBS0239" },
                    { new Guid("fe09a91e-aa63-47b0-9464-b00d32ebfe54"), "E92000001", "HOLBEACH HOSPITAL", "TBS0246" },
                    { new Guid("330041e1-4952-4bba-ab0e-71a36cd7b69b"), "E92000001", "HIGHBURY HOSPITAL", "TBS0239" },
                    { new Guid("584eceab-da04-47c8-82dd-4ab23a1f6d21"), "E92000001", "HORN HALL HOSPITAL", "TBS0254" },
                    { new Guid("a2f561d5-4128-4ea5-b272-b4bd23da30f4"), "E92000001", "HORSHAM HOSPITAL", "TBS0256" },
                    { new Guid("d0a0c2bd-6974-46e5-b18c-63a6812f1d41"), "E92000001", "ISEBROOK HOSPITAL", "TBS0263" },
                    { new Guid("2c046e90-e7b3-41f0-bc8d-39accfc0879f"), "E92000001", "IPSWICH HOSPITAL", "TBS0263" },
                    { new Guid("dc491bb8-c451-4ec3-b010-f83f826b6232"), "E92000001", "ILLKESTON COMMUNITY HOSPITAL", "TBS0263" },
                    { new Guid("fb02a43a-455a-4e3c-a16e-3799bfcf6312"), "E92000001", "HYTHE HOSPITAL (PERIPHERAL CLINIC)", "TBS0262" },
                    { new Guid("02f37746-53f5-4651-94fc-bd3607e07709"), "E92000001", "HYDE HOSPITAL", "TBS0262" },
                    { new Guid("120555e5-8b92-4698-9377-a74be41ac936"), "E92000001", "HUNTERS MOOR HOSPITAL", "TBS0262" },
                    { new Guid("0463d983-c9ae-478c-8f7d-2593cc1d7bef"), "E92000001", "HORNSEA COTTAGE HOSPITAL", "TBS0255" },
                    { new Guid("a45d75e0-e056-4adf-b269-00f09e89945f"), "E92000001", "HULL ROYAL INFIRMARY", "TBS0262" },
                    { new Guid("a02016f4-22d0-4181-bb0d-832801640419"), "E92000001", "HUDDERSFIELD HOSPITAL", "TBS0261" },
                    { new Guid("bc13b36e-d30d-4795-aa0e-d0a69894534c"), "E92000001", "HSH BROADMOOR HOSPITAL", "TBS0260" },
                    { new Guid("8c5315c5-afbc-4a64-bf67-d8d0ffbe8b76"), "E92000001", "HRH PRINCESS CHRISTIAN'S HOSPITAL", "TBS0259" },
                    { new Guid("5a13a8ed-d1ef-452c-ba33-dfc6332b07fc"), "E92000001", "HOSPITAL OF ST CROSS", "TBS0259" },
                    { new Guid("0cb0050c-6fdd-4663-862f-f8bb2d5ecbee"), "E92000001", "HOSPITAL FOR TROPICAL DISEASES", "TBS0258" },
                    { new Guid("f7f36ffc-549a-4ea1-8802-3efcf7b46ab6"), "E92000001", "HORTON GENERAL HOSPITAL", "TBS0257" },
                    { new Guid("8e0b5799-0e39-42fc-ba4c-652cc6fbb918"), "E92000001", "HUDDERSFIELD ROYAL INFIRMARY", "TBS0262" },
                    { new Guid("1aae3804-cca4-4e8a-8e38-947c77a50230"), "E92000001", "HEXHAM GENERAL HOSPITAL", "TBS0238" },
                    { new Guid("f143cc8c-b3bd-4225-a0ba-2ecfcd46144c"), "E92000001", "HERTS AND ESSEX HOSPITAL", "TBS0237" },
                    { new Guid("1859ff13-82f8-4851-be86-8239fc495eed"), "E92000001", "HERTFORD COUNTY HOSPITAL", "TBS0237" },
                    { new Guid("5702d9be-1222-44f9-baeb-3bdfb1642fe8"), "E92000001", "HAREFIELD HOSPITAL", "TBS0225" },
                    { new Guid("70c91a44-6abb-4866-841f-44550c2cb8cc"), "E92000001", "HARBOUR HOSPITAL", "TBS0224" },
                    { new Guid("cc2a6530-f000-4fd6-bc4e-33b1d7d861e6"), "E92000001", "HAMPSHIRE CLINIC", "TBS0223" },
                    { new Guid("e4aaaad2-f694-491f-b83e-e35f43fcd643"), "E92000001", "HAMMERSMITH HOSPITAL", "TBS0222" },
                    { new Guid("33c3bf94-1cfc-4778-a076-3a91e2efaf75"), "E92000001", "HAM GREEN HOSPITAL", "TBS0221" },
                    { new Guid("d6d3ad1b-1470-4840-bbd4-1127d47295cd"), "E92000001", "HALTWHISTLE WAR MEMORIAL HOSPITAL", "TBS0220" },
                    { new Guid("1b1edf62-ee5e-45ca-9ab1-1e76808c3423"), "E92000001", "HAROLD WOOD HOSPITAL", "TBS0226" },
                    { new Guid("677ce666-0152-44e2-8fa1-3b56d372b87a"), "E92000001", "HALTON HOSPITAL", "TBS0219" },
                    { new Guid("b5d14f32-ab54-4fb0-bfa0-09c6d3af9eef"), "W92000004", "HAFEN COED M.I. DAY HOSPITAL", "TBS0217" },
                    { new Guid("40ef3e55-4023-44e7-bdbc-dddc966f3c29"), "W92000004", "H M STANLEY HOSPITAL", "TBS0216" },
                    { new Guid("6f7d6597-8fdb-4fcd-8642-7db216b5dff2"), "E92000001", "GUY'S HOSPITAL", "TBS0215" },
                    { new Guid("1176df5e-8ff1-414f-abb7-0db747120689"), "E92000001", "GULSON HOSPITAL", "TBS0214" },
                    { new Guid("4227ec35-4c3a-46af-9bdf-61f0d87888f0"), "E92000001", "GUISBOROUGH GENERAL HOSPITAL", "TBS0213" },
                    { new Guid("f696ad39-f193-415c-9b25-e29a00aa2315"), "E92000001", "GUEST HOSPITAL", "TBS0212" },
                    { new Guid("0d43f157-c95d-49f9-9c8a-33b8f1321c37"), "E92000001", "HALSTEAD HOSPITAL", "TBS0218" },
                    { new Guid("9a14b7bb-89ad-440b-9fcc-cea488a9deab"), "E92000001", "HARPENDEN MEMORIAL HOSPITAL", "TBS0227" },
                    { new Guid("9df6af00-f484-4f6c-8aa6-84acc47c64aa"), "E92000001", "HARPERBURY HOSPITAL", "TBS0228" },
                    { new Guid("f11f97b1-3e83-4c7a-a5ef-06c1846ce98e"), "E92000001", "HARPLANDS HOSPITAL", "TBS0229" },
                    { new Guid("7b7a1322-e94d-4272-adae-75dd7df17ee7"), "E92000001", "HEREFORD COUNTY HOSPITAL", "TBS0237" },
                    { new Guid("84eebf2c-d40b-4e57-b392-8085eaf6fbdf"), "E92000001", "HERBERT HOSPITAL", "TBS0236" },
                    { new Guid("29c2825c-016f-4f44-b899-eda58cf2fabf"), "E92000001", "HEMEL HEMPSTEAD GENERAL HOSPITAL", "TBS0235" },
                    { new Guid("79b5d236-2766-4f92-9ce3-826ef6bc319d"), "E92000001", "HELSTON HOSPITAL", "TBS0234" },
                    { new Guid("6f8f4ef3-1f6f-4706-8eec-461772615796"), "E92000001", "HEAVITREE HOSPITAL", "TBS0234" },
                    { new Guid("3bd5ddb5-9dcf-42b6-8ba7-aa16e9c7bbb3"), "E92000001", "HEATHERWOOD HOSPITAL", "TBS0233" },
                    { new Guid("a79f488b-6e6d-420c-b7db-f81b0657d1ce"), "E92000001", "HEARTLANDS HOSPITAL", "TBS0233" },
                    { new Guid("23e2de92-787d-4962-bc63-24d37b477a26"), "E92000001", "HEART HOSPITAL", "TBS0232" },
                    { new Guid("a53aa091-d475-4f4e-bc7c-20361c476a29"), "E92000001", "HAYWOOD HOSPITAL", "TBS0232" },
                    { new Guid("de169d34-4dc6-4a05-87eb-20901416dafb"), "E92000001", "HAWKHURST HOSPITAL", "TBS0232" },
                    { new Guid("83ad62d2-28ce-4746-b2d2-fc92059a1fa4"), "W92000004", "HAVENWAY DAY HOSPITAL", "TBS0232" },
                    { new Guid("f261f4c6-1812-48a1-851f-90e88cac5ddc"), "E92000001", "HAVANT WAR MEMORIAL HOSPITAL", "TBS0232" },
                    { new Guid("3d8fb352-645a-4a16-9ab2-db0ed891c3bb"), "E92000001", "HASLEMERE HOSPITAL", "TBS0232" },
                    { new Guid("b13452a4-becf-4e6c-8edf-2e6995b4c5a5"), "E92000001", "HARTISMERE HOSPITAL", "TBS0231" },
                    { new Guid("bc598aaf-f398-40ff-99ad-0c608bb47484"), "E92000001", "HARROGATE DISTRICT HOSPITAL", "TBS0230" },
                    { new Guid("a25a8a6c-4223-437e-a1a4-96894227549c"), "E92000001", "ELIZABETH GARRETT ANDERSON HOSPITAL", "TBS0160" },
                    { new Guid("107ca001-2370-4a82-9cdf-5591499dcbf0"), "E92000001", "ZACHARY MERTON HOSPITAL", "TBS0157" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("003ade77-4099-4aff-afc9-564757f54ecb"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("00bddb9c-07f5-46d0-afb0-12f66bb85ab9"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("00c14073-322b-433b-aec1-f1c0698d5489"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("00caa2cd-0224-4b6d-beeb-caedc8a6dd2a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("01037c28-d804-437c-a7d8-7240bac7e32c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0111f8e8-dabf-4d6a-9a00-c5878d8d9366"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("01c3b0dc-ff1e-4a09-a59d-647862562b9d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("023f1000-b4d1-4887-b601-333a87bb6514"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("025dcd69-efb7-45f4-9b02-8c260cfc77f4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0275af55-727a-4cac-8a96-2285ef484b50"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("027f7fd2-36f8-4504-8683-41dd4510d339"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("02c4ada5-6853-448e-ae72-33e1395f7cf3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("02ecfb85-cdab-4a30-b29d-18277190de8f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("02f37746-53f5-4651-94fc-bd3607e07709"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0370a142-f5ed-4c1e-991c-5d92b2aacb40"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("037bd8f1-701f-4dfc-a685-7cea2327c806"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0391ed46-0661-4cbe-924f-af0f5959ec19"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("03f4894f-0ef3-4c23-8701-245f83d9e66c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("04248670-4d1f-4026-bf22-e913208e672e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("042fa0e4-791a-47a0-9f5f-2c1348954954"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0463d983-c9ae-478c-8f7d-2593cc1d7bef"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("04dd6731-97a7-4fad-bfde-d95c98cd04c5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("053ae50b-3b39-44b7-89ec-1bce415bd846"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("05e4acfc-593c-42b5-984e-98a54187e0d3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("05eab19b-68c9-43b1-9f38-c87bc1ddc0da"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("06749b63-ae06-4cc6-9d66-9dd5c4a0245e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("06e43793-c2a7-45db-91f1-7a32882c7437"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("07186db5-833f-4d9d-a0aa-e1374dd64fb8"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("072c1d97-ac4f-459b-b27f-f5a0b9d2abb8"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("076c7e3b-c9a2-43d0-942d-558a9d6371e3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("07c51540-3fb0-44d6-8c40-c98a4ff59ae2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0838cd26-5690-461e-bcf4-11de5e20d606"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("085d3235-827e-430e-b70d-b889b042af97"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("086a670b-bbed-49f5-879e-6f402a43309c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("08b374a6-3d0d-462d-b39c-818dbbb17ec1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("097ea09c-71a4-4178-bf54-c2858ad9d493"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("099481b6-ff46-4247-85ef-d04d8d0524cc"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("09deb24a-ce0d-4add-8bf9-59b831f9caa4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0a5a2531-1f6c-438b-90d1-427d14b35632"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0a77a26e-c6cf-4f2d-8501-8d94ea136d1d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0a8ab02a-c64b-459e-8ff8-1f96bb8a0159"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0ac033ab-9a11-4fa6-aa1a-1fca71180c2f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0aefd55a-7dd0-4b80-abec-3ecf44a104b9"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0b2bfb8d-0e9e-4138-8751-1dc1b2581ae5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0ba53cca-6c30-4743-a21e-0664294e64d3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0c144e14-ed87-4980-b962-6480bb945afd"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0cb0050c-6fdd-4663-862f-f8bb2d5ecbee"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0cb2dff3-5ab3-46b2-ad45-e70716d8b853"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0d43f157-c95d-49f9-9c8a-33b8f1321c37"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0d6e8617-02cc-4491-b29f-72a902139d1b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0d92e7ff-aba9-4e80-a048-a3ed4e0c44f0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0dcae460-f8cc-44af-b6c1-8b86b0a725d5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0df7bebb-e589-4bfb-a2bb-352ac1ccd2e1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0e4b9e79-b47c-45de-89b9-58ffe3c3b2b4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0e757d5c-57b8-41f1-b2ae-a75ab69c4854"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0e76ada3-8ccd-4426-a6d0-4b08663c38a5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0e7e5809-d38e-40d7-9c70-b69660c244c7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0eee2ec2-1f3e-4175-be90-85aa33f0686c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0ef08231-4b51-4887-91cb-316408c02657"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0f2aa7de-6615-400e-bb14-ec960d42994d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0f6d1869-c347-4a32-be45-ceea4df5f537"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1001ba58-bb01-4745-a1d2-b15ff4070ef0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("101c3e99-befb-48c9-bcd1-7c716197fa11"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("107ca001-2370-4a82-9cdf-5591499dcbf0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1087f367-ce99-4bcd-b7e1-704cbfd9105d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("10ade1d8-e91f-44e1-b6fb-6fdbcf38edca"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("11081272-a36b-459d-9e6b-7e8c486fdc9f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1176df5e-8ff1-414f-abb7-0db747120689"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1199d23b-65f8-49a9-aee1-eafeba2ddcae"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("11b304c6-f0bc-498f-925b-1fc8e1c7031a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("11d5a99a-ebbb-43aa-8643-07a3317cfea5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("11e3674c-17d3-4c4e-8f2c-a4af70dd6fb0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("11fe3038-e071-491a-a50b-53ce2933ce55"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1200824a-cce6-491d-bd93-33e44f0b383b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("120555e5-8b92-4698-9377-a74be41ac936"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1219b1c8-850c-483f-bf3b-16dc9090dc7b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("121c9756-16e7-4186-a1d4-e36b51b57dfb"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("126bc5ef-cde0-4747-96aa-02aa804865ff"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("12759488-b4db-4f7c-ab2e-f13bd8c6e73d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1280ecc8-ff7d-473a-83d7-02e7fb2bafdb"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("12ab2e21-d3b9-4120-b623-8f786467e5af"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("12f92666-3a55-49ea-b298-a996411385cf"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("131ccafa-9147-4bd3-93f4-157c70b56b4b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("13737a9f-f4aa-4831-b151-edd051eecf68"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1387c5d9-b653-43bd-93fc-20ce75edd420"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1393efe2-9921-45d4-a384-c3ee9400c5b5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("13958bea-c1bc-4ccf-ad03-e9bbfa016502"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("13967c5c-8172-44f0-bdab-b206c8bdd8d2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("13b336fe-92de-474f-a896-2d859d233c3d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("13b6c143-587d-46f8-ba7b-5341482066f4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("13c912bf-3127-4dd2-908b-6bbcaa3bc984"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("13f42cb8-4430-487a-b995-f076dc4a26e0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("14334461-c8de-477e-b881-04df0c0e7c32"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("14476a1d-6db0-49ae-bf4d-c0d95b51af21"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("148130af-457a-4509-946d-f6e6736f22ff"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("14f099af-044d-4a30-ae8d-6aef04dbd1a1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("15e4f8fb-09bc-45e2-8d7c-d5f654465680"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("15e5950d-fc36-439d-aab0-ca520e0b617c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("160b52ad-9881-4c07-930c-933315dab76e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("16908b7a-58f8-4e17-a8e8-91888d86f52f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("16a5bc66-9d12-4156-8db9-2b75292bfceb"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("16aba4bf-274c-465e-9bfd-ff3309266e97"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("171806a0-98a7-4ae6-bac2-ffcb4f3fc092"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("172b8133-4b56-4858-982a-a2b8cb4cd46d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("17340cde-67c7-4203-9957-afa0c3275895"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("177e56f2-dd72-45e4-94c1-34244f00ede9"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("177f8d11-d5b6-48cc-bcf4-2e49519949e4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("179b430d-5d8c-4f21-9ba8-c6a652df0d62"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("17f93ffc-01e5-48e1-b1fd-10fc7308f9ec"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("181d832c-0210-42a9-9f8a-370aa4816196"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1859ff13-82f8-4851-be86-8239fc495eed"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("18695d7f-b64d-4600-8f73-3c890cc3a3ce"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1895e8b7-ab2c-4307-adf3-f36a119a32a4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("18b079d8-ba76-4145-930b-547d4c3b623f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("18b6e8b8-c327-4f37-9143-c215bad7faf3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("18bb0905-d268-4f33-9c94-b6a15791c7f4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("18f24540-166d-4d59-84d7-61aa1ccb46c0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("195baaa9-0008-47f5-84f5-ac75b7127a1b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("19835e7e-2d97-4ee5-833e-62a603ba2685"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1a2ad18f-94ef-4c63-ab09-40ac192343c7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1a498852-ef37-4a94-9a87-44fba3869b70"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1a69d10b-f4c7-4061-abed-58f6131d45f7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1aae3804-cca4-4e8a-8e38-947c77a50230"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1acefe30-cb48-432c-9bbc-10b371aa0a59"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1b1edf62-ee5e-45ca-9ab1-1e76808c3423"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1b5ef984-a883-4248-9285-7e02be2db1c3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1b917fe6-3fb7-4fba-b5f5-744c69f9ca60"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1bdbc2b9-1971-4af8-ac2a-d3a8f06b8399"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1be6ea32-ab5d-4efe-ac25-371d7b2525bd"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1c39b75b-ba22-4574-afc0-459c0009d4b7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1ca2bb07-2fb7-4e0c-a853-0568d7452ba0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1cf07cb2-b6b4-44f0-9161-8e4349a7fbee"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1cfd833f-76ca-41e5-b151-65045df09358"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1d024fdf-bde9-4b5d-8e85-7682186cff14"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1d6e8331-f3c2-4b5d-8415-e98dc82f72de"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1e017501-e18e-471c-9a41-433367097fba"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1e1a4fb3-b8e4-4162-ab5f-913ceb70e007"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1e2e25df-bd19-4647-82f0-bd3b3af5ec19"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1e558266-7db6-445f-915d-42510a23e737"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1ebcb983-55d2-4365-ae0f-c91d1d19ef86"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1ee2b39a-428f-44c7-b4bb-000649636591"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1ee330e7-3759-473b-93eb-a65a09dee01d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1f801293-d20c-41c7-8f65-808f8092543b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1fdcb443-7459-4af7-8980-007e62ef2e90"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1fde9ae3-9a76-406b-93d6-e3bea0cb2250"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1ff7f5f2-1f35-47f0-8e3f-891eb4fdd2bd"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("200add54-aeae-4ae8-83fb-c17f4086b86f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("20120fac-8e26-49e2-83cd-0c1ec364fa9e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("20494aba-dd68-401a-bef2-1b5fa0315828"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2082a678-403f-4969-8b07-557ce2240409"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("20c6e25b-ca1f-46f4-a52c-27036a7858d4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("20f1a936-225b-4b47-9ecd-cedc4fb2fd45"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("21049817-2eed-40a9-bcca-0e3b5f6185c1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("213920fc-bf88-43e1-858f-bf29f08c4ae1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("214ec28c-2a69-4940-89ef-136bc462dfbe"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("21c98b1a-bce5-482d-bcdd-a66090acdd31"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("21f28284-c7b7-4cf3-9b55-03b80ba14477"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("225fad12-be15-414d-8e4c-178f7c6aa763"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("227d2a1e-42aa-4d34-98b6-db4b489533d5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2284b28d-6722-4d27-b30b-8ec34e413985"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("22ddd848-f889-4a26-8b48-1b70d5d77719"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("22e73828-676d-4c3e-bc25-092cb00c0ae0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("23290947-b60d-4e69-b014-b77a443d4caf"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2332b4f9-b6c0-42a5-86d6-eca9837d40a7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("23aa321d-33d9-4b4f-ac38-f585a004b1d2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("23c0ea5a-a196-4a80-a923-055631fd6866"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("23d82b05-d327-4aca-b01f-afc856fb21a4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("23e2de92-787d-4962-bc63-24d37b477a26"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("24766cb2-0f78-4c8f-a188-57f1e3fe9a30"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("251ca4b4-7699-4927-a36c-f2d53f737d1f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2529c035-1cad-4bd7-8b3a-e02227a12e05"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2599dd55-6274-41e1-ad21-3dbc0d2b8349"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("25bf770e-faa6-4e5e-b412-a125ddec94ed"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("264f90dc-2c62-4e92-8f64-69f1085fe3e2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("265ba139-52f9-4207-b10a-feedb861cf85"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("266014b2-0bc5-4daf-9bff-219f1c77db68"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("266fa510-3c54-4aad-92bf-f30bda272d68"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("26b15340-f812-4993-87d8-1a6e32c8115c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("26b1f5b9-cfc1-46ed-9984-dfacdf41544d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("26b4b173-8990-41c6-8099-405d26f158d2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("26c2e669-97e7-4e78-97e2-b87af2729ea7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2773dd7b-6184-4569-8a6f-a1df9a9d23d3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("27911115-f3ea-4f56-b343-4e9e52b0a84c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("27988285-43f2-41d6-a40b-a98fe588da6e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("27bb6d5a-ffb2-448c-8583-19dd3d471c02"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("28cbee25-66f4-4a20-bf3d-f26077792ef5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("28dcb0dc-31d4-4b7a-ad44-3e27cb37016c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2906e828-41e8-47a6-9fd4-3da2e1ff9a06"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("292d7a2b-1b87-4644-ad43-83f1301a0c94"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("293604af-6d7e-43fe-8354-37e810d3be14"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("29bb0ca3-3329-4c44-a7d1-f5d0f77c3308"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("29c2825c-016f-4f44-b899-eda58cf2fabf"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("29c3bf29-502c-40fa-92a0-e91455b1ed5c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("29d4ded5-bcfb-4cfd-b6da-1f3ef98d3a64"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("29f693f4-4738-4163-a6a5-71005d9601bb"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2a1aede9-90b4-4967-84c7-e2788fde1eec"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2a3b7e0e-7e95-4090-acff-d4759996a3cd"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2a4164c7-4487-4e33-acb1-7cd51f7ad158"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2a474e42-4634-4990-84ab-b0ee3ae8327e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2a4b6534-5b02-47bf-8c3d-c7fdcf8eaf61"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2a6fa966-d70f-48f0-a72a-782827ee1974"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2a9c7e60-af14-4929-8233-79deb17855bd"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2ae06413-a942-4c89-8de1-1fa212fcd64b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2b0c8ab0-4909-4c9c-9441-fc510b936d41"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2b1d1d3d-1ce0-4a95-9d5b-fbad03e4b75c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2b4a2cf9-2f58-4b45-8828-cb955be2161f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2b5f6287-4b9f-4eb6-97c3-abbbfbfeb84e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2b73ae84-1c40-4098-9679-86f512999682"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2c046e90-e7b3-41f0-bc8d-39accfc0879f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2c136d6d-d644-4cf2-a995-b2f9e5218a4d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2c18e17d-0e80-4342-8c85-4839f71c45ef"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2c216f57-6c1a-4e50-811a-5f8a5b8568b3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2c222cd0-880d-4b83-9e61-31b820c01626"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2c36399e-8aba-4be8-99d9-de1a01341f9e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2c74c36c-6227-4a4b-9b9c-bcf57f70443d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2c8ba49a-64a7-43fa-81eb-a072b2a0253b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2cdcb5e9-4d13-4cb7-941c-5f6b8a45e7c2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2d14f397-3474-45b5-a8ef-995666fb7563"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2d19c818-ba9d-4c38-baf4-42cf14844619"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2da3c93c-0df4-46d1-8370-b64c3ca8acf7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2dc2d71a-b79f-4ef9-8b84-6bbf325bb9cf"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2dd2fe7c-62a8-45d9-b787-5ad15808ecc3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2dfe6f2d-1a1e-45ca-82aa-cd19e85fdaee"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2e04b26e-1e81-4022-8d4e-f3817c3fbe47"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2e32407c-b568-447f-8c4a-052dd3a1fd67"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2e96d1a5-91d4-4f82-9b37-e090dbe091be"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2f262b76-8fef-47f8-b650-eeda51d76200"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2f7deab6-80f3-4f2a-811c-c096bacb650b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2fefbf18-8fed-467a-86af-cfc54d14964d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2ff75d5a-0145-4912-afa5-9d52c63bcd68"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("303e9e42-6d29-43a2-858c-e67554ba4ed9"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("306eb541-8669-4c0a-a65a-6413efa8b96a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3074f823-eb92-47b5-b434-75b860c5d7cc"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("30b7756e-486a-40c7-a24d-ff5ca4860db9"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("31140ae9-27d7-4860-833b-673dab14fd0e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("312b9df1-b85b-4b11-94e8-4b2034125b3f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("318b6d3c-4854-4ed2-871d-056e56b8d120"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("31b2d7b3-2fb4-42a7-8251-ce84ea728fd5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("31b6677d-f5df-4101-950d-cd47878edf55"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("31c14107-8e42-43d6-8dfe-bbe5b19f1d4a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("323f047e-5341-424a-886c-0324ab669fe7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("330041e1-4952-4bba-ab0e-71a36cd7b69b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("33464912-e5b1-4998-afca-083c3ae65a80"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("33468af8-31bf-40bf-a40c-a733879bf5e2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("334d0057-4c0a-47a0-a833-66f0184770cd"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("335fe746-12f2-4c42-9f28-7f21a3535ad4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("337014ab-d284-4e98-8ae6-65d00232fc8b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("339e7a48-b7cb-4947-99d2-e5943533f2cc"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("33c3bf94-1cfc-4778-a076-3a91e2efaf75"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("33e41bc7-1a80-4fb3-a1b8-1ae4e9543f4e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("33ee2791-894f-45b2-880c-e9b83854fd07"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("344852e4-4684-4297-b44c-20de42c8234f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("348e403c-7f80-4b6b-b5f3-0b4c62134e2b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("34a57071-c138-4f11-b5e8-15bbc4b5462d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("34b5922b-b889-4135-b628-45ca2a15a8a0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("34cc991a-c066-4dc9-a272-dadbddd46d0f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3522c2e0-f45e-4716-b1c3-de9c70cafee0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3550401a-119c-48c3-883e-46977a8c6032"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("356f6b1a-ab11-44bf-8db0-1afc5d455fee"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("35ce13f9-a65c-424f-8309-b0fbdf769557"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("35d8d223-8e22-4601-9abf-32b86bb8bc0c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("36029668-0d6e-4ad8-a578-d6b0baace5a2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("36910905-2275-4316-beb5-241396e8b893"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("37104a55-4db0-4964-8443-2bc9119c5753"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("37187ac2-5da3-46a1-b6b1-2dff7c8c9720"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3725a52c-1e6f-4a30-a55a-5fb4aa983973"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("37545212-7893-400a-a413-6e75f79500ac"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("379d9d0f-a629-4d21-bdbb-1702f2960ee8"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("37c40954-7727-4726-804a-c7582f5c61d5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("383386f0-421f-46d6-b037-b73b44be95ad"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3845346d-169d-47b0-903b-49cec8824b89"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3846f91b-107f-430a-badf-49fdf130079a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3871aa2d-b501-4e53-8d6e-346beebe9167"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("38872531-c7ec-4f84-b3c2-3d3f0c923c18"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("388884ed-1946-40a2-ab98-21c0cddeb2ad"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("38b840bd-8862-46fb-9120-0088e0b4fdf8"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("391ed44b-76fb-4cd1-8eb8-00ad07336412"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("391fbb68-4531-4f6c-9e8d-b79b8ca93608"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("392a986c-a664-46c9-9c1b-d7f7f5c30b06"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("39395bd2-5f8c-4f1a-9245-e4337817e278"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("39530369-9873-483e-a218-114c3bcc6240"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("397448bf-759f-4f96-bf78-a885710a2450"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("39ef6af3-6c00-4d52-83fe-39f398809e46"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3a1e6946-5300-4de1-93af-de7452f7a809"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3a2b45af-6c2c-4420-9b93-c54476743324"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3a356b79-9fce-4193-8f6a-b43de3d74d7b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3af604c4-15d8-4408-be3c-4afed220c84a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3b110ed9-3faf-458e-a188-e6c6d719707c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3b194a7c-455b-4dbd-a9c7-95acc378e68e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3ba14e3a-67a5-4488-a15c-1075e07e9d32"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3ba986ff-d53a-43f3-bf55-361061559204"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3bd5ddb5-9dcf-42b6-8ba7-aa16e9c7bbb3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3be2648f-d5b5-4e8d-880c-ab4dbeb989a6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3be91589-f3e1-4200-a956-7973459ded35"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3c55dd08-0747-470d-8a17-ba1deb200662"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3c6b423a-2d73-4bf4-a10f-28b98014ac0c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3cd116f8-c660-46a1-96a0-ef6ce8fb532e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3cdda2a9-ab20-4b83-8caf-68e7ecac9cf5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3d2b0065-bb83-4e0e-9b23-d007500e703f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3d450887-56ad-48f6-9599-fdf5625126d4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3d46d50f-22d7-44fa-bc9c-358c9a00dcfa"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3d8fb352-645a-4a16-9ab2-db0ed891c3bb"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3daf070a-73e3-4449-867a-c69dfc90c476"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3dbcf14f-400a-46b0-b179-1aa84e656724"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3df82031-1575-465b-a075-415e457bf7f5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3e39aae9-008b-4cd9-89e4-40d94fcb49dd"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3e5952e3-4d49-4c01-8ac4-e91928af25a3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3e7b18d4-4e67-44bd-9c49-4d8e75f18607"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3e806307-573a-45f8-b295-06f95884ffc9"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3e855da9-6a91-49d8-b94a-000863127f59"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3ecac202-c204-4384-b3f9-0d3ff412dc36"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3ed63450-486c-4ade-a188-07f6c9e8139d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3ef911af-689d-41ae-8125-dcbb121c7c66"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3f1725c7-758a-42a4-ab5d-21ee6ad2c8d5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3f83592d-30eb-4f87-a13e-94b62308cc5c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3fa1136d-a326-49e0-907f-8e12db7f8429"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3fe6c7bf-c0bd-4b9f-87f5-1bac12969a91"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("40bcdeef-7d37-4632-9893-70f454a50045"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("40c7827b-9556-492f-af7b-f2c4a5a3f1f2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("40ef3e55-4023-44e7-bdbc-dddc966f3c29"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("41036c83-c26e-4590-91a1-cf167e32fd1d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("41230103-eafc-4e94-8a18-00a4233ae90d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("41ba2e29-1862-47bc-bf15-56599ad510e1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("41e962f8-d31b-445d-a58d-6d91b74bf966"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("42047d4a-7837-419c-87b0-ec879f751745"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4220440e-b0e6-4f2f-827c-539552ff32ad"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4227ec35-4c3a-46af-9bdf-61f0d87888f0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("42c12a49-b7ef-4539-bdce-ef314ae9e460"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("42c35589-dd1e-41e1-bc7c-e2b965e2d7ad"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("42f9a891-2dec-40c6-8475-d0eb28b51d51"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4340fe30-9786-4feb-9d90-e1c82f340113"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4424dc08-b730-4a64-b252-0b138dd52699"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("447a23ce-6449-47c3-90ea-081681d6c5fd"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4485e161-a44c-43fd-b43f-a038f3beb012"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("44c3608f-231e-4dd7-963c-4492d804e894"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4513e509-0e43-4c2f-b4b4-9abd4d81a856"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("455db200-3f89-4780-bf6c-e970fe8dc06c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("457925d3-db5b-459a-8130-a4d0d367f4ae"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("458809a5-5ae8-4d9d-8af3-d4249d14a5de"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("459820f1-9c30-4998-a207-9dbada17108c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("459f4747-a065-4288-8184-2536237f016b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("462368bc-c8db-4369-83a3-6b8addcc6246"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("463ee869-7763-46a5-9319-5dc314b0f488"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("46ddbeee-a422-4794-9640-0c114e1f6ac9"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("470649c0-3f2f-4f13-a23d-fcb1d6b9b6e4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("47159ea7-a205-4c4f-a93f-8d2d4498c39f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4745febe-293f-40ec-bc10-8490a4b245cd"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("475fb009-c4b1-4b28-b526-96f06b922db6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("476fec49-f090-42f4-9c7c-058e1c10e226"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4770940a-f28c-4179-bac6-9f99d56aa7a5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("47d83ca2-4ece-4f3c-8771-c8c24564245e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("482e828f-3d1f-46fa-9269-0ce594879246"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4836a4e5-d0a3-43de-b8e7-d4bfe81b7e9c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("485bf2da-6079-4160-bd84-8eb30559c63c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("48614141-0cee-4337-803c-9ae4e0b12ae3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("489189b5-f8b5-465b-9b6a-7573d21cc238"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("48b7a7b0-ed79-45af-92cf-25445c376102"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("48c263ce-33a1-4016-91d8-1beb5d08c99b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("48fc350e-6fd5-4087-9b3d-a5b2c43c2b45"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("490806c6-db6b-40ca-a528-84f5df075231"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4970ea1f-b07d-4290-98c7-1cbfc055829e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4987e982-7774-42ac-b22f-a24bdf5e2c22"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("49ae657f-2a66-4674-8aba-060bc6388a08"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("49c09825-1bec-45d2-a2e9-d6219c5b02eb"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4a1177f9-6a35-43e9-b106-c309bd5313dd"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4a8aa3dc-1da9-42cd-b9ae-4e882255d86a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4a93fc07-d694-46be-bd34-fb31de211c57"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4ad9271b-a7b6-41b3-b8cb-e5b9a1d0c10d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4b055366-6301-456f-855d-555f5aa7c73f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4b61f76e-b9a2-4aef-a403-95a0e922b603"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4b6fa7ab-b480-4651-8831-abc0f78755af"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4b90f8da-261c-44af-b4ad-d91fbe6915a4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4bbf4924-b27a-4ec3-94ae-df1004ec415c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4c04567b-5ad0-4660-9186-e6c36734dfcf"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4c35309f-7645-48a3-9acd-b461687404e1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4c68430e-9088-4351-814d-a17cae6e55fe"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4c6d94b4-e66c-46f5-8fad-4167a858a9d6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4c86a46c-f9c9-45ea-8f85-409731b2d9e2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4ca759bf-600e-4e4d-be99-0f5b09f8ad4c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4caded10-fff2-4e38-9b28-cb45cee309aa"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4cc99e95-6993-466a-8c7a-1bfce7159bb0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4cdaac14-f923-4735-91cd-ee695752fee3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4d23d7b3-bbb8-4642-a218-b0063a833a14"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4d3c8bfa-4dd5-48b9-8e52-6215b93e1132"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4d5c6fdb-dc53-44d3-8800-6a6babc9ea35"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4de9449e-65f5-40c0-9c32-953d189f20df"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4e5b013c-66d5-4098-ad3f-78d03587c653"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4e872775-ef55-4ad9-b176-27c9cf4e61ef"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4e9c7a1d-a42d-414a-9d14-4e2986c939f0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4ee171e5-a4ae-4838-8cba-c4f493508145"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4eecfcad-b8e1-4cbe-860b-6f7d42c06da0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4f16fca0-be6e-4edd-9b2c-5ca70511e480"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4f24dd69-5bc8-4f1c-99f5-2260817643a6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4f27a05e-b05b-4a76-8ae7-358d947c59c9"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4f7ba3f9-c2c7-453a-8775-1283615f61ff"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4fc9ec5f-e93e-4447-9cdd-62a8f8e8170f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4fcc9e17-fee4-42e9-b4ea-151de79540ad"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4fcd15ef-14c7-454b-aae4-4ecdb49f9d6e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("506123fe-47b0-4fbe-b7ec-c6644b184702"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("506c8e5f-467d-4ebf-8fca-66cf0a8b5e30"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("50757c7c-705d-44e7-968e-c3189b9c94a1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("509797da-05a6-4c9e-8168-d46a7f22d867"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("511fedce-81a6-44da-ab0a-7950ae3762fc"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("514246ae-1e23-41c2-a6db-2bd6e4a9df4b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5154dc63-f7cb-40c9-b612-3abec95e54ed"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("51e00361-b228-4e21-8efd-06edd9cbb42c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("51f4291e-dddd-497d-bd04-158763e1a131"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("51f9c113-1d15-4a02-b1a8-3d7edb3fefdb"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("52399c6d-9d4a-4286-963f-7ec91adf946e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("52afafbe-de61-449d-9d8a-29d54aae6e35"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("52b6a55a-6ff7-4101-aa17-0b82367f5b8e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("52d019a7-3fa9-46e4-bef9-a645db4c783f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5334cfd4-c2d9-48ad-b83b-edb432894f4c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("533f62df-234b-41a9-8735-27b0849fa018"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("53737ff9-2b2e-4800-9851-5967dacd26d9"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("54279a3a-dbbd-41a3-a317-fa565d1b6e3b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("545e51b4-30f1-4a2b-bc99-a4103e00b963"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("54a4a2b1-6b88-43e6-8c8c-299526dd9afb"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("54b0bd76-51e1-484a-8ad8-deeaa419d564"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("54d734b4-327a-4595-96ef-2f6633735c60"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("54e4d207-1a85-46b9-9bfe-225b6ce450e5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("54f67f43-a30e-4a7d-8248-783119f1a360"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5524785c-7823-41df-9341-6b4a65aa9c19"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5543c2b5-bde5-4c41-b856-e1469fa11df9"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("555f7e63-2756-4765-a9eb-1de1acdc9c72"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("55f81174-38cc-43b4-911a-1d1246db6f5c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("56b105cc-0f86-4260-b421-7f93628879dd"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5702d9be-1222-44f9-baeb-3bdfb1642fe8"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5740e112-41b7-4b5a-aec0-1d15fdb6d997"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("57725cc0-5c4d-4ac9-8de6-5355cd8abe66"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5786ff70-02fe-4d20-bad7-affd201fe419"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("57e673cc-ed70-442f-bd96-0f261fc88c8c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("58395bde-7579-47c3-ade3-854acb2e2c5d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("584eceab-da04-47c8-82dd-4ab23a1f6d21"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("590b226a-d7f6-4f1c-a30e-f94a0a1ba53f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5911c2f6-5441-430b-98f4-369132ce4513"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("59755880-6b5d-4ce2-8b12-5125a7ddf832"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("597979fe-6365-4b9f-8043-83d9897f90e6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5a13a8ed-d1ef-452c-ba33-dfc6332b07fc"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5a723464-db3e-4387-ad11-ac66c0da2653"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5a943494-26bd-4c4e-8277-5ed74d4d55d5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5ab01d61-cf4b-46ec-8792-44632e8a7e4c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5ac5c52f-75ff-4787-9fcb-655e7020b3f9"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5afb1591-0296-42f0-8e1a-4abe24fcfb7a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5b13ce84-37b9-43ad-afa0-3407e277bfe2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5b41832a-3656-4a4c-b3a7-e6d3d67d76d8"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5b733263-b172-492b-9fa2-81d5cd867d6d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5baced7b-0109-482a-bc8f-799aea68b010"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5c0068e6-fa8e-43de-ad7e-6d2b7c497e10"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5c5cbc05-7e6a-4b10-add0-7f7638ec1918"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5d39f71c-ede7-44bc-a1f8-5376f59666ea"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5d3afb26-c8a6-44f6-a00a-f50d23fb220e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5d75da70-d99e-46d6-a066-97347f6ee5c4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5d9e6668-df0b-4235-a9f7-e047a5eed8a3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5d9f3d17-74f1-412e-9b7f-70d6debd7a12"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5e32e34a-16cd-4287-9788-d243793f1ba8"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5e4cfca5-f1da-4233-afe8-f41f9e34392c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5f17d1c6-6964-447a-aaa5-791e8895b8b0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5f5233ee-f540-466f-b022-04c1da465ef3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5f9ceddf-54d4-4279-b306-fa5382fbb4b4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5fa85ad6-b12c-4b32-a3be-71697bfe26e9"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5ff90321-280c-4cc1-87d3-58b8012224e7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6050e0bd-f829-403f-83f2-6ff2865b1f2d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6057e81e-a0ad-46f2-8ae9-dee253f2c600"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("60593be6-cc9f-4a86-bbc0-f65da4b79b51"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("609bbae6-b46b-4e9f-bcc4-44bd5b01599a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("60d1768d-6d86-4a22-adc9-220b6a075308"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("60e405a2-4d47-4a49-a739-dbedfb7bf9b3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("61ed6301-8b4a-4ed0-93ba-2fa07b5c8796"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("61fcc8a0-34d4-418c-a77d-55328773dbb2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("628e70b0-af6b-4fe6-b63a-36b3c116bebf"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("629bfd57-7c1d-438e-9cdf-c1524f885090"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("62be61a4-7d43-415b-baae-92e83e521140"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("631c5c01-a844-49eb-8cdf-7555416cab1f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6344cd67-0f25-4288-9026-6af917df5790"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("634f5af4-8a00-48b1-bbf4-cd68cbd1338d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("63824924-492d-4e99-92f9-92fcef210ff0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6385964d-19af-4fb0-ac9f-122a64305752"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("63fb8f7e-54b8-4900-9e90-d171a1763fc7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("64150436-2aac-45af-b6c0-5f948dc3cbf3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("642b2ea5-16c2-4642-b811-b7920074a656"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("64a797c5-6852-4095-97ae-3bd45beb357f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("65225bbb-bd5a-431c-9b0f-46e6349c03c2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("653f8e76-c913-4894-8957-f4d812593f1b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("656a1449-4e9b-4dee-bf0f-8c6d0070df2f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6576fd71-1dfd-414d-b743-deda97b6af0b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("65937cd7-855c-4e0b-afbb-354d10942588"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("65d18006-fe12-4b5b-b781-96a8a8ad877c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("65e05417-0c38-40a8-bc03-a60ac083cdd8"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("65f565da-c32c-4dc3-a357-08d4022f334b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6614eacb-155f-4a13-aba1-c585877bc9a6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("66290930-5cfe-40e1-96c0-d918f90f4191"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("66a284f5-97a0-4962-876d-f33f0750a34b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("66f7ed14-a941-4c1e-b1e9-c72d15671816"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("66f8bd41-18fb-4c36-a7df-f82b02c31541"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("671be757-50e5-4636-912e-707241af5149"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("67320253-12f2-4c75-ba77-a916db405cb3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("67396be1-9139-4766-b08f-05a4c4c1bcff"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("677ce666-0152-44e2-8fa1-3b56d372b87a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("67956308-b9df-4107-b50f-3f6c911005ae"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("679bc917-497f-49dd-8b91-ba5accf9be86"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("67da876c-ac6f-4b1b-8083-385f6abb7fd1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("68806afc-e4a9-472b-8899-ba0426b1ebe5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("695d7449-a7bf-46dd-9e7e-6d49f05c24d1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("698353ff-870c-4c0d-9f6b-011bd25f3787"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("69870407-443d-458e-9190-565f8e08127a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("698f063b-feaa-4921-b3b1-db97668615d3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("69bddd08-ed80-4885-869c-4714dc02ce06"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("69c94cd1-0423-4df6-a64d-aab6e96a569c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6a0db87b-8a6e-48ab-964a-2e785cc3d6d3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6a203fa2-8f39-404f-af66-d28e23925625"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6a4403f4-4e9d-44d5-b78e-b330547bb781"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6a4c0d9b-fcbe-44b5-900f-558b10ed9ecc"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6a7f7bd2-5667-4cf8-a857-e8ae47871898"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6a87886f-97b7-40ab-813a-f063c8222a94"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6a939b10-83d6-4ecd-97df-c3f732f3f77f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6a9e42da-c4ba-48f9-a6ac-acc852027a87"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6ac374a6-4151-40a4-943e-4e9f42d30b36"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6b3e8c14-2249-4d1b-b95b-2459cd4d86cb"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6b91313f-05c5-4556-848f-0d4bd77b5b11"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6b96e2f9-75a6-4c49-923d-98d448b2a266"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6bae1060-fac0-4515-8ce5-d09dd5b6eefe"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6d393c1b-2af0-4be6-8450-fad7d03d4de1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6d3e4698-d919-4d98-bd8e-2e98d6473bc4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6db4a299-9431-4e71-a0d9-c97b186ce043"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6dc8b7ba-3315-49ae-aa00-9f7649998706"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6e03ad18-2623-42cf-a39a-1a78dcef8714"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6e1a2fb2-f33d-407e-b3df-8f01115d9d47"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6e27a0ed-58d1-4a1a-93bb-d757594c6a1b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6e2b2f97-c1f4-4ecb-b40f-9080bebdc965"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6ea6858c-cf45-4143-8359-02fe6294ccda"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6edfc88e-fa8e-490c-bad6-f54f483ab7aa"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6f637c51-b8aa-4fff-a796-cf69bdaa49f7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6f797a5c-078a-48f8-b27f-2e3d67203175"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6f7d6597-8fdb-4fcd-8642-7db216b5dff2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6f8f4ef3-1f6f-4706-8eec-461772615796"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6fb3bb96-51c0-4ada-9c9c-8ac06beed41c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6fdcb0f8-4f98-41bf-a57e-e7f73f2f725d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6fe27200-3464-48af-99f3-745c6a868089"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6fe59de3-1917-4f36-bf38-610259f71378"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7021e407-fb6a-462f-87f7-25ef32a85915"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("70693b46-61f4-4321-a896-a9913d0aa600"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("70c91a44-6abb-4866-841f-44550c2cb8cc"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("70f2cca0-2289-4482-9ebf-0be8e031203f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("71179d01-f00f-4579-b87a-d622e9dfbb36"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("71251e92-d6f6-48c2-946a-47b310243215"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("71259dbf-b9b8-46ed-a7bd-9c256bdf848b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("716c4d1e-34fc-4eb7-ac79-e65b3b5af255"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7292e6aa-d4ee-41fc-a09e-cf9365b38c09"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7306f844-2521-48d0-a765-2c1e45621d88"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("73119e11-1702-460c-b0f6-fb14ce31f0b0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7337c2ec-97e0-44b9-b72c-1a7f5408884c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("733fd6d6-be41-42ed-b105-68acd32b678f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("73a5df50-ee94-4dd2-b0b0-e00bef38fff9"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("73d6dd94-75db-4ec8-a497-ea04af0c8bea"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("745f7bed-f6be-42c9-ba43-537954cdd284"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("74bf9182-ddd1-4bf5-87f0-d3195a79f0a9"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7502a3d3-de66-4140-a6d6-960ef7c7917f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7545fc82-4a58-4c7c-aab4-3e169c0dfc13"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("75a8d82b-93a2-411f-b082-b26fde40217d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7640dec2-6863-47bb-933b-b43df54b0866"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("766271cd-8bb8-4b14-9c4d-50424a30f6bc"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("768011c9-e0c4-4b1b-84a3-317c4bf2f402"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("768cfc30-fc07-420f-86ce-d6f91501d253"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("769e9162-2ae1-4689-9721-a65912933bf6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("76e51312-1e1b-42c2-8059-6ffb0dae1eae"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("775cb1aa-73b9-480b-9edd-23108f1f05e7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("778a2fae-8eb6-440b-b2c6-056b637b94e7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("778e92af-2f03-47f3-b692-dc1b20635544"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("77b00623-0efd-4e7e-9cfd-b6a017cc9738"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("77ba7c6e-6aba-4575-8468-e8a522d5c478"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("77f47444-1ab3-4245-ae3f-4472ea7e4ade"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7877b857-c94d-47c1-9636-e0ef6005ef89"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("78b6d05e-e916-4536-b091-3552019e326c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("790391e9-6c41-4d28-8341-c1b8811036de"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("79236fd5-e97d-45e7-9a5b-5ee9b623c6d3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("795bb9c1-8d85-4b38-ad40-d77344139fc1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("79b5d236-2766-4f92-9ce3-826ef6bc319d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("79f9b55e-5bd5-4862-a8fc-e5bff1a957f9"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7a0af4b6-241e-4dde-bcbb-e7aa7908ff3e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7a96bb0c-38d0-4b16-bf21-4f49df83d86f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7aaec4ba-fee4-4db5-8498-df3987bb620f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7ade1b25-109f-4b96-8eed-7fc6c7b32d6b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7b0d2fa1-757d-4fed-940d-b697572e6315"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7b4ce341-b369-4fa4-8410-15fd7e5a7499"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7b5935b4-c2e1-4b8c-a8cf-a48ee9cc8617"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7b7a1322-e94d-4272-adae-75dd7df17ee7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7bcb517f-6cf3-4396-9d17-fa116d8ece59"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7c1c7f89-d326-42ec-8edd-d67cd3e52488"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7c5a090e-b20b-4dd0-938d-03aabc914eb2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7c5c250b-619d-4bda-9dfb-24b7c580c76c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7c6e024a-27a7-4763-b1a7-30f442aea7bf"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7d3100cc-0a92-4256-8988-4f9101ee8f45"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7d3350c0-9637-43b9-b866-00562796ebf1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7e1071f0-410c-4f32-8dd0-3763d23605e3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7e12c815-5491-4cf9-aa03-5f4528aed0e4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7e9c715d-0248-4d97-8f67-1134fc133588"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7eb349c6-db5a-423c-b996-a793b1e4db1b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7eda8526-ce1a-4bba-98d3-32e6e3ca816e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7fd94f8b-83fd-46bc-ad21-9cf5b0fda3d2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7fe7d00a-f53a-4069-a4ec-0065f14c2339"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("80018752-3ab1-4285-b26f-34655f5c4ed7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("802123fa-680b-4d03-bf0a-8d4d53c38067"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("805559be-9862-4194-9a37-9a9d93b73be6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("80608b56-dc0e-4205-ad07-5fbbc878d679"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("80948edb-30fa-4d22-8f2c-c9bcc886c6ee"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("80ffe3cc-baba-4785-963c-81b6a30a217d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8105af2a-2934-4b33-bd4c-faca0759208d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("811b7df1-6b75-46d7-8bfe-04513996f7e0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("818340c5-52a3-44c5-be80-957033a3dcda"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("81f824fb-01f7-4941-bbd6-17a7e9a7a77f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8218a639-27b1-4a8c-8f44-3ca722294c4f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("82321cab-3092-4fbf-8b4a-b82ba7ef341d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("82b70e77-fb46-44f1-85df-4809167bef8b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("834b99ca-f6cc-47aa-bf9a-318726e89dc1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("83ad62d2-28ce-4746-b2d2-fc92059a1fa4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("83fab0e7-b142-4326-9897-e73394885d2c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("842f158f-9354-4222-81c5-e4416e37255e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("84576348-2dca-4487-b0a9-46268f222fa6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("84e57dd3-4cb6-495e-92ae-f09810a6743f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("84eebf2c-d40b-4e57-b392-8085eaf6fbdf"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("84ef9b5f-cd49-4c74-9cd3-a2e613a86674"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("850c1f6b-5bef-4660-9a74-7656f784d86c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("85364a25-a67d-4194-83f0-d48e848b3fac"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("858f0d2c-e3c5-4cff-bd82-c311d8f376c9"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("85d14bcc-8351-47f8-bfb9-000534f66834"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("86101239-4962-40a4-90f3-48d6fd2bf8e2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8680a1b4-8271-4004-8b8c-b906ce6b8b4a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("868dbe9d-8a26-4e06-ae43-9abeb6202aec"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("868e426f-b11d-45a3-bf2c-e0c31bed2c44"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("86a7ae93-f6ed-414d-a980-d279d583dae2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("86aac5eb-db9d-496e-929d-9d7e34609e58"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("87ac9a14-5333-4de8-bac9-cbc1d5bf86a6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("87ec205d-9067-4c55-b3b2-0595705af675"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("87eed220-c1a8-4f17-9bbb-59c9e8f90e6c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("87f31e3d-d28f-4f61-8b8c-17ffef38e149"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("884e400b-1b68-4234-96f4-7e1a2029b15d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("88b3881c-9401-480c-8f95-53fa0ad345c2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("88ddf0f9-1f3c-450d-a541-6f253bb83ae6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("88fcd776-b1fb-4e1e-9e8c-31ea47ab719a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("891688fd-d707-4413-bef4-d6692e1582d0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("89a79fd3-e59f-4255-9e7a-2b536dbb5075"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8a42fca1-8e62-4805-990f-ee19f85a6a4f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8a702ecd-96fd-457f-a931-4b367f12277a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8a7d861d-5086-4fba-a813-20fb895c8412"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8b3b8368-fcf4-4612-b78a-e7a00e7fa826"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8b46b848-3543-4dca-98b7-e6148aa6afb3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8bac3ff5-fd44-4989-b4e7-0b1846686eb3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8c00f2ef-4a63-4f7c-a264-552e5ea5c1f2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8c3a922e-4f05-4e07-8143-37ec157db294"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8c5315c5-afbc-4a64-bf67-d8d0ffbe8b76"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8d01039e-986e-4e26-ad3e-2e30729da071"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8d630c33-b77d-439a-b68a-a1bf4410c573"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8d879b88-5e60-4a5e-91f0-83ecc4967eea"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8d98d3c3-2705-40c3-9c9c-c22730bd711e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8e0b5799-0e39-42fc-ba4c-652cc6fbb918"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8e2e428a-341c-4f76-9633-124d5ca342b5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8e3a276f-e332-4bbf-955a-16ec59556460"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8e7f80d2-ecba-4b5a-b878-6587d94da1c5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8ec2b877-6d80-4d09-8dd0-314964f81a16"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8f3159bc-446d-4e05-ad90-b26b5a983c6c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8f58e133-4d63-4527-a5c8-29a4eb1a726e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8fa5e547-b7c7-45cb-8912-8609d6c16f1d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("904ac274-f26b-4b8a-9fe3-8ab27cca3e9f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("90546964-e70f-4518-a11c-e912c7812587"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("90554de3-8d9a-4332-bfa6-ac3c174a00dd"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("90cd195c-ca0b-41fe-a04b-70d2c90c15b7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("910d3ae1-0c2c-4ed6-baac-0e1e69260969"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("915fe2f1-3327-467b-a2e9-295ec96c7c5e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("919633a7-e62d-490f-8e4b-84339da0ab9b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("91c4fcb5-6ca9-468f-9085-55d267414263"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("91d21969-295e-4a6e-a936-723ebd16e496"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9278a04d-c67a-47a6-963d-25048b4fd834"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("92908614-3504-420f-866f-8f53d2d796ae"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("92dce1f7-c2aa-4371-a2e6-1a8f24b22b4c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("930f0749-18d8-47f3-b29e-424c352209be"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9324dfba-f970-4d80-89a5-4af86ab62ed5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("938e9578-51e7-4bc6-8760-97069a6cf535"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("93add9f7-2d74-4fd4-ba27-36fc483cb9eb"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("93bf8408-fe62-4958-b031-0aabfcfe32e4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("93ccb810-7dfd-44a5-afe4-69c9d9c6d314"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("93fa0a6c-474d-4ae8-af23-952076f96336"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("94259dec-cb66-438a-9d2b-2d0e7f707fca"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("94e6eba6-4e24-4a85-b3ba-c71ba0cde4b1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("95563a29-52ea-4d9d-b22f-c644462cebf6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9583590e-1925-4f46-9025-aeb6d5a04b07"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9623526c-0c4b-4ff2-a99d-4b8ed508a0ce"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9670dd65-0d74-423c-ad51-cd1d4c638d03"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("96ec72d1-fdcf-4988-b73a-e3fc5a83f49d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("96f950fb-2331-413c-966e-5b7cf4c1c4a3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("96fa60e4-55df-43a9-bb6e-86e303cd2d67"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9718b416-b72a-4fe0-b394-c59f0add5d7e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("973ce72a-b4de-46ba-98e3-3e03df0e5594"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("978fbf0b-9c22-4c29-bd4d-c3b613429519"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("97b68a41-f8c9-49b5-be79-0f646c3e3ba1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("97e4ac81-920c-4877-9151-96b8aef40c36"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9857ff70-d992-4e8d-b97d-d174b70dfb74"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9878d735-1022-4001-86ba-7e53f576f850"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("98ac8f80-56be-43bc-a433-7e92e0cabeb1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("98b4f15b-dc64-440d-9df4-9e16c47c5b64"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("98bce02c-7f69-4cee-9f1f-8ecb6667fa7f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("98eed1ac-5537-4254-b0ff-7b590bc7fbb0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("98f248ae-6791-40a6-b241-5753041596db"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("98f82c09-d243-4c39-8260-4874c71a0433"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9a08a374-5112-4386-9ea1-100819754d8b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9a14b7bb-89ad-440b-9fcc-cea488a9deab"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9a45c03f-ab96-45a7-8f11-36c4bb210f60"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9a4f657a-01e7-4ca6-b8d5-b9e033401eb6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9a60d410-2ce3-4611-82cf-b4a0196ff5e1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9a6d0dfa-73f2-4df5-be61-95e4834cf564"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9aa7ca5d-bac1-4efa-a393-db5636d183b1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9b11d0f4-3c0e-4da3-b11c-bd90c0e4d65a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9b5ee9bb-78d0-4207-91c3-7c8437f54b2d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9ba3ecfd-157a-418d-9fe4-5fc83add48b0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9c62e25a-f43f-4483-bdf9-f2000aff61d4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9c8a6802-5175-45b4-8a62-82d19c5377ce"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9cadcafd-3526-4afc-8a4b-d1f1185c6e60"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9d01552f-d274-4cbc-a4a6-a15e4d34a72c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9d17ed6f-dea4-4c69-8228-ebbe708e3148"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9d9c3480-5023-48d8-8850-8ad2e57de397"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9dbbac50-5607-4e9b-b21c-a1fafdd1c36d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9dc201fb-9fe3-4960-92d7-8b8db7f3a0ec"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9dd22d58-44dc-4a73-9d52-e36cf488e570"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9de16b29-e316-4d88-ad2a-e47a0b413a3d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9df0c003-65cb-4c9b-a667-8eba40d5fe62"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9df6af00-f484-4f6c-8aa6-84acc47c64aa"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9e12885c-7511-4266-8eb9-19175f116167"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9e2bd90d-dd34-4355-af2a-2917065629f0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9e4fd059-9f62-4e75-b049-33bf60d7e594"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9f8eb5c4-0fc0-4c84-bfdb-eed9b1d9cc94"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9fa43296-0946-4e57-bb3c-1e599b6d611b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9fd2fb99-4a9b-463f-8b23-a4ad8961de06"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a01ef32a-f363-4276-b02f-256f6acdfe58"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a02016f4-22d0-4181-bb0d-832801640419"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a08d156f-0224-486c-850e-785408b7489a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a0e8bd6c-90eb-4c96-a868-834b41d3390b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a10905be-4797-4e7d-9911-b1be4534f6d8"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a12c1fd7-a6c9-4e31-87a5-1cc213aebdd6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a13d2e29-6f40-488f-8fc4-ba84101cfe0f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a147d206-4a86-4665-bc81-21c651334073"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a204ba25-78a5-409c-8e47-1f0de979d15f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a219bc91-317d-4d34-9b1f-2aebd842ff67"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a22dda9e-0c7d-475f-ba10-6b76d549f920"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a25a8a6c-4223-437e-a1a4-96894227549c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a2c4eace-1bef-4773-bdbc-d6fad216867b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a2f561d5-4128-4ea5-b272-b4bd23da30f4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a33f56be-8bc2-4b50-b64e-a533a2e495d6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a3c6922e-3566-4f31-8b7b-9bf6f1b55708"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a45d75e0-e056-4adf-b269-00f09e89945f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a4cba359-68e2-4188-91a7-0991b72b2013"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a4da4b5d-3907-4962-88c7-04b387471131"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a4ddff83-2c02-4377-a034-0304e5e3c374"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a4eaac12-239b-4e65-ac79-e7858080a94e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a53921b3-05d5-485c-9dcf-9cd7371d5a33"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a53aa091-d475-4f4e-bc7c-20361c476a29"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a54f2a19-1469-40d9-8913-799107453654"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a5594f9e-9eec-4626-b62a-21f724be2117"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a5804acc-75eb-42ac-ba4d-66867cf52964"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a594c0c8-227b-414c-83c2-f069b3089f7c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a5aa2617-09b7-4889-a863-2dfa6d254236"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a5f820dd-37d5-4ef6-88d1-d779183c306b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a63548af-fe08-47cf-acd0-d9ea702b4e0c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a661eb9f-262c-49e8-bc53-1010973021ab"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a6c371f6-a52e-4821-bb68-c59e7f0a3b1e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a6ee57cb-dda6-4d29-852c-62b762d147a1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a6f34849-54f7-44e7-8fce-265a9a8b604f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a72c02db-34ae-4be2-96eb-7ed3f992499d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a7311eba-644f-436f-ba95-dbdd124d999a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a74eda3a-bf03-49f6-a1bb-4ba277d6336b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a79f488b-6e6d-420c-b7db-f81b0657d1ce"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a7ecceeb-ea05-42e8-a6ef-0119e309c5a6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a82e9405-d427-4db3-9a4b-5292eb76f31e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a8853a77-64b4-4eea-b3a1-017d2309d8b0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a8b004aa-1c5c-4ec3-83b4-8a932bb8117c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a8ea4caa-1e57-4a50-9817-c718dad73539"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a90ee26b-ec10-4047-bf21-ddbff9735e6d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a977da6b-3014-449e-8fc9-477f50d7d502"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a9820956-84cd-4373-8e93-04eb01c45c87"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("aa0aa029-25ab-41ee-9414-46b1c6d6c238"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("aa1463dd-93e9-4315-a039-1dfd867fe5f5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("aa2806f9-ea36-48b1-a5b2-04fee3f81a0d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("aa31ed2b-808a-4919-8f7c-bca57aa9b2e1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("aa778a08-5ad9-4268-b8c0-f457c8b591ab"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("aaebaac0-3ace-4ab2-b473-8a09749c7787"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ab3317d3-f91e-408f-a639-d6f322b6fa0f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("abe80c99-0bbf-4e48-933b-874b65358eca"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ac1f4e4a-6f25-4349-b7a3-8966bd6ac2db"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ac2a5bd8-b96b-4c0c-9540-6e9539fafd52"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ac84273e-bbe7-4720-b731-5892e4ee1b2c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ac9539f5-8879-4127-90d9-5511cbec1645"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("acc36813-9c02-46fd-88a6-3358b3bfe182"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("acf1a783-686a-4ef2-a3ca-0e87a7006541"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ad0e1247-6c1b-4056-bafc-071f67c97550"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ada8c647-dce1-44b9-8d6a-003927478fc1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("adc02fa9-c7dd-484f-bab0-830241d8dd43"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ae1bc8c1-6025-455b-900d-9476965e1904"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ae531d23-0411-47ee-b5a4-29e7074b7e21"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ae74328a-9f61-4d1e-9247-4895ae7045da"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("aead5943-bc92-402f-8687-9e6db1203f04"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("aeb507d7-b0fc-466c-a26a-c2b4165bd01a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("af0562fd-363c-4f51-832f-41a133fe8147"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("af1a9b2b-5a89-4b4b-a434-d58e123afc32"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("af29e41e-2f82-41ac-983e-ff1df2294e58"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b033556e-8bd5-4277-bf79-9875774dc403"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b0406779-891a-4ba2-9c79-46b78b4966eb"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b0dd2ccd-a351-4621-92e7-8adacb8e55b4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b0ff6cdd-9485-4e2f-b7cb-40c5161d631b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b11da708-86fc-4ada-ba28-623aaffd1732"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b13452a4-becf-4e6c-8edf-2e6995b4c5a5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b1880968-3dad-446f-a8e3-65cdb87bc2c1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b1b3cdb9-8e54-4e1b-b94c-9cbe63d3d86a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b1ce8793-4601-44ff-b71b-fdd3ccec7cfd"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b2574388-808a-4a1b-98b5-af09e9caac28"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b284d100-3ee7-4f46-873d-3f75cf0f1540"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b2a38a25-3fd9-4274-8ee5-f85744670ef1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b2ad12e9-2017-461f-8938-c6d04129bc04"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b2b3ca4a-fa44-4040-8ee5-44b05a56628c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b2efe4a8-51f0-452b-9a9b-6b6d4ce1ca69"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b302d0dd-7b4b-4a20-925f-ba0d951ca21e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b3055ec0-0a3f-45d1-bba1-4bbd43aae087"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b30f30f7-51e8-4805-b1d3-6d719a0b5b9a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b3997026-533f-4e18-ae41-4d24f0a0f3d5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b3b6cb43-1e6e-4857-bc35-b271b9bd42b7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b400850e-8fc9-46f0-9018-c6bd26460b44"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b481c80c-fead-4b41-9eb7-5bcc7dca7d7b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b4e9975d-9bf3-4e8c-82ba-a7a777dcd912"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b50b66ed-8182-4ef8-9b7d-848f14cb3e91"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b52f9c45-7719-43f3-bc40-d1b140fe0f5c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b568b5fb-ff20-4cd5-bb76-47375e796653"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b5788a2e-a238-49dd-813d-d1ce921258c9"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b5a9c2bf-f95b-4d72-8d50-c213d2dd7d19"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b5cc1c63-8ab5-4c8a-b181-1488c29d59a7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b5cee958-0e64-41a4-9f66-bc008c7a59e9"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b5d14f32-ab54-4fb0-bfa0-09c6d3af9eef"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b6148321-d7b1-4d3f-905d-258ff2711655"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b6333773-5b85-45d6-aa19-a60bb101b659"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b6d8ea7c-1d31-4102-baf2-a799480ca577"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b700b31e-6d6d-4ee5-90c7-5680b8cf34d7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b748f1d2-0fd3-4155-99a9-93da8be2c960"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b74e4e2f-f51f-4fd3-8167-9ad2c8e588eb"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b77e9ffa-06f8-48d1-9fdc-668d0d13f772"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b787c819-9215-4757-80e6-5349c10d1564"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b7a478b2-102b-4f92-870e-4ed979c162e4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b7a95499-6115-4c97-ba9f-8f2fbef86169"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b840b161-3fb0-470c-91be-a8107c1c90e4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b868aa30-5b13-44db-9d2d-8bd163e9bc7b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b8aa918d-233f-4c41-b9ae-be8a8dc8be7a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b9262d3f-b00c-47e4-a6fb-7891523fce51"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b95f2b70-0a48-4470-bf16-8ba6fc1a2264"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b9b71847-9616-4993-a609-93801703ae02"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ba9b7f24-7e91-42ca-abfe-ce530f26aa36"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("baeb671b-1b0d-4064-b637-87c2dbbd97a3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bb4484b3-b0ca-4e63-88f6-cb84192835b2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bbe36c0a-6070-475a-9db8-d75dbbdbf425"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bc13b36e-d30d-4795-aa0e-d0a69894534c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bc50c79a-5248-4b84-b4b7-027b2c59371d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bc598aaf-f398-40ff-99ad-0c608bb47484"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bc5a7dc6-be8a-47f3-b02e-5b2efe6a379e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bc985eea-5f97-4e02-8bea-c7987fcfa7dd"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bc9d011b-b150-4d67-843f-d52fcc7c026a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bcf74c38-ce96-4cbf-8656-eb25021d23b0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bcfc88e8-ead4-4e40-9d7e-be7896adbd4a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bd24c256-a61d-4c8e-b656-0f90bd2cff3d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bd42b32e-820f-4961-a732-5e3bcb953f3f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bd6dc2e0-43d6-42d7-8f78-d424abeaef2b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bd77d4f6-5a7e-48e4-a4fb-6c2e6bb41a60"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bd7f3cf7-8686-45eb-932e-a4b0d1853149"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bdb0bc8f-01de-4c50-bd28-f9536cb91355"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("be016df2-0002-46b3-afa2-eb4be27cda7d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("be836647-61ad-4068-824d-06198752c7e0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("becd2e23-8d27-42b8-9b03-d96571023580"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bf02e2de-09bf-4f0f-bd63-6e8dd1dc5c8e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bfa31f50-c61d-4ff9-b92a-193242ebf89e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bfaa15e7-93f6-4b1a-822d-7a6bf5e1ecb6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bfaf326c-6452-4ce0-84e9-18fb607d0c2f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bfcbe8f5-4e7c-44e2-880f-95a96e738ee6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bfe242b1-00bb-4e30-a0a8-9614bbd2fd52"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c0185c82-7014-4a10-85a5-a644cbf4572b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c0754abf-09ff-429f-a464-fc9d00834b26"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c07e2713-e938-4f52-92ff-ef19624eb235"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c109b0b2-33f3-4d29-8d37-e25623d9a2f6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c1a0da74-9e02-43fc-beec-14589057478c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c1adb3e8-8d55-4a30-86e1-8834c328e6f0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c1c90529-f7d0-40d0-b644-9c15e3046c25"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c1ce55db-1f5b-4ad1-92ee-2c636716110d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c1e0fe37-5d3f-436a-bbcf-adf64a269f37"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c1fba9f4-1d9f-45fe-b27c-71bd354dc286"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c267a057-4558-4f6a-aa42-3a034e868d64"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c2769933-d4a2-4571-90a3-d624c0ab6c70"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c29f5a18-29e6-45ca-9f70-f436002750ae"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c2e2f1fe-7a7e-4f18-8c45-869a53dd6124"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c2e40f80-03ff-47f4-b092-a5176085bf35"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c2e5388f-a002-4e74-b924-ae34eb7fd6b0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c39362be-0a0c-440a-a471-5daa4d41b33a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c3bf9ab5-a70f-425d-9c61-4d270ae5a80c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c3d08dd5-1216-4448-b62c-a0d2ca781294"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c4460d63-44e4-412c-8de9-b052514e41b5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c499b778-7933-49dc-b944-155184fdf522"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c4f4d541-f5ec-4f12-b04d-ee2d1dbdedff"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c562c7b6-2cf8-4ec1-8cb2-2fd0d40169aa"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c58dfe16-bcf6-4380-97e2-a2250c63b41d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c5a3dc21-16c3-4d47-a945-331d08f42f89"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c5db5f25-9997-4df8-8c5f-9307c5005843"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c62075a8-2590-4623-a907-43d3109f6139"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c62715cd-d9c2-460c-a2fb-7ded14087b63"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c6b597dd-0f08-47b1-95a9-2aba06df4db9"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c6f1ace8-1305-4cd6-a1ad-db34fca5105f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c717489d-8ca6-4da9-91fa-f22bc9cd7427"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c72ae942-1baa-4136-a21f-39438bf52f71"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c7ce583f-996d-4116-afca-71e1efe96efe"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c8088eb9-9205-42a4-9e5e-92679f36123b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c80bb3a7-eef4-48a6-aeed-a3e6ea055b17"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c860f05d-6460-402e-a6f9-cacf8e963396"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c88a545c-fe4a-4bff-ab44-0f00f40e721f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c89248df-265c-4881-a9ae-2aa39b6a7a5c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c910f3d0-3549-4ae8-be9b-3a1ee795b22e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c9235f5d-775f-40de-aa3b-c9dec5678d73"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c9256947-12e1-4cca-bce3-50124b7fd273"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c9c50578-f0c3-4ff0-8438-409c9a17a7a0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ca29f4a2-7b4e-4d06-aeb3-a08ff41d2978"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("cad2c172-1c1b-4194-81ee-582deef55f14"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("cb07a530-0aef-4909-a8d2-a364f9f8183b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("cb7c01c0-f60e-4071-b83f-3520279d6c4c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("cbcc7e03-0e5c-40b8-9dd2-e1e4ca2f2145"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("cc2a4a36-92dd-4ac1-97ee-9648940a0dea"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("cc2a6530-f000-4fd6-bc4e-33b1d7d861e6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("cc66f1f9-f212-4007-9797-bc61d23eb847"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ccb52a7d-4fa7-40e9-a737-67122b16f974"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ccef02e8-9448-4762-84c3-36e16f0bf43e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("cd24e785-e620-4163-a001-feac1082711c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("cd292c05-fcdb-44a1-9648-7b7521836236"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("cd65d55f-1eb0-43c2-a2f0-f08790090ac8"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("cd6e1eb8-0a2c-470d-8c2f-c8a6089a7e02"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("cdfa84d5-e31d-4920-bff0-53cbc78142f4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ce450a3a-d9b5-4db8-9472-d1a36936528c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("cea254e5-b953-4bf2-82f1-8304337ff73c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ceb2aa32-3740-43a3-aa8a-a153da3cc82f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("cebbe2c3-f6e8-49a9-adfc-7264fe6f0539"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ced155c4-7eff-4c5e-8b38-1123d717d0bd"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("cef2d5ad-29fa-4d9d-9dee-549c7802a5f0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("cf00498b-dbb6-48f1-83ec-aae4572daad4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("cf060c72-425b-47ac-a892-891658b59481"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("cf398c9f-2fb9-4f22-a5a7-21687b670fb0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("cfcedb91-8957-4483-98c8-a796b2d36508"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d016bc16-ed2e-43cd-837d-8867a4435c99"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d0424f82-f884-450e-b259-821f08f1c29f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d04eede9-ba6f-4190-9eaf-e93398312f5c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d059dd3f-7ee1-4fd3-bdd1-56034e08e99f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d0602bd5-530d-474e-bf74-532bfbfeb6d8"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d0a0c2bd-6974-46e5-b18c-63a6812f1d41"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d19da815-0dcb-469b-aae0-08b73255e1fd"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d1a119d9-92bd-4863-92b5-a03536c18cb1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d1de4314-6221-4270-a11f-4c230da0c71d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d20f8b13-6717-47ca-a235-f772a8d16514"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d21489d6-7d1e-46fb-9d11-c4b536285347"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d2207ecd-576b-43b9-9a26-bbe647c96c0a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d2558cd1-71ca-4c7f-802d-98498b2eaf90"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d2563ac7-9d6e-428f-8b4e-6cd1254f2612"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d27fe1ae-3758-48db-9da9-7c0ea3c6c5ca"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d30ce642-60ee-4c35-943e-3a5ca2075b72"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d32fe9db-0260-4107-a1f0-2f1e2c7615a4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d3717548-d4e0-499a-9ed0-f933624773af"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d383e248-a391-4cad-8394-85871b372412"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d3bf3541-5f1e-4bdb-b779-0225fef56501"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d43a3585-c851-48ae-9ffd-8cf9045befc2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d473c823-ec99-43f8-ae8f-23eb332b1b29"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d4bd1125-6c1e-4cfa-8f86-bc0ab6fc7216"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d4d54135-d9e7-4664-b6f2-38d91b3da119"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d586409a-1bf3-4b39-8d0e-7ca0537d1cef"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d5a39d4b-bfa0-4ff0-b6b5-d1b75d75d10c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d5b0e5e7-051f-4f72-9980-288125b680f1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d5e9268f-e45e-48cd-aa4d-35a846fecb45"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d66633d4-3d4a-435c-a8ae-90845e79a6e7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d6a781cb-700f-490d-b2c9-009b772be2dc"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d6bf4347-d6c1-49c2-9eac-f3cb57e082cc"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d6d3ad1b-1470-4840-bbd4-1127d47295cd"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d6f1b12c-0200-42c7-b8e9-7d2bdde46969"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d719e975-644b-4fda-b001-c77ab132a672"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d788a508-76be-494d-98b1-69319c71cff8"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d79cea01-9e36-48c2-8b0c-782e5d2bdd2a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d7d5c158-2e6f-4133-a601-14d729efb1f5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d7deb78d-a28f-488d-b31a-58fab992083d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d804bebd-f872-4b4a-868e-6879a0bf4bce"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d8705a5e-3662-406d-a540-afa447aabca3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d907b8c5-ea96-4ded-a696-5e906b2537bc"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d9348c9d-2b43-4439-bb05-3c1736a13e02"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d9863306-f367-4ffb-92fa-90301ddabf2a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d9968ce6-7359-494e-8848-71d5f97c2446"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("db023ca7-b477-4825-ba71-35046ee0337f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("db2877fd-2b4b-402e-9964-af271f1cd9ae"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("db50fe9a-3561-4848-8aa7-f04ef2f69fa2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("db51d970-e418-4755-b7cd-beedeb36137f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("db5d0848-c1cd-4e30-a28c-a6199621174c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("db5ec265-0e69-4987-9f3b-8632840fe10e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("db845686-5165-43fd-9848-2751e2e5d27e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("dbd45055-c1c6-46f5-828f-392e9c9ba04b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("dbf3318e-ce56-4ff7-b3d5-ef84c846b9eb"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("dc26d176-1277-4ce2-a1a9-4ccadd053e27"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("dc37346d-f34c-451b-90f0-35b5d69aae46"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("dc46aa79-983d-4c10-84c9-1181fddd2f4c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("dc491bb8-c451-4ec3-b010-f83f826b6232"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("dc90bc15-6ebe-4d24-b9d1-d1d2b2ace8eb"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("dd30aad5-4955-43fc-806e-3700ed14257e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("dd42651a-effa-460a-b5bc-3401bf75a628"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("dd4fbf70-7485-4afd-94a2-4e2f2fa550b7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("dd610c20-10eb-49ff-aed0-062780823320"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("de169d34-4dc6-4a05-87eb-20901416dafb"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("de3ccd70-38df-460b-9a20-e0857c55c292"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("de747d91-aa94-411e-a7c0-55873c96ad2c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("de919ba5-357f-4440-9e13-962762f90535"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ded7a769-fded-46ff-864d-f0d687db5b61"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("deda56b0-32e1-409b-8109-27f96c7308f7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("df433bc5-840a-42b2-b329-04df152de40e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("df4bf235-5604-4256-ab72-a9ccf68988ad"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("df8427d8-4e0e-45d4-ab5c-7bd9fdbb6a78"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("df9a25f3-dade-4fcc-acde-82e8ae84b5fb"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("dfe60214-36ba-43cb-b863-401a259747eb"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e0e07649-eeda-4df2-baea-ddb6836d1ac4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e0fd498c-85f1-49d2-a283-0dcc0dc367b6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e1dda57d-81ab-4c5e-bffc-c631204ed830"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e1e68b14-02cc-40a2-b7db-0f3ec16ecc89"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e1fc8251-56c4-47fb-8019-2bc0b22c6966"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e251a3ab-ebcf-4128-a93d-aefb37b39fda"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e26d81f7-1557-4193-bc39-0edb829e0f37"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e2dda559-e27c-4e94-99eb-814e4f9fd476"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e310a5bd-842e-4298-a162-667abb8e9dd2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e36442ed-35cb-40e0-882c-1c5d41ba3f68"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e3792b8f-19f7-4ae1-ad55-43655ae27012"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e3e38a75-0cd0-4f91-bd71-653752d7f209"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e4aaaad2-f694-491f-b83e-e35f43fcd643"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e4d073fa-9f5a-48d4-a0fd-10d4e667a634"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e57e2006-33bd-4432-acab-36b0d3237e81"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e5984fde-45f2-45fa-9d8a-bf17a8313d61"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e59daa5f-b8f2-441d-b116-9a5eb1ba05cd"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e5afd154-c762-434b-ac16-a54d71737d31"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e738594b-19ef-4a88-9dbf-a5a7a5ff443a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e78d3e47-1f78-4873-bef0-105912a4a28a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e78dc003-3cb5-4af6-bac6-822748137109"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e798fcd0-c648-4a84-aa52-4274711438e1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e7cf1925-02b2-4761-a526-98733667e908"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e8324d8b-03ab-47c6-8385-514fac6dca6a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e83fcec2-0045-4e25-b86d-95451479d08b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e883218f-e410-4256-aa98-47be80158122"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e88b61ba-4d65-4a7c-b7b2-833a86472322"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e9241b59-41a5-463a-8933-2abe381d91b6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e96c269c-e38f-4abc-8d92-02ab29ded69c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e97e21fb-2810-4664-a21c-cd845b44d4f8"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e9ca976c-fa94-4525-9eaa-f8f4f550249e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ea38954b-b569-46b4-ac8f-e36a236eb4d2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ea3f028d-3b47-4241-938c-e8c3ddf15e54"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("eaad3369-bcc6-4f59-9e05-a9f8503322da"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("eac084df-b5e4-449c-aa97-df6d3cbac41f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ead4dc97-a84a-4deb-a1ce-73cf295a0095"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("eaeb2563-5cff-44f4-91b2-e4ef6d28d6cc"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ecad63ff-94da-48ff-927a-db56bff7b4e7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ecb3de7f-012b-4f8c-a56e-6d5813cc01ad"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ecbc8d01-bd24-4f9f-85a7-cf3d3f4d4f22"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ed102eed-92f5-449e-8a9a-ac26b587c0a5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ed46f84f-98ba-444f-a27d-f0e27427cb1b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ed4c6fc7-cde2-4352-8829-d6d5516f6be9"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ed941380-859c-4c53-aed0-1f4655a8da3e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("eda85b84-4400-49fe-a011-cc06ea57aee3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("edce7d04-977d-424a-80a8-3782dafd1475"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("eddf6efc-738a-4235-9c80-54d7506556d2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ee749b92-c917-46da-adc3-b2a67faa98ed"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ee8f217b-e6e3-4838-be3a-9916b24be160"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("eea9a534-f4b5-457b-a8bb-73a2d40330b6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("eeaeea6e-08c1-4096-9866-588752a91764"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("eeb163a4-245b-403d-838c-f03b4e195b5c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("efc96ca7-099f-4d15-9420-de1e381833de"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f025f44d-515c-4e9c-aabe-e914d94e6694"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f025fbe2-464a-4f53-aa78-6e02c5c116b3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f026fdcd-7baf-4c96-994c-20e436cc8c59"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f0698af5-6f64-4a2e-b364-95779b438955"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f07e8eb9-d4ae-4591-b27e-d858be34fba2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f0ac5cc9-186e-4854-b103-98b515e91eac"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f0d93c98-3a4d-4efd-90b6-70d7cfb25617"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f0dd939f-2d7c-407c-9573-34a4fa543670"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f11294d0-1f00-4582-9d24-3707bf5280d6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f11f97b1-3e83-4c7a-a5ef-06c1846ce98e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f128c767-8664-40b9-a086-fd929ae133a2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f13b200d-fd21-4078-a951-fc5fc331718d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f143cc8c-b3bd-4225-a0ba-2ecfcd46144c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f1484bb2-5277-45f9-8549-0c4ea1255aff"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f16b7c49-8180-438f-a114-9a2e848cac52"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f1fbf3a6-4119-40c4-84fd-c7f06b45aacb"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f261f4c6-1812-48a1-851f-90e88cac5ddc"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f26b852b-1da4-443b-a062-44696556e5f3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f3083498-736f-421a-a001-feb4cf951422"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f370618c-e33a-4663-90fc-7c986e4053ae"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f376d84f-37b1-4013-bb85-209788983603"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f40b47db-0d19-40a3-80b1-55458b7ca68b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f41ce433-dee7-416c-b1ed-e723e740f5f8"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f424ce2a-d643-4cb6-99dd-98126cb2c2ce"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f4bf1e54-d0b4-421c-aa7e-eaa31101eff0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f4d45e6f-c8ef-4542-8dc5-a0c680d47e71"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f5326f7f-d87f-4f89-b7c2-36220872fcaf"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f5406aa8-f218-4ec3-a457-e2f73789d5c9"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f572f90a-512c-4271-825b-ef6c82c28610"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f5d814fc-d3a5-4b3b-af84-2895a05815f1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f5fb7f72-8df9-4b9a-b5b3-680b9c19d11f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f606a608-5d17-4226-8fbf-92956ef34c27"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f6484b85-6324-42ae-b650-5bbcc8e41950"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f6558ad2-1df5-4d80-8f5d-17fa2235b97b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f6801336-866e-4767-9019-b4b576acb729"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f688b1b4-7f21-42ef-b23e-9d69ea97cd05"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f696ad39-f193-415c-9b25-e29a00aa2315"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f6af9dae-a163-419f-ade9-d4231621e518"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f6f75ed0-c2ba-4af1-b62a-28559b3f34ef"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f72c4f52-85c3-4ef8-9abf-b636b293b417"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f768b1f6-dfc3-43bd-ab29-f03277c812b3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f778894d-48c3-4f1d-99ed-7755507682fa"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f788e479-e3de-4372-87f6-00ad90df8132"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f7d4b02a-494c-466e-b7d6-9a11cb09267a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f7f36ffc-549a-4ea1-8802-3efcf7b46ab6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f81364f2-4002-4d20-8728-3997c24a62b4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f835d1ab-0e33-4291-b567-60aac42f8c6e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f862ce99-cca8-46f1-baa9-e7407c7093d6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f8c58840-0164-47e2-af08-9913cd994f64"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f8d1338d-9db8-4866-b060-c3ca4a7a3d08"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f8e5aac4-adb0-4ecf-87d1-851f32559c76"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f8eeccf0-529d-4a86-ac0a-8795a315b33a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f934b137-6e22-4dc8-bcc4-272f75d7f6df"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f937f259-3e02-4207-b62f-5528bc469424"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f9454382-9fbd-4524-8b65-04c1b449469c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f9509f7f-1858-49cf-9bdb-0af54a20afdd"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f98655e6-a6cc-4759-8e6f-5bb39dd74403"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f99825d4-8949-42bd-af1c-3ad4eb9b14e1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fa1d124f-8a8e-4555-941c-8db2a0964659"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fa6a9204-08c5-41ae-911e-d3510ff9b9ba"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fa7f0d01-6347-41c3-b183-e504eb17cc6c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fa8f6ee9-81a3-4d37-ba59-2399036597e9"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fa8fcdc3-4477-4b51-afc6-1deb64bf99fe"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fb02a43a-455a-4e3c-a16e-3799bfcf6312"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fb9ed9e8-6779-427e-bafb-8c5c9cb2a119"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fbcc8132-cdd9-4a07-9305-12ed8755ca17"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fbf5eed1-36ed-49ef-8205-89fc822b3f28"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fc02f232-e6c9-477a-bc1c-28b800038857"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fc53ef0a-c36f-46d9-b6f0-f8f86cd9cb00"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fc865a59-a750-4924-84bf-75d222287f4d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fc9372fa-0d90-4257-9b2f-e4a80aaee992"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fcaec936-3aff-4b19-a1a0-048ea2a06ae0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fcbb64dd-c63a-4f4c-9925-617cdbf3ff51"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fcecc812-b678-42b4-9273-d7766014e515"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fd423059-a4c8-4eab-bc72-9da422524e12"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fd8337ea-cb3f-46b0-8586-fdd9dbd64862"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fd883e81-6f40-4d86-a7b0-776a1b39bc75"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fd8ca1cc-a143-4cf5-a03d-ca1e02014964"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fd9c1d05-d245-4da8-b447-34b0f59244c6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fdab0616-a8a2-4a2d-9cdb-e664f3820b43"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fe09a91e-aa63-47b0-9464-b00d32ebfe54"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fe42ede2-95f5-4a2b-a6f9-6c7b4fd66bf4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fe6a5f2d-0389-45de-8391-56d5b0f65374"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ff098a16-6549-4b31-ae3f-e23f5d68713e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ff62a052-f283-4f0b-804a-bf5d744ef777"));
        }
    }
}
