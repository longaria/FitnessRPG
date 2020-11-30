# FitnessRPG

# Project Description:
 
'FitnessRPG' was a Bachelors Thesis Project, in which the idea of using Gamification in a Fitness Center Context was 
explored in order to motivate its members to exercise more often and more consistently.

Gamification: The use of Gaming Elements (like badges, points, achievments, sense of progression, competition etc.) in 
a non-gaming context for motivational purposes.

The project consists of a classical RPG game developed in Unity/C# for Mobile Phones and Windows.
It contains virtual resources called 'Activity Points' (AP) that are tied to the activity and participation in a (physical) gym.
AP are required to progress in the game by challenging monsters, acquiring experience points, loot etc.
AP are earned by booking courses on the website of a fitness center.

#Setup:

The whole project consists of three components - Mobile App, Public Display App, Local Server running MySQL containing 
two databases: a 'DummyGym' database and a 'GameData' database. 

'DummyGym' simulates the database of a physical gym containing membership information and an upcoming courses schedule.
'GameData' contains player information of registered players, tracking their activity points, stats etc. 
It also contains important game data like item and monster stats.  

The Mobile App is the game running on a mobile phone (build in Unity as an Android build, but also works as an iOS or Windows Desktop Build).
This is the app that members of a fitness center can use to actually play the game. The Mobile App connects to a local server

# Project Contents:

The complete Unity Project SourceCode folder is available under FitnessRPG Source (for both Mobile App and Public Display App components).
A Windows executable build of the 'Mobile App' component under FitnessRPGMA (currently won't work without a local server + MYSQL database setup).

TODO/Upcoming: 

Executable Android Build of the Mobile App Version with Fingerprint Interactions and Gestures (requiring a local server + database).
Executable Windows Build of the Public Display App Version of the Game under FitnessRPGPD (will also require a local server + MySQL database setup).
'Offline' executable Versions of both Mobile App and Public Display App to run without requiring a local network + database setup.

 

# Installation: