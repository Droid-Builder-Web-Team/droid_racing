<?php

include "config.php";

// Create connection
$conn = new mysqli($database_host, $database_user, $database_pass, $database_name);
// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

$name = preg_replace('/\xc2\xa0/', ' ', $_REQUEST['name']);
$sql_races = "SELECT count(*) as count FROM racing_games WHERE name = \"".$name."\";";
$sql_laps = "SELECT count(*) as count, avg(lap_time) as average FROM racing_times WHERE name =\"".$name."\";";
$sql_fastest = "SELECT lap_time FROM racing_times WHERE name =\"".$name."\" ORDER BY lap_time ASC LIMIT 1;";
$sql_total = "SELECT SUM(lap_time) as total FROM racing_times WHERE name =\"".$name."\";";
$avg = number_format($conn->query($sql_laps)->fetch_object()->average, 4);
$laps = $conn->query($sql_laps)->fetch_object()->count;
$total = gmdate("H:i:s", $conn->query($sql_total)->fetch_object()->total);
$races = $conn->query($sql_races)->fetch_object()->count;
$fastest = $conn->query($sql_fastest)->fetch_object()->lap_time;
$html = "";
$html .= "<div class=\"top-card\">";
$html .= " <div class=\"top-card-name\">";
$html .= "Name: ".$name;
$html .= " </div>";
$html .= " <div class=\"top-class-stats\">";
$html .= "  <div class=\"top-class-stat\">Laps<br>";
$html .= $laps;
$html .= "  </div>";
$html .= "  <div class=\"top-class-stat\">Races<br>";
$html .= $races;
$html .= "  </div>";
$html .= "  <div class=\"top-class-stat\">Average<br>";
$html .= $avg;
$html .= "  </div>";
$html .= "  <div class=\"top-class-stat\">Fastest<br>";
$html .= $fastest;
$html .= "  </div>";
$html .= "  <div class=\"top-class-stat\">Total Time<br>";
$html .= $total;
$html .= "  </div>";
$html .= " </div>";
$html .= "</div>";

$jsonData = array(
	"html"	=> $html,
);
echo json_encode($jsonData);
