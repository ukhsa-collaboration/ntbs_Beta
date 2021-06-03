Feature: Read only user

  Background: Log in as read only user
    Given I navigate to the app
    And I have logged in as ReadOnlyUser
    
  Scenario: Read only user has no edit links abd view test result link on notification overview
    Given I am on seeded 'MAXIMUM_DETAILS' notification overview page

    Then The 'view' link is present in the 'TestResults' overview section
    Then The 'edit' link is not present in the 'PatientDetails' overview section
    Then The 'edit' link is not present in the 'HospitalDetails' overview section
    Then The 'edit' link is not present in the 'ClinicalDetails' overview section
    Then The 'edit' link is not present in the 'TestResults' overview section
    Then The 'edit' link is not present in the 'ContactTracing' overview section
    Then The 'edit' link is not present in the 'SocialRiskFactors' overview section
    Then The 'edit' link is not present in the 'Travel' overview section
    Then The 'edit' link is not present in the 'Comorbidities' overview section
    Then The 'edit' link is not present in the 'SocialContextAddresses' overview section
    Then The 'edit' link is not present in the 'SocialContextVenues' overview section
    Then The 'edit' link is not present in the 'PreviousHistory' overview section
    Then The 'edit' link is not present in the 'TreatmentEvents' overview section
    Then The 'edit' link is not present in the 'MDRDetails' overview section
