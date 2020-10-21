<?

include "config.php";

$conn = new mysqli($database_host, $database_user, $database_pass, $database_name);
mysqli_set_charset($conn, "utf8");

$api = $_REQUEST['api'];
$type = $_REQUEST['type'];

$json_string = json_encode($_REQUEST);

// Decode request
if ($api == $racing_api) {
	if ($type == "lap") {
      		$sql = "INSERT INTO racing_times(email, name, lap_time, participents, room_name, course_num) VALUES (?, ?, ?, ?, ?, ?)";
      		$stmt = $conn->prepare($sql);
      		$time = $_REQUEST['lap_time'];
      		$email = $_REQUEST['email'];
      		$name = $_REQUEST['name'];
      		$participents = $_REQUEST['participents'];
      		$room_name = $_REQUEST['room_name'];
					if(isset($_REQUEST['course_num']))
							$course_num = $_REQUEST['course_num'];
					else
							$course_num = 1;
      		$stmt->bind_param("ssdisi", $email, $name, $time, $participents, $room_name, $course_num);
		$stmt->execute();
		$stmt->close();
	}

	if ($type == "race") {
		$avgsql = "SELECT avg(lap_time) as average FROM racing_times WHERE name = \"".$_REQUEST['name']."\" AND room_name LIKE \"".$_REQUEST['room_name']."%\";";
  	$avg = $conn->query($avgsql)->fetch_object()->average;
		$file_handle = fopen('average.txt', 'w');
		$sql = "INSERT INTO racing_games(email, name, fastest_lap, number_laps, average_lap, participents, room_name, race_length, course_num) VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?)";
		$stmt = $conn->prepare($sql);
		$email = $_REQUEST['email'];
		$name = $_REQUEST['name'];
		$participents = $_REQUEST['participents'];
		$room_name = $_REQUEST['room_name'];
		if(isset($_REQUEST['course_num']))
				$course_num = $_REQUEST['course_num'];
		else
				$course_num = 1;
		$fastest_time = $_REQUEST['fastest_time'];
		$number_laps = $_REQUEST['number_laps'];
		$race_length = $_REQUEST['race_length'];
		$stmt->bind_param("ssdidisdi", $email, $name, $fastest_time, $number_laps, $avg, $participents, $room_name, $race_length, $course_num);
		$stmt->execute();
		$stmt->close();

	}

} else {
    echo "Invalid key";
}

?>
