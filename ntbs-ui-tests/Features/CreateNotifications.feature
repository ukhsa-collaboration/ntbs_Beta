Feature: Notification creation
    Happy and error paths for notification creation

    Background: Create new notification
        Given I am on the Search page
        When I enter 1 into 'SearchParameters_IdFilter'
        And I click on 'search-button'
        Then I should be on the Search page
        When I click on 'create-button'
        Then I should be on the Patient page

    Scenario: Create and submit notification without errors
        # Patient page
        When I enter Test into 'Patient_GivenName'
        And I enter User into 'Patient_FamilyName'
        And I enter 1 into 'FormattedDob_Day'
        And I enter 1 into 'FormattedDob_Month'
        And I enter 1970 into 'FormattedDob_Year'
        And I select radio value 'sexId-1'
        And I select 1 for 'Patient_EthnicityId'
        And I select radio value 'nhs-number-unknown'
        And I select 1 for 'Patient_CountryId'
        And I select radio value 'postcode-no'
        And I click on 'save-button'

        # Episode page
        Then I should be on the Episode page
        When I enter 1 into 'FormattedNotificationDate_Day'
        And I enter 1 into 'FormattedNotificationDate_Month'
        And I enter 2019 into 'FormattedNotificationDate_Year'
        And I select TBS0008 for 'Episode_TBServiceCode'
        # Wait until javascript has populated the hospital dropdown
        And I wait for 1 second
        And I select 868e426f-b11d-45a3-bf2c-e0c31bed2c44 for 'Episode_HospitalId'
        And I click on 'save-button'

        # Clinical details page
        Then I should be on the ClinicalDetails page
        When I check 'NotificationSiteMap_PULMONARY_'
        When I enter 1 into 'FormattedDiagnosisDate_Day'
        And I enter 1 into 'FormattedDiagnosisDate_Month'
        And I enter 2018 into 'FormattedDiagnosisDate_Year'
        And I click on 'save-button'

        Then I should be on the ContactTracing page
        When I click on 'submit-button'
        Then I should see the Notification

    Scenario: Create and submit notification without content
        When I click on 'submit-button'
        Then I should be on the Patient page
        And I should see all submission error messages

    Scenario: Create and deelte notification draft
        When I click on 'delte-draft-button'
        Then I should be on the Delete page
        When I click on 'confirm-deletion-button'
        Then I should be on the Confirm page
        When I click on 'return-to-homepage'
        Then I should be on the Homepage
