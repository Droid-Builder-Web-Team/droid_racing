<?php

include "config.php";

// Create connection
$conn = new mysqli($database_host, $database_user, $database_pass, $database_name);
// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

$page = 0;
if (isset($_POST['page'])) {
	$page  = $_POST['page'];
} else {
	$page=1;
};
$startFrom = ($page-1) * $perPage;
$sqlQuery = "SELECT MAX(x.uid), x.name, x.lap_time, x.added FROM racing_times x
    JOIN (SELECT p.name, MIN(lap_time)
    AS lap_fastest FROM racing_times p GROUP BY p.name) y ON y.name = x.name
    AND y.lap_fastest = x.lap_time GROUP BY x.name, x.lap_time ORDER BY x.lap_time
    LIMIT $startFrom, $perPage";
	//echo $sqlQuery;
$result = mysqli_query($conn, $sqlQuery);
$paginationHtml = '';
$pos = $perPage * ($page-1);
while ($row = mysqli_fetch_assoc($result)) {
  $pos++;
	$paginationHtml.='<tr class=item ';
  switch ($pos) {
	case 1:
		$paginationHtml.= "bgcolor='#FFD700'";
		break;
	case 2:
		$paginationHtml.= "bgcolor='#C0C0C0'";
		break;
	case 3:
		$paginationHtml.= "bgcolor='#CD7F32'";
		break;
	}
	$paginationHtml.= ">";
  $paginationHtml.= "<td>".$row['added']."</td>";
	$paginationHtml.= "<td align=center>".$pos."</td>";
	$paginationHtml.= "<td onclick=updateRacer(\"".str_replace(' ', '&nbsp;', $row['name'])."\")>".$row['name']."</a></td>";
  $paginationHtml.= "<td>".$row['lap_time']."s</td>";
  $paginationHtml.= "</tr>";

}
$jsonData = array(
	"html"	=> $paginationHtml,
);
echo json_encode($jsonData);


?>
