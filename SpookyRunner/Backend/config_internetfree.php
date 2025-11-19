<?php
header("Access-Control-Allow-Origin: *");
header("Content-Type: application/json");

$host = "sql306.infinityfree.com";
$username = "if0_40111492";
$password = "respawn2025";
$database = "if0_40111492_game_database";

$conn = new mysqli($host, $username, $password, $database);

if ($conn->connect_error) {
    die(json_encode(["success" => false, "message" => "Connection Failure: " . $conn->connect_error]));
}
?>