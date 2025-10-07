<?php
include 'config.php';

$username = $_GET['username'];

$stmt = $conn->prepare("SELECT best_score FROM users WHERE username = ?");
$stmt->bind_param("s", $username);
$stmt->execute();
$result = $stmt->get_result();

if ($result->num_rows > 0) {
    $row = $result->fetch_assoc();
    echo json_encode(["success" => true, "best_score" => $row['best_score']]);
} else {
    echo json_encode(["success" => false, "message" => "Cannot find a user"]);
}

$conn->close();
?>