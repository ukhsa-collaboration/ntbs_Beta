Feature: Import legacy notification

  Scenario: Import legacy notification
    Given I navigate to the app
    And I have logged in as BirminghamServiceUser
    And I am on the Search page
    When I enter 189045 into 'SearchParameters_IdFilter'
    And I click on the 'search-button' button
    And I wait
    And I click on the 189045 link
    And I click on the 'import-button' button
    And I wait

    Then A new notification should have been created
    And I can see the value 28 Jul 2014 for element with id 'banner-notification-date'
    And I can see the value Birmingham & Solihull for element with id 'banner-tb-service'
    And I can see the value Jeens Parasseril for element with id 'banner-case-manager'
    And I can see the value STROBLE, Esteban for element with id 'banner-name'
    And I can see the value 18 Jul 1955 for element with id 'banner-dob'
    And I can see the value Angola for element with id 'banner-country-of-birth'
    And I can see the value 963 657 7830 for element with id 'banner-nhs-number'
    And I can see the value Male for element with id 'banner-sex'
    And I can see the value Sensitive to first line for element with id 'banner-drug-resistance'
    And I can see the value Complete for element with id 'banner-treatment-outcome'