<?

include "config.php";

$conn = new mysqli($database_host, $database_user, $database_pass, $database_name);
mysqli_set_charset($conn, "utf8");

$api = $_REQUEST['api'];
$type = $_REQUEST['type'];

$json_string = json_encode($_REQUEST);

$file_handle = fopen('debug.json', 'w');
fwrite($file_handle, $json_string);
fclose($file_handle);

// Decode request
if ($api == $config->racing_api) {
	if ($type == "lap") {
      		$sql = "INSERT INTO racing_times(email, name, lap_time, participents, room_name) VALUES (?, ?, ?, ?, ?)";
      		$stmt = $conn->prepare($sql);
      		$time = $_REQUEST['lap_time'];
      		$email = $_REQUEST['email'];
      		$name = $_REQUEST['name'];
      		$participents = $_REQUEST['participents'];
      		$room_name = $_REQUEST['room_name'];
      		$stmt->bind_param("ssdis", $email, $name, $time, $participents, $room_name);
		$stmt->execute();
		$stmt->close();
	} 

	if ($type == "race") {
		$sql = "INSERT INTO racing_games(email, name, fastest_lap, number_laps, participents, room_name, race_length) VALUES(?, ?, ?, ?, ?, ?, ?)";
		$stmt = $conn->prepare($sql);
		$email = $_REQUEST['email'];
		$name = $_REQUEST['name'];
		$participents = $_REQUEST['participents'];
		$room_name = $_REQUEST['room_name'];
		$fastest_time = $_REQUEST['fastest_time'];
		$number_laps = $_REQUEST['number_laps'];
		$race_length = $_REQUEST['race_length'];
		$stmt->bind_param("ssdiisd", $email, $name, $fastest_time, $number_laps, $participents, $room_name, $race_length);
		$stmt->execute();
		$stmt->close();

	}

} else {
    echo "Invalid key";
}

?>

