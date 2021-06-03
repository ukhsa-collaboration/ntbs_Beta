Feature: Notification editing
  Happy and error paths for notification creation
  Notification deletion

  Background: Log in and navigate to notification
    Given I navigate to the app
    Given I have logged in as BirminghamServiceUser
    Given I am on seeded 'MAXIMUM_DETAILS' notification overview page

  Scenario: Edit notification patient details fields
    When I go to edit the 'PatientDetails' section
    Then I should be on the PatientDetails page
    When I enter Jester into 'PatientDetails_GivenName'
    And I enter Leicester into 'PatientDetails_FamilyName'
    And I enter 4 into 'FormattedDob_Day'
    And I enter 8 into 'FormattedDob_Month'
    And I enter 1932 into 'FormattedDob_Year'
    And I select radio value 'sexId-2'
    And I enter 9866698 into 'PatientDetails_LocalPatientId'
    And I select Chinese for 'PatientDetails_EthnicityId'
    And I select radio value 'nhs-number-known'
    And I enter 9452651220 into 'PatientDetails_NhsNumber'
    And I select Other from input list 'PatientDetails_CountryId'
    And I enter 1955 into 'year-of-entry'
    And I enter 56 Schnowzer Boulevard into 'PatientDetails_Address'
    And I select radio value 'postcode-yes'
    And I enter ST1 1AA into 'PatientDetails_Postcode'
    And I make selection 26 from Other section for 'PatientDetails_OccupationId'
    When I click on the 'save-button' button

    Then I can see the value 'LEICESTER, Jester' for the field 'full-name' in the 'PatientDetails' overview section
    And I can see the value 'Female' for the field 'sex' in the 'PatientDetails' overview section
    And I can see the value '945 265 1220' for the field 'nhs-number' in the 'PatientDetails' overview section
    And I can see the value 'Retired' for the field 'occupation' in the 'PatientDetails' overview section
    And I can see the value '04 Aug 1932' for the field 'dob' in the 'PatientDetails' overview section
    And I can see the value '56 Schnowzer Boulevard' for the field 'address' in the 'PatientDetails' overview section
    And I can see the value 'ST1 1AA' for the field 'postcode' in the 'PatientDetails' overview section
    And I can see the value 'Chinese' for the field 'ethnicity' in the 'PatientDetails' overview section
    And I can see the value 'Other' for the field 'country' in the 'PatientDetails' overview section
    And I can see the value '1955' for the field 'uk-entry' in the 'PatientDetails' overview section
    And I can see the value '9866698' for the field 'local-patient-id' in the 'PatientDetails' overview section

  Scenario: Edit notification hospital details fields
    When I go to edit the 'HospitalDetails' section
    Then I should be on the HospitalDetails page
    When I enter 2 into 'FormattedNotificationDate_Day'
    And I enter 2 into 'FormattedNotificationDate_Month'
    And I enter 2011 into 'FormattedNotificationDate_Year'
      # Wait until javascript has populated the hospital dropdown
    And I wait
    And I select GOOD HOPE HOSPITAL for 'HospitalDetails_HospitalId'
    And I enter Dr Howard Foreman into 'HospitalDetails_Consultant'
    And I enter Birmingham UITester into 'HospitalDetails_CaseManagerId'
    When I click on the 'save-button' button

    Then I can see the value 'Birmingham & Solihull' for the field 'tb-service' in the 'HospitalDetails' overview section
    And I can see the value 'GOOD HOPE HOSPITAL' for the field 'hospital-name' in the 'HospitalDetails' overview section
    And I can see the value 'Birmingham UITester' for the field 'case-manager' in the 'HospitalDetails' overview section
    And I can see the value 'Dr Howard Foreman' for the field 'consultant' in the 'HospitalDetails' overview section
    And I can see the value '02 Feb 2011' for the field 'notification-date' in the 'PatientDetails' overview section

  Scenario: Edit notification clinical details fields
    When I go to edit the 'ClinicalDetails' section
    Then I should be on the ClinicalDetails page
    When I check 'NotificationSiteMap_PULMONARY_'
    And I expand the 'extra-pulmonary' section
    And I check 'NotificationSiteMap_BONE_SPINE_'
    And I check 'NotificationSiteMap_CRYPTIC_'
    And I check 'bcg-vaccination-no'
    And I enter Offered and done into 'ClinicalDetails_HIVTestState'
    And I check 'symptomatic-yes'
    And I enter 1 into 'FormattedSymptomDate_Day'
    And I enter 2 into 'FormattedSymptomDate_Month'
    And I enter 2010 into 'FormattedSymptomDate_Year'
    And I enter 14 into 'FormattedFirstPresentationDate_Day'
    And I enter 7 into 'FormattedFirstPresentationDate_Month'
    And I enter 2010 into 'FormattedFirstPresentationDate_Year'
    And I check 'healtcare-setting-other'
    And I enter Sexual health clinic into 'ClinicalDetails_HealthcareDescription'
    And I enter 9 into 'FormattedTbServicePresentationDate_Day'
    And I enter 11 into 'FormattedTbServicePresentationDate_Month'
    And I enter 2010 into 'FormattedTbServicePresentationDate_Year'
    And I enter 23 into 'FormattedDiagnosisDate_Day'
    And I enter 12 into 'FormattedDiagnosisDate_Month'
    And I enter 2010 into 'FormattedDiagnosisDate_Year'
    And I check 'treatment-yes'
    And I enter 7 into 'FormattedTreatmentDate_Day'
    And I enter 1 into 'FormattedTreatmentDate_Month'
    And I enter 2011 into 'FormattedTreatmentDate_Year'
    And I check 'home-visit-unknown'
    And I check 'postmortem-no'
    And I check 'dot-offered-yes'
    And I check 'dot-status-DotRefused'
    And I check 'enhanced-case-management-yes'
    And I check 'ecm-1'
    And I enter Patient has no cool shoes into 'ClinicalDetails_Notes'
    When I click on the 'save-button' button

    Then I can see the value 'Pulmonary, Spine, Cryptic' for the field 'sites' in the 'ClinicalDetails' overview section
    And I can see the value 'Yes' for the field 'symptomatic' in the 'ClinicalDetails' overview section
    And I can see the value '01 Feb 2010' for the field 'symptom-date' in the 'ClinicalDetails' overview section
    And I can see the value '340' for the field 'symptom-to-treatment' in the 'ClinicalDetails' overview section
    And I can see the value '14 Jul 2010' for the field 'presentation-date' in the 'ClinicalDetails' overview section
    And I can see the value '163' for the field 'onset-to-presentation' in the 'ClinicalDetails' overview section
    And I can see the value 'Other - Sexual health clinic' for the field 'healthcare-setting' in the 'ClinicalDetails' overview section
    And I can see the value '9 Nov 2010' for the field 'service-presentation-date' in the 'ClinicalDetails' overview section
    And I can see the value '118' for the field 'presentation-to-service' in the 'ClinicalDetails' overview section
    And I can see the value '23 Dec 2010' for the field 'diagnosis-date' in the 'ClinicalDetails' overview section
    And I can see the value '44' for the field 'service-to-diagnosis' in the 'ClinicalDetails' overview section
    And I can see the value '7 Jan 2011' for the field 'treatment-start-date' in the 'ClinicalDetails' overview section
    And I can see the value '15' for the field 'diagnosis-to-treatment' in the 'ClinicalDetails' overview section
    And I can see the value 'No' for the field 'post-mortem' in the 'ClinicalDetails' overview section
    And I can see the value 'Unknown' for the field 'home-visit' in the 'ClinicalDetails' overview section
    And I can see the value 'Offered and done' for the field 'hiv-test' in the 'ClinicalDetails' overview section
    And I can see the value 'Yes' for the field 'dot-offered' in the 'ClinicalDetails' overview section
    And I can see the value 'No' for the field 'bcg-vaccination' in the 'ClinicalDetails' overview section
    And I can see the value 'Yes' for the field 'case-management' in the 'ClinicalDetails' overview section
    And I can see the value 'Patient has no cool shoes' for the field 'notes' in the 'ClinicalDetails' overview section

  Scenario: Edit notification test results fields
    When I go to edit the 'TestResults' section
    Then I should be on the TestResults page
    When I click on the Edit link
    And I enter 11 into 'FormattedTestDate_Day'
    And I enter 3 into 'FormattedTestDate_Month'
    And I enter 2011 into 'FormattedTestDate_Year'
    And I enter Histology into 'TestResultForEdit_ManualTestTypeId'
    And I enter Bone and joint into 'TestResultForEdit_SampleTypeId'
    And I enter Negative into 'TestResultForEdit_Result'
    And I click on the 'save-test-result-button' button
    When I click on the 'save-button' button

    Then I can see the value 'Yes' for the field 'test-carried-out' in the 'TestResults' overview section
    And I can see the value 'Bone and joint' in the 'TestResults' table overview section
    And I can see the value 'Histology' in the 'TestResults' table overview section
    And I can see the value 'Negative' in the 'TestResults' table overview section
    And I can see the value '11 Mar 2011' in the 'TestResults' table overview section

  Scenario: Edit notification contact tracing fields
    When I go to edit the 'ContactTracing' section
    Then I should be on the ContactTracing page
    When I enter 13 into 'ContactTracing_AdultsIdentified'
    And I enter 12 into 'ContactTracing_AdultsScreened'
    And I enter 5 into 'ContactTracing_AdultsActiveTB'
    And I enter 3 into 'ContactTracing_AdultsLatentTB'
    And I enter 2 into 'ContactTracing_AdultsStartedTreatment'
    And I enter 2 into 'ContactTracing_AdultsFinishedTreatment'
    And I enter 7 into 'ContactTracing_ChildrenIdentified'
    And I enter 7 into 'ContactTracing_ChildrenScreened'
    And I enter 0 into 'ContactTracing_ChildrenActiveTB'
    And I enter 1 into 'ContactTracing_ChildrenLatentTB'
    And I enter 1 into 'ContactTracing_ChildrenStartedTreatment'
    And I enter 1 into 'ContactTracing_ChildrenFinishedTreatment'
    When I click on the 'save-button' button

    Then I can see the value '13' for the field 'adults-identified' in the 'ContactTracing' overview section
    And I can see the value '7' for the field 'children-identified' in the 'ContactTracing' overview section
    And I can see the value '20' for the field 'total-identified' in the 'ContactTracing' overview section
    And I can see the value '3' for the field 'adults-latent-tb' in the 'ContactTracing' overview section
    And I can see the value '1' for the field 'children-latent-tb' in the 'ContactTracing' overview section
    And I can see the value '4' for the field 'total-latent-tb' in the 'ContactTracing' overview section
    And I can see the value '12' for the field 'adults-screened' in the 'ContactTracing' overview section
    And I can see the value '7' for the field 'children-screened' in the 'ContactTracing' overview section
    And I can see the value '19' for the field 'total-screened' in the 'ContactTracing' overview section
    And I can see the value '2' for the field 'adults-started-treatment' in the 'ContactTracing' overview section
    And I can see the value '1' for the field 'children-started-treatment' in the 'ContactTracing' overview section
    And I can see the value '3' for the field 'total-started-treatment' in the 'ContactTracing' overview section
    And I can see the value '5' for the field 'adults-active-tb' in the 'ContactTracing' overview section
    And I can see the value '0' for the field 'children-active-tb' in the 'ContactTracing' overview section
    And I can see the value '5' for the field 'total-active-tb' in the 'ContactTracing' overview section
    And I can see the value '2' for the field 'adults-finished-treatment' in the 'ContactTracing' overview section
    And I can see the value '1' for the field 'children-finished-treatment' in the 'ContactTracing' overview section
    And I can see the value '3' for the field 'total-finished-treatment' in the 'ContactTracing' overview section

  Scenario: Edit notification social risk factor fields
    When I go to edit the 'SocialRiskFactors' section
    Then I should be on the SocialRiskFactors page
    When I check 'alcohol-radio-button-Yes'
    And I check 'RiskFactorDrugs-radio-button-no'
    And I check 'RiskFactorHomelessness-radio-button-yes'
    And I check 'SocialRiskFactors_RiskFactorHomelessness_IsCurrentView'
    And I check 'RiskFactorImprisonment-radio-button-unknown'
    And I check 'mentalhealth-radio-button-No'
    And I check 'RiskFactorSmoking-radio-button-yes'
    And I check 'SocialRiskFactors_RiskFactorSmoking_MoreThanFiveYearsAgoView'
    And I check 'asylum-seeker-radio-button-Yes'
    And I check 'immigration-detainee-radio-button-No'
    When I click on the 'save-button' button

    Then I can see the value 'No' for the field 'drugs' in the 'SocialRiskFactors' overview section
    And I can see the value 'Yes' for the field 'homelessness' in the 'SocialRiskFactors' overview section
    And I can see the value 'current' for the field 'homelessness-time-period' in the 'SocialRiskFactors' overview section
    And I can see the value 'Unknown' for the field 'imprisonment' in the 'SocialRiskFactors' overview section
    And I can see the value 'Yes' for the field 'smoking' in the 'SocialRiskFactors' overview section
    And I can see the value 'more than 5 years ago' for the field 'smoking-time-period' in the 'SocialRiskFactors' overview section
    And I can see the value 'Yes' for the field 'alcohol' in the 'SocialRiskFactors' overview section
    And I can see the value 'No' for the field 'mental-health' in the 'SocialRiskFactors' overview section
    And I can see the value 'Yes' for the field 'asylum-seeker' in the 'SocialRiskFactors' overview section
    And I can see the value 'No' for the field 'immigration-detainee' in the 'SocialRiskFactors' overview section

  Scenario: Edit notification travel and visitor fields
    When I go to edit the 'Travel' section
    Then I should be on the Travel page
    When I check 'travelDetails-hasTravelNo'
    And I check 'visitorDetails-hasVisitorYes'
    And I enter 5 into 'VisitorDetails_TotalNumberOfCountries'
    And I select Brazil from input list 'VisitorDetails_Country1Id'
    And I enter 1 into 'VisitorDetails_StayLengthInMonths1'
    And I select Comoros from input list 'VisitorDetails_Country2Id'
    And I enter 12 into 'VisitorDetails_StayLengthInMonths2'
    And I select Fiji from input list 'VisitorDetails_Country3Id'
    And I enter 7 into 'VisitorDetails_StayLengthInMonths3'
    When I click on the 'save-button' button

    Then I can see the value 'No' for the field 'has-travel' in the 'Travel' overview section
    And I can see the value 'Yes' for the field 'has-visitors' in the 'Travel' overview section
    And I can see the value '5' for the field 'visitor-countries-total' in the 'Travel' overview section
    And I can see the value '20' for the field 'visitor-duration-total' in the 'Travel' overview section
    And I can see the value 'Brazil' for the field 'visitor-country1' in the 'Travel' overview section
    And I can see the value 'Comoros' for the field 'visitor-country2' in the 'Travel' overview section
    And I can see the value 'Fiji' for the field 'visitor-country3' in the 'Travel' overview section

  Scenario: Edit notification comorbidity fields
    When I go to edit the 'Comorbidities' section
    Then I should be on the Comorbidities page
    When I check 'diabetes-radio-button-Yes'
    And I check 'hepatitis-b-radio-button-Unknown'
    And I check 'hepatitis-c-radio-button-Yes'
    And I check 'liver-radio-button-No'
    And I check 'renal-radio-button-No'
    And I check 'immunosuppression-yes'
    And I check 'HasTransplantation'
    And I uncheck 'HasBioTherapy'
    When I click on the 'save-button' button

    Then I can see the value 'Yes' for the field 'diabetes' in the 'Comorbidities' overview section
    And I can see the value 'Yes' for the field 'hepatitis-c' in the 'Comorbidities' overview section
    And I can see the value 'Unknown' for the field 'hepatitis-b' in the 'Comorbidities' overview section
    And I can see the value 'No' for the field 'liver-disease' in the 'Comorbidities' overview section
    And I can see the value 'No' for the field 'renal-disease' in the 'Comorbidities' overview section
    And I can see the value 'Yes' for the field 'immunosuppression-status' in the 'Comorbidities' overview section
    And I can see the value 'Transplantation' for the field 'immunosuppression-types' in the 'Comorbidities' overview section

  Scenario: Edit notification social context addresses fields
    When I go to edit the 'SocialContextAddresses' section
    Then I should be on the SocialContextAddresses page
    When I click on the Edit link
    And I enter 129 Durand Pass into 'Address_Address'
    And I enter W7 3EA into 'Address_Postcode'
    And I enter 30 into 'FormattedDateFrom_Day'
    And I enter 5 into 'FormattedDateFrom_Month'
    And I enter 2010 into 'FormattedDateFrom_Year'
    And I enter 9 into 'FormattedDateTo_Day'
    And I enter 12 into 'FormattedDateTo_Month'
    And I enter 2012 into 'FormattedDateTo_Year'
    And I enter Made soup here daily into 'Address_Details'
    And I click on the 'save-address-button' button
    When I click on the Notification details link

    Then I can see the value '129 Durand Pass' in the 'SocialContextAddresses' table overview section
    And I can see the value '30 May 2010 to 09 Dec 2012' in the 'SocialContextAddresses' table overview section
    And I can see the value 'Made soup here daily' in the 'SocialContextAddresses' table overview section

  Scenario: Edit notification social context venues fields
    When I go to edit the 'SocialContextVenues' section
    Then I should be on the SocialContextVenues page
    When I click on the Edit link
    And I enter Nursery into 'Venue_VenueTypeId'
    And I enter McDonalds into 'Venue_Name'
    And I enter 666 Stan Drive into 'Venue_Address'
    And I enter LS6 2EB into 'Venue_Postcode'
    And I enter Daily into 'Venue_Frequency'
    And I enter 4 into 'FormattedDateFrom_Day'
    And I enter 4 into 'FormattedDateFrom_Month'
    And I enter 2004 into 'FormattedDateFrom_Year'
    And I enter 18 into 'FormattedDateTo_Day'
    And I enter 12 into 'FormattedDateTo_Month'
    And I enter 2015 into 'FormattedDateTo_Year'
    And I enter Singing lessons given into 'Venue_Details'
    And I click on the 'save-venue-button' button
    When I click on the Notification details link

    Then I can see the value '666 Stan Drive' in the 'SocialContextVenues' table overview section
    And I can see the value '04 Apr 2004 to 18 Dec 2015' in the 'SocialContextVenues' table overview section
    And I can see the value 'Singing lessons given' in the 'SocialContextVenues' table overview section
    And I can see the value 'McDonalds' in the 'SocialContextVenues' table overview section
    And I can see the value 'Nursery' in the 'SocialContextVenues' table overview section

  Scenario: Edit notification previous tb history fields
    When I go to edit the 'PreviousHistory' section
    Then I should be on the PreviousHistory page
    When I check 'previous-tb-yes'
    And I enter 1991 into 'PreviousTbHistory_PreviousTbDiagnosisYear'
    And I check 'previous-treatment-yes'
    And I enter Angola into 'PreviousTbHistory_PreviousTreatmentCountryId'
    And I click on the 'save-button' button

    Then I can see the value 'Yes' for the field 'had-tb' in the 'PreviousHistory' overview section
    And I can see the value '1991' for the field 'diagnosis-year' in the 'PreviousHistory' overview section
    And I can see the value 'Yes' for the field 'treated' in the 'PreviousHistory' overview section
    And I can see the value 'Angola' for the field 'treatment-country' in the 'PreviousHistory' overview section

  Scenario: Edit notification treatment event fields
    When I go to edit the 'TreatmentEvents' section
    Then I should be on the TreatmentEvents page
    When I click on the Edit link
    And I enter 5 into 'FormattedEventDate_Day'
    And I enter 4 into 'FormattedEventDate_Month'
    And I enter 2012 into 'FormattedEventDate_Year'
    And I enter Treatment outcome into 'treatmentevent-type'
    And I enter Lost to follow-up into 'treatmentoutcome-type'
    And I enter Patient left UK into 'treatmentoutcome-subtype'
    And I enter Moved to Yemen we think into 'treatmentevent-note'
    And I click on the 'save-treatment-event-button' button
    And I click on the Notification details link

    Then I can see the value 'Outcome at 12 months No outcome recorded' for the field '12-month-outcome' in the 'TreatmentEvents' overview section
    And I can see the value 'Outcome at 24 months Lost to follow-up' for the field '24-month-outcome' in the 'TreatmentEvents' overview section
    And I can see the value 'Treatment period 1' for the field '1' in the 'TreatmentEvents' overview section
    And I can see the value '05 Apr 2012 to 05 Apr 2012' for the field '1-dates' in the 'TreatmentEvents' overview section
    And I can see the value 'Treatment outcome - Lost to follow-up' in the 'TreatmentEvents' table overview section
    And I can see the value 'Patient left UK' in the 'TreatmentEvents' table overview section
    And I can see the value 'Birmingham UITester' in the 'TreatmentEvents' table overview section

  Scenario: Edit notification MDR detail fields
    When I go to edit the 'MDRDetails' section
    Then I should be on the MDRDetails page
    When I check 'exposure-yes'
    And I enter Chauffeur into 'MDRDetails_RelationshipToCase'
    And I enter Guyana into 'MDRDetails_CountryId'
    And I click on the 'save-button' button

    Then I can see the value 'Yes' for the field 'known-case-exposure' in the 'MDRDetails' overview section
    And I can see the value 'Chauffeur' for the field 'relationship-to-case' in the 'MDRDetails' overview section
    And I can see the value 'Guyana' for the field 'country-name' in the 'MDRDetails' overview section
