<?php

	include 'config.php';

	$con = mysqli_connect($MyServer, $MyUser, $MyPassword, $MyDatabase);

	//check that connection happened
	if(mysqli_connect_errno())
	{
		echo "i"; //error code #1 = connection failed
		exit();
	}
	
	$username = mysqli_real_escape_string ($con, $_POST["username"]); //access name field
	$usernameclean = filter_var($username, FILTER_SANITIZE_STRING, FILTER_FLAG_STRIP_LOW | FILTER_FLAG_STRIP_HIGH);

	$playerdetails = "SELECT * FROM playerdetails WHERE username ='" . $usernameclean . "'; ";

	$details = mysqli_query($con, $playerdetails) or die("1 : could not get playerdetails");

	if(mysqli_num_rows($details) > 1)
	{
		echo "3 : multiple rows in playerdetails found"; //error code "3" - name exists in register
		exit();
	}

     $rowdetails = mysqli_fetch_array($details);

     echo utf8_encode($rowdetails['AvatarID'] . "\t" . $rowdetails['FirstLogin'] . "\t" . $rowdetails['Badge1'] . "\t" . $rowdetails['Badge2'] . "\t" . $rowdetails['Badge3'] . "\t" . $rowdetails['ActivityStreak'] . "\t" . $rowdetails['ActivityPoints'] . "\t" . $rowdetails['ActivityPointsGained'] . "\t" . $rowdetails['Level'] . "\t" . $rowdetails['XP'] . "\t" . $rowdetails['FreeStatPoints'] . "\t" . $rowdetails['Gold'] . "\t" . $rowdetails['BestStageBeaten'] . "\t" . $rowdetails['Stages'] . "\t" . $rowdetails['Inventory'] . "\t" . $rowdetails['Equipment'] . "\t" . $rowdetails['Stats']);
?>