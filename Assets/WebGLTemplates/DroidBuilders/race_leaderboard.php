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

<script src="https://www.w3schools.com/lib/w3.js"></script>
<script src="plugin/simple-bootstrap-paginator.js"></script>
<script src="js/pagination-fastest.js"></script>
<script src="js/pagination-championship.js"></script>

<script>


    function updateRacer(r) {
      console.log("Racer name: " + r);
      $.ajax({
        url: "get_racer.php?name=" + encodeURIComponent(r),
        dataType: "json",
        cache: false,
        success: function(data) {
          $('#racer').html(data.html);
        }
      });

    }


</script>

<?php
include("config.php");
$conn = new mysqli($database_host, $database_user, $database_pass, $database_name);
// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}
$sqlQuery = "SELECT MAX(x.uid), x.name, x.lap_time, x.added FROM racing_times x
    JOIN (SELECT p.name, MIN(lap_time)
    AS lap_fastest FROM racing_times p GROUP BY p.name) y ON y.name = x.name
    AND y.lap_fastest = x.lap_time GROUP BY x.name, x.lap_time ORDER BY x.lap_time";
$result = mysqli_query($conn, $sqlQuery);
$totalRecords = mysqli_num_rows($result);
$totalPagesFastest = ceil($totalRecords/$perPage);

$sqlQuery = "SELECT count(*) FROM racing_games";
$result = mysqli_query($conn, $sqlQuery);
$totalRecords = mysqli_fetch_array($result)[0];
$totalPagesChampionship = ceil($totalRecords/$perPage);
?>

<div class="grid-container">
  <div class="Logo"><a href="https://droidbuilders.uk/"><img height=290px src=logo.png></a></div>

  <div class="Fastest-Laps">
    <table class=leaderboard id=leaderboard>
      <thead>
        <tr>
          <th>Run date</th>
          <th>Position</th>
          <th>Contender</th>
          <th>Final Time</th>
        </tr>
      </thead>
      <tbody id="fastest-content">
      </tbody>
    </table>
    <div id="pagination-fastest"></div>
    <input type="hidden" id="totalPagesFastest" value="<?php echo $totalPagesFastest; ?>">
  </div>

  <div class=Championship>
    <table class=leaderboard id=leaderboard>
      <thead>
        <tr>
          <th>Run date</th>
          <th>Position</th>
          <th>Contender</th>
          <th>Laps</th>
          <th>Average Time</th>
          <th>Fastest Time</th>
        </tr>
      </thead>
      <tbody id="championship-content">
      </tbody>
    </table>
    <div id="pagination-championship"></div>
    <input type="hidden" id="totalPagesChampionship" value="<?php echo $totalPagesChampionship; ?>">
  </div>

  <div class="Top-Racers" id="racer">
  </div>

  <div class="Footer">(c) Droidbuilders UK 2020</div>
</div>
