Feature: Denotify notifications

    Scenario: Denotify a notification
        Given I have logged in as ServiceUser
        Given I am on seeded 'TO_BE_DENOTIFIED' notification overview page
        When I expand manage notification section
        And I click on the 'denotify-button' button
        Then I should be on the Denotify page
        When I select radio value 'denotify-radio-DuplicateEntry'
        And I click on the 'confirm-denotification-button' button
        Then I should see the Notification
        And The notification should be denotified