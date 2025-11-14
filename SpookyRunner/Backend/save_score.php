<?php
include 'config.php';

$data = json_decode(file_get_contents("php://input"), true);
$username = $data['username'];
$score = $data['score'];

// Check previous best score
$check = $conn->prepare("SELECT best_score FROM users WHERE username = ?");
$check->bind_param("s", $username);
$check->execute();
$result = $check->get_result();

if ($result->num_rows > 0) {
    $row = $result->fetch_assoc();
    if ($score > $row['best_score']) {
        // Update BEST Score
        $update = $conn->prepare("UPDATE users SET best_score = ? WHERE username = ?");
        $update->bind_param("is", $score, $username);
        $update->execute();
        echo json_encode(["success" => true, "message" => "New Record!", "best_score" => $score]);
    } else {
        echo json_encode(["success" => true, "message" => "Not Changed Record", "best_score" => $row['best_score']]);
    }
} else {
    // Create new user
    $insert = $conn->prepare("INSERT INTO users (username, best_score) VALUES (?, ?)");
    $insert->bind_param("si", $username, $score);
    $insert->execute();
    echo json_encode(["success" => true, "message" => "First Score!", "best_score" => $score]);
}

$conn->close();
?>