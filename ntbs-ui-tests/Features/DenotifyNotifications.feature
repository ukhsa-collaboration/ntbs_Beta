Feature: Denotify notifications

    Scenario: Denotify a notification
      Given I navigate to the app
      Given I have logged in as BirminghamServiceUser
      Given I am on seeded 'MINIMAL_DETAILS' notification overview page
      When I expand manage notification section
      And I click on the 'denotify-button' button
      Then I should be on the Denotify page
      When I select radio value 'denotify-radio-DuplicateEntry'
      And I click on the 'confirm-denotification-button' button
      Then I should see the Notification
      And The notification should be denotified