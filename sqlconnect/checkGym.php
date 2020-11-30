<?php

	include 'config.php';

	$con = mysqli_connect($GymServer, $GymUser, $GymPassword, $GymDatabase);

	//check that connection happened
	if(mysqli_connect_errno())
	{
		echo "i"; //error code #1 = connection failed
		exit();
	}

	$username = mysqli_real_escape_string ($con, $_POST["name"]); //access name field
	$usernameclean = filter_var($username, FILTER_SANITIZE_STRING, FILTER_FLAG_STRIP_LOW | FILTER_FLAG_STRIP_HIGH);

	//check if name exists
	$namecheckquery = "SELECT Username FROM userbase WHERE Username ='" . $usernameclean . "'; ";

	$namecheck = mysqli_query($con, $namecheckquery) or die("2: Name check query failed");

	if(mysqli_num_rows($namecheck) == 0)
	{
		echo "3 : user does not exist in gym data base"; //error code "3" - name exists in register
		exit();
	}

	echo("0");

?>