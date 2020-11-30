<?php

	include 'config.php';

	$con = mysqli_connect($MyServer, $MyUser, $MyPassword, $MyDatabase);

	//check that connection happened
	if(mysqli_connect_errno())
	{
		echo "i"; //error code #1 = connection failed
		exit();
	}

	$itemQuery = "SELECT * FROM items";

	$items = mysqli_query($con, $itemQuery) or die("1 : could not get items");

    while ($row = mysqli_fetch_array($items))
    {
    	echo utf8_encode($row['id'] . "\t" . $row['name'] . "\t" . $row['description'] . "\t" . $row['type'] . "\t" . $row['cost'] . "\t" . $row['stats'] . "\t" . $row['Duration'] . "\t" . $row['Charges'] . "\n");
    } 
?>