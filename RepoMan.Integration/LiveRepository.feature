Feature: LiveRepository
	In order to write good integration tests
	As a TDD fanatic software developer
	I need to be able to interact with a real database

Scenario: Query an entity from the repository
	Given I have a Person entity in my database like:
	| FirstName |
	| Britton   |
	When I query the Person repository
	Then I should find 1 Person
	And the Person's name should be Britton

Scenario: Delete an entity from the repository
	Given I have a Person entity in my database like:
	| FirstName |
	| Britton   |
	When I delete the Person from the repository
	And I query the Person repository
	Then I should find 0 People

Scenario: Query with columns should only return selected columns
	Given I have a Person entity in my database like:
	| Id | FirstName |
	| 1   | Britton  |
	| 2   | SomeDude |
	When I query the Person repository for only Id
	Then The results should only have Id populated

Scenario: FirstOrDefault should return first match
	Given I have a Person entity in my database like:
	| Id | FirstName |
	| 1   | Britton  |
	| 2   | SomeDude |
	When I lookup the Person repository for the name Britton
	Then The Person's name should be Britton