<?php

	include 'config.php';

	$con = mysqli_connect($GymServer, $GymUser, $GymPassword, $GymDatabase);

	//check that connection happened
	if(mysqli_connect_errno())
	{
		echo "i"; //error code #1 = connection failed
		exit();
	}

	$courseID = mysqli_real_escape_string ($con, $_POST["courseID"]); //access name field
	$courseIDclean = filter_var($courseID, FILTER_SANITIZE_STRING, FILTER_FLAG_STRIP_LOW | FILTER_FLAG_STRIP_HIGH);
	
	$participantsQuery = "SELECT * FROM participating WHERE courseID = '" . $courseIDclean . "'; ";

	$participants = mysqli_query($con, $participantsQuery) or die("1 : could not get participants");

    while ($row = mysqli_fetch_array($participants))
    {
    	$text = $row['courseID'] . "\t" . $row['UserID'] . "\n";
    	echo utf8_encode($text);
    }
?>