<?php

	include 'config.php';

	$con = mysqli_connect($GymServer, $GymUser, $GymPassword, $GymDatabase);

	//check that connection happened
	if(mysqli_connect_errno())
	{
		echo "i"; //error code #1 = connection failed
		exit();
	}

	$scheduleQuery = "SELECT * FROM courses WHERE date(Dates) = CURDATE();";

	$schedule = mysqli_query($con, $scheduleQuery) or die("1 : could not get enemies");

    while ($row = mysqli_fetch_array($schedule))
    {
    	$text = $row['courseID'] . "\t" . $row['Dates'] . "\n";
    	echo utf8_encode($text);
    }
?>