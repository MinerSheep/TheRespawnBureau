# This is for local test, 
# For Online it is in InfinityFree phpMyAdmin

CREATE DATABASE IF NOT EXISTS game_database;
USE game_database;

CREATE TABLE IF NOT EXISTS users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) UNIQUE NOT NULL,
    best_score INT DEFAULT 0,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

SELECT * FROM users;