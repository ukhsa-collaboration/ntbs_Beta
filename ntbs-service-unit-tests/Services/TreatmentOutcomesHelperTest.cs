using System;
using System.Collections.Generic;
using System.Linq;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.Services
{
    public class TreatmentOutcomesHelperTest
    {
        // The example notifications are taken from pdf attached to ticket NTBS-923 TODO: update this to actual googlesheet
        // Each notification is seeded with an expected treatment outcome at 12/24/36 months based on what
        // GetTreatmentOutcomeAtXYears_ReturnsCorrectOutcomeAtXYear should return

        private class NotificationAndExpectedOutcomes
        {
            public string ExampleName { get; set; }
            public Notification Notification { get; set; }
            public TreatmentOutcomeType? ExpectedOutcomeAt1Year { get; set; }
            public TreatmentOutcomeType? ExpectedOutcomeAt2Years { get; set; }
            public TreatmentOutcomeType? ExpectedOutcomeAt3Years { get; set; }
        }

        private static IEnumerable<NotificationAndExpectedOutcomes> NotificationsAndExpectedOutcome()
        {
            yield return new NotificationAndExpectedOutcomes
            {
                ExampleName = "Example 1",
                Notification = CreateNotificationWithNotificationEvents(1,
                    new List<TreatmentEvent>
                    {
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 1, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentStart
                        },
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 6, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                            TreatmentOutcome = new TreatmentOutcome
                            {
                                TreatmentOutcomeType = TreatmentOutcomeType.Cured
                            }
                        }
                    }),
                ExpectedOutcomeAt1Year = TreatmentOutcomeType.Cured,
                ExpectedOutcomeAt2Years = null,
                ExpectedOutcomeAt3Years = null
            };
            yield return new NotificationAndExpectedOutcomes
            {
                ExampleName = "Example 2",
                Notification = CreateNotificationWithNotificationEvents(2,
                    new List<TreatmentEvent>
                    {
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 1, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentStart
                        },
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 3, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                            TreatmentOutcome = new TreatmentOutcome
                            {
                                TreatmentOutcomeType = TreatmentOutcomeType.Lost
                            }
                        },
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 8, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentRestart
                        },
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 11, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                            TreatmentOutcome = new TreatmentOutcome
                            {
                                TreatmentOutcomeType = TreatmentOutcomeType.Completed,
                            }
                        }
                    }),
                ExpectedOutcomeAt1Year = TreatmentOutcomeType.Completed,
                ExpectedOutcomeAt2Years = null,
                ExpectedOutcomeAt3Years = null
            };
            yield return new NotificationAndExpectedOutcomes
            {
                ExampleName = "Example 3",
                Notification = CreateNotificationWithNotificationEvents(3, new List<TreatmentEvent>
                    {
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 1, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentStart
                        },
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 3, 2),
                            TreatmentEventType = TreatmentEventType.TransferOut
                        },
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 5, 2),
                            TreatmentEventType = TreatmentEventType.TransferIn
                        },
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 8, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentRestart
                        },
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 11, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                            TreatmentOutcome = new TreatmentOutcome
                            {
                                TreatmentOutcomeType = TreatmentOutcomeType.Failed,
                            }
                        }
                    }),
                ExpectedOutcomeAt1Year = TreatmentOutcomeType.Failed,
                ExpectedOutcomeAt2Years = null,
                ExpectedOutcomeAt3Years = null
            };
            yield return new NotificationAndExpectedOutcomes
            {
                ExampleName = "Example 4",
                Notification = CreateNotificationWithNotificationEvents(4, new List<TreatmentEvent>
                    {
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 1, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentStart
                        },
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 5, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                            TreatmentOutcome = new TreatmentOutcome
                            {
                                TreatmentOutcomeType = TreatmentOutcomeType.Lost,
                            }
                        },
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 8, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentRestart
                        },
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2011, 1, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                            TreatmentOutcome = new TreatmentOutcome
                            {
                                TreatmentOutcomeType = TreatmentOutcomeType.Cured,
                            }
                        }
                    }),
                ExpectedOutcomeAt1Year = null,
                ExpectedOutcomeAt2Years = TreatmentOutcomeType.Cured,
                ExpectedOutcomeAt3Years = null
            };
            yield return new NotificationAndExpectedOutcomes
            {
                ExampleName = "Example 5",
                Notification = CreateNotificationWithNotificationEvents(5, new List<TreatmentEvent>
                    {
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 1, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentStart
                        }
                    }),
                ExpectedOutcomeAt1Year = null,
                ExpectedOutcomeAt2Years = null,
                ExpectedOutcomeAt3Years = null
            };
            yield return new NotificationAndExpectedOutcomes
            {
                ExampleName = "Example 6",
                Notification = CreateNotificationWithNotificationEvents(6, new List<TreatmentEvent>
                    {
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 1, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentStart
                        },
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 11, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                            TreatmentOutcome = new TreatmentOutcome
                            {
                                TreatmentOutcomeType = TreatmentOutcomeType.NotEvaluated,
                            }
                        }
                    }),
                ExpectedOutcomeAt1Year = TreatmentOutcomeType.NotEvaluated,
                ExpectedOutcomeAt2Years = null,
                ExpectedOutcomeAt3Years = null
            };
            yield return new NotificationAndExpectedOutcomes
            {
                ExampleName = "Example 7",
                Notification = CreateNotificationWithNotificationEvents(7, new List<TreatmentEvent>
                    {
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 1, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentStart
                        },
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 6, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                            TreatmentOutcome = new TreatmentOutcome
                            {
                                TreatmentOutcomeType = TreatmentOutcomeType.NotEvaluated,
                            }
                        },
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 12, 30),
                            TreatmentEventType = TreatmentEventType.TreatmentRestart
                        },
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2012, 2, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                            TreatmentOutcome = new TreatmentOutcome
                            {
                                TreatmentOutcomeType = TreatmentOutcomeType.Cured,
                            }
                        }
                    }),
                ExpectedOutcomeAt1Year = null,
                ExpectedOutcomeAt2Years = null,
                ExpectedOutcomeAt3Years = TreatmentOutcomeType.Cured
            };
            yield return new NotificationAndExpectedOutcomes
            {
                ExampleName = "Example 8",
                Notification = CreateNotificationWithNotificationEvents(8, new List<TreatmentEvent>
                    {
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 1, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentStart
                        },
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 6, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                            TreatmentOutcome = new TreatmentOutcome
                            {
                                TreatmentOutcomeType = TreatmentOutcomeType.Cured,
                            }
                        },
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 7, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                            TreatmentOutcome = new TreatmentOutcome
                            {
                                TreatmentOutcomeType = TreatmentOutcomeType.Failed,
                            }
                        },
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2011, 1, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                            TreatmentOutcome = new TreatmentOutcome
                            {
                                TreatmentOutcomeType = TreatmentOutcomeType.Died,
                            }
                        }
                    }),
                ExpectedOutcomeAt1Year = TreatmentOutcomeType.Failed,
                ExpectedOutcomeAt2Years = TreatmentOutcomeType.Died,
                ExpectedOutcomeAt3Years = null
            };
            yield return new NotificationAndExpectedOutcomes
            {
                ExampleName = "Example 9",
                Notification = CreateNotificationWithNotificationEvents(9, new List<TreatmentEvent>
                    {
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 1, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentStart
                        },
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 6, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                            TreatmentOutcome = new TreatmentOutcome
                            {
                                TreatmentOutcomeType = TreatmentOutcomeType.Lost,
                            }
                        },
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 12, 30),
                            TreatmentEventType = TreatmentEventType.TreatmentRestart
                        }
                    }),
                ExpectedOutcomeAt1Year = null,
                ExpectedOutcomeAt2Years = null,
                ExpectedOutcomeAt3Years = null
            };
            yield return new NotificationAndExpectedOutcomes
            {
                ExampleName = "Example 10",
                Notification = CreateNotificationWithNotificationEvents(10, new List<TreatmentEvent>
                    {
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 1, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentStart
                        },
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 6, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                            TreatmentOutcome = new TreatmentOutcome
                            {
                                TreatmentOutcomeType = TreatmentOutcomeType.Lost,
                            }
                        },
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 12, 30),
                            TreatmentEventType = TreatmentEventType.TreatmentRestart
                        }
                    }),
                ExpectedOutcomeAt1Year = null,
                ExpectedOutcomeAt2Years = null,
                ExpectedOutcomeAt3Years = null
            };
            yield return new NotificationAndExpectedOutcomes
            {
                ExampleName = "Example 11",
                Notification = CreateNotificationWithNotificationEvents(11, new List<TreatmentEvent>
                    {
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 1, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentStart
                        },
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 6, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                            TreatmentOutcome = new TreatmentOutcome
                            {
                                TreatmentOutcomeType = TreatmentOutcomeType.NotEvaluated,
                            }
                        },
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2012, 12, 30),
                            TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                            TreatmentOutcome = new TreatmentOutcome
                            {
                                TreatmentOutcomeType = TreatmentOutcomeType.NotEvaluated
                            }
                        }
                    }),
                ExpectedOutcomeAt1Year = TreatmentOutcomeType.NotEvaluated,
                ExpectedOutcomeAt2Years = null,
                ExpectedOutcomeAt3Years = TreatmentOutcomeType.NotEvaluated
            };
            yield return new NotificationAndExpectedOutcomes
            {
                ExampleName = "Example 12",
                Notification = CreateNotificationWithNotificationEvents(12, new List<TreatmentEvent>
                    {
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 1, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentStart
                        },
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 6, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                            TreatmentOutcome = new TreatmentOutcome
                            {
                                TreatmentOutcomeType = TreatmentOutcomeType.NotEvaluated,
                            }
                        },
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2012, 12, 30),
                            TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                            TreatmentOutcome = new TreatmentOutcome
                            {
                                TreatmentOutcomeType = TreatmentOutcomeType.NotEvaluated
                            }
                        }
                    }),
                ExpectedOutcomeAt1Year = TreatmentOutcomeType.NotEvaluated,
                ExpectedOutcomeAt2Years = null,
                ExpectedOutcomeAt3Years = TreatmentOutcomeType.NotEvaluated
            };
            yield return new NotificationAndExpectedOutcomes
            {
                ExampleName = "Example 13",
                Notification = CreateNotificationWithNotificationEvents(13, new List<TreatmentEvent>
                    {
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 1, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentStart
                        },
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 12, 30),
                            TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                            TreatmentOutcome = new TreatmentOutcome
                            {
                                TreatmentOutcomeType = TreatmentOutcomeType.Lost,
                            }
                        },
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2011, 1, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentRestart
                        },
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2011, 12, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                            TreatmentOutcome = new TreatmentOutcome
                            {
                                TreatmentOutcomeType = TreatmentOutcomeType.Cured
                            }
                        }
                    }),
                ExpectedOutcomeAt1Year = TreatmentOutcomeType.Lost,
                ExpectedOutcomeAt2Years = TreatmentOutcomeType.Cured,
                ExpectedOutcomeAt3Years = null
            };
            yield return new NotificationAndExpectedOutcomes
            {
                ExampleName = "Example 14 (NTBS-1347)",
                Notification = CreateNotificationWithNotificationEvents(14, new List<TreatmentEvent>
                {
                    new TreatmentEvent
                    {
                        EventDate = new DateTime(2010, 1, 1),
                        TreatmentEventType = TreatmentEventType.TreatmentStart
                    },
                    new TreatmentEvent
                    {
                        EventDate = new DateTime(2010, 2, 1),
                        TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                        TreatmentOutcome = new TreatmentOutcome
                        {
                            TreatmentOutcomeType = TreatmentOutcomeType.Completed,
                        }
                    },
                    new TreatmentEvent
                    {
                        EventDate = new DateTime(2010, 3, 1),
                        TreatmentEventType = TreatmentEventType.TreatmentRestart
                    },
                    new TreatmentEvent
                    {
                        EventDate = new DateTime(2010, 3, 1),
                        TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                        TreatmentOutcome = new TreatmentOutcome
                        {
                            TreatmentOutcomeType = TreatmentOutcomeType.NotEvaluated,
                            TreatmentOutcomeSubType = TreatmentOutcomeSubType.StillOnTreatment
                        }
                    }
                }),
                ExpectedOutcomeAt1Year = TreatmentOutcomeType.NotEvaluated,
                ExpectedOutcomeAt2Years = null,
                ExpectedOutcomeAt3Years = null
            };
            yield return new NotificationAndExpectedOutcomes
            {
                ExampleName = "Example 15 (NTBS-1650)",
                Notification = new Notification
                {
                    NotificationId = 15,
                    ClinicalDetails =
                        new ClinicalDetails { DiagnosisDate = new DateTime(2010, 1, 1), IsPostMortem = true },
                    TreatmentEvents = new List<TreatmentEvent>
                    {
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2009, 1, 1),
                            TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                            TreatmentOutcome = new TreatmentOutcome
                            {
                                TreatmentOutcomeType = TreatmentOutcomeType.Died,
                                TreatmentOutcomeSubType = TreatmentOutcomeSubType.Unknown
                            }
                        }
                    }
                },
                ExpectedOutcomeAt1Year = TreatmentOutcomeType.Died,
                ExpectedOutcomeAt2Years = null,
                ExpectedOutcomeAt3Years = null
            };
        }

        public static IEnumerable<object[]> NotificationsAndExpectedOutcomesAtYear1 =>
            NotificationsAndExpectedOutcome().Select(n => new object[] { n.Notification, n.ExpectedOutcomeAt1Year });

        public static IEnumerable<object[]> NotificationsAndExpectedOutcomesAtYear2 =>
            NotificationsAndExpectedOutcome().Select(n => new object[] { n.Notification, n.ExpectedOutcomeAt2Years });

        public static IEnumerable<object[]> NotificationsAndExpectedOutcomesAtYear3 =>
            NotificationsAndExpectedOutcome().Select(n => new object[] { n.Notification, n.ExpectedOutcomeAt3Years });

        [Theory, MemberData(nameof(NotificationsAndExpectedOutcomesAtYear1))]
        public void GetTreatmentOutcomeAtXYears_ReturnsCorrectOutcomeAt1Year(
            Notification notification,
            TreatmentOutcomeType? expectedOutcomeAt1Year)
        {
            var treatmentOutcomeAt1Year = TreatmentOutcomesHelper.GetTreatmentOutcomeAtXYears(notification, 1);
            Assert.Equal(treatmentOutcomeAt1Year?.TreatmentOutcomeType, expectedOutcomeAt1Year);
        }

        [Theory, MemberData(nameof(NotificationsAndExpectedOutcomesAtYear2))]
        public void GetTreatmentOutcomeAtXYears_ReturnsCorrectOutcomeAt2Years(
            Notification notification,
            TreatmentOutcomeType? expectedOutcomeAt2Years)
        {
            var treatmentOutcomeAt2Years = TreatmentOutcomesHelper.GetTreatmentOutcomeAtXYears(notification, 2);
            Assert.Equal(treatmentOutcomeAt2Years?.TreatmentOutcomeType, expectedOutcomeAt2Years);
        }

        [Theory, MemberData(nameof(NotificationsAndExpectedOutcomesAtYear3))]
        public void GetTreatmentOutcomeAtXYears_ReturnsCorrectOutcomeAt3Years(
            Notification notification,
            TreatmentOutcomeType? expectedOutcomeAt3Years)
        {
            var treatmentOutcomeAt3Years = TreatmentOutcomesHelper.GetTreatmentOutcomeAtXYears(notification, 3);
            Assert.Equal(treatmentOutcomeAt3Years?.TreatmentOutcomeType, expectedOutcomeAt3Years);
        }

        private static Notification CreateNotificationWithNotificationEvents(int notificationId, IList<TreatmentEvent> treatmentEvents)
        {
            return new Notification
            {
                NotificationId = notificationId,
                ClinicalDetails = new ClinicalDetails { TreatmentStartDate = new DateTime(2010, 1, 1) },
                TreatmentEvents = treatmentEvents
            };
        }
    }
}
