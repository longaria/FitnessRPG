<?php

	include 'config.php';

	$con = mysqli_connect($MyServer, $MyUser, $MyPassword, $MyDatabase);
	$con1 = mysqli_connect($GymServer, $GymUser, $GymPassword, $GymDatabase);

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
	$namecheckquery = "SELECT username FROM Players WHERE username ='" . $usernameclean . "'; ";

	$namecheck = mysqli_query($con, $namecheckquery) or die("2: Name check query failed");

	if(mysqli_num_rows($namecheck) > 0)
	{
		echo "3 : name already exists"; //error code "3" - name exists in register
		exit();
	}

	//add user to the table
	$salt = "\$5\$rounds=5000\$" . "steamedhams" . $username . "\$";
	$hash = crypt($password, $salt);

	$insertuserquery = "INSERT INTO players (username, hash, salt) VALUES ('" . $username . "', '" . $hash . "', '" . $salt . "');" ;
	mysqli_query($con, $insertuserquery) or die("4: insert player query failed");

	$insertuserquery1 = "INSERT INTO playerdetails  VALUES ('" . $username . "', '" . $username . "', '', 1, 0, 0, 0, 1.0, 0, 50, 1, 0, 0, 500, 0, '' ,'', '', 'Strength,5,Dexterity,5,Intelligence,5,Vitality,5,MinDmg,10,MaxDmg,30,PhysicalDefense,10,MinMDmg,10,MaxMDmg,10,MagicDefense,10,AttackSpeed,3,CritChance,5,DodgeChance,5,Recovery,5,Endurance,100,Health,200,DamageM,1,PhysicalDefenseM,1,MagicDefenseM,1,HealthM,1,');" ;
	mysqli_query($con, $insertuserquery1) or die("5: insert playerdetails query failed");

	$insertuserquery2 = "INSERT INTO userbase (vorname, name, username) VALUES ('', '', '" . $username . "');" ;
	mysqli_query($con1, $insertuserquery2) or die("6: insert into gym userbase query failed");

	$insertuserquery3 = "INSERT INTO participating (courseID, userID) VALUES (3, '". $username . "');" ;
	mysqli_query($con1, $insertuserquery3) or die("7: insert into gym participating query failed");

	echo("0");

?>