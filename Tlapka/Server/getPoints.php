<?php 

class Answer{
	public $text;
	public $score;
	public $right;
}

class Question{
	public $text;
	public $answers = array();

	public function __construct($text,$number){
		$this->text = $text;

$conn = new mysqli("#######","########","########","########");

		if($conn->connect_error){
			exit();
		}

		$conn->set_charset("utf8");
		$querry1 = "select * from tlapka_answers where otazka = ".$number;
		$result1 = $conn->query($querry1);

		if($result1->num_rows > 0){
			while($row1 = $result1->fetch_assoc()){
				//$row1 = mb_convert_encoding($row1, "utf8");
				$answer = new Answer();
				$answer->text = $row1["odpoved"];
				$answer->score = $row1["body"];
				$answer->right = $row1["spravna"];
				array_push($this->answers,$answer);
			}
		}
		else{
			echo "No answers on question";
		}	
	}	
}

class Point{
	public $lat;
	public $long;
	public $alt;
	public $question;
	public $image;

	public function __construct($lat,$long,$alt,$question,$image){
		$this->lat = $lat;
		$this->long = $long;
		$this->alt = $alt;
		$this->image = $image;

$conn = new mysqli("#######","########","########","########");

		if($conn->connect_error){
			exit();
		}

		$conn->set_charset("utf8");

		$querry2 = "select * from tlapka_questions where ID = ".$question;
		$result2 = $conn->query($querry2);

			if($result2->num_rows > 0){
				$row2 = $result2->fetch_assoc();
				//$row2 = mb_convert_encoding($row2, "utf8");
				$this->question = new Question($row2["otazka"],$row2["ID"]);
			}
	}
}



$conn = new mysqli("#######","########","########","########");

if($conn->connect_error){
	exit();
}

$conn->set_charset("utf8");
$location = $_POST["location"];
$querry = "select * from tlapka_points where location = ".$location;

$result = $conn->query($querry);
$points = array();

if($result->num_rows > 0){
	while($row = $result->fetch_assoc()){
	//$row = mb_convert_encoding($row, "utf8");
	//echo $row["lat"] . "," . $row["longitude"] . "," . $row["alt"] . "||" . $row["question"] . "<br>";
	array_push($points,new Point($row["lat"],$row["longitude"],$row["floor"],$row["question"],$row["image"]));
	}
}

echo json_encode($points);
/*$conn->set_charset("utf8");
echo $conn->character_set_name();*/

?>
