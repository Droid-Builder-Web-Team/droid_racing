<html>
 <head>
  <title>DBUK 2020 - Virtal 39.1% Racing Leaderboard</title>
  <link rel="stylesheet" href="https://www.w3schools.com/w3css/4/w3.css">
  <link rel="stylesheet" href="TemplateData/style.css">
 </head>

 <body>

<script src="https://www.w3schools.com/lib/w3.js"></script>

<script>
function myFunction() {
  // Declare variables
  var input, filter, table, tr, td, i;
  input = document.getElementById("nameSearch");
  filter = input.value.toUpperCase();
  table = document.getElementById("leaderboard");
  tr = table.getElementsByTagName("tr");

  // Loop through all table rows, and hide those who don't match the search query
  for (i = 0; i < tr.length; i++) {
    td = tr[i].getElementsByTagName("td")[2];
    if (td) {
      if (td.innerHTML.toUpperCase().indexOf(filter) > -1) {
        tr[i].style.display = "";
      } else {
        tr[i].style.display = "none";
      }
    }
  }
}
</script>


<?


include "config.php";

// Create connection
$conn = new mysqli($database_host, $database_user, $database_pass, $database_name);
// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
} 

echo "<div class=\"grid-container\">";


echo "<div class=\"Fastest-Laps\">";
$sql = "SELECT MAX(x.uid), x.name, x.lap_time, x.added FROM racing_times x JOIN (SELECT p.name, MIN(lap_time) AS lap_fastest FROM racing_times p GROUP BY p.name) y ON y.name = x.name AND y.lap_fastest = x.lap_time GROUP BY x.name, x.lap_time ORDER BY x.lap_time";
$runs = $conn->query($sql);
if ($runs->num_rows > 0) {
    $pos = 0;
    echo "<input type=\"text\" id=\"nameSearch\" onkeyup=\"myFunction()\" placeholder=\"Search for names..\">";
    echo "<table class=leaderboard id=leaderboard>";
    echo "<tr>";
    echo "<th onclick=\"w3.sortHTML('#leaderboard','.item', 'td:nth-child(1)')\">Run date</th>";
    echo "<th onclick=\"w3.sortHTML('#leaderboard','.item', 'td:nth-child(2)')\">Position</th>";
    echo "<th onclick=\"w3.sortHTML('#leaderboard','.item', 'td:nth-child(3)')\">Contender</th>";
    echo "<th onclick=\"w3.sortHTML('#leaderboard','.item', 'td:nth-child(4)')\">Final Time</th>";
    while($row = $runs->fetch_assoc()) {
	$pos++;
	echo "<tr class=item ";
	switch ($pos) {
	case 1:
		echo "bgcolor='#FFD700'";
		break;
	case 2:
		echo "bgcolor='#C0C0C0'";
		break;
	case 3:
		echo "bgcolor='#CD7F32'";
		break;
	}
	echo ">";
  echo "<td>".$row['added']."</td>";
	echo "<td align=center>".$pos."</td>";
	echo "<td><a class=leaderboard href=\"?name=".$row['name']."\">".$row['name']."</a></td>";
  echo "<td>".$row['lap_time']."s</td>";
  echo "</tr>";
  }
} else {
    echo "No runs";
}
echo "</table>";
echo "</div>";


echo "<div class=Championship>";
$sql = "SELECT * FROM racing_games ORDER BY number_laps desc, fastest_lap asc LIMIT 20";
$games = $conn->query($sql);
if ($games->num_rows > 0) {
	$pos = 0;
	echo "<input type=\"text\" id=\"nameSearch\" onkeyup=\"myFunction()\" placeholder=\"Search for names..\">";
        echo "<table class=leaderboard id=leaderboard>";
        echo "<tr>";
	echo "<th onclick=\"w3.sortHTML('#leaderboard','.item', 'td:nth-child(1)')\">Run date</th>";
	echo "<th onclick=\"w3.sortHTML('#leaderboard','.item', 'td:nth-child(2)')\">Position</th>";
	echo "<th onclick=\"w3.sortHTML('#leaderboard','.item', 'td:nth-child(3)')\">Contender</th>";
	echo "<th onclick=\"w3.sortHTML('#leaderboard','.item', 'td:nth-child(4)')\">Laps</th>";
	echo "<th onclick=\"w3.sortHTML('#leaderboard','.item', 'td:nth-child(5)')\">Average Time</th>";
	echo "<th onclick=\"w3.sortHTML('#leaderboard','.item', 'td:nth-child(6)')\">Fastest Lap</th>";
	while($row = $games->fetch_assoc()) {
        $avgsql = "SELECT avg(lap_time) as average FROM racing_times WHERE name = \"".$row['name']."\" AND room_name LIKE \"".$row['room_name']."%\";";
        $avg = $conn->query($avgsql)->fetch_object()->average;
	      $pos++;
	      echo "<tr class=item ";
	      switch ($pos) {
	      case 1:
	              echo "bgcolor='#FFD700'";
	              break;
	      case 2:
	              echo "bgcolor='#C0C0C0'";
	              break;
	      case 3:
	              echo "bgcolor='#CD7F32'";
	              break;
	      }
        echo ">";
        echo "<td>".$row['timestamp']."</td>";
	echo "<td align=center>".$pos."</td>";
	echo "<td><a class=leaderboard href=\"?name=".$row['name']."\">".$row['name']."</a></td>";
	echo "<td>".$row['number_laps']."</td>";
	echo "<td>".number_format($avg, 4)."</td>";
	echo "<td>".$row['fastest_lap']."s</td>";
        echo "</tr>";
    }
}
echo "</table>";
echo "</div>"; // End of Championship div

echo "<div class=\"Logo\"><a href=\"https://droidbuilders.uk/\"><img height=290px src=logo.png></a></div>"; // Main logo
echo "<div class=\"Footer\">(c) Droidbuilders UK 2020</div>"; // Footer text

echo "<div class=\"Top-Racers\">"; // Start of top section
if ($_REQUEST['name'] != "") {
  $sql_races = "SELECT count(*) as count FROM racing_games WHERE name = \"".$_REQUEST['name']."\";";
  $sql_laps = "SELECT count(*) as count, avg(lap_time) as average FROM racing_times WHERE name =\"".$_REQUEST['name']."\";";
  $sql_fastest = "SELECT lap_time FROM racing_times WHERE name =\"".$_REQUEST['name']."\" ORDER BY lap_time ASC LIMIT 1;";
  $avg = number_format($conn->query($sql_laps)->fetch_object()->average, 4);
  $laps = $conn->query($sql_laps)->fetch_object()->count;
  $races = $conn->query($sql_races)->fetch_object()->count;
  $fastest = $conn->query($sql_fastest)->fetch_object()->lap_time;
  echo "<div class=\"top-card\">";
  echo " <div class=\"top-card-name\">";
  echo "Name: ".$_REQUEST['name'];
  echo " </div>";
  echo " <div class=\"top-class-stats\">";
  echo "  <div class=\"top-class-stat\">Laps<br>";
  echo $laps;
  echo "  </div>";
  echo "  <div class=\"top-class-stat\">Races<br>";
  echo $races;
  echo "  </div>";
  echo "  <div class=\"top-class-stat\">Average<br>";
  echo $avg;
  echo "  </div>";
  echo "  <div class=\"top-class-stat\">Fastest<br>";
  echo $fastest;
  echo "  </div>";  
  echo " </div>";
  echo "</div>";
}
echo "</div>"; // End of top section


echo "</div>";
$conn->close();

?>


