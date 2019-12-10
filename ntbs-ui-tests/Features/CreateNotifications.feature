Feature: Notification creation
    Happy and error paths for notification creation

    Background: Create new notification
        Given I am on the Search page
        When I enter 1 into 'SearchParameters_IdFilter'
        And I click on the 'search-button' button
        Then I should be on the Search page
        When I click on the 'create-button' button
        Then I should be on the PatientDetails page

    Scenario: Create and submit notification without errors
        # PatientDetails page
        When I enter Test into 'PatientDetails_GivenName'
        And I enter User into 'PatientDetails_FamilyName'
        And I enter 1 into 'FormattedDob_Day'
        And I enter 1 into 'FormattedDob_Month'
        And I enter 1970 into 'FormattedDob_Year'
        And I select radio value 'sexId-1'
        And I select 1 for 'PatientDetails_EthnicityId'
        And I select radio value 'nhs-number-unknown'
        And I enter Afghanistan into 'PatientDetails_CountryId'
        And I wait
        And I select radio value 'postcode-no'
        And I click on the 'save-button' button

        # Episode page
        Then I should be on the Episode page
        When I enter 1 into 'FormattedNotificationDate_Day'
        And I enter 1 into 'FormattedNotificationDate_Month'
        And I enter 2019 into 'FormattedNotificationDate_Year'
        And I select TBS0008 for 'Episode_TBServiceCode'
        # Wait until javascript has populated the hospital dropdown
        And I wait
        And I select 868e426f-b11d-45a3-bf2c-e0c31bed2c44 for 'Episode_HospitalId'
        And I click on the 'save-button' button

        # Clinical details page
        Then I should be on the ClinicalDetails page
        When I check 'NotificationSiteMap_PULMONARY_'
        And I enter 1 into 'FormattedDiagnosisDate_Day'
        And I enter 1 into 'FormattedDiagnosisDate_Month'
        And I enter 2018 into 'FormattedDiagnosisDate_Year'
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
        
        # Contact tracing page + submission
        Then I should be on the ContactTracing page
        When I click on the 'submit-button' button
        Then I should see the Notification

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


    Scenario: Denotify a notification
        # TODO NTBS-663
        # This is an aweful copy-paste to make sure the notification is created.
        # We should figure out a better way of seeding these
        # A half-way point would be to at least create a separate feature file, but
        # that's currentyl blocked by the setup which for some reason ignores requets
        # to run features sequentially, but fials in parallel due to port competition
        When I enter Test into 'PatientDetails_GivenName'
        And I enter User into 'PatientDetails_FamilyName'
        And I enter 1 into 'FormattedDob_Day'
        And I enter 1 into 'FormattedDob_Month'
        And I enter 1970 into 'FormattedDob_Year'
        And I select radio value 'sexId-1'
        And I select 1 for 'PatientDetails_EthnicityId'
        And I select radio value 'nhs-number-unknown'
        And I enter Afghanistan into 'PatientDetails_CountryId'
        And I wait
        And I select radio value 'postcode-no'
        And I click on the 'save-button' button

        Then I should be on the Episode page
        When I enter 1 into 'FormattedNotificationDate_Day'
        And I enter 1 into 'FormattedNotificationDate_Month'
        And I enter 2019 into 'FormattedNotificationDate_Year'
        And I select TBS0008 for 'Episode_TBServiceCode'
        And I wait
        And I select 868e426f-b11d-45a3-bf2c-e0c31bed2c44 for 'Episode_HospitalId'
        And I click on the 'save-button' button

        Then I should be on the ClinicalDetails page
        When I check 'NotificationSiteMap_PULMONARY_'
        When I enter 1 into 'FormattedDiagnosisDate_Day'
        And I enter 1 into 'FormattedDiagnosisDate_Month'
        And I enter 2018 into 'FormattedDiagnosisDate_Year'
        And I click on the 'save-button' button

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

        Then I should be on the ContactTracing page
        When I click on the 'submit-button' button
        Then I should see the Notification

        # This is where the actual test starts
        Given I am on current notification overview page
        When I expand manage notification section
        And I click on the 'denotify-button' button
        Then I should be on the Denotify page
        When I select radio value 'denotify-radio-DuplicateEntry'
        And I click on the 'confirm-denotification-button' button
        Then I should see the Notification
        And The notification should be denotified
