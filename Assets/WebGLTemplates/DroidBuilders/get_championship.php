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
$sqlQuery = "SELECT * FROM racing_games ORDER BY number_laps desc, average_lap asc, fastest_lap asc
    LIMIT $startFrom, $perPage";
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
  $paginationHtml.= "<td>".$row['timestamp']."</td>";
	$paginationHtml.= "<td align=center>".$pos."</td>";
	$paginationHtml.= "<td onclick=updateRacer(\"".str_replace(' ', '&nbsp;', $row['name'])."\")>".$row['name']."</a></td>";
  $paginationHtml.= "<td>".$row['number_laps']."</td>";
  $paginationHtml.= "<td>".number_format($row['average_lap'], 4)."s</td>";
	$paginationHtml.= "<td>".$row['fastest_lap']."s</td>";
  $paginationHtml.= "</tr>";

}
$jsonData = array(
	"html"	=> $paginationHtml,
);
echo json_encode($jsonData);


?>
