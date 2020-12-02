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
This is the app that members of a fitness center can use to actually play the game. The Mobile App connects to a MySQL database on a local server
to load game and player data from the 'GameData' database. Before the players can login, they need to sign up first with the same name, they used 
to sign up for the gym. This is cross-checked with the 'dymmygym' data.

The Public Display App is a Windows Build App intended to be run locally on a big screen display in the gym. It contains leaderboards of the best
gym members in various categories, displaying the avatars    

# Project Contents:

The complete Unity Project SourceCode folder is available under FitnessRPG Source (for both Mobile App and Public Display App components).

Written code / script files can be found under FitnessRPG Source/FitnessRPG/Assets/Scripts/.

A Windows executable build of the 'Mobile App' component under FitnessRPGMA (currently won't work without a local server + MYSQL database setup).

TODO/Upcoming: 

Executable Android Build of the Mobile App Version with Fingerprint Interactions and Gestures (requiring a local server + database).

Executable Windows Build of the Public Display App Version of the Game under FitnessRPGPD (will also require a local server + MySQL database setup).

'Offline' executable Versions of both Mobile App and Public Display App to run without requiring a local network + database setup.

 

# Installation:

Setting up a local web server with a MySQL database:
The recommended (and utilized) way is to install XAMPP. Once installed press 'Start' in the XAMPP control panel for the Apache and MySQL Module. 

Creating and importing data into a database: 
Press 'Admin' on the MySQL Module in the XAMPP control panel. This will open phpMyAdmin in your web browser. First you'll need to create a 
new user and a password, then create two empty databases 'dummygym' and 'unityaccess'. Import the player and game data into them by 
clicking 'Import' and browsing the two sql-Files 'dummygym.sql' and 'unityaccess.sql' included in the project. 

Adding php files folder to the local web server:
Open your installed XAMPP folder, usually found in C:\xampp. Copy the folder 'sqlconnect' included in the project into the subfolder htdocs. 
The contents of the htdocs folder are accessed by the localhost URL. The sqlconnect folder contains all php-files that are called by our apps
via Webrequests to access our database. Open the file config.php in the sqlconnect folder and make sure that the database names,
your username and password you used in phpMyAdmin match the ones in this file. (You can rename them as you like).

Now our mobile app and public display app should be able to connect to our database, provided the Apache server and the MySQL Module are both
running.
