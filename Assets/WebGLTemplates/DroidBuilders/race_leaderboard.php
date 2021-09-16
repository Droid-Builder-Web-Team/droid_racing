<html>
 <head>
  <title>DBUK 2020 - Virtal 39.1% Racing Leaderboard</title>
  <link rel="stylesheet" href="https://www.w3schools.com/w3css/4/w3.css">
  <link rel="stylesheet" href="TemplateData/style.css">
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css">
  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap-theme.min.css">
  <link rel="stylesheet" href="https://cdn.datatables.net/responsive/2.2.1/css/responsive.dataTables.min.css">
  <link rel="stylesheet" href="https://www.w3schools.com/w3css/4/w3.css">
  <link rel="stylesheet" href="TemplateData/style.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.3/jquery.min.js"></script>
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js"></script>
  <link rel="stylesheet" type="text/css" href="TemplateData/dataTables.css">

  <script type="text/javascript" charset="utf8" src="https://cdn.datatables.net/1.10.22/js/jquery.dataTables.js"></script>
  <script src="https://www.w3schools.com/lib/w3.js"></script>

 </head>

 <body>


<script>
$(document).ready( function () {
    $('#leaderboard').DataTable();
} );
</script>
<?php
include("config.php");
$conn = new mysqli($database_host, $database_user, $database_pass, $database_name);
// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

if (!isset($_REQUEST['course_num'])) {
  $course_num = 0;
} else {
  $course_num = $_REQUEST['course_num'];
}
$sqlQuery = "SELECT MAX(x.uid), x.name, x.lap_time, x.added FROM racing_times x
    JOIN (SELECT p.name, MIN(lap_time)
    AS lap_fastest FROM racing_times p WHERE course_num = ".$course_num." GROUP BY p.name) y ON y.name = x.name
    AND y.lap_fastest = x.lap_time GROUP BY x.added, x.name, x.lap_time ORDER BY x.lap_time";
$result_laptimes = mysqli_query($conn, $sqlQuery);
$totalRecords = mysqli_num_rows($result_laptimes);
?>

<div class="grid-container">
  <div class="Logo"><a href="https://droidbuilders.uk/"><img height=290px src=logo.png></a></div>

  <div class="Courses">
    <h2>Select Course</h2>
    <ul>
      <li><a href="?course_num=0">Classic</a></li>
      <li><a href="?course_num=2">Practice</a></li>
      <li><a href="?course_num=1">Halloween</a></li>
      <li><a href="?course_num=3">Xmas</a></li>
      <li><a href="?course_num=4">Easter</a></li>
      <li><a href="?course_num=5">Summer</a></li>

    </ul>
  </div>

  <div class="Fastest-Laps">
    <table class=leaderboard id=leaderboard>
      <thead>
        <tr>
          <th>Position</th>
          <th>Run date</th>
          <th>Contender</th>
          <th>Final Time</th>
        </tr>
      </thead>
      <tbody id="fastest-content">
        <?php
          $pos=0;
          while($row = $result_laptimes->fetch_assoc()) {
            $pos++;
            echo "<tr>";
            echo "<td>".$pos."</td>";
            echo "<td>".$row['added']."</td>";
            echo "<td>".$row['name']."</td>";
            echo "<td>".$row['lap_time']."</td>";
            echo "</tr>";
          }
        ?>
      </tbody>
    </table>
  </div>

  <div class="Top-Racers" id="racer">
  </div>

  <div class="Footer">(c) Droidbuilders UK 2020</div>
</div>
