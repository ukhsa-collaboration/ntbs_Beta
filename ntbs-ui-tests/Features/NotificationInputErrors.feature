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

  Scenario: Create and delete notification draft
    When I click on the 'delete-draft-button' button
    Then I should be on the Delete page
    When I click on the 'confirm-deletion-button' button
    Then I should be on the Confirm page
    When I click on the 'return-to-homepage' button
    Then I should be on the Homepage
