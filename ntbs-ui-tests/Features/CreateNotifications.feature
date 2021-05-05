Feature: Notification creation
  Happy and error paths for notification creation
  Notification deletion

  Background: Create new notification
    Given I have logged in as ServiceUser
    Given I am on the Search page
    When I enter 1 into 'SearchParameters_IdFilter'
    And I click on the 'search-button' button
    Then I should be on the Search page
    When I click on the 'create-button' button
    Then A new notification should have been created
    And I should be on the PatientDetails page

  Scenario: Create notification with all fields
        # PatientDetails page
    When I enter Test into 'PatientDetails_GivenName'
    And I enter User into 'PatientDetails_FamilyName'
    And I enter 1 into 'FormattedDob_Day'
    And I enter 1 into 'FormattedDob_Month'
    And I enter 1970 into 'FormattedDob_Year'
    And I select radio value 'sexId-1'
    And I enter 1234 into 'PatientDetails_LocalPatientId'
    And I select Mixed - White and Asian for 'PatientDetails_EthnicityId'
    And I select radio value 'nhs-number-unknown'
    And I select Afghanistan from input list 'PatientDetails_CountryId'
    And I enter 1999 into 'year-of-entry'
    And I enter 44 Poodle Terrace into 'PatientDetails_Address'
    And I select radio value 'postcode-no'
    And I make selection 28 from Other section for 'PatientDetails_OccupationId'
    And I enter Software Developer into 'occupation-other'
    And I click on the 'save-button' button

        # HospitalDetails page
    Then I should be on the HospitalDetails page
    When I enter 1 into 'FormattedNotificationDate_Day'
    And I enter 1 into 'FormattedNotificationDate_Month'
    And I enter 2019 into 'FormattedNotificationDate_Year'
    And I select Birmingham & Solihull for 'HospitalDetails_TBServiceCode'
        # Wait until javascript has populated the hospital dropdown
    And I wait
    And I select SELLY OAK HOSPITAL for 'HospitalDetails_HospitalId'
    And I enter Dr Lawton into 'HospitalDetails_Consultant'
    And I enter Birmingham User into 'HospitalDetails_CaseManagerId'
    And I click on the 'save-button' button

        # Clinical details page
    Then I should be on the ClinicalDetails page
    When I check 'NotificationSiteMap_PULMONARY_'
    And I check 'bcg-vaccination-yes'
    And I enter 1990 into 'ClinicalDetails_BCGVaccinationYear'
    And I enter Not offered into 'ClinicalDetails_HIVTestState'
    And I check 'symptomatic-yes'
    And I enter 2 into 'FormattedSymptomDate_Day'
    And I enter 2 into 'FormattedSymptomDate_Month'
    And I enter 2017 into 'FormattedSymptomDate_Year'
    And I enter 3 into 'FormattedFirstPresentationDate_Day'
    And I enter 3 into 'FormattedFirstPresentationDate_Month'
    And I enter 2017 into 'FormattedFirstPresentationDate_Year'
    And I check 'healtcare-setting-AccidentAndEmergency'
    And I enter 4 into 'FormattedTbServicePresentationDate_Day'
    And I enter 4 into 'FormattedTbServicePresentationDate_Month'
    And I enter 2017 into 'FormattedTbServicePresentationDate_Year'
    And I enter 1 into 'FormattedDiagnosisDate_Day'
    And I enter 1 into 'FormattedDiagnosisDate_Month'
    And I enter 2018 into 'FormattedDiagnosisDate_Year'
    And I check 'treatment-yes'
    And I enter 2 into 'FormattedTreatmentDate_Day'
    And I enter 1 into 'FormattedTreatmentDate_Month'
    And I enter 2018 into 'FormattedTreatmentDate_Year'
    And I check 'home-visit-yes'
    And I enter 2 into 'FormattedHomeVisitDate_Day'
    And I enter 1 into 'FormattedHomeVisitDate_Month'
    And I enter 2018 into 'FormattedHomeVisitDate_Year'
    And I check 'postmortem-no'
    And I check 'dot-offered-yes'
    And I check 'dot-status-DotReceived'
    And I check 'enhanced-case-management-yes'
    And I check 'ecm-2'
    And I check 'regimen-standardTherapy'
    And I enter Patient has been doing well into 'ClinicalDetails_Notes'
    And I click on the 'save-button' button
        
        # Test results page
    Then I should be on the TestResults page
    When I select radio value 'test-carried-out-yes'
    And I click on the 'add-new-manual-test-result-button' button
    And I enter 1 into 'FormattedTestDate_Day'
    And I enter 1 into 'FormattedTestDate_Month'
    And I enter 2012 into 'FormattedTestDate_Year'
    And I enter Smear into 'TestResultForEdit_ManualTestTypeId'
    And I enter BronchialWashings into 'TestResultForEdit_SampleTypeId'
    And I enter Negative into 'TestResultForEdit_Result'
    And I click on the 'save-test-result-button' button
    And I click on the 'save-button' button
        
        # Contact tracing page
    Then I should be on the ContactTracing page
    When I enter 2 into 'ContactTracing_AdultsIdentified'
    And I enter 1 into 'ContactTracing_AdultsScreened'
    And I enter 1 into 'ContactTracing_AdultsActiveTB'
    And I enter 12 into 'ContactTracing_ChildrenIdentified'
    And I enter 5 into 'ContactTracing_ChildrenScreened'
    And I enter 5 into 'ContactTracing_ChildrenLatentTB'
    And I enter 5 into 'ContactTracing_ChildrenStartedTreatment'
    When I click on the 'save-button' button

        # Social risk factors page
    Then I should be on the SocialRiskFactors page
    When I check 'alcohol-radio-button-Yes'
    And I check 'RiskFactorDrugs-radio-button-yes'
    And I check 'SocialRiskFactors_RiskFactorDrugs_InPastFiveYearsView'
    And I check 'RiskFactorHomelessness-radio-button-unknown'
    And I check 'RiskFactorImprisonment-radio-button-no'
    And I check 'mentalhealth-radio-button-Yes'
    And I check 'RiskFactorSmoking-radio-button-unknown'
    And I check 'asylum-seeker-radio-button-No'
    And I check 'immigration-detainee-radio-button-Yes'
    When I click on the 'save-button' button

        # Travel/visitor history page
    Then I should be on the Travel page
    When I check 'travelDetails-hasTravelNo'
    And I check 'visitorDetails-hasVisitorYes'
    And I enter 1 into 'VisitorDetails_TotalNumberOfCountries'
    And I select Albania from input list 'VisitorDetails_Country1Id'
    And I enter 4 into 'VisitorDetails_StayLengthInMonths1'
    When I click on the 'save-button' button

        # Co-morbidities and immunosuppression page
    Then I should be on the Comorbidities page
    When I check 'diabetes-radio-button-No'
    And I check 'hepatitis-b-radio-button-Yes'
    And I check 'hepatitis-c-radio-button-Yes'
    And I check 'liver-radio-button-Unknown'
    And I check 'renal-radio-button-Yes'
    And I check 'immunosuppression-yes'
    And I check 'HasOther'
    And I enter Bad illness into 'OtherDescription'
    When I click on the 'save-button' button

        # Social context addresses page
    Then I should be on the SocialContextAddresses page
    When I click on the 'add-new-social-context-address-button' button
    And I enter 12 George Court into 'Address_Address'
    And I enter G1 3RR into 'Address_Postcode'
    And I enter 16 into 'FormattedDateFrom_Day'
    And I enter 5 into 'FormattedDateFrom_Month'
    And I enter 2000 into 'FormattedDateFrom_Year'
    And I enter 16 into 'FormattedDateTo_Day'
    And I enter 5 into 'FormattedDateTo_Month'
    And I enter 2002 into 'FormattedDateTo_Year'
    And I enter Lived here a while into 'Address_Details'
    And I click on the 'save-address-button' button
    When I click on the 'save-button' button
        
        # Social context venues page
    Then I should be on the SocialContextVenues page
    When I click on the 'add-new-social-context-venue-button' button
    And I enter Catering into 'Venue_VenueTypeId'
    And I enter Nandos into 'Venue_Name'
    And I enter 445 Star Gates into 'Venue_Address'
    And I enter S55 4EP into 'Venue_Postcode'
    And I enter Monthly into 'Venue_Frequency'
    And I enter 23 into 'FormattedDateFrom_Day'
    And I enter 3 into 'FormattedDateFrom_Month'
    And I enter 2003 into 'FormattedDateFrom_Year'
    And I enter 23 into 'FormattedDateTo_Day'
    And I enter 3 into 'FormattedDateTo_Month'
    And I enter 2010 into 'FormattedDateTo_Year'
    And I enter He loves chicken into 'Venue_Details'
    And I click on the 'save-venue-button' button
    When I click on the 'save-button' button
    
      # Previous History page
    Then I should be on the PreviousHistory page
    When I check 'previous-tb-yes'
    And I enter 2000 into 'PreviousTbHistory_PreviousTbDiagnosisYear'
    And I check 'previous-treatment-unknown'
    When I click on the 'save-button' button

      # Treatment Events page
    Then I should be on the TreatmentEvents page
    When I click on the 'add-new-treatment-event-button' button
    And I enter 30 into 'FormattedEventDate_Day'
    And I enter 1 into 'FormattedEventDate_Month'
    And I enter 2019 into 'FormattedEventDate_Year'
    And I enter Treatment restart into 'treatmentevent-type'
    And I enter Gave some medicine into 'treatmentevent-note'
    And I click on the 'save-treatment-event-button' button
    And I wait
    When I click on the 'save-button' button
    
      # Treatment Events page again + submission
    Then I should be on the TreatmentEvents page
    When I click on the 'submit-button' button
    
      # Notification overview + checks that submission went correctly
    Then I should see the Notification
      # Check patient details
    And The value 'Test' for the field 'Given name' in section 'Patient details' is in the database
    And The value 'Software Developer' for the field 'Occupation other' in section 'Patient details' is in the database
      # Check hospital details
    And The value 'Dr Lawton' for the field 'Consultant' in section 'Hospital details' is in the database
    And The date '1/1/2019' for the field 'Notification date' in section '' is in the database
      # Check clinical details
    And The value 'Patient has been doing well' for the field 'Notes' in section 'Clinical details' is in the database
    And The status 'Yes' for the field 'BCG vaccination' in section 'Clinical details' is in the database
    And The status 'Yes' for the field 'Home visit' in section 'Clinical details' is in the database
    And The date '2/1/2018' for the field 'Home visit date' in section 'Clinical details' is in the database
      # Check test results - BROKEN
        # And The value for the field 'Test carried out' in section 'Test results' in the database is true
          # Check contact tracing
    And The number '2' for the field 'Adults identified' in section 'Contact tracing' is in the database
    And The number '1' for the field 'Adults screened' in section 'Contact tracing' is in the database
    And The number '12' for the field 'Children identified' in section 'Contact tracing' is in the database
    And The number '5' for the field 'Children latent TB' in section 'Contact tracing' is in the database
      # Check social risk factors
    And The status 'Yes' for the field 'Alcohol misuse' in section 'Social risk factors' is in the database
      # Need to work out a way to access more nested fields
        # And The status 'Unknown' for the field 'Homelessness' in section 'Social risk factors' is in the database
    And The status 'No' for the field 'Asylum seeker' in section 'Social risk factors' is in the database
      # Check travel/visitor history
    And The status 'Yes' for the field 'Has visitor' in section 'Visitor details' is in the database
    And The number '1' for the field 'Total number of countries' in section 'Visitor details' is in the database
    And The number '4' for the field 'Stay length for country 1' in section 'Visitor details' is in the database
      # Check co-morbidities and immunosuppression
    And The status 'Yes' for the field 'HepatitisBStatus' in section 'Comorbidities' is in the database
    And The status 'Unknown' for the field 'LiverDiseaseStatus' in section 'Comorbidities' is in the database
    And The status 'Yes' for the field 'Immunosuppression status' in section 'Immunosuppression' is in the database
    And The value 'Bad illness' for the field 'Immunosuppression other' in section 'Immunosuppression' is in the database
      # Check social context addresses - BROKEN
    #And The value '12 George Court' for the field 'Address' in section 'Social context addresses' is in the database
    #And The date '16/5/2002' for the field 'DateTo' in section 'Social context addresses' is in the database
      # Check social context venues - BROKEN
    #And The value 'Nandos' for the field 'Name' in section 'Social context venues' is in the database
    #And The date '23/3/2003' for the field 'DateFrom' in section 'Social context venues' is in the database
      # Check previous history
    And The status 'Yes' for the field 'PreviouslyHadTb' in section 'Previous history' is in the database
    And The number '2000' for the field 'PreviousTbDiagnosisYear' in section 'Previous history' is in the database
      # Check treatment events
    And I can see the starting event 'Diagnosis date` dated `01 Jan 2018'

  Scenario: Create and submit notification without content
    When I click on the 'submit-button' button
    Then I should be on the PatientDetails page
    And I should see all submission error messages

  Scenario: Create and delete notification draft
    When I click on the 'delete-draft-button' button
    Then I should be on the Delete page
    When I click on the 'confirm-deletion-button' button
    Then I should be on the Confirm page
    When I click on the 'return-to-homepage' button
    Then I should be on the Homepage
