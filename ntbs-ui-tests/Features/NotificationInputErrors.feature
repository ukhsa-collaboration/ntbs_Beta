Feature: Notification input errors
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

  Scenario: Create and submit notification without content
    When I click on the 'submit-button' button
    Then I should be on the PatientDetails page
    And I should see all submission error messages
    
  Scenario: NHS number validation works correctly
    When I enter 0000000000 into 'PatientDetails_NhsNumber'
    And I click on the 'save-button' button
    Then I can see the error 'This NHS number is not valid. If you do not know the NHS number please select "Not Known"'
    
    When I enter 100100100 into 'PatientDetails_NhsNumber'
    And I click on the 'save-button' button
    Then I can see the error 'NHS number needs to be 10 digits long'

    When I enter TreeSurgeon into 'PatientDetails_NhsNumber'
    And I click on the 'save-button' button
    Then I can see the error 'NHS number can only contain digits 0-9'

    When I enter 4333222111 into 'PatientDetails_NhsNumber'
    And I click on the 'save-button' button
    Then I can see the error 'This NHS number is not valid. Confirm you have entered it correctly'
    
  Scenario: Clinical date verification shows correct errors
    # Some clinical date validation requires a date of birth to compare against
    When I enter 4 into 'FormattedDob_Day'
    And I enter 8 into 'FormattedDob_Month'
    And I enter 2011 into 'FormattedDob_Year'
    And I click on the 'save-button' button
    And I click 'Clinical details' on the navigation bar
    
    When I check 'symptomatic-yes'
    And I enter 1 into 'FormattedSymptomDate_Day'
    And I enter 8 into 'FormattedSymptomDate_Month'
    And I enter 2011 into 'FormattedSymptomDate_Year'
    And I click on the 'save-button' button
    Then I can see the error 'Symptom onset date must be later than date of birth'
    
    When I enter 1 into 'FormattedSymptomDate_Day'
    And I enter 8 into 'FormattedSymptomDate_Month'
    And I enter 1977 into 'FormattedSymptomDate_Year'
    And I click on the 'save-button' button
    Then I can see the error 'Symptom onset date must not be before 01/01/2010'

    When I enter 1 into 'FormattedSymptomDate_Day'
    And I enter 8 into 'FormattedSymptomDate_Month'
    And I enter  into 'FormattedSymptomDate_Year'
    And I click on the 'save-button' button
    Then I can see the error 'Symptom onset date does not have a valid date selection'

  Scenario: Clinical date verification shows correct warnings
    When I click 'Clinical details' on the navigation bar
    When I check 'symptomatic-yes'
    And I enter 1 into 'FormattedSymptomDate_Day'
    And I enter 8 into 'FormattedSymptomDate_Month'
    And I enter 2012 into 'FormattedSymptomDate_Year'
    
    When I enter 14 into 'FormattedFirstPresentationDate_Day'
    And I enter 7 into 'FormattedFirstPresentationDate_Month'
    And I enter 2010 into 'FormattedFirstPresentationDate_Year'
    Then I see the warning 'Presentation to any health service is earlier than Symptom onset date' for 'date-warning-text-1'
    
    When I enter 9 into 'FormattedTbServicePresentationDate_Day'
    And I enter 11 into 'FormattedTbServicePresentationDate_Month'
    And I enter 2010 into 'FormattedTbServicePresentationDate_Year'
    Then I see the warning 'Presentation to TB service is earlier than Symptom onset date' for 'date-warning-text-2'

    When I enter 16 into 'FormattedTbServicePresentationDate_Day'
    And I enter 3 into 'FormattedTbServicePresentationDate_Month'
    And I enter 2010 into 'FormattedTbServicePresentationDate_Year'
    Then I see the warning 'Presentation to TB service is earlier than Presentation to any health service' for 'date-warning-text-2'
    
    When I enter 16 into 'FormattedDiagnosisDate_Day'
    And I enter 2 into 'FormattedDiagnosisDate_Month'
    And I enter 2010 into 'FormattedDiagnosisDate_Year'
    Then I see the warning 'Diagnosis date is earlier than Presentation to TB service' for 'date-warning-text-3'

    When I check 'treatment-yes'
    And I enter 7 into 'FormattedTreatmentDate_Day'
    And I enter 1 into 'FormattedTreatmentDate_Month'
    And I enter 2010 into 'FormattedTreatmentDate_Year'
    Then I see the warning 'Treatment start date is earlier than Diagnosis date' for 'date-warning-text-4'
    
    When I check 'home-visit-yes'
    And I enter 8 into 'FormattedHomeVisitDate_Day'
    And I enter 1 into 'FormattedHomeVisitDate_Month'
    And I enter 2010 into 'FormattedHomeVisitDate_Year'
    Then I see the warning 'First home visit date is earlier than Diagnosis date' for 'date-warning-text-5'

    When I check 'home-visit-yes'
    And I enter 1 into 'FormattedHomeVisitDate_Day'
    And I enter 1 into 'FormattedHomeVisitDate_Month'
    And I enter 2010 into 'FormattedHomeVisitDate_Year'
    Then I see the warning 'First home visit date is earlier than Treatment start date' for 'date-warning-text-5'
    
  Scenario: Contact tracing verification shows correct errors
    When I click 'Contact tracing' on the navigation bar
    When I enter 3 into 'ContactTracing_AdultsIdentified'
    And I enter 5 into 'ContactTracing_AdultsScreened'
    And I click on the 'save-button' button
    And I click on the 'save-button' button
    Then I can see the error 'Adults screened must not be greater than the number of adults identified'
    
    When I enter 3 into 'ContactTracing_AdultsScreened'
    And I enter 2 into 'ContactTracing_AdultsActiveTB'
    And I enter 2 into 'ContactTracing_AdultsLatentTB'
    And I click on the 'save-button' button
    And I click on the 'save-button' button
    Then I can see the error 'Adults with active TB must not be greater than number of adults screened minus those with latent TB'
    And I can see the error 'Adults with latent TB must not be greater than number of adults screened minus those with active TB'
    
    When I enter 12 into 'ContactTracing_ChildrenIdentified'
    And I enter 4 into 'ContactTracing_ChildrenStartedTreatment'
    And I enter 6 into 'ContactTracing_ChildrenFinishedTreatment'
    And I click on the 'save-button' button
    And I click on the 'save-button' button
    Then I can see the error 'Children that have started treatment must not be greater than number of children with latent TB'
    And I can see the error 'Children that have finished treatment must not be greater than number of children that started treatment'