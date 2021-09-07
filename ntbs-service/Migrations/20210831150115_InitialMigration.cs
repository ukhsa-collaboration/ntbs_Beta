using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ReferenceData");

            migrationBuilder.CreateTable(
                name: "Country",
                schema: "ReferenceData",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsoCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasHighTbOccurence = table.Column<bool>(type: "bit", nullable: false),
                    IsLegacy = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "Ethnicity",
                schema: "ReferenceData",
                columns: table => new
                {
                    EthnicityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Label = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ethnicity", x => x.EthnicityId);
                });

            migrationBuilder.CreateTable(
                name: "FrequentlyAskedQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    AnchorLink = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrequentlyAskedQuestion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LegacyImportMigrationRun",
                columns: table => new
                {
                    LegacyImportMigrationRunId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppRelease = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LegacyIdList = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RangeStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RangeEndDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegacyImportMigrationRun", x => x.LegacyImportMigrationRunId);
                });

            migrationBuilder.CreateTable(
                name: "LocalAuthority",
                schema: "ReferenceData",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalAuthority", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "ManualTestType",
                schema: "ReferenceData",
                columns: table => new
                {
                    ManualTestTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManualTestType", x => x.ManualTestTypeId);
                });

            migrationBuilder.CreateTable(
                name: "NotificationAndDuplicateIds",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    DuplicateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "NotificationGroup",
                columns: table => new
                {
                    NotificationGroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationGroup", x => x.NotificationGroupId);
                });

            migrationBuilder.CreateTable(
                name: "Occupation",
                schema: "ReferenceData",
                columns: table => new
                {
                    OccupationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sector = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    HasFreeTextField = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Occupation", x => x.OccupationId);
                });

            migrationBuilder.CreateTable(
                name: "PHEC",
                schema: "ReferenceData",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AdGroup = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PHEC", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "ReleaseVersion",
                columns: table => new
                {
                    Version = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseVersion", x => x.Version);
                });

            migrationBuilder.CreateTable(
                name: "SampleType",
                schema: "ReferenceData",
                columns: table => new
                {
                    SampleTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SampleType", x => x.SampleTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Sex",
                schema: "ReferenceData",
                columns: table => new
                {
                    SexId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Label = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sex", x => x.SexId);
                });

            migrationBuilder.CreateTable(
                name: "Site",
                schema: "ReferenceData",
                columns: table => new
                {
                    SiteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Site", x => x.SiteId);
                });

            migrationBuilder.CreateTable(
                name: "TreatmentOutcome",
                schema: "ReferenceData",
                columns: table => new
                {
                    TreatmentOutcomeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TreatmentOutcomeType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TreatmentOutcomeSubType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentOutcome", x => x.TreatmentOutcomeId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    GivenName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    FamilyName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    AdGroups = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsCaseManager = table.Column<bool>(type: "bit", nullable: false),
                    IsReadOnly = table.Column<bool>(type: "bit", nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmailPrimary = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmailSecondary = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhoneNumberPrimary = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhoneNumberSecondary = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLoginEvent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    LoginDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SystemName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLoginEvent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VenueType",
                schema: "ReferenceData",
                columns: table => new
                {
                    VenueTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VenueType", x => x.VenueTypeId);
                });

            migrationBuilder.CreateTable(
                name: "LegacyImportNotificationLogMessage",
                columns: table => new
                {
                    LegacyImportNotificationLogMessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LegacyImportMigrationRunId = table.Column<int>(type: "int", nullable: false),
                    OldNotificationId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LogMessageLevel = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegacyImportNotificationLogMessage", x => x.LegacyImportNotificationLogMessageId);
                    table.ForeignKey(
                        name: "FK_LegacyImportNotificationLogMessage_LegacyImportMigrationRun_LegacyImportMigrationRunId",
                        column: x => x.LegacyImportMigrationRunId,
                        principalTable: "LegacyImportMigrationRun",
                        principalColumn: "LegacyImportMigrationRunId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LegacyImportNotificationOutcome",
                columns: table => new
                {
                    LegacyImportNotificationOutcomeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LegacyImportMigrationRunId = table.Column<int>(type: "int", nullable: false),
                    OldNotificationId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NtbsId = table.Column<int>(type: "int", nullable: true),
                    SuccessfullyMigrated = table.Column<bool>(type: "bit", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegacyImportNotificationOutcome", x => x.LegacyImportNotificationOutcomeId);
                    table.ForeignKey(
                        name: "FK_LegacyImportNotificationOutcome_LegacyImportMigrationRun_LegacyImportMigrationRunId",
                        column: x => x.LegacyImportMigrationRunId,
                        principalTable: "LegacyImportMigrationRun",
                        principalColumn: "LegacyImportMigrationRunId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostcodeLookup",
                schema: "ReferenceData",
                columns: table => new
                {
                    Postcode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LocalAuthorityCode = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    CountryCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PCT = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostcodeLookup", x => x.Postcode);
                    table.ForeignKey(
                        name: "FK_PostcodeLookup_LocalAuthority_LocalAuthorityCode",
                        column: x => x.LocalAuthorityCode,
                        principalSchema: "ReferenceData",
                        principalTable: "LocalAuthority",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LegacySource = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ETSID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LTBRPatientId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LTBRID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletionReason = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    GroupId = table.Column<int>(type: "int", nullable: true),
                    ClusterId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NotificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NotificationStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notification_NotificationGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "NotificationGroup",
                        principalColumn: "NotificationGroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LocalAuthorityToPHEC",
                schema: "ReferenceData",
                columns: table => new
                {
                    PHECCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LocalAuthorityCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalAuthorityToPHEC", x => new { x.PHECCode, x.LocalAuthorityCode });
                    table.ForeignKey(
                        name: "FK_LocalAuthorityToPHEC_LocalAuthority_LocalAuthorityCode",
                        column: x => x.LocalAuthorityCode,
                        principalSchema: "ReferenceData",
                        principalTable: "LocalAuthority",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LocalAuthorityToPHEC_PHEC_PHECCode",
                        column: x => x.PHECCode,
                        principalSchema: "ReferenceData",
                        principalTable: "PHEC",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbService",
                schema: "ReferenceData",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ServiceAdGroup = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    PHECCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsLegacy = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbService", x => x.Code);
                    table.ForeignKey(
                        name: "FK_TbService_PHEC_PHECCode",
                        column: x => x.PHECCode,
                        principalSchema: "ReferenceData",
                        principalTable: "PHEC",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ManualTestTypeSampleType",
                schema: "ReferenceData",
                columns: table => new
                {
                    ManualTestTypeId = table.Column<int>(type: "int", nullable: false),
                    SampleTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManualTestTypeSampleType", x => new { x.ManualTestTypeId, x.SampleTypeId });
                    table.ForeignKey(
                        name: "FK_ManualTestTypeSampleType_ManualTestType_ManualTestTypeId",
                        column: x => x.ManualTestTypeId,
                        principalSchema: "ReferenceData",
                        principalTable: "ManualTestType",
                        principalColumn: "ManualTestTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ManualTestTypeSampleType_SampleType_SampleTypeId",
                        column: x => x.SampleTypeId,
                        principalSchema: "ReferenceData",
                        principalTable: "SampleType",
                        principalColumn: "SampleTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClinicalDetails",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    IsSymptomatic = table.Column<bool>(type: "bit", nullable: true),
                    SymptomStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FirstPresentationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TBServicePresentationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DiagnosisDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TreatmentStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartedTreatment = table.Column<bool>(type: "bit", nullable: true),
                    IsPostMortem = table.Column<bool>(type: "bit", nullable: true),
                    BCGVaccinationState = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    BCGVaccinationYear = table.Column<int>(type: "int", nullable: true),
                    HIVTestState = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    TreatmentRegimen = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    MDRTreatmentStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TreatmentRegimenOtherDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsDotOffered = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    DotStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    EnhancedCaseManagementStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    EnhancedCaseManagementLevel = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    HomeVisitCarriedOut = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    FirstHomeVisitDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HealthcareSetting = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    HealthcareDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicalDetails", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_ClinicalDetails_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComorbidityDetails",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    DiabetesStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    HepatitisBStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    HepatitisCStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    LiverDiseaseStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    RenalDiseaseStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComorbidityDetails", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_ComorbidityDetails_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactTracing",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    AdultsIdentified = table.Column<int>(type: "int", nullable: true),
                    ChildrenIdentified = table.Column<int>(type: "int", nullable: true),
                    AdultsScreened = table.Column<int>(type: "int", nullable: true),
                    ChildrenScreened = table.Column<int>(type: "int", nullable: true),
                    AdultsActiveTB = table.Column<int>(type: "int", nullable: true),
                    ChildrenActiveTB = table.Column<int>(type: "int", nullable: true),
                    AdultsLatentTB = table.Column<int>(type: "int", nullable: true),
                    ChildrenLatentTB = table.Column<int>(type: "int", nullable: true),
                    AdultsStartedTreatment = table.Column<int>(type: "int", nullable: true),
                    ChildrenStartedTreatment = table.Column<int>(type: "int", nullable: true),
                    AdultsFinishedTreatment = table.Column<int>(type: "int", nullable: true),
                    ChildrenFinishedTreatment = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactTracing", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_ContactTracing_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DenotificationDetails",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    DateOfDenotification = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    OtherDescription = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DenotificationDetails", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_DenotificationDetails_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DrugResistanceProfile",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    Species = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "No result"),
                    DrugResistanceProfileString = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "No result")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugResistanceProfile", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_DrugResistanceProfile_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImmunosuppressionDetails",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    HasBioTherapy = table.Column<bool>(type: "bit", nullable: true),
                    HasTransplantation = table.Column<bool>(type: "bit", nullable: true),
                    HasOther = table.Column<bool>(type: "bit", nullable: true),
                    OtherDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImmunosuppressionDetails", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_ImmunosuppressionDetails_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MBovisDetails",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    ExposureToKnownCasesStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    UnpasteurisedMilkConsumptionStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    OccupationExposureStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    AnimalExposureStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MBovisDetails", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_MBovisDetails_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MDRDetails",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    ExposureToKnownCaseStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    RelationshipToCase = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    NotifiedToPheStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    RelatedNotificationId = table.Column<int>(type: "int", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MDRDetails", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_MDRDetails_Country_CountryId",
                        column: x => x.CountryId,
                        principalSchema: "ReferenceData",
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MDRDetails_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationSite",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    SiteDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationSite", x => new { x.NotificationId, x.SiteId });
                    table.ForeignKey(
                        name: "FK_NotificationSite_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationSite_Site_SiteId",
                        column: x => x.SiteId,
                        principalSchema: "ReferenceData",
                        principalTable: "Site",
                        principalColumn: "SiteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    FamilyName = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    GivenName = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    NhsNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NhsNumberNotKnown = table.Column<bool>(type: "bit", nullable: false),
                    LocalPatientId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Postcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostcodeToLookup = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    NoFixedAbode = table.Column<bool>(type: "bit", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    YearOfUkEntry = table.Column<int>(type: "int", nullable: true),
                    EthnicityId = table.Column<int>(type: "int", nullable: true),
                    SexId = table.Column<int>(type: "int", nullable: true),
                    OccupationId = table.Column<int>(type: "int", nullable: true),
                    OccupationOther = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Patients_Country_CountryId",
                        column: x => x.CountryId,
                        principalSchema: "ReferenceData",
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Patients_Ethnicity_EthnicityId",
                        column: x => x.EthnicityId,
                        principalSchema: "ReferenceData",
                        principalTable: "Ethnicity",
                        principalColumn: "EthnicityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Patients_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Patients_Occupation_OccupationId",
                        column: x => x.OccupationId,
                        principalSchema: "ReferenceData",
                        principalTable: "Occupation",
                        principalColumn: "OccupationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Patients_PostcodeLookup_PostcodeToLookup",
                        column: x => x.PostcodeToLookup,
                        principalSchema: "ReferenceData",
                        principalTable: "PostcodeLookup",
                        principalColumn: "Postcode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Patients_Sex_SexId",
                        column: x => x.SexId,
                        principalSchema: "ReferenceData",
                        principalTable: "Sex",
                        principalColumn: "SexId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PreviousTbHistory",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    PreviouslyHadTb = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    PreviousTbDiagnosisYear = table.Column<int>(type: "int", nullable: true),
                    PreviouslyTreated = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    PreviousTreatmentCountryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreviousTbHistory", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_PreviousTbHistory_Country_PreviousTreatmentCountryId",
                        column: x => x.PreviousTreatmentCountryId,
                        principalSchema: "ReferenceData",
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PreviousTbHistory_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PreviousTbService",
                columns: table => new
                {
                    PreviousTbServiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TbServiceCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhecCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransferDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreviousTbService", x => x.PreviousTbServiceId);
                    table.ForeignKey(
                        name: "FK_PreviousTbService_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SocialContextAddress",
                columns: table => new
                {
                    SocialContextAddressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Postcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Details = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialContextAddress", x => x.SocialContextAddressId);
                    table.ForeignKey(
                        name: "FK_SocialContextAddress_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SocialContextVenue",
                columns: table => new
                {
                    SocialContextVenueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VenueTypeId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Frequency = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Postcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Details = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialContextVenue", x => x.SocialContextVenueId);
                    table.ForeignKey(
                        name: "FK_SocialContextVenue_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SocialContextVenue_VenueType_VenueTypeId",
                        column: x => x.VenueTypeId,
                        principalSchema: "ReferenceData",
                        principalTable: "VenueType",
                        principalColumn: "VenueTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SocialRiskFactors",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    AlcoholMisuseStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    MentalHealthStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    AsylumSeekerStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ImmigrationDetaineeStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialRiskFactors", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_SocialRiskFactors_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestData",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    HasTestCarriedOut = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestData", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_TestData_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TravelDetails",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    HasTravel = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    TotalNumberOfCountries = table.Column<int>(type: "int", nullable: true),
                    Country1Id = table.Column<int>(type: "int", nullable: true),
                    StayLengthInMonths1 = table.Column<int>(type: "int", nullable: true),
                    Country2Id = table.Column<int>(type: "int", nullable: true),
                    StayLengthInMonths2 = table.Column<int>(type: "int", nullable: true),
                    Country3Id = table.Column<int>(type: "int", nullable: true),
                    StayLengthInMonths3 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelDetails", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_TravelDetails_Country_Country1Id",
                        column: x => x.Country1Id,
                        principalSchema: "ReferenceData",
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TravelDetails_Country_Country2Id",
                        column: x => x.Country2Id,
                        principalSchema: "ReferenceData",
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TravelDetails_Country_Country3Id",
                        column: x => x.Country3Id,
                        principalSchema: "ReferenceData",
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TravelDetails_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VisitorDetails",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    HasVisitor = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    TotalNumberOfCountries = table.Column<int>(type: "int", nullable: true),
                    Country1Id = table.Column<int>(type: "int", nullable: true),
                    StayLengthInMonths1 = table.Column<int>(type: "int", nullable: true),
                    Country2Id = table.Column<int>(type: "int", nullable: true),
                    StayLengthInMonths2 = table.Column<int>(type: "int", nullable: true),
                    Country3Id = table.Column<int>(type: "int", nullable: true),
                    StayLengthInMonths3 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitorDetails", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_VisitorDetails_Country_Country1Id",
                        column: x => x.Country1Id,
                        principalSchema: "ReferenceData",
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisitorDetails_Country_Country2Id",
                        column: x => x.Country2Id,
                        principalSchema: "ReferenceData",
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisitorDetails_Country_Country3Id",
                        column: x => x.Country3Id,
                        principalSchema: "ReferenceData",
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisitorDetails_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alert",
                columns: table => new
                {
                    AlertId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationId = table.Column<int>(type: "int", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AlertStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ClosureDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClosingUserId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    AlertType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DuplicateId = table.Column<int>(type: "int", nullable: true),
                    TransferReason = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    OtherReasonDescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TransferRequestNote = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TbServiceCode = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    CaseManagerId = table.Column<int>(type: "int", nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DecliningUserAndTbServiceString = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SpecimenId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alert", x => x.AlertId);
                    table.ForeignKey(
                        name: "FK_Alert_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Alert_TbService_TbServiceCode",
                        column: x => x.TbServiceCode,
                        principalSchema: "ReferenceData",
                        principalTable: "TbService",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Alert_User_CaseManagerId",
                        column: x => x.CaseManagerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CaseManagerTbService",
                columns: table => new
                {
                    TbServiceCode = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    CaseManagerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseManagerTbService", x => new { x.CaseManagerId, x.TbServiceCode });
                    table.ForeignKey(
                        name: "FK_CaseManagerTbService_TbService_TbServiceCode",
                        column: x => x.TbServiceCode,
                        principalSchema: "ReferenceData",
                        principalTable: "TbService",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseManagerTbService_User_CaseManagerId",
                        column: x => x.CaseManagerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hospital",
                schema: "ReferenceData",
                columns: table => new
                {
                    HospitalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CountryCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TBServiceCode = table.Column<string>(type: "nvarchar(16)", nullable: true),
                    IsLegacy = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hospital", x => x.HospitalId);
                    table.ForeignKey(
                        name: "FK_Hospital_TbService_TBServiceCode",
                        column: x => x.TBServiceCode,
                        principalSchema: "ReferenceData",
                        principalTable: "TbService",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TreatmentEvent",
                columns: table => new
                {
                    TreatmentEventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TreatmentEventType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TreatmentOutcomeId = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    TbServiceCode = table.Column<string>(type: "nvarchar(16)", nullable: true),
                    CaseManagerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentEvent", x => x.TreatmentEventId);
                    table.ForeignKey(
                        name: "FK_TreatmentEvent_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TreatmentEvent_TbService_TbServiceCode",
                        column: x => x.TbServiceCode,
                        principalSchema: "ReferenceData",
                        principalTable: "TbService",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TreatmentEvent_TreatmentOutcome_TreatmentOutcomeId",
                        column: x => x.TreatmentOutcomeId,
                        principalSchema: "ReferenceData",
                        principalTable: "TreatmentOutcome",
                        principalColumn: "TreatmentOutcomeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TreatmentEvent_User_CaseManagerId",
                        column: x => x.CaseManagerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MBovisAnimalExposure",
                columns: table => new
                {
                    MBovisAnimalExposureId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    YearOfExposure = table.Column<int>(type: "int", nullable: true),
                    AnimalType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Animal = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    AnimalTbStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ExposureDuration = table.Column<int>(type: "int", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    OtherDetails = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MBovisAnimalExposure", x => x.MBovisAnimalExposureId);
                    table.ForeignKey(
                        name: "FK_MBovisAnimalExposure_Country_CountryId",
                        column: x => x.CountryId,
                        principalSchema: "ReferenceData",
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MBovisAnimalExposure_MBovisDetails_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "MBovisDetails",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MBovisExposureToKnownCase",
                columns: table => new
                {
                    MBovisExposureToKnownCaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    YearOfExposure = table.Column<int>(type: "int", nullable: true),
                    ExposureSetting = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ExposureNotificationId = table.Column<int>(type: "int", nullable: true),
                    NotifiedToPheStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    OtherDetails = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MBovisExposureToKnownCase", x => x.MBovisExposureToKnownCaseId);
                    table.ForeignKey(
                        name: "FK_MBovisExposureToKnownCase_MBovisDetails_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "MBovisDetails",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MBovisOccupationExposures",
                columns: table => new
                {
                    MBovisOccupationExposureId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    YearOfExposure = table.Column<int>(type: "int", nullable: true),
                    OccupationSetting = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    OccupationDuration = table.Column<int>(type: "int", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    OtherDetails = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MBovisOccupationExposures", x => x.MBovisOccupationExposureId);
                    table.ForeignKey(
                        name: "FK_MBovisOccupationExposures_Country_CountryId",
                        column: x => x.CountryId,
                        principalSchema: "ReferenceData",
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MBovisOccupationExposures_MBovisDetails_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "MBovisDetails",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MBovisUnpasteurisedMilkConsumption",
                columns: table => new
                {
                    MBovisUnpasteurisedMilkConsumptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    YearOfConsumption = table.Column<int>(type: "int", nullable: true),
                    MilkProductType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ConsumptionFrequency = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    OtherDetails = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MBovisUnpasteurisedMilkConsumption", x => x.MBovisUnpasteurisedMilkConsumptionId);
                    table.ForeignKey(
                        name: "FK_MBovisUnpasteurisedMilkConsumption_Country_CountryId",
                        column: x => x.CountryId,
                        principalSchema: "ReferenceData",
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MBovisUnpasteurisedMilkConsumption_MBovisDetails_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "MBovisDetails",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiskFactorDrugs",
                columns: table => new
                {
                    SocialRiskFactorsNotificationId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "Drugs"),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: true),
                    InPastFiveYears = table.Column<bool>(type: "bit", nullable: true),
                    MoreThanFiveYearsAgo = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskFactorDrugs", x => x.SocialRiskFactorsNotificationId);
                    table.ForeignKey(
                        name: "FK_RiskFactorDrugs_SocialRiskFactors_SocialRiskFactorsNotificationId",
                        column: x => x.SocialRiskFactorsNotificationId,
                        principalTable: "SocialRiskFactors",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiskFactorHomelessness",
                columns: table => new
                {
                    SocialRiskFactorsNotificationId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "Homelessness"),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: true),
                    InPastFiveYears = table.Column<bool>(type: "bit", nullable: true),
                    MoreThanFiveYearsAgo = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskFactorHomelessness", x => x.SocialRiskFactorsNotificationId);
                    table.ForeignKey(
                        name: "FK_RiskFactorHomelessness_SocialRiskFactors_SocialRiskFactorsNotificationId",
                        column: x => x.SocialRiskFactorsNotificationId,
                        principalTable: "SocialRiskFactors",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiskFactorImprisonment",
                columns: table => new
                {
                    SocialRiskFactorsNotificationId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "Imprisonment"),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: true),
                    InPastFiveYears = table.Column<bool>(type: "bit", nullable: true),
                    MoreThanFiveYearsAgo = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskFactorImprisonment", x => x.SocialRiskFactorsNotificationId);
                    table.ForeignKey(
                        name: "FK_RiskFactorImprisonment_SocialRiskFactors_SocialRiskFactorsNotificationId",
                        column: x => x.SocialRiskFactorsNotificationId,
                        principalTable: "SocialRiskFactors",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiskFactorSmoking",
                columns: table => new
                {
                    SocialRiskFactorsNotificationId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "Smoking"),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: true),
                    InPastFiveYears = table.Column<bool>(type: "bit", nullable: true),
                    MoreThanFiveYearsAgo = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskFactorSmoking", x => x.SocialRiskFactorsNotificationId);
                    table.ForeignKey(
                        name: "FK_RiskFactorSmoking_SocialRiskFactors_SocialRiskFactorsNotificationId",
                        column: x => x.SocialRiskFactorsNotificationId,
                        principalTable: "SocialRiskFactors",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ManualTestResult",
                columns: table => new
                {
                    ManualTestResultId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    TestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ManualTestTypeId = table.Column<int>(type: "int", nullable: false),
                    SampleTypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManualTestResult", x => x.ManualTestResultId);
                    table.ForeignKey(
                        name: "FK_ManualTestResult_ManualTestType_ManualTestTypeId",
                        column: x => x.ManualTestTypeId,
                        principalSchema: "ReferenceData",
                        principalTable: "ManualTestType",
                        principalColumn: "ManualTestTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ManualTestResult_SampleType_SampleTypeId",
                        column: x => x.SampleTypeId,
                        principalSchema: "ReferenceData",
                        principalTable: "SampleType",
                        principalColumn: "SampleTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ManualTestResult_TestData_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "TestData",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HospitalDetails",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    Consultant = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CaseManagerId = table.Column<int>(type: "int", nullable: true),
                    TBServiceCode = table.Column<string>(type: "nvarchar(16)", nullable: true),
                    HospitalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HospitalDetails", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_HospitalDetails_Hospital_HospitalId",
                        column: x => x.HospitalId,
                        principalSchema: "ReferenceData",
                        principalTable: "Hospital",
                        principalColumn: "HospitalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HospitalDetails_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HospitalDetails_TbService_TBServiceCode",
                        column: x => x.TBServiceCode,
                        principalSchema: "ReferenceData",
                        principalTable: "TbService",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HospitalDetails_User_CaseManagerId",
                        column: x => x.CaseManagerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "ReferenceData",
                table: "Country",
                columns: new[] { "CountryId", "HasHighTbOccurence", "IsLegacy", "IsoCode", "Name" },
                values: new object[,]
                {
                    { 138, true, false, "MH", "Marshall Islands" },
                    { 145, true, false, "MD", "Moldova" },
                    { 144, true, false, "FM", "Micronesia, Federated States of" },
                    { 143, true, false, "MX", "Mexico" },
                    { 142, true, false, "YT", "Mayotte" },
                    { 141, true, false, "MU", "Mauritius" },
                    { 140, true, false, "MR", "Mauritania" },
                    { 139, true, false, "MQ", "Martinique" },
                    { 137, true, false, "MT", "Malta" },
                    { 136, true, false, "ML", "Mali" },
                    { 135, true, false, "MV", "Maldives" },
                    { 134, true, false, "MY", "Malaysia" },
                    { 133, true, false, "MW", "Malawi" },
                    { 132, true, false, "MG", "Madagascar" },
                    { 131, true, false, "MK", "North Macedonia" },
                    { 130, true, false, "MO", "Macao" },
                    { 129, false, false, "LU", "Luxembourg" },
                    { 128, true, false, "LT", "Lithuania" },
                    { 146, false, false, "MC", "Monaco" },
                    { 147, true, false, "MN", "Mongolia" },
                    { 148, true, false, "ME", "Montenegro" },
                    { 149, true, false, "MS", "Montserrat" },
                    { 167, true, false, "OM", "Oman" },
                    { 166, false, false, "NO", "Norway" },
                    { 165, true, false, "MP", "Northern Mariana Islands" },
                    { 164, true, false, "NF", "Norfolk Island" },
                    { 163, true, false, "NU", "Niue" },
                    { 162, true, false, "NG", "Nigeria" },
                    { 161, true, false, "NE", "Niger" },
                    { 160, true, false, "NI", "Nicaragua" },
                    { 127, false, false, "LI", "Liechtenstein" },
                    { 159, false, false, "NZ", "New Zealand" },
                    { 157, true, false, "AN", "Netherlands Antilles" },
                    { 156, false, false, "NL", "Netherlands" },
                    { 155, true, false, "NP", "Nepal" },
                    { 154, true, false, "NR", "Nauru" },
                    { 153, true, false, "NA", "Namibia" },
                    { 152, true, false, "MM", "Myanmar" },
                    { 151, true, false, "MZ", "Mozambique" },
                    { 150, true, false, "MA", "Morocco" },
                    { 158, true, false, "NC", "New Caledonia" },
                    { 168, true, false, "  ", "Other" }
                });

            migrationBuilder.InsertData(
                schema: "ReferenceData",
                table: "Country",
                columns: new[] { "CountryId", "HasHighTbOccurence", "IsLegacy", "IsoCode", "Name" },
                values: new object[,]
                {
                    { 126, true, false, "LY", "Libyan Arab Jamahiriya" },
                    { 124, true, false, "LS", "Lesotho" },
                    { 101, true, false, "IN", "India" },
                    { 100, false, false, "IS", "Iceland" },
                    { 99, true, false, "HU", "Hungary" },
                    { 98, true, false, "HK", "Hong Kong" },
                    { 97, true, false, "HN", "Honduras" },
                    { 96, false, false, "VA", "Holy See (Vatican City State)" },
                    { 95, true, false, "HM", "Heard Island and Mcdonald Islands" },
                    { 94, true, false, "HT", "Haiti" },
                    { 93, true, false, "GY", "Guyana" },
                    { 92, true, false, "GW", "Guinea-Bissau" },
                    { 90, false, false, "GG", "Guernsey" },
                    { 89, true, false, "GT", "Guatemala" },
                    { 88, true, false, "GU", "Guam" },
                    { 87, true, false, "GP", "Guadeloupe" },
                    { 86, true, false, "GD", "Grenada" },
                    { 85, false, false, "GL", "Greenland" },
                    { 84, false, false, "GR", "Greece" },
                    { 102, true, false, "ID", "Indonesia" },
                    { 103, true, false, "IR", "Iran, Islamic Republic of" },
                    { 104, true, false, "IQ", "Iraq" },
                    { 105, false, false, "IE", "Ireland" },
                    { 123, true, false, "LB", "Lebanon" },
                    { 122, true, false, "LV", "Latvia" },
                    { 121, true, false, "LA", "Lao People's Democratic Republic" },
                    { 120, true, false, "KG", "Kyrgyzstan" },
                    { 119, true, false, "KW", "Kuwait" },
                    { 118, true, false, "XK", "Kosovo" },
                    { 117, true, false, "KR", "Korea, Republic of" },
                    { 116, true, false, "KP", "Korea, Democratic People's Republic of" },
                    { 125, true, false, "LR", "Liberia" },
                    { 115, true, false, "KI", "Kiribati" },
                    { 113, true, false, "KZ", "Kazakhstan" },
                    { 112, true, false, "JO", "Jordan" },
                    { 111, false, false, "JE", "Jersey" },
                    { 110, true, false, "JP", "Japan" },
                    { 109, true, false, "JM", "Jamaica" },
                    { 108, false, false, "IT", "Italy" },
                    { 107, true, false, "IL", "Israel" },
                    { 106, false, false, "IM", "Isle Of Man" },
                    { 114, true, false, "KE", "Kenya" }
                });

            migrationBuilder.InsertData(
                schema: "ReferenceData",
                table: "Country",
                columns: new[] { "CountryId", "HasHighTbOccurence", "IsLegacy", "IsoCode", "Name" },
                values: new object[,]
                {
                    { 83, true, false, "GI", "Gibraltar" },
                    { 169, true, false, "PK", "Pakistan" },
                    { 171, true, false, "PS", "Palestine, State of" },
                    { 231, true, false, "TV", "Tuvalu" },
                    { 230, true, false, "TC", "Turks and Caicos Islands" },
                    { 229, true, false, "TM", "Turkmenistan" },
                    { 228, true, false, "TR", "Turkey" },
                    { 227, true, false, "TN", "Tunisia" },
                    { 226, true, false, "TT", "Trinidad and Tobago" },
                    { 225, true, false, "TO", "Tonga" },
                    { 224, true, false, "TK", "Tokelau" },
                    { 223, true, false, "TG", "Togo" },
                    { 222, true, false, "TL", "Timor-Leste" },
                    { 221, true, false, "TH", "Thailand" },
                    { 220, true, false, "TZ", "Tanzania, United Republic of" },
                    { 219, true, false, "TJ", "Tajikistan" },
                    { 218, true, false, "TW", "Taiwan, Province of China" },
                    { 217, true, false, "SY", "Syrian Arab Republic" },
                    { 216, false, false, "CH", "Switzerland" },
                    { 215, false, false, "SE", "Sweden" },
                    { 232, true, false, "UG", "Uganda" },
                    { 233, true, false, "UA", "Ukraine" },
                    { 234, true, false, "AE", "United Arab Emirates" },
                    { 235, false, false, "GB", "United Kingdom" },
                    { 253, false, true, "ZR", "Zaire" },
                    { 252, false, true, "YU", "Yugoslavia" },
                    { 251, false, true, "CS", "Serbia & Montenegro" },
                    { 250, true, false, "ZW", "Zimbabwe" },
                    { 249, true, false, "ZM", "Zambia" },
                    { 248, true, false, "YE", "Yemen" },
                    { 247, true, false, "EH", "Western Sahara" },
                    { 246, true, false, "WF", "Wallis and Futuna" },
                    { 214, true, false, "SZ", "Swaziland" },
                    { 245, true, false, "VI", "Virgin Islands, U.S." },
                    { 243, true, false, "VN", "Viet Nam" },
                    { 242, true, false, "VE", "Venezuela" },
                    { 241, true, false, "VU", "Vanuatu" },
                    { 240, true, false, "UZ", "Uzbekistan" },
                    { 239, true, false, "UY", "Uruguay" },
                    { 238, true, false, "-", "Unknown" },
                    { 237, false, false, "UM", "United States Minor Outlying Islands" },
                    { 236, false, false, "US", "United States" }
                });

            migrationBuilder.InsertData(
                schema: "ReferenceData",
                table: "Country",
                columns: new[] { "CountryId", "HasHighTbOccurence", "IsLegacy", "IsoCode", "Name" },
                values: new object[,]
                {
                    { 244, true, false, "VG", "Virgin Islands, British" },
                    { 170, true, false, "PW", "Palau" },
                    { 213, true, false, "SJ", "Svalbard and Jan Mayen" },
                    { 211, true, false, "SD", "Sudan" },
                    { 188, true, false, "KN", "Saint Kitts and Nevis" },
                    { 187, true, false, "SH", "Saint Helena" },
                    { 186, true, false, "BL", "Saint Barthélemy" },
                    { 185, true, false, "RW", "Rwanda" },
                    { 184, true, false, "RU", "Russian Federation" },
                    { 183, true, false, "RO", "Romania" },
                    { 182, true, false, "RE", "Reunion" },
                    { 181, true, false, "QA", "Qatar" },
                    { 180, true, false, "PR", "Puerto Rico" },
                    { 179, false, false, "PT", "Portugal" },
                    { 178, true, false, "PL", "Poland" },
                    { 177, true, false, "PN", "Pitcairn" },
                    { 176, true, false, "PH", "Philippines" },
                    { 175, true, false, "PE", "Peru" },
                    { 174, true, false, "PY", "Paraguay" },
                    { 173, true, false, "PG", "Papua New Guinea" },
                    { 172, true, false, "PA", "Panama" },
                    { 189, true, false, "LC", "Saint Lucia" },
                    { 190, true, false, "MF", "Saint Martin" },
                    { 191, true, false, "PM", "Saint Pierre and Miquelon" },
                    { 192, true, false, "VC", "Saint Vincent and The Grenadines" },
                    { 210, true, false, "LK", "Sri Lanka" },
                    { 209, false, false, "ES", "Spain" },
                    { 208, true, false, "SSD", "South Sudan" },
                    { 207, true, false, "GS", "South Georgia and the South Sandwich Islands" },
                    { 206, true, false, "ZA", "South Africa" },
                    { 205, true, false, "SO", "Somalia" },
                    { 204, true, false, "SB", "Solomon Islands" },
                    { 203, true, false, "SI", "Slovenia" },
                    { 212, true, false, "SR", "Suriname" },
                    { 202, true, false, "SK", "Slovakia" },
                    { 200, true, false, "SL", "Sierra Leone" },
                    { 199, true, false, "SC", "Seychelles" },
                    { 198, true, false, "RS", "Serbia" },
                    { 197, true, false, "SN", "Senegal" },
                    { 196, true, false, "SA", "Saudi Arabia" },
                    { 195, true, false, "ST", "Sao Tome and Principe" },
                    { 194, true, false, "SM", "San Marino" }
                });

            migrationBuilder.InsertData(
                schema: "ReferenceData",
                table: "Country",
                columns: new[] { "CountryId", "HasHighTbOccurence", "IsLegacy", "IsoCode", "Name" },
                values: new object[,]
                {
                    { 193, true, false, "WS", "Samoa" },
                    { 201, true, false, "SG", "Singapore" },
                    { 82, true, false, "GH", "Ghana" },
                    { 91, true, false, "GN", "Guinea" },
                    { 80, true, false, "GE", "Georgia" },
                    { 21, true, false, "BY", "Belarus" },
                    { 22, false, false, "BE", "Belgium" },
                    { 23, true, false, "BZ", "Belize" },
                    { 24, true, false, "BJ", "Benin" },
                    { 25, true, false, "BM", "Bermuda" },
                    { 26, true, false, "BT", "Bhutan" },
                    { 27, true, false, "BO", "Bolivia" },
                    { 28, true, false, "BA", "Bosnia and Herzegovina" },
                    { 29, true, false, "BW", "Botswana" },
                    { 30, true, false, "BV", "Bouvet Island" },
                    { 31, true, false, "BR", "Brazil" },
                    { 33, true, false, "BN", "Brunei Darussalam" },
                    { 34, true, false, "BG", "Bulgaria" },
                    { 35, true, false, "BF", "Burkina Faso" },
                    { 36, true, false, "BI", "Burundi" },
                    { 37, true, false, "KH", "Cambodia" },
                    { 38, true, false, "CM", "Cameroon" },
                    { 20, true, false, "BB", "Barbados" },
                    { 39, false, false, "CA", "Canada" },
                    { 19, true, false, "BD", "Bangladesh" },
                    { 17, true, false, "BS", "Bahamas" },
                    { 81, false, false, "DE", "Germany" },
                    { 1, true, false, "AF", "Afghanistan" },
                    { 2, true, false, "AX", "Åland Islands" },
                    { 3, true, false, "AL", "Albania" },
                    { 4, true, false, "DZ", "Algeria" },
                    { 5, true, false, "AS", "American Samoa" },
                    { 6, false, false, "AD", "Andorra" },
                    { 7, true, false, "AO", "Angola" },
                    { 8, true, false, "AI", "Anguilla" },
                    { 9, true, false, "AQ", "Antarctica" },
                    { 10, true, false, "AG", "Antigua and Barbuda" },
                    { 11, true, false, "AR", "Argentina" },
                    { 12, true, false, "AM", "Armenia" },
                    { 13, true, false, "AW", "Aruba" },
                    { 14, false, false, "AU", "Australia" },
                    { 15, false, false, "AT", "Austria" }
                });

            migrationBuilder.InsertData(
                schema: "ReferenceData",
                table: "Country",
                columns: new[] { "CountryId", "HasHighTbOccurence", "IsLegacy", "IsoCode", "Name" },
                values: new object[,]
                {
                    { 16, true, false, "AZ", "Azerbaijan" },
                    { 18, true, false, "BH", "Bahrain" },
                    { 40, true, false, "CV", "Cape Verde" },
                    { 32, true, false, "IO", "British Indian Ocean Territory" },
                    { 42, true, false, "CF", "Central African Republic" },
                    { 63, true, false, "EC", "Ecuador" },
                    { 64, true, false, "EG", "Egypt" },
                    { 65, true, false, "SV", "El Salvador" },
                    { 66, true, false, "GQ", "Equatorial Guinea" },
                    { 67, true, false, "ER", "Eritrea" },
                    { 41, true, false, "KY", "Cayman Islands" },
                    { 69, true, false, "ET", "Ethiopia" },
                    { 70, true, false, "FK", "Falkland Islands (Malvinas)" },
                    { 71, true, false, "FO", "Faroe Islands" },
                    { 72, true, false, "FJ", "Fiji" },
                    { 73, false, false, "FI", "Finland" },
                    { 74, false, false, "FR", "France" },
                    { 75, true, false, "GF", "French Guiana" },
                    { 76, true, false, "PF", "French Polynesia" },
                    { 77, true, false, "TF", "French Southern Territories" },
                    { 78, true, false, "GA", "Gabon" },
                    { 79, true, false, "GM", "Gambia" },
                    { 62, true, false, "DO", "Dominican Republic" },
                    { 61, true, false, "DM", "Dominica" },
                    { 68, true, false, "EE", "Estonia" },
                    { 59, false, false, "DK", "Denmark" },
                    { 43, true, false, "TD", "Chad" },
                    { 44, true, false, "CL", "Chile" },
                    { 60, true, false, "DJ", "Djibouti" },
                    { 46, true, false, "CX", "Christmas Island" },
                    { 47, true, false, "CC", "Cocos (Keeling) Islands" },
                    { 48, true, false, "CO", "Colombia" },
                    { 49, true, false, "KM", "Comoros" },
                    { 50, true, false, "CG", "Congo" },
                    { 45, true, false, "CN", "China" },
                    { 52, true, false, "CK", "Cook Islands" },
                    { 53, true, false, "CR", "Costa Rica" },
                    { 58, true, false, "CZ", "Czech Republic" },
                    { 54, true, false, "CI", "Côte D'ivoire" },
                    { 55, true, false, "HR", "Croatia" },
                    { 56, true, false, "CU", "Cuba" },
                    { 57, false, false, "CY", "Cyprus" }
                });

            migrationBuilder.InsertData(
                schema: "ReferenceData",
                table: "Country",
                columns: new[] { "CountryId", "HasHighTbOccurence", "IsLegacy", "IsoCode", "Name" },
                values: new object[] { 51, true, false, "CD", "Congo, Democratic Republic of the" });

            migrationBuilder.InsertData(
                schema: "ReferenceData",
                table: "Ethnicity",
                columns: new[] { "EthnicityId", "Code", "Label", "Order" },
                values: new object[,]
                {
                    { 9, "J", "Pakistani", 3 },
                    { 10, "K", "Bangladeshi", 4 },
                    { 11, "L", "Any other Asian background", 5 },
                    { 13, "N", "Black African", 6 },
                    { 12, "M", "Black Caribbean", 7 },
                    { 14, "P", "Any other Black Background", 8 },
                    { 16, "R", "Chinese", 9 },
                    { 7, "G", "Any other mixed background", 13 },
                    { 5, "E", "Mixed - White and Black African", 11 },
                    { 4, "D", "Mixed - White and Black Caribbean", 12 },
                    { 15, "S", "Any other ethnic group", 14 },
                    { 17, "Z", "Not stated", 15 },
                    { 8, "H", "Indian", 2 },
                    { 6, "F", "Mixed - White and Asian", 10 },
                    { 1, "WW", "White", 1 }
                });

            migrationBuilder.InsertData(
                schema: "ReferenceData",
                table: "ManualTestType",
                columns: new[] { "ManualTestTypeId", "Description" },
                values: new object[,]
                {
                    { 1, "Smear" },
                    { 2, "Culture" },
                    { 3, "Histology" },
                    { 5, "PCR" },
                    { 4, "Chest x-ray" },
                    { 6, "Line probe assay" }
                });

            migrationBuilder.InsertData(
                schema: "ReferenceData",
                table: "Occupation",
                columns: new[] { "OccupationId", "HasFreeTextField", "Role", "Sector" },
                values: new object[,]
                {
                    { 19, false, "Prison/detention official", "Social/prison service" },
                    { 16, false, "PM attendant", "Laboratory/Pathology" },
                    { 15, false, "Pathologist", "Laboratory/Pathology" },
                    { 13, false, "Laboratory staff", "Laboratory/Pathology" },
                    { 12, false, "Other", "Health care" },
                    { 11, false, "Nurse", "Health care" },
                    { 10, false, "Doctor", "Health care" },
                    { 9, false, "Dentist", "Health care" },
                    { 7, false, "Other", "Education" },
                    { 17, false, "Other", "Laboratory/Pathology" },
                    { 6, false, "Teacher incl. nursery", "Education" },
                    { 5, false, "Lecturer", "Education" },
                    { 4, false, "Full-time student", "Education" },
                    { 3, false, "Other", "Agricultural/animal care" },
                    { 2, false, "Works with wild animals", "Agricultural/animal care" },
                    { 1, false, "Works with cattle", "Agricultural/animal care" },
                    { 8, false, "Community care worker", "Health care" },
                    { 18, false, "Homeless sector worker", "Social/prison service" },
                    { 14, false, "Microbiologist", "Laboratory/Pathology" },
                    { 22, false, "Other", "Social/prison service" }
                });

            migrationBuilder.InsertData(
                schema: "ReferenceData",
                table: "Occupation",
                columns: new[] { "OccupationId", "HasFreeTextField", "Role", "Sector" },
                values: new object[,]
                {
                    { 20, false, "Probation officer", "Social/prison service" },
                    { 24, false, "Housewife/househusband", "Other" },
                    { 25, false, "Prisoner", "Other" },
                    { 26, false, "Retired", "Other" },
                    { 27, false, "Unemployed", "Other" },
                    { 28, true, "Other", "Other" },
                    { 21, false, "Social worker", "Social/prison service" },
                    { 23, false, "Child", "Other" }
                });

            migrationBuilder.InsertData(
                schema: "ReferenceData",
                table: "SampleType",
                columns: new[] { "SampleTypeId", "Category", "Description" },
                values: new object[,]
                {
                    { 10, "Non-Respiratory", "Faeces" },
                    { 3, "Respiratory", "Lung bronchial tree tissue" },
                    { 4, "Respiratory", "Sputum (induced)" },
                    { 5, "Respiratory", "Sputum (spontaneous)" },
                    { 6, "Non-Respiratory", "Blood" },
                    { 7, "Non-Respiratory", "Bone and joint" },
                    { 8, "Non-Respiratory", "CNS" },
                    { 2, "Respiratory", "Bronchoscopy sample" },
                    { 1, "Respiratory", "Bronchial washings" },
                    { 12, "Non-Respiratory", "Gastrointestinal" },
                    { 23, "Non-Respiratory", "Not known" },
                    { 22, "Non-Respiratory", "Other tissues" },
                    { 21, "Non-Respiratory", "Urine" },
                    { 20, "Non-Respiratory", "Skin" },
                    { 19, "Non-Respiratory", "Pus" },
                    { 18, "Non-Respiratory", "Pleural fluid or biopsy" },
                    { 17, "Non-Respiratory", "Pleural" },
                    { 16, "Non-Respiratory", "Peritoneal fluid" },
                    { 15, "Non-Respiratory", "Lymph node" },
                    { 14, "Non-Respiratory", "Gynaecological" },
                    { 13, "Non-Respiratory", "Genitourinary" },
                    { 9, "Non-Respiratory", "CSF" },
                    { 11, "Non-Respiratory", "Gastric washings" }
                });

            migrationBuilder.InsertData(
                schema: "ReferenceData",
                table: "Sex",
                columns: new[] { "SexId", "Label" },
                values: new object[,]
                {
                    { 3, "Unknown" },
                    { 2, "Female" },
                    { 1, "Male" }
                });

            migrationBuilder.InsertData(
                schema: "ReferenceData",
                table: "Site",
                columns: new[] { "SiteId", "Description" },
                values: new object[,]
                {
                    { 10, "Lymph nodes: Intra-thoracic" },
                    { 17, "Other extra-pulmonary" },
                    { 15, "Pericardial" },
                    { 14, "Pleural" },
                    { 13, "Miliary" },
                    { 12, "Laryngeal" },
                    { 11, "Lymph nodes: Extra-thoracic" },
                    { 9, "Genitourinary" }
                });

            migrationBuilder.InsertData(
                schema: "ReferenceData",
                table: "Site",
                columns: new[] { "SiteId", "Description" },
                values: new object[,]
                {
                    { 16, "Soft tissue/Skin" },
                    { 8, "Gastrointestinal/peritoneal" },
                    { 6, "Ocular" },
                    { 5, "CNS: Other" },
                    { 4, "Meningitis" },
                    { 3, "Bone/joint: Other" },
                    { 2, "Spine" },
                    { 1, "Pulmonary" },
                    { 7, "Cryptic disseminated" }
                });

            migrationBuilder.InsertData(
                schema: "ReferenceData",
                table: "TreatmentOutcome",
                columns: new[] { "TreatmentOutcomeId", "TreatmentOutcomeSubType", "TreatmentOutcomeType" },
                values: new object[,]
                {
                    { 3, "Other", "Completed" },
                    { 5, "MdrRegimen", "Cured" },
                    { 2, "MdrRegimen", "Completed" },
                    { 6, "Other", "Cured" },
                    { 7, "TbCausedDeath", "Died" },
                    { 8, "TbContributedToDeath", "Died" },
                    { 9, "TbIncidentalToDeath", "Died" },
                    { 10, "Unknown", "Died" },
                    { 11, "PatientLeftUk", "Lost" },
                    { 1, "StandardTherapy", "Completed" },
                    { 12, "PatientNotLeftUk", "Lost" },
                    { 14, null, "TreatmentStopped" },
                    { 15, "TransferredAbroad", "NotEvaluated" },
                    { 16, "StillOnTreatment", "NotEvaluated" },
                    { 17, "Other", "NotEvaluated" },
                    { 18, "CulturePositive", "Failed" },
                    { 19, "AdditionalResistance", "Failed" },
                    { 13, "Other", "Lost" },
                    { 4, "StandardTherapy", "Cured" },
                    { 21, "Other", "Failed" },
                    { 20, "AdverseReaction", "Failed" }
                });

            migrationBuilder.InsertData(
                schema: "ReferenceData",
                table: "VenueType",
                columns: new[] { "VenueTypeId", "Category", "Name" },
                values: new object[,]
                {
                    { 26, "Place of worship", "Other place of worship" },
                    { 27, "Social", "Arcade/gambling venue" },
                    { 28, "Social", "Pub, bar or club" },
                    { 29, "Social", "Restaurant or cafe" },
                    { 30, "Social", "Library" },
                    { 31, "Social", "Cinema" },
                    { 32, "Social", "Shopping centre" },
                    { 33, "Social", "Hair/beauty salon" },
                    { 34, "Social", "Health club or spa" },
                    { 35, "Social", "Exercise class" },
                    { 36, "Social", "Recreational centre" },
                    { 37, "Social", "Music classes" }
                });

            migrationBuilder.InsertData(
                schema: "ReferenceData",
                table: "VenueType",
                columns: new[] { "VenueTypeId", "Category", "Name" },
                values: new object[,]
                {
                    { 38, "Social", "Community centre" },
                    { 39, "Social", "Job/unemployment centre" },
                    { 40, "Social", "Crack house/smoking den" },
                    { 41, "Social", "Friends house" },
                    { 42, "Social", "Other social venue" },
                    { 43, "Childcare & education", "Pre-school or play group" },
                    { 44, "Childcare & education", "Nursery" },
                    { 25, "Place of worship", "Synagogue" },
                    { 24, "Place of worship", "Multi-faith centre" },
                    { 23, "Place of worship", "Community centre" },
                    { 22, "Place of worship", "Mosque" },
                    { 2, "Workplace", "Catering" },
                    { 3, "Workplace", "Construction" },
                    { 4, "Workplace", "Driving" },
                    { 5, "Workplace", "Education" },
                    { 6, "Workplace", "Emergency services" },
                    { 7, "Workplace", "Factory" },
                    { 8, "Workplace", "Farming" },
                    { 9, "Workplace", "Hospital or medical centre" },
                    { 10, "Workplace", "Office" },
                    { 45, "Childcare & education", "Primary school" },
                    { 11, "Workplace", "Pub, bar or club" },
                    { 13, "Workplace", "Hospitality" },
                    { 14, "Workplace", "Retail" },
                    { 15, "Workplace", "Warehouse" },
                    { 16, "Workplace", "Hair/beauty salon" },
                    { 17, "Workplace", "Health club or spa" },
                    { 18, "Workplace", "Recreational centre" },
                    { 19, "Workplace", "Other workplace" },
                    { 20, "Place of worship", "Church" },
                    { 21, "Place of worship", "Temple" },
                    { 12, "Workplace", "Restaurant or cafe" },
                    { 91, "Transport", "Other transport" },
                    { 46, "Childcare & education", "Secondary school" },
                    { 48, "Childcare & education", "After school clubs" },
                    { 72, "Residential", "Sofa surfing" },
                    { 73, "Residential", "Other residential" },
                    { 74, "Health and care", "Hospital" },
                    { 75, "Health and care", "Walk-in Centre/Minor Injuries" },
                    { 76, "Health and care", "Pharmacy" },
                    { 77, "Health and care", "GP Practice" },
                    { 78, "Health and care", "Nursing Home" }
                });

            migrationBuilder.InsertData(
                schema: "ReferenceData",
                table: "VenueType",
                columns: new[] { "VenueTypeId", "Category", "Name" },
                values: new object[,]
                {
                    { 79, "Health and care", "Hospice" },
                    { 80, "Health and care", "Health Centre/Clinic" },
                    { 81, "Health and care", "Other heathcare" },
                    { 82, "Transport", "Car" },
                    { 83, "Transport", "Bus" },
                    { 84, "Transport", "Train" },
                    { 85, "Transport", "Tram" },
                    { 86, "Transport", "Metro" },
                    { 87, "Transport", "Plane" },
                    { 88, "Transport", "Taxi" },
                    { 89, "Transport", "Boat" },
                    { 90, "Transport", "Cruise ship" },
                    { 71, "Residential", "Hall of residence" },
                    { 47, "Childcare & education", "College or sixth form" },
                    { 70, "Residential", "Hostel" },
                    { 68, "Residential", "Care home" },
                    { 49, "Childcare & education", "University" },
                    { 50, "Childcare & education", "Adult education" },
                    { 51, "Childcare & education", "Private tutoring" },
                    { 52, "Childcare & education", "Religious learning centre" },
                    { 53, "Childcare & education", "Other childcare & education" },
                    { 54, "Place of detention", "Immigration detention centre" },
                    { 55, "Place of detention", "Prison" },
                    { 56, "Place of detention", "Youth detention centre" },
                    { 57, "Place of detention", "Other place of detention" },
                    { 58, "Treatment and rehab", "Alcohol rehabilitation centre" },
                    { 59, "Treatment and rehab", "Drug rehabilitation centre" },
                    { 60, "Treatment and rehab", "Medical or physical rehabilitation centre" },
                    { 61, "Treatment and rehab", "Mental health rehabilitation centre" },
                    { 62, "Treatment and rehab", "Other treatment or rehab centre" },
                    { 63, "Health and care", "Crisis centre or refuge" },
                    { 64, "Residential", "Initial accommodation centre " },
                    { 65, "Residential", "Dispersal accommodation " },
                    { 66, "Residential", "Homeless shelter" },
                    { 67, "Residential", "Squat" },
                    { 69, "Residential", "Halfway house" },
                    { 1, "Workplace", "Armed forces" }
                });

            migrationBuilder.InsertData(
                schema: "ReferenceData",
                table: "ManualTestTypeSampleType",
                columns: new[] { "ManualTestTypeId", "SampleTypeId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 3, 17 },
                    { 6, 16 },
                    { 5, 16 },
                    { 2, 16 },
                    { 1, 16 },
                    { 6, 15 },
                    { 5, 15 },
                    { 3, 15 },
                    { 2, 15 },
                    { 1, 15 },
                    { 6, 14 },
                    { 5, 14 },
                    { 2, 14 },
                    { 1, 14 },
                    { 3, 13 },
                    { 3, 12 },
                    { 6, 11 },
                    { 1, 18 },
                    { 5, 11 },
                    { 2, 18 },
                    { 6, 18 },
                    { 3, 23 },
                    { 2, 23 },
                    { 1, 23 },
                    { 6, 22 },
                    { 5, 22 },
                    { 3, 22 },
                    { 2, 22 },
                    { 1, 22 },
                    { 6, 21 },
                    { 5, 21 },
                    { 2, 21 },
                    { 1, 21 },
                    { 3, 20 },
                    { 6, 19 },
                    { 5, 19 },
                    { 2, 19 },
                    { 1, 19 },
                    { 5, 18 },
                    { 5, 23 },
                    { 2, 11 }
                });

            migrationBuilder.InsertData(
                schema: "ReferenceData",
                table: "ManualTestTypeSampleType",
                columns: new[] { "ManualTestTypeId", "SampleTypeId" },
                values: new object[,]
                {
                    { 6, 10 },
                    { 6, 4 },
                    { 5, 4 },
                    { 2, 4 },
                    { 1, 4 },
                    { 6, 3 },
                    { 5, 3 },
                    { 3, 3 },
                    { 2, 3 },
                    { 1, 3 },
                    { 6, 2 },
                    { 5, 2 },
                    { 3, 2 },
                    { 2, 2 },
                    { 1, 2 },
                    { 6, 1 },
                    { 5, 1 },
                    { 2, 1 },
                    { 1, 5 },
                    { 1, 11 },
                    { 2, 5 },
                    { 6, 5 },
                    { 5, 10 },
                    { 2, 10 },
                    { 1, 10 },
                    { 6, 9 },
                    { 5, 9 },
                    { 2, 9 },
                    { 1, 9 },
                    { 3, 8 },
                    { 6, 7 },
                    { 5, 7 },
                    { 3, 7 },
                    { 2, 7 },
                    { 1, 7 },
                    { 6, 6 },
                    { 5, 6 },
                    { 2, 6 },
                    { 1, 6 },
                    { 5, 5 },
                    { 6, 23 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alert_AlertStatus_AlertType",
                table: "Alert",
                columns: new[] { "AlertStatus", "AlertType" });

            migrationBuilder.CreateIndex(
                name: "IX_Alert_CaseManagerId",
                table: "Alert",
                column: "CaseManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Alert_NotificationId_AlertType",
                table: "Alert",
                columns: new[] { "NotificationId", "AlertType" });

            migrationBuilder.CreateIndex(
                name: "IX_Alert_TbServiceCode",
                table: "Alert",
                column: "TbServiceCode");

            migrationBuilder.CreateIndex(
                name: "IX_CaseManagerTbService_TbServiceCode",
                table: "CaseManagerTbService",
                column: "TbServiceCode");

            migrationBuilder.CreateIndex(
                name: "IX_Country_IsLegacy_Name",
                schema: "ReferenceData",
                table: "Country",
                columns: new[] { "IsLegacy", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Hospital_TBServiceCode",
                schema: "ReferenceData",
                table: "Hospital",
                column: "TBServiceCode");

            migrationBuilder.CreateIndex(
                name: "IX_HospitalDetails_CaseManagerId",
                table: "HospitalDetails",
                column: "CaseManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_HospitalDetails_HospitalId",
                table: "HospitalDetails",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_HospitalDetails_TBServiceCode",
                table: "HospitalDetails",
                column: "TBServiceCode");

            migrationBuilder.CreateIndex(
                name: "IX_LegacyImportNotificationLogMessage_LegacyImportMigrationRunId",
                table: "LegacyImportNotificationLogMessage",
                column: "LegacyImportMigrationRunId");

            migrationBuilder.CreateIndex(
                name: "IX_LegacyImportNotificationOutcome_LegacyImportMigrationRunId",
                table: "LegacyImportNotificationOutcome",
                column: "LegacyImportMigrationRunId");

            migrationBuilder.CreateIndex(
                name: "IX_LocalAuthorityToPHEC_LocalAuthorityCode",
                schema: "ReferenceData",
                table: "LocalAuthorityToPHEC",
                column: "LocalAuthorityCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ManualTestResult_ManualTestTypeId",
                table: "ManualTestResult",
                column: "ManualTestTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ManualTestResult_NotificationId",
                table: "ManualTestResult",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_ManualTestResult_SampleTypeId",
                table: "ManualTestResult",
                column: "SampleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ManualTestTypeSampleType_SampleTypeId",
                schema: "ReferenceData",
                table: "ManualTestTypeSampleType",
                column: "SampleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MBovisAnimalExposure_CountryId",
                table: "MBovisAnimalExposure",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_MBovisAnimalExposure_NotificationId",
                table: "MBovisAnimalExposure",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_MBovisExposureToKnownCase_NotificationId",
                table: "MBovisExposureToKnownCase",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_MBovisOccupationExposures_CountryId",
                table: "MBovisOccupationExposures",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_MBovisOccupationExposures_NotificationId",
                table: "MBovisOccupationExposures",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_MBovisUnpasteurisedMilkConsumption_CountryId",
                table: "MBovisUnpasteurisedMilkConsumption",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_MBovisUnpasteurisedMilkConsumption_NotificationId",
                table: "MBovisUnpasteurisedMilkConsumption",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_MDRDetails_CountryId",
                table: "MDRDetails",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_ClusterId",
                table: "Notification",
                column: "ClusterId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_ETSID",
                table: "Notification",
                column: "ETSID",
                unique: true,
                filter: "[ETSID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_GroupId",
                table: "Notification",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_LTBRID",
                table: "Notification",
                column: "LTBRID",
                unique: true,
                filter: "[LTBRID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_LTBRPatientId",
                table: "Notification",
                column: "LTBRPatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_NotificationStatus",
                table: "Notification",
                column: "NotificationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_NotificationStatus_SubmissionDate",
                table: "Notification",
                columns: new[] { "NotificationStatus", "SubmissionDate" });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationSite_SiteId",
                table: "NotificationSite",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_CountryId",
                table: "Patients",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_EthnicityId",
                table: "Patients",
                column: "EthnicityId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_OccupationId",
                table: "Patients",
                column: "OccupationId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PostcodeToLookup",
                table: "Patients",
                column: "PostcodeToLookup");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_SexId",
                table: "Patients",
                column: "SexId");

            migrationBuilder.CreateIndex(
                name: "IX_PostcodeLookup_LocalAuthorityCode",
                schema: "ReferenceData",
                table: "PostcodeLookup",
                column: "LocalAuthorityCode");

            migrationBuilder.CreateIndex(
                name: "IX_PreviousTbHistory_PreviousTreatmentCountryId",
                table: "PreviousTbHistory",
                column: "PreviousTreatmentCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_PreviousTbService_NotificationId",
                table: "PreviousTbService",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_SocialContextAddress_NotificationId",
                table: "SocialContextAddress",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_SocialContextVenue_NotificationId",
                table: "SocialContextVenue",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_SocialContextVenue_VenueTypeId",
                table: "SocialContextVenue",
                column: "VenueTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TbService_IsLegacy_Name",
                schema: "ReferenceData",
                table: "TbService",
                columns: new[] { "IsLegacy", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_TbService_Name",
                schema: "ReferenceData",
                table: "TbService",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_TbService_PHECCode",
                schema: "ReferenceData",
                table: "TbService",
                column: "PHECCode");

            migrationBuilder.CreateIndex(
                name: "IX_TbService_ServiceAdGroup",
                schema: "ReferenceData",
                table: "TbService",
                column: "ServiceAdGroup",
                unique: true,
                filter: "[ServiceAdGroup] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TravelDetails_Country1Id",
                table: "TravelDetails",
                column: "Country1Id");

            migrationBuilder.CreateIndex(
                name: "IX_TravelDetails_Country2Id",
                table: "TravelDetails",
                column: "Country2Id");

            migrationBuilder.CreateIndex(
                name: "IX_TravelDetails_Country3Id",
                table: "TravelDetails",
                column: "Country3Id");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentEvent_CaseManagerId",
                table: "TreatmentEvent",
                column: "CaseManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentEvent_NotificationId",
                table: "TreatmentEvent",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentEvent_TbServiceCode",
                table: "TreatmentEvent",
                column: "TbServiceCode");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentEvent_TreatmentOutcomeId",
                table: "TreatmentEvent",
                column: "TreatmentOutcomeId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Username",
                table: "User",
                column: "Username",
                unique: true,
                filter: "[Username] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_VisitorDetails_Country1Id",
                table: "VisitorDetails",
                column: "Country1Id");

            migrationBuilder.CreateIndex(
                name: "IX_VisitorDetails_Country2Id",
                table: "VisitorDetails",
                column: "Country2Id");

            migrationBuilder.CreateIndex(
                name: "IX_VisitorDetails_Country3Id",
                table: "VisitorDetails",
                column: "Country3Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alert");

            migrationBuilder.DropTable(
                name: "CaseManagerTbService");

            migrationBuilder.DropTable(
                name: "ClinicalDetails");

            migrationBuilder.DropTable(
                name: "ComorbidityDetails");

            migrationBuilder.DropTable(
                name: "ContactTracing");

            migrationBuilder.DropTable(
                name: "DenotificationDetails");

            migrationBuilder.DropTable(
                name: "DrugResistanceProfile");

            migrationBuilder.DropTable(
                name: "FrequentlyAskedQuestion");

            migrationBuilder.DropTable(
                name: "HospitalDetails");

            migrationBuilder.DropTable(
                name: "ImmunosuppressionDetails");

            migrationBuilder.DropTable(
                name: "LegacyImportNotificationLogMessage");

            migrationBuilder.DropTable(
                name: "LegacyImportNotificationOutcome");

            migrationBuilder.DropTable(
                name: "LocalAuthorityToPHEC",
                schema: "ReferenceData");

            migrationBuilder.DropTable(
                name: "ManualTestResult");

            migrationBuilder.DropTable(
                name: "ManualTestTypeSampleType",
                schema: "ReferenceData");

            migrationBuilder.DropTable(
                name: "MBovisAnimalExposure");

            migrationBuilder.DropTable(
                name: "MBovisExposureToKnownCase");

            migrationBuilder.DropTable(
                name: "MBovisOccupationExposures");

            migrationBuilder.DropTable(
                name: "MBovisUnpasteurisedMilkConsumption");

            migrationBuilder.DropTable(
                name: "MDRDetails");

            migrationBuilder.DropTable(
                name: "NotificationAndDuplicateIds");

            migrationBuilder.DropTable(
                name: "NotificationSite");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "PreviousTbHistory");

            migrationBuilder.DropTable(
                name: "PreviousTbService");

            migrationBuilder.DropTable(
                name: "ReleaseVersion");

            migrationBuilder.DropTable(
                name: "RiskFactorDrugs");

            migrationBuilder.DropTable(
                name: "RiskFactorHomelessness");

            migrationBuilder.DropTable(
                name: "RiskFactorImprisonment");

            migrationBuilder.DropTable(
                name: "RiskFactorSmoking");

            migrationBuilder.DropTable(
                name: "SocialContextAddress");

            migrationBuilder.DropTable(
                name: "SocialContextVenue");

            migrationBuilder.DropTable(
                name: "TravelDetails");

            migrationBuilder.DropTable(
                name: "TreatmentEvent");

            migrationBuilder.DropTable(
                name: "UserLoginEvent");

            migrationBuilder.DropTable(
                name: "VisitorDetails");

            migrationBuilder.DropTable(
                name: "Hospital",
                schema: "ReferenceData");

            migrationBuilder.DropTable(
                name: "LegacyImportMigrationRun");

            migrationBuilder.DropTable(
                name: "TestData");

            migrationBuilder.DropTable(
                name: "ManualTestType",
                schema: "ReferenceData");

            migrationBuilder.DropTable(
                name: "SampleType",
                schema: "ReferenceData");

            migrationBuilder.DropTable(
                name: "MBovisDetails");

            migrationBuilder.DropTable(
                name: "Site",
                schema: "ReferenceData");

            migrationBuilder.DropTable(
                name: "Ethnicity",
                schema: "ReferenceData");

            migrationBuilder.DropTable(
                name: "Occupation",
                schema: "ReferenceData");

            migrationBuilder.DropTable(
                name: "PostcodeLookup",
                schema: "ReferenceData");

            migrationBuilder.DropTable(
                name: "Sex",
                schema: "ReferenceData");

            migrationBuilder.DropTable(
                name: "SocialRiskFactors");

            migrationBuilder.DropTable(
                name: "VenueType",
                schema: "ReferenceData");

            migrationBuilder.DropTable(
                name: "TreatmentOutcome",
                schema: "ReferenceData");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Country",
                schema: "ReferenceData");

            migrationBuilder.DropTable(
                name: "TbService",
                schema: "ReferenceData");

            migrationBuilder.DropTable(
                name: "LocalAuthority",
                schema: "ReferenceData");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "PHEC",
                schema: "ReferenceData");

            migrationBuilder.DropTable(
                name: "NotificationGroup");
        }
    }
}
