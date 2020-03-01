using System;
using System.Collections.Generic;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.Services
{
    public class TreatmentOutcomeServiceTest
    {
        private readonly ITreatmentOutcomeService _treatmentOutcomeService;

        public TreatmentOutcomeServiceTest()
        {
            _treatmentOutcomeService = new TreatmentOutcomeService();
        }

        // The example notifications are taken from pdf attached to ticket NTBS-923 TODO: update this to actual googlesheet
        // Each notification is seeded with an expected treatment outcome at 12/24/36 months based on what
        // GetTreatmentOutcomeAtXYears_ReturnsCorrectOutcomeAtXYear should return
        public static IEnumerable<object[]> NotificationsAndExpectedOutcome()
        {
            // Example 1
            yield return new object[] 
            {
                CreateNotificationWithNotificationEvents(1, 
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
                    TreatmentOutcomeType.Cured,
                    null,
                    null
                };
            // Example 2
            yield return new object[] 
            { 
                CreateNotificationWithNotificationEvents(2, 
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
                TreatmentOutcomeType.Completed,
                null,
                null
            };
            // Example 3
            yield return new object[] 
            { 
                CreateNotificationWithNotificationEvents(3, new List<TreatmentEvent>
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
                TreatmentOutcomeType.Failed,
                null,
                null
            };
            // Example 4
            yield return new object[] 
            { 
                CreateNotificationWithNotificationEvents(4, new List<TreatmentEvent>
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
                null,
                TreatmentOutcomeType.Cured,
                null
            };
            // Example 5
            yield return new object[]
            {
                CreateNotificationWithNotificationEvents(5, new List<TreatmentEvent>
                    {
                        new TreatmentEvent
                        {
                            EventDate = new DateTime(2010, 1, 2),
                            TreatmentEventType = TreatmentEventType.TreatmentStart
                        }
                    }),
                null,
                null,
                null
            };
            // Example 6
            yield return new object[]
            {
                CreateNotificationWithNotificationEvents(6, new List<TreatmentEvent>
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
                TreatmentOutcomeType.NotEvaluated,
                null,
                null
            };
            // Example 7
            yield return new object[]
            {
                CreateNotificationWithNotificationEvents(7, new List<TreatmentEvent>
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
                null,
                null,
                TreatmentOutcomeType.Cured
            };
            // Example 8
            yield return new object[]
            {
                CreateNotificationWithNotificationEvents(8, new List<TreatmentEvent>
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
                TreatmentOutcomeType.Failed,
                TreatmentOutcomeType.Died,
                null
            };
            // Example 9
            yield return new object[]
            {
                CreateNotificationWithNotificationEvents(9, new List<TreatmentEvent>
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
                null,
                null,
                null
            };
            // Example 10
            yield return new object[]
            {
                CreateNotificationWithNotificationEvents(10, new List<TreatmentEvent>
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
                null,
                null,
                null
            };
            // Example 11
            yield return new object[]
            {
                CreateNotificationWithNotificationEvents(11, new List<TreatmentEvent>
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
                TreatmentOutcomeType.NotEvaluated,
                null,
                TreatmentOutcomeType.NotEvaluated
            };
            // Example 12
            yield return new object[]
            {
                CreateNotificationWithNotificationEvents(12, new List<TreatmentEvent>
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
                TreatmentOutcomeType.NotEvaluated,
                null,
                TreatmentOutcomeType.NotEvaluated
            };
            // Example 13
            yield return new object[]
            {
                CreateNotificationWithNotificationEvents(13, new List<TreatmentEvent>
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
                TreatmentOutcomeType.Lost,
                TreatmentOutcomeType.Cured,
                null
            };
        }

        // MemberData contains the expected outcomes at 1, 2 and 3 years, for each test the unnecessary outcomes are
        // unused as "_" and "__"
        [Theory, MemberData(nameof(NotificationsAndExpectedOutcome))]
        public void GetTreatmentOutcomeAtXYears_ReturnsCorrectOutcomeAt1Year(Notification notification, 
            TreatmentOutcomeType? expectedOutcomeAt1Year, 
            TreatmentOutcomeType? _, 
            TreatmentOutcomeType? __)
        {
            var treatmentOutcomeAt1Year = _treatmentOutcomeService.GetTreatmentOutcomeAtXYears(notification, 1);
            Assert.Equal(treatmentOutcomeAt1Year?.TreatmentOutcomeType, expectedOutcomeAt1Year);
        }
        
        [Theory, MemberData(nameof(NotificationsAndExpectedOutcome))]
        public void GetTreatmentOutcomeAtXYears_ReturnsCorrectOutcomeAt2Years(Notification notification, 
            TreatmentOutcomeType? _, 
            TreatmentOutcomeType? expectedOutcomeAt2Years, 
            TreatmentOutcomeType? __)
        {
            var treatmentOutcomeAt1Year = _treatmentOutcomeService.GetTreatmentOutcomeAtXYears(notification, 2);
            Assert.Equal(treatmentOutcomeAt1Year?.TreatmentOutcomeType, expectedOutcomeAt2Years);
        }
        
        [Theory, MemberData(nameof(NotificationsAndExpectedOutcome))]
        public void GetTreatmentOutcomeAtXYears_ReturnsCorrectOutcomeAt3Years(Notification notification, 
            TreatmentOutcomeType? _, 
            TreatmentOutcomeType? __,
            TreatmentOutcomeType? expectedOutcomeAt3Years)
        {
            var treatmentOutcomeAt1Year = _treatmentOutcomeService.GetTreatmentOutcomeAtXYears(notification, 3);
            Assert.Equal(treatmentOutcomeAt1Year?.TreatmentOutcomeType, expectedOutcomeAt3Years);
        }

        private static Notification CreateNotificationWithNotificationEvents(int notificationId, IList<TreatmentEvent> treatmentEvents)
        {
            return new Notification
            {
                NotificationId = notificationId,
                ClinicalDetails = new ClinicalDetails {TreatmentStartDate = new DateTime(2010, 1, 1)},
                TreatmentEvents = treatmentEvents
            };
        }
    }
}
