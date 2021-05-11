Feature: Linked notification creation
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
    When I enter Trevor into 'PatientDetails_GivenName'
    And I enter Collins into 'PatientDetails_FamilyName'
    And I enter 6 into 'FormattedDob_Day'
    And I enter 6 into 'FormattedDob_Month'
    And I enter 1966 into 'FormattedDob_Year'
    And I select radio value 'sexId-1'
    And I select White for 'PatientDetails_EthnicityId'
    And I select radio value 'nhs-number-unknown'
    And I select United Kingdom from input list 'PatientDetails_CountryId'
    And I select radio value 'postcode-no'
    And I click on the 'save-button' button
    # Mandatory hospital details fields
    And I select Birmingham & Solihull for 'HospitalDetails_TBServiceCode'
    And I wait
    And I select SPIRE PARKWAY HOSPITAL for 'HospitalDetails_HospitalId'
    And I click on the 'save-button' button
    # Mandatory clinical details fields
    When I check 'NotificationSiteMap_PULMONARY_'
    And I enter 17 into 'FormattedDiagnosisDate_Day'
    And I enter 5 into 'FormattedDiagnosisDate_Month'
    And I enter 2018 into 'FormattedDiagnosisDate_Year'
    And I click on the 'save-button' button
    # Mandatory test fields
    When I select radio value 'test-carried-out-no'
    And I click on the 'save-button' button
    And I click 'Hospital details' on the navigation bar

  Scenario: Create linked notification which has matching patient details
    # Enter notification date over a year ago
    When I enter 12 into 'FormattedNotificationDate_Day'
    And I enter 8 into 'FormattedNotificationDate_Month'
    And I enter 2017 into 'FormattedNotificationDate_Year'
    And I click on the 'submit-button' button
    And I expand manage notification section
    And I click on the 'new-linked-notification-button' button
    
    Then A new notification should have been created

    When I click on the 'save-button' button
    When I enter 20 into 'FormattedNotificationDate_Day'
    And I enter 2 into 'FormattedNotificationDate_Month'
    And I enter 2020 into 'FormattedNotificationDate_Year'
    And I select Birmingham & Solihull for 'HospitalDetails_TBServiceCode'
    And I wait
    And I select SPIRE PARKWAY HOSPITAL for 'HospitalDetails_HospitalId'
    And I click on the 'save-button' button
    When I check 'NotificationSiteMap_LARYNGEAL_'
    And I enter 10 into 'FormattedDiagnosisDate_Day'
    And I enter 2 into 'FormattedDiagnosisDate_Month'
    And I enter 2020 into 'FormattedDiagnosisDate_Year'
    And I click on the 'save-button' button
    When I select radio value 'test-carried-out-no'
    And I click on the 'submit-button' button

    Then I can see the value 'COLLINS, Trevor' for the field 'full-name' in the 'PatientDetails' overview section
    And I can see the value 'Male' for the field 'sex' in the 'PatientDetails' overview section
    And I can see the value 'Not known' for the field 'nhs-number' in the 'PatientDetails' overview section
    And I can see the value '06 Jun 1966' for the field 'dob' in the 'PatientDetails' overview section
    And I can see the value 'No fixed abode' for the field 'postcode' in the 'PatientDetails' overview section
    And I can see the value 'White' for the field 'ethnicity' in the 'PatientDetails' overview section
    And I can see the value 'United Kingdom' for the field 'country' in the 'PatientDetails' overview section
  
  Scenario: No option to create linked notification for recent notification
    # Enter notification date within a year
    When I enter 7 into 'FormattedNotificationDate_Day'
    And I enter 5 into 'FormattedNotificationDate_Month'
    And I enter 2021 into 'FormattedNotificationDate_Year'
    And I click on the 'submit-button' button
    And I expand manage notification section
    
    Then I element with id 'new-linked-notification-button' is not present