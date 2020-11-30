<?php

	include 'config.php';

	$con = mysqli_connect($MyServer, $MyUser, $MyPassword, $MyDatabase);

	//check that connection happened
	if(mysqli_connect_errno())
	{
		echo "i"; //error code #1 = connection failed
		exit();
	}

	$playersQuery = "SELECT * FROM playerdetails";

	$players = mysqli_query($con, $playersQuery) or die("1 : could not get players");

    while ($row = mysqli_fetch_array($players))
    {
    	$text = $row['username'] . "\t" . $row['AvatarID'] . "\t" . $row['FirstLogin'] . "\t" . $row['Badge1'] . "\t" . $row['Badge2'] . "\t" . $row['Badge3'] . "\t" . $row['ActivityStreak'] . "\t" . $row['ActivityPoints'] . "\t" . $row['ActivityPointsGained'] . "\t" . $row['Level'] . "\t" . $row['XP'] . "\t" . $row['FreeStatPoints'] . "\t" . $row['Gold'] . "\t" . $row['BestStageBeaten'] . "\t" . $row['Stages'] . "\t" . $row['Inventory'] . "\t" . $row['Equipment'] . "\t" . $row['Stats'] . "\n";
    	echo utf8_encode($text);
    }
?>