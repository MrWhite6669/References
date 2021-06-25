<?php 

class Location{
	public $index;
	public $location;
	public $name;
	public $maxpoints;

	public function __construct($index,$location,$name,$maxpoints){
		$this->index = $index;
		$this->location = $location;
		$this->name = $name;
		$this->maxpoints = $maxpoints;
	}
}



$conn = new mysqli("#######","########","########","########");

if($conn->connect_error){
	exit();
}

$conn->set_charset("utf8");
$querry = "select * from tlapka_locations";

$result = $conn->query($querry);
$locations = array();

if($result->num_rows > 0){
	while($row = $result->fetch_assoc()){

	//$row = mb_convert_encoding($row, "utf8");
	//echo $row["lat"] . "," . $row["longitude"] . "," . $row["alt"] . "||" . $row["question"] . "<br>";
	array_push($locations,new Location($row["ID"],$row["location"],$row["serialCode"],$row["maxPoints"]));
	}

	echo json_encode($locations);
}

/*$conn->set_charset("utf8");
echo $conn->character_set_name();*/

?>
