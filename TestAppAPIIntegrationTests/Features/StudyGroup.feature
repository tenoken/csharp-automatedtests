Feature: Study Group

# Check creation data
# We want to record when Study Groups were created;
@smoke @positive
Scenario Outline: Create study group
Given there is no group created for the avaiable subjects
When I set the name <name> and subject <subject> 
And I send the request
Then I receive the status <http_status> from the server
And I check if the creation date was recorded
And I check if it successfully added to database

Examples: 
	| name					   | subject   | http_status |
	| Irrationals			   | Math      | 200         |
	| Breaking Bad			   | Chemistry | 200         |
	| Super Massive Black Hole | Physics   | 200         |

# Users are able to create only one Study Group for a single Subject;
@negative @ignore
Scenario Outline: Create more than one study group for same subject
Given the study groups are already created
When I set the name <name> and subject <subject> 
And I send the request
Then I receive the status <http_status> from the server
And error message is equal to <message>

Examples: 
	| name                     | subject   | http_status | message                                        |
	| Irrationals              | Math      | 400         | There is already a group for selected subject. |
	| Breaking Bad             | Chemistry | 400         | There is already a group for selected subject. |
	| Super Massive Black Hole | Physics   | 400         | There is already a group for selected subject. |

# Users can provide a name for the Study Group with size between 5-30 characters
@negative
Scenario Outline: Create a group with invalid name length
When I set the name <name> and subject <subject> 
And I send the request
Then I receive the status <http_status> from the server

Examples: 
	| name                               | subject | http_status |
	| Pi                                 | Math    | 400         |
	| A name that should not be accepted | Math    | 400         |

# The only valid Subjects are: Math, Chemistry and Physics;
@negative
Scenario Outline: Create a group study with no valid subject
When I set the name <name> and subject <subject> 
And I send the request
Then I receive the status <http_status> from the server

Examples: 
	| name	    | subject   | http_status |
	| Supernova	| Astronomy | 400         |

# Users can join Study Groups for different Subjects;
@smoke @positive @user
Scenario Outline: Join to a study group
Given I am a valid user <user>
And the study groups are already created
And the study group about <subject> I want to join exists 
When choose to join the study group
Then I receive the status <http_status>  from the server

Examples: 
	| user			| subject   | http_status |
	| Walter White	| Chemistry | 200         |
	| Walter White	| Math	    | 200         |
	| Walter White	| Physics   | 200         |

# Users can check the list of all existing Study Groups
@smoke @positive
Scenario Outline: Check study groups list
Given the study groups are already created
When I check the avaiable study groups 
Then I receive the status <http_status>  from the server
And the number of avaiable study groups is equal to <study_groups_count>

Examples: 
	| http_status | study_groups_count |
	| 200         | 3                  |

# Users can also filter Study Groups by a given Subject
@smoke @positive 
Scenario Outline: Search for avaiable study groups filtering by subject
Given the study groups are already created
When I set the name <name> and subject <subject> 
And I send the request
And I search for groups by subject <subject>
Then I receive the status <http_status> from the server
And reponse has subject equals to <subject>
And the number of avaiable study groups is equal to <study_groups_count>

Examples: 
	| name			| subject   | http_status | study_groups_count |
	| Breaking Bad  | Chemistry | 200         | 2                  |

# Users can sort to see most recently created Study Groups or oldest ones
@smoke @positive
Scenario Outline: Search for avaiable study groups filtering by creation date
Given the study groups are already created from <date> with <days> days past
When I search for groups by creation date <date> and sorted <sorted>
Then I receive the status <http_status> from the server
And the number of avaiable study groups is equal to <study_groups_count>
And validate the expected date <expectedDate>

Examples: 
	| date       | days | sorted | http_status | study_groups_count | expectedDate |
	| 2024-07-07 | 3    | true   | 200         | 3                  | 2024-07-10   |
	| 2024-07-07 | 3	| false  | 200         | 3                  | 2024-07-08   |

# Users can leave Study Groups they joined
@smoke @positive @user
Scenario Outline: Leave from a study group
Given I am an user <user> that alredy is part of a study group <subject>
When choose to leave the study group
Then I receive the status <http_status> from the server

Examples: 
	| user			| subject   | http_status |
	| Walter White	| Chemistry | 200         |