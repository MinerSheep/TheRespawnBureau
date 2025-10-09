<?php
include 'config.php';

$limit = isset($_GET['limit']) ? intval($_GET['limit']) : 10;

$stmt = $conn->prepare("SELECT username, best_score FROM users ORDER BY best_score DESC LIMIT ?");
$stmt->bind_param("i", $limit);
$stmt->execute();
$result = $stmt->get_result();

$leaderboard = [];
while ($row = $result->fetch_assoc()) {
    $leaderboard[] = $row;
}

echo json_encode(["success" => true, "leaderboard" => $leaderboard]);

$conn->close();
?>