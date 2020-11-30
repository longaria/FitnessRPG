<?php

	include 'config.php';

	$con = mysqli_connect($MyServer, $MyUser, $MyPassword, $MyDatabase);

	//check that connection happened
	if(mysqli_connect_errno())
	{
		echo "i"; //error code #1 = connection failed
		exit();
	}

	$username = mysqli_real_escape_string ($con, $_POST["name"]); //access name field
	$usernameclean = filter_var($username, FILTER_SANITIZE_STRING, FILTER_FLAG_STRIP_LOW | FILTER_FLAG_STRIP_HIGH);
	$password = $_POST["password"];

	//check if name exists
	$namecheckquery = "SELECT * FROM Players WHERE username ='" . $usernameclean . "'; ";

	$namecheck = mysqli_query($con, $namecheckquery) or die("2: Name check query failed");

	if(mysqli_num_rows($namecheck) != 1)
	{
		echo "5 : Either no user with name or more than one" ; //error code "5" - number of names matching not one
		exit();
	}

	//get login info from query
	$existinginfo = mysqli_fetch_assoc($namecheck);
	$salt = $existinginfo["salt"];
	$hash = $existinginfo["hash"];

	$loginhash = crypt($password, $salt);
	if($hash != $loginhash)
	{
		echo "6: Incorrect Password";
		exit();
	}

	echo("0");

?>