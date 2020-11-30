<?php

	include 'config.php';

	$con = mysqli_connect($MyServer, $MyUser, $MyPassword, $MyDatabase);

	//check that connection happened
	if(mysqli_connect_errno())
	{
		echo "i"; //error code #1 = connection failed
		exit();
	}

	$enemiesQuery = "SELECT * FROM enemies";

	$enemies = mysqli_query($con, $enemiesQuery) or die("1 : could not get enemies");

    while ($row = mysqli_fetch_array($enemies))
    {
    	$text = $row['id'] . "\t" . $row['name'] . "\t" . $row['description'] . "\t" . $row['xp'] . "\t" . $row['mingold'] . "\t" . $row['maxgold'] . "\t" . $row['stage'] . "\t" . $row['loot'] . "\t" . $row['stats'] . "\n";
    	echo utf8_encode($text);
    }
?>