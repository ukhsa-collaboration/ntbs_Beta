Feature: Transfer notification

  Scenario: Transfer notification between services
    Given I navigate to the app
    And I have logged in as BirminghamServiceUser
    And I am on seeded 'TO_TRANSFER' notification overview page
    Then I can see the value 'Birmingham & Solihull' for the field 'tb-service' in the 'HospitalDetails' overview section
    
    When I expand manage notification section
    And I click on the 'transfer-button' button
    And I enter Yorkshire and Humber into 'PhecCode'
    And I wait
    And I enter LCHC (Leeds Community Healthcare NHS Trust) into 'TransferRequest_TbServiceCode'
    And I check 'transfer-radio-Relocation'
    And I enter Patient didn't want to be treated outside Leeds into 'TransferRequest_TransferRequestNote'
    And I click on the 'confirm-transfer-button' button
    When I log out
    
    Given I choose to log in with a different account
    Given I have logged in as LeedsServiceUser
    Given I navigate to the url of the current notification
    And I take action on the alert with title Transfer request
    And I check 'accept-transfer-input'
    And I click on the 'submit-transfer-button' button
    And I click on the 'return-to-notification' button

    Then I should see the Notification
    Then I can see the value 'LCHC (Leeds Community Healthcare NHS Trust)' for the field 'tb-service' in the 'HospitalDetails' overview section