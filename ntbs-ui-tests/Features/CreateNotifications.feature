Feature: Notification creation
  Happy and error paths for notification creation
  Notification deletion

  Background: Create new notification
    Given I navigate to the app
    Given I have logged in as BirminghamServiceUser
    Given I am on the Search page
    When I enter 1 into 'SearchParameters_IdFilter'
    And I click on the 'search-button' button
    Then I should be on the Search page
    When I click on the 'create-button' button
    Then A new notification should have been created
    And I should be on the PatientDetails page
    # Mandatory patient details field
    When I enter Test into 'PatientDetails_GivenName'
    And I enter User into 'PatientDetails_FamilyName'
    And I enter 1 into 'FormattedDob_Day'
    And I enter 1 into 'FormattedDob_Month'
    And I enter 1970 into 'FormattedDob_Year'
    And I select radio value 'sexId-1'
    And I select Mixed - White and Asian for 'PatientDetails_EthnicityId'
    And I select radio value 'nhs-number-unknown'
    And I select Afghanistan from input list 'PatientDetails_CountryId'
    And I select radio value 'postcode-no'
    And I click on the 'save-button' button
    # Mandatory hospital details fields
    When I enter 1 into 'FormattedNotificationDate_Day'
    And I enter 1 into 'FormattedNotificationDate_Month'
    And I enter 2019 into 'FormattedNotificationDate_Year'
    And I select Birmingham & Solihull for 'HospitalDetails_TBServiceCode'
    And I wait
    And I select SELLY OAK HOSPITAL for 'HospitalDetails_HospitalId'
    And I click on the 'save-button' button
    # Mandatory clinical details fields
    When I check 'NotificationSiteMap_PULMONARY_'
    And I enter 1 into 'FormattedDiagnosisDate_Day'
    And I enter 1 into 'FormattedDiagnosisDate_Month'
    And I enter 2018 into 'FormattedDiagnosisDate_Year'
    And I click on the 'save-button' button
    # Mandatory test fields
    When I select radio value 'test-carried-out-no'
    And I click on the 'save-button' button

  Scenario: Create notification with all patient details fields
    When I click 'Personal details' on the navigation bar
    Then I should be on the PatientDetails page
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
    When I click on the 'submit-button' button

    Then I can see the value 'USER, Test' for the field 'full-name' in the 'PatientDetails' overview section
    And I can see the value 'Male' for the field 'sex' in the 'PatientDetails' overview section
    And I can see the value 'Not known' for the field 'nhs-number' in the 'PatientDetails' overview section
    And I can see the value 'Other - Software Developer' for the field 'occupation' in the 'PatientDetails' overview section
    And I can see the value '01 Jan 1970' for the field 'dob' in the 'PatientDetails' overview section
    And I can see the value '44 Poodle Terrace' for the field 'address' in the 'PatientDetails' overview section
    And I can see the value 'No fixed abode' for the field 'postcode' in the 'PatientDetails' overview section
    And I can see the value 'Mixed - White and Asian' for the field 'ethnicity' in the 'PatientDetails' overview section
    And I can see the value 'Afghanistan' for the field 'country' in the 'PatientDetails' overview section
    And I can see the value '1999' for the field 'uk-entry' in the 'PatientDetails' overview section
    And I can see the value '1234' for the field 'local-patient-id' in the 'PatientDetails' overview section

  Scenario: Create notification with all hospital details fields
    When I click 'Hospital details' on the navigation bar
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
    When I click on the 'submit-button' button

    Then I can see the value 'Birmingham & Solihull' for the field 'tb-service' in the 'HospitalDetails' overview section
    And I can see the value 'SELLY OAK HOSPITAL' for the field 'hospital-name' in the 'HospitalDetails' overview section
    And I can see the value 'Birmingham User' for the field 'case-manager' in the 'HospitalDetails' overview section
    And I can see the value 'Dr Lawton' for the field 'consultant' in the 'HospitalDetails' overview section
    And I can see the value '01 Jan 2019' for the field 'notification-date' in the 'PatientDetails' overview section

  Scenario: Create notification with all clinical details fields
    When I click 'Clinical details' on the navigation bar
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
    When I click on the 'submit-button' button

    Then I can see the value 'Pulmonary' for the field 'sites' in the 'ClinicalDetails' overview section
    And I can see the value 'Yes' for the field 'symptomatic' in the 'ClinicalDetails' overview section
    And I can see the value '02 Feb 2017' for the field 'symptom-date' in the 'ClinicalDetails' overview section
    And I can see the value '334' for the field 'symptom-to-treatment' in the 'ClinicalDetails' overview section
    And I can see the value '03 Mar 2017' for the field 'presentation-date' in the 'ClinicalDetails' overview section
    And I can see the value '29' for the field 'onset-to-presentation' in the 'ClinicalDetails' overview section
    And I can see the value 'A&E' for the field 'healthcare-setting' in the 'ClinicalDetails' overview section
    And I can see the value '04 Apr 2017' for the field 'service-presentation-date' in the 'ClinicalDetails' overview section
    And I can see the value '32' for the field 'presentation-to-service' in the 'ClinicalDetails' overview section
    And I can see the value '01 Jan 2018' for the field 'diagnosis-date' in the 'ClinicalDetails' overview section
    And I can see the value '272' for the field 'service-to-diagnosis' in the 'ClinicalDetails' overview section
    And I can see the value '02 Jan 2018' for the field 'treatment-start-date' in the 'ClinicalDetails' overview section
    And I can see the value '1' for the field 'diagnosis-to-treatment' in the 'ClinicalDetails' overview section
    And I can see the value 'No' for the field 'post-mortem' in the 'ClinicalDetails' overview section
    And I can see the value 'Yes' for the field 'home-visit' in the 'ClinicalDetails' overview section
    And I can see the value 'Not offered' for the field 'hiv-test' in the 'ClinicalDetails' overview section
    And I can see the value 'Yes' for the field 'dot-offered' in the 'ClinicalDetails' overview section
    And I can see the value 'Yes' for the field 'bcg-vaccination' in the 'ClinicalDetails' overview section
    And I can see the value 'Yes' for the field 'case-management' in the 'ClinicalDetails' overview section
    And I can see the value 'Standard therapy' for the field 'treatment-regimen' in the 'ClinicalDetails' overview section
    And I can see the value 'Patient has been doing well' for the field 'notes' in the 'ClinicalDetails' overview section
    
  Scenario: Create notification with all test results fields
    When I click 'Test results' on the navigation bar
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
    When I click on the 'submit-button' button
    
    Then I can see the value 'Yes' for the field 'test-carried-out' in the 'TestResults' overview section
    And I can see the value 'Bronchial washings' in the 'TestResults' table overview section
    And I can see the value 'Smear' in the 'TestResults' table overview section
    And I can see the value 'Negative' in the 'TestResults' table overview section
    And I can see the value '01 Jan 2012' in the 'TestResults' table overview section

  Scenario: Create notification with all contact tracing fields
    When I click 'Contact tracing' on the navigation bar
    Then I should be on the ContactTracing page
    When I enter 3 into 'ContactTracing_AdultsIdentified'
    And I enter 2 into 'ContactTracing_AdultsScreened'
    And I enter 1 into 'ContactTracing_AdultsActiveTB'
    And I enter 1 into 'ContactTracing_AdultsLatentTB'
    And I enter 1 into 'ContactTracing_AdultsStartedTreatment'
    And I enter 1 into 'ContactTracing_AdultsFinishedTreatment'
    And I enter 12 into 'ContactTracing_ChildrenIdentified'
    And I enter 5 into 'ContactTracing_ChildrenScreened'
    And I enter 1 into 'ContactTracing_ChildrenActiveTB'
    And I enter 4 into 'ContactTracing_ChildrenLatentTB'
    And I enter 4 into 'ContactTracing_ChildrenStartedTreatment'
    And I enter 2 into 'ContactTracing_ChildrenFinishedTreatment'
    When I click on the 'submit-button' button

    Then I can see the value '3' for the field 'adults-identified' in the 'ContactTracing' overview section
    And I can see the value '12' for the field 'children-identified' in the 'ContactTracing' overview section
    And I can see the value '15' for the field 'total-identified' in the 'ContactTracing' overview section
    And I can see the value '1' for the field 'adults-latent-tb' in the 'ContactTracing' overview section
    And I can see the value '4' for the field 'children-latent-tb' in the 'ContactTracing' overview section
    And I can see the value '5' for the field 'total-latent-tb' in the 'ContactTracing' overview section
    And I can see the value '2' for the field 'adults-screened' in the 'ContactTracing' overview section
    And I can see the value '5' for the field 'children-screened' in the 'ContactTracing' overview section
    And I can see the value '7' for the field 'total-screened' in the 'ContactTracing' overview section
    And I can see the value '1' for the field 'adults-started-treatment' in the 'ContactTracing' overview section
    And I can see the value '4' for the field 'children-started-treatment' in the 'ContactTracing' overview section
    And I can see the value '5' for the field 'total-started-treatment' in the 'ContactTracing' overview section
    And I can see the value '1' for the field 'adults-active-tb' in the 'ContactTracing' overview section
    And I can see the value '1' for the field 'children-active-tb' in the 'ContactTracing' overview section
    And I can see the value '2' for the field 'total-active-tb' in the 'ContactTracing' overview section
    And I can see the value '1' for the field 'adults-finished-treatment' in the 'ContactTracing' overview section
    And I can see the value '2' for the field 'children-finished-treatment' in the 'ContactTracing' overview section
    And I can see the value '3' for the field 'total-finished-treatment' in the 'ContactTracing' overview section

  Scenario: Create notification with all social risk factor fields
    When I click 'Social risk factors' on the navigation bar
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
    When I click on the 'submit-button' button
    
    Then I can see the value 'Yes' for the field 'drugs' in the 'SocialRiskFactors' overview section
    And I can see the value 'less than 5 years ago' for the field 'drugs-time-period' in the 'SocialRiskFactors' overview section
    And I can see the value 'Unknown' for the field 'homelessness' in the 'SocialRiskFactors' overview section
    And I can see the value 'No' for the field 'imprisonment' in the 'SocialRiskFactors' overview section
    And I can see the value 'Unknown' for the field 'smoking' in the 'SocialRiskFactors' overview section
    And I can see the value 'Yes' for the field 'alcohol' in the 'SocialRiskFactors' overview section
    And I can see the value 'Yes' for the field 'mental-health' in the 'SocialRiskFactors' overview section
    And I can see the value 'No' for the field 'asylum-seeker' in the 'SocialRiskFactors' overview section
    And I can see the value 'Yes' for the field 'immigration-detainee' in the 'SocialRiskFactors' overview section

  Scenario: Create notification with all travel and visitor fields
    When I click 'Travel/visitor history' on the navigation bar
    Then I should be on the Travel page
    When I check 'travelDetails-hasTravelYes'
    And I enter 4 into 'TravelDetails_TotalNumberOfCountries'
    And I select Lithuania from input list 'TravelDetails_Country1Id'
    And I enter 5 into 'TravelDetails_StayLengthInMonths1'
    And I select Cuba from input list 'TravelDetails_Country2Id'
    And I enter 12 into 'TravelDetails_StayLengthInMonths2'
    And I select Djibouti from input list 'TravelDetails_Country3Id'
    And I enter 1 into 'TravelDetails_StayLengthInMonths3'
    And I check 'visitorDetails-hasVisitorYes'
    And I enter 3 into 'VisitorDetails_TotalNumberOfCountries'
    And I select Albania from input list 'VisitorDetails_Country1Id'
    And I enter 4 into 'VisitorDetails_StayLengthInMonths1'
    And I select Algeria from input list 'VisitorDetails_Country2Id'
    And I enter 5 into 'VisitorDetails_StayLengthInMonths2'
    And I select Macao from input list 'VisitorDetails_Country3Id'
    And I enter 14 into 'VisitorDetails_StayLengthInMonths3'
    When I click on the 'submit-button' button

    Then I can see the value 'Yes' for the field 'has-travel' in the 'Travel' overview section
    And I can see the value '4' for the field 'travel-countries-total' in the 'Travel' overview section
    And I can see the value '18' for the field 'travel-duration-total' in the 'Travel' overview section
    And I can see the value 'Lithuania' for the field 'travel-country1' in the 'Travel' overview section
    And I can see the value 'Cuba' for the field 'travel-country2' in the 'Travel' overview section
    And I can see the value 'Djibouti' for the field 'travel-country3' in the 'Travel' overview section
    And I can see the value 'Yes' for the field 'has-visitors' in the 'Travel' overview section
    And I can see the value '3' for the field 'visitor-countries-total' in the 'Travel' overview section
    And I can see the value '23' for the field 'visitor-duration-total' in the 'Travel' overview section
    And I can see the value 'Albania' for the field 'visitor-country1' in the 'Travel' overview section
    And I can see the value 'Algeria' for the field 'visitor-country2' in the 'Travel' overview section
    And I can see the value 'Macao' for the field 'visitor-country3' in the 'Travel' overview section

  Scenario: Create notification with all comorbidity fields
    When I click 'Co-morbidities' on the navigation bar
    Then I should be on the Comorbidities page
    When I check 'diabetes-radio-button-No'
    And I check 'hepatitis-b-radio-button-Yes'
    And I check 'hepatitis-c-radio-button-Yes'
    And I check 'liver-radio-button-Unknown'
    And I check 'renal-radio-button-Yes'
    And I check 'immunosuppression-yes'
    And I check 'HasOther'
    And I enter Bad illness into 'OtherDescription'
    When I click on the 'submit-button' button

    Then I can see the value 'No' for the field 'diabetes' in the 'Comorbidities' overview section
    And I can see the value 'Yes' for the field 'hepatitis-c' in the 'Comorbidities' overview section
    And I can see the value 'Yes' for the field 'hepatitis-b' in the 'Comorbidities' overview section
    And I can see the value 'Unknown' for the field 'liver-disease' in the 'Comorbidities' overview section
    And I can see the value 'Yes' for the field 'renal-disease' in the 'Comorbidities' overview section
    And I can see the value 'Yes' for the field 'immunosuppression-status' in the 'Comorbidities' overview section
    And I can see the value 'Yes' for the field 'hepatitis-b' in the 'Comorbidities' overview section
    And I can see the value 'Bad illness' for the field 'immunosuppression-other' in the 'Comorbidities' overview section

  Scenario: Create notification with all social context addresses fields
    When I click 'Social context - addresses' on the navigation bar
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
    When I click on the 'submit-button' button

    Then I can see the value '12 George Court' in the 'SocialContextAddresses' table overview section
    And I can see the value '16 May 2000 to 16 May 2002' in the 'SocialContextAddresses' table overview section
    And I can see the value 'Lived here a while' in the 'SocialContextAddresses' table overview section

  Scenario: Create notification with all social context venues fields
    When I click 'Social context - venues' on the navigation bar
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
    When I click on the 'submit-button' button

    Then I can see the value '445 Star Gates' in the 'SocialContextVenues' table overview section
    And I can see the value '23 Mar 2003 to 23 Mar 2010' in the 'SocialContextVenues' table overview section
    And I can see the value 'He loves chicken' in the 'SocialContextVenues' table overview section
    And I can see the value 'Nandos' in the 'SocialContextVenues' table overview section
    And I can see the value 'Catering' in the 'SocialContextVenues' table overview section

  Scenario: Create notification with all previous tb history fields
    When I click 'Previous history' on the navigation bar
    Then I should be on the PreviousHistory page
    When I check 'previous-tb-yes'
    And I enter 2000 into 'PreviousTbHistory_PreviousTbDiagnosisYear'
    And I check 'previous-treatment-unknown'
    When I click on the 'submit-button' button
    
    Then I can see the value 'Yes' for the field 'had-tb' in the 'PreviousHistory' overview section
    And I can see the value '2000' for the field 'diagnosis-year' in the 'PreviousHistory' overview section
    And I can see the value 'Unknown' for the field 'treated' in the 'PreviousHistory' overview section

  Scenario: Create notification with all treatment event fields
    When I click 'Treatment events' on the navigation bar
    Then I should be on the TreatmentEvents page
    When I click on the 'add-new-treatment-event-button' button
    And I enter 31 into 'FormattedEventDate_Day'
    And I enter 1 into 'FormattedEventDate_Month'
    And I enter 2019 into 'FormattedEventDate_Year'
    And I enter Treatment outcome into 'treatmentevent-type'
    And I enter Cured into 'treatmentoutcome-type'
    And I enter Multi-drug resistant regimen into 'treatmentoutcome-subtype'
    And I enter He did it into 'treatmentevent-note'
    And I click on the 'save-treatment-event-button' button
    And I wait
    When I click on the 'save-button' button
    When I click on the 'submit-button' button

    Then I can see the value 'Outcome at 12 months No outcome recorded' for the field '12-month-outcome' in the 'TreatmentEvents' overview section
    And I can see the value 'Outcome at 24 months Cured' for the field '24-month-outcome' in the 'TreatmentEvents' overview section
    And I can see the value 'Treatment period 1' for the field '1' in the 'TreatmentEvents' overview section
    And I can see the value '01 Jan 2018 to 31 Jan 2019' for the field '1-dates' in the 'TreatmentEvents' overview section
    And I can see the value 'Treatment outcome - Cured' in the 'TreatmentEvents' table overview section
    And I can see the value 'Multi-drug resistant regimen' in the 'TreatmentEvents' table overview section
    And I can see the value 'Birmingham User' in the 'TreatmentEvents' table overview section

  Scenario: Create and delete notification draft
    When I click on the 'delete-draft-button' button
    Then I should be on the Delete page
    When I click on the 'confirm-deletion-button' button
    Then I should be on the Confirm page
    When I click on the 'return-to-homepage' button
    Then I should be on the Homepage
