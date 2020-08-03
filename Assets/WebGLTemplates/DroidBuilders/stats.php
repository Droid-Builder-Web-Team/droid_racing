<html>
 <head>
  <title>DBUK 2020 - Virtal 39.1% Racing Leaderboard</title>
  <link rel="stylesheet" href="https://www.w3schools.com/w3css/4/w3.css">
  <link rel="stylesheet" href="TemplateData/style.css">
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css">
  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap-theme.min.css">
  <link rel="stylesheet" href="https://www.w3schools.com/w3css/4/w3.css">
  <link rel="stylesheet" href="TemplateData/style.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.3/jquery.min.js"></script>
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js"></script>
 </head>

 <body>
   
<?php
include("config.php");
$conn = new mysqli($database_host, $database_user, $database_pass, $database_name);
// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

$sql = "SELECT count(lap_time) as count, SUM(lap_time) as total FROM racing_times;";
$result = $conn->query($sql)->fetch_object();
$count = $result->count;
$total = gmdate("z:H:m:s", $result->total);
?>
   
   
<a href="https://droidbuilders.uk/"><img height=290px src=logo.png></a>

<h1>39.1% Racing Stats</h1>

<ul>
  <li>Total Laps: <?php echo $count; ?></li>
  <li>Top Racers:
<?php

?>
  </li>
  <li>Total Time Raced: <?php echo $total; ?></li>
</ul>
