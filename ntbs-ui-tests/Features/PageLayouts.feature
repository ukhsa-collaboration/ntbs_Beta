Feature: Page layouts

  Background: Create a transfer request
    Given I navigate to the app
    And I have logged in as BirminghamServiceUser
    
  Scenario: The homepage has correct titles for tables
    Given I am on the Homepage
    Then I can see the correct titles for the 'alerts-table' table
    Then I can see the correct titles for the 'draft-notifications-table' table
    Then I can see the correct titles for the 'recent-notifications-table' table
    
  Scenario: The notification overview data has correct labels
    Given I am on seeded 'MAXIMUM_DETAILS' notification overview page
    Then I can see the correct labels for the 'PatientDetails' overview section
    Then I can see the correct labels for the 'HospitalDetails' overview section
    Then I can see the correct labels for the 'ClinicalDetails' overview section
    Then I can see the correct labels for the 'ContactTracing' overview section
    Then I can see the correct labels for the 'SocialRiskFactors' overview section
    Then I can see the correct labels for the 'Travel' overview section
    Then I can see the correct labels for the 'Comorbidities' overview section
    Then I can see the correct labels for the 'PreviousHistory' overview section
