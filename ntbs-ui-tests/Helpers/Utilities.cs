using System;
using System.Collections.Generic;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_ui_tests.Hooks;

namespace ntbs_ui_tests.Helpers
{
    public static class Utilities
    {
        public static Notification GetNotificationForUser(string notificationName, UserConfig userConfig)
        {
            if (!Notifications.TryGetValue(notificationName, out var baseNotification))
            {
                throw new ArgumentException($"Unexpected notification name {notificationName} given");
            }
            baseNotification.CreationDate = DateTime.Now;

            baseNotification.HospitalDetails ??= new HospitalDetails();
            baseNotification.HospitalDetails.TBServiceCode = userConfig.TbServiceCode;
            baseNotification.HospitalDetails.CaseManagerId = userConfig.UserId;

            return baseNotification;
        }

        private static IDictionary<string, Notification> Notifications =>
            new Dictionary<string, Notification>
            {
                {
                    "MINIMAL_DETAILS",
                    new Notification
                    {
                        NotificationStatus = NotificationStatus.Notified,
                        NotificationSites = new List<NotificationSite>
                        {
                            new NotificationSite
                            {
                                SiteId = (int)SiteId.PULMONARY
                            }
                        }
                    }
                },
                {
                    "M_BOVIS",
                    new Notification
                    {
                        NotificationStatus = NotificationStatus.Notified,
                        NotificationSites = new List<NotificationSite>
                        {
                            new NotificationSite
                            {
                                SiteId = (int)SiteId.PULMONARY
                            }
                        },
                        DrugResistanceProfile = new DrugResistanceProfile {Species = "M. bovis"}
                    }
                },
                {
                    "MAXIMUM_DETAILS",
                    new Notification
                    {
                        PatientDetails = new PatientDetails
                        {
                            GivenName = "Scott", FamilyName = "Lang", Address = "42 Stonefall Ridge",
                            Dob = new DateTime(1980, 4, 3), Postcode = "RF44 3RR",
                            CountryId = 2, EthnicityId = 8, SexId = 2, OccupationId = 6, LocalPatientId = "3455",
                            NhsNumber = "9124325111", NhsNumberNotKnown = false
                        },
                        // TB Service Code and Case Manager ID are set when we get the notification for user
                        HospitalDetails = new HospitalDetails { Consultant = "Dr Frank Lotchewski" },
                        ClinicalDetails = new ClinicalDetails
                        {
                            BCGVaccinationState = Status.Yes, BCGVaccinationYear = 1990,
                            DiagnosisDate = new DateTime(2011, 3, 23), DotStatus = DotStatus.DotReceived,
                            StartedTreatment = false, EnhancedCaseManagementLevel = 2,
                            EnhancedCaseManagementStatus = Status.Yes, FirstPresentationDate = new DateTime(2010, 12, 25),
                            HomeVisitCarriedOut = Status.No, HealthcareDescription = "Been giving lots of paracetamol",
                            HealthcareSetting = HealthcareSetting.GP, HIVTestState = HIVTestStatus.OfferedButRefused,
                            IsSymptomatic = true, IsDotOffered = Status.Unknown,
                            IsPostMortem = false, MDRTreatmentStartDate = new DateTime(2013, 1, 2),
                            Notes = "Patient has cool hats", SymptomStartDate = new DateTime(2010, 12, 20),
                            TreatmentRegimen = TreatmentRegimen.MdrTreatment, TreatmentStartDate = new DateTime(2011, 4, 1),
                            TBServicePresentationDate = new DateTime(2010, 12, 30)
                        },
                        TestData = new TestData
                        {
                            HasTestCarriedOut = true,
                            ManualTestResults = new List<ManualTestResult>
                            {
                                new ManualTestResult
                                {
                                    TestDate = new DateTime(2011, 1, 2), SampleTypeId = 6,
                                    Result = Result.Positive, ManualTestTypeId = 1
                                }
                            }
                        },
                        ContactTracing = new ContactTracing
                        {
                            AdultsIdentified = 3, ChildrenIdentified = 3, AdultsScreened = 2, ChildrenScreened = 3,
                            AdultsActiveTB = 1, ChildrenActiveTB = 0, AdultsLatentTB = 1, ChildrenLatentTB = 2,
                            AdultsStartedTreatment = 1, ChildrenStartedTreatment = 0, AdultsFinishedTreatment = 1,
                            ChildrenFinishedTreatment = 0
                        },
                        SocialRiskFactors = new SocialRiskFactors
                        {
                            AlcoholMisuseStatus = Status.Unknown, AsylumSeekerStatus = Status.No,
                            ImmigrationDetaineeStatus = Status.Yes, MentalHealthStatus = Status.Yes,
                            RiskFactorHomelessness = new RiskFactorDetails
                            {
                                Status = Status.Yes, Type = RiskFactorType.Homelessness, IsCurrent = false,
                                InPastFiveYears = true, MoreThanFiveYearsAgo = false
                            },
                            RiskFactorDrugs = new RiskFactorDetails
                            {
                                Status = Status.Yes, Type = RiskFactorType.Drugs, IsCurrent = false,
                                InPastFiveYears = true, MoreThanFiveYearsAgo = false
                            },
                            RiskFactorImprisonment = new RiskFactorDetails
                            {
                                Status = Status.Yes, Type = RiskFactorType.Imprisonment, IsCurrent = false,
                                InPastFiveYears = true, MoreThanFiveYearsAgo = false
                            },
                            RiskFactorSmoking = new RiskFactorDetails
                            {
                                Status = Status.Yes, Type = RiskFactorType.Smoking, IsCurrent = false,
                                InPastFiveYears = true, MoreThanFiveYearsAgo = false
                            }
                        },
                        TravelDetails = new TravelDetails
                        {
                            HasTravel = Status.Yes, Country1Id = 3, Country2Id = 7, Country3Id = 5,
                            StayLengthInMonths1 = 4, StayLengthInMonths2 = 4, StayLengthInMonths3 = 12,
                            TotalNumberOfCountries = 3
                        },
                        VisitorDetails = new VisitorDetails
                        {
                            HasVisitor = Status.Yes, Country1Id = 8, Country2Id = 11, Country3Id = 21,
                            StayLengthInMonths1 = 2, StayLengthInMonths2 = 4, StayLengthInMonths3 = 6,
                            TotalNumberOfCountries = 3
                        },
                        ComorbidityDetails = new ComorbidityDetails
                        {
                            DiabetesStatus = Status.No, HepatitisBStatus = Status.Yes, LiverDiseaseStatus = Status.Unknown,
                            RenalDiseaseStatus = Status.Unknown, HepatitisCStatus = Status.No
                        },
                        ImmunosuppressionDetails = new ImmunosuppressionDetails
                        {
                            Status = Status.Yes, HasOther = false, HasTransplantation = false, HasBioTherapy = true
                        },
                        SocialContextAddresses = new List<SocialContextAddress>{new SocialContextAddress
                        {
                            Address = "5 Freshman House", Details = "He went here lots", Postcode = "GL9 1EF",
                            DateFrom = new DateTime(2000, 4, 1), DateTo = new DateTime(2006, 5, 1)
                        }},
                        SocialContextVenues = new List<SocialContextVenue>{new SocialContextVenue()
                        {
                            Address = "51 Cheese Fields", Details = "Made cheese", Postcode = "BH25 6QX",
                            DateFrom = new DateTime(1997, 4, 15), DateTo = new DateTime(1999, 7, 15)
                        }},
                        PreviousTbHistory = new PreviousTbHistory
                        {
                            PreviouslyHadTb = Status.Yes, PreviousTbDiagnosisYear = 1982, PreviouslyTreated = Status.Unknown,
                            PreviousTreatmentCountryId = 15
                        },
                        TreatmentEvents = new List<TreatmentEvent>{new TreatmentEvent
                        {
                            EventDate = new DateTime(2012, 2, 2), Note = "He wanted meds",
                            TreatmentEventType = TreatmentEventType.TreatmentRestart
                        }},
                        MDRDetails = new MDRDetails
                        {
                            RelationshipToCase = "Godmother", CountryId = 38,
                            ExposureToKnownCaseStatus = Status.Yes, NotifiedToPheStatus = Status.Yes
                        },
                        NotificationDate = new DateTime(2011, 1, 1),
                        NotificationStatus = NotificationStatus.Notified,
                        NotificationSites = new List<NotificationSite>
                        {
                            new NotificationSite
                            {
                                SiteId = (int)SiteId.PULMONARY
                            }
                        }
                    }
                }
            };
    }
}
