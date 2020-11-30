<?php

	include 'config.php';

	$con = mysqli_connect($MyServer, $MyUser, $MyPassword, $MyDatabase);

	//check that connection happened
	if(mysqli_connect_errno())
	{
		echo "i"; //error code #1 = connection failed
		exit();
	}

	//playerdetails values
	$username = $_POST["username"]; //access name field
	$Avatarname = $_POST["Avatarname"];
	$AvatarID = $_POST["AvatarID"];
	$FirstLogin = $_POST["FirstLogin"];
	$Badge1 = $_POST["Badge1"];
	$Badge2 = $_POST["Badge2"];
	$Badge3 = $_POST["Badge3"];
	$ActivityStreak = $_POST["ActivityStreak"];
	$ActivityPoints = $_POST["ActivityPoints"];
	$ActivityPointsGained = $_POST["ActivityPointsGained"];
	$Level = $_POST["Level"];
	$XP = $_POST["XP"];
	$FreeStatPoints = $_POST["FreeStatPoints"];
	$Gold = $_POST["Gold"];
	$BestStageBeaten = $_POST["BestStageBeaten"];
	$Stages = $_POST["Stages"];
	$Inventory = $_POST["Inventory"];
	$Equipment = $_POST["Equipment"];
	$Stats = $_POST["Stats"];


	$updatequery = "UPDATE playerdetails SET  AvatarName = '" . $Avatarname . "', AvatarID = '" . $AvatarID . "', FirstLogin= " . $FirstLogin . ", Badge1 = " . $Badge1 . ", Badge2= " . $Badge2 . ", Badge3= " . $Badge3 . ", ActivityStreak = " . $ActivityStreak . ", ActivityPoints = " . $ActivityPoints . ", ActivityPointsGained= " . $ActivityPointsGained . ", Level= " . $Level . ", XP= " . $XP . ", FreeStatPoints= " . $FreeStatPoints . ", Gold= " . $Gold . ", BestStageBeaten = " . $BestStageBeaten . ", Stages= '" . $Stages . "', Inventory= '" . $Inventory . "', Equipment=  '" . $Equipment . "', Stats= '" . $Stats . "' WHERE username= '" . $username . "';";
	mysqli_query($con, $updatequery) or die("1: playerdetails save failed");

	echo("0");
?>