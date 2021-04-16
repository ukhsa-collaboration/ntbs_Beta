Feature: M. bovis

  Background: Create new notification
    Given I have logged in as ServiceUser

  Scenario: Fill in M. bovis exposure to known cases page
    Given I am on seeded 'M_BOVIS' notification overview page
    When I go to edit the 'overview-mbovis-exposure-details' section
    Then I should be on the ExposureToKnownCases page
    When I select radio value 'has-exposure-yes'
    And I click on the 'add-new-button' button
    Then I should be on the New page
    When I select Household for 'MBovisExposureToKnownCase_ExposureSetting'
    And I select radio value 'notified-no'
    And I click on the 'save-button' button
    Then I should be on the ExposureToKnownCases page
    
  Scenario: Fill in M. bovis consumption of unpasteurised milk page
    Given I am on seeded 'M_BOVIS' notification overview page
    When I go to edit the 'overview-mbovis-milk-consumption-details' section
    Then I should be on the UnpasteurisedMilkConsumptions page
    When I select radio value 'has-milk-consumption-yes'
    And I click on the 'add-new-button' button
    Then I should be on the New page
    When I select Milk for 'MBovisUnpasteurisedMilkConsumption_MilkProductType'
    And I click on the 'save-button' button
    Then I should be on the UnpasteurisedMilkConsumptions page
    
  Scenario: Fill in M. bovis occupation exposure page
    Given I am on seeded 'M_BOVIS' notification overview page
    When I go to edit the 'overview-mbovis-occupation-exposure' section
    Then I should be on the OccupationExposures page
    When I select radio value 'has-exposure-yes'
    And I click on the 'add-new-button' button
    Then I should be on the New page
    When I select Vet for 'MBovisOccupationExposure_OccupationSetting'
    And I click on the 'save-button' button
    Then I should be on the OccupationExposures page
    
  Scenario: Fill in M. bovis animal exposure page
    Given I am on seeded 'M_BOVIS' notification overview page
    When I go to edit the 'overview-mbovis-animal-exposure-details' section
    Then I should be on the AnimalExposures page
    When I select radio value 'has-exposure-yes'
    And I click on the 'add-new-button' button
    Then I should be on the New page
    When I select Wild animal for 'MBovisAnimalExposure_AnimalType'
    And I click on the 'save-button' button
    Then I should be on the AnimalExposures page
    