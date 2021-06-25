<?php 
class Result{
	public $status;
	public $data = "";
}

$resultClass = new Result();

$code = "Empty";
$code = $_POST["log"];
$date = date("m_d_y h_i_s");

$logFile = fopen("ErrorLogs/Log - $date .txt","w") or die("Unable to save the log!");
fwrite($logFile,$code);
fclose($logFile);

$resultClass->status = "Ok";
$resultClass->data = "Log succesfuly created.";
echo json_encode($resultClass);



?>