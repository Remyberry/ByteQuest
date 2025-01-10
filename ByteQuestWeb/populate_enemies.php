<?php
// Database credentials
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "unitybytequest";

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);

// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// Get JSON data (you'll need to adapt this to how you're receiving the JSON)
$json = file_get_contents('php://input'); // For POST requests
$json = '{
    "Enemies": [
      {
        "enemyName": "Test",
        "attackSpeed": 10,
        "timeLimit": 60,
        "bestTime": 60,
        "requirement": "Hello",
        "requirementDescription": "Type: \nhello",
        "codeTemplate": "",
        "correctCode": "hello"
      },
      {
        "enemyName": "Byte Bug",
        "attackSpeed": 15,
        "timeLimit": 120,
        "bestTime": 60,
        "requirement": "Basic MOV and ADD instructions",
        "requirementDescription": "Use MOV and ADD to initialize 3 and add 5 to the value of AL.",
        "codeTemplate": "",
        "correctCode": "MOV AL, 3\nADD AL, 5"
      },
      {
        "enemyName": "Stack Spider",
        "attackSpeed": 20,
        "timeLimit": 300,
        "bestTime": 60,
        "requirement": "Stack operations.",
        "requirementDescription": "Push AX and BX onto the stack and then pop them back to their original registers.",
        "codeTemplate": "",
        "correctCode": "PUSH AX\nPUSH BX\nPOP BX\nPOP AX"
      },
      {
        "enemyName": "Binary Bat",
        "attackSpeed": 10,
        "timeLimit": 180,
        "bestTime": 60,
        "requirement": "Simple data movement.",
        "requirementDescription": "Move 3 into AL, 5 into BL, and add them to store the result in CL.",
        "codeTemplate": "",
        "correctCode": "MOV AL, 3\nMOV BL, 5\nADD CL, AL\nADD CL, BL"
      },
      {
        "enemyName": "Hex Hopper",
        "attackSpeed": 10,
        "timeLimit": 300,
        "bestTime": 60,
        "requirement": "Arithmetic in hex",
        "requirementDescription": "Load AX with hex 0x0A, BX with hex 0x05, and perform addition, storing the result in CX.",
        "codeTemplate": "",
        "correctCode": "MOV AX, 0x0A\nMOV BX, 0x05\nADD CX, AX\nADD CX, BX"
      },
      {
        "enemyName": "Shifty Grump",
        "attackSpeed": 5,
        "timeLimit": 480,
        "bestTime": 60,
        "requirement": "Shift operations.",
        "requirementDescription": "Load AL with 0x01 and perform a left shift by 3 bits.",
        "codeTemplate": "",
        "correctCode": "MOV AL, 0x01\nSHL AL, 3"
      },
      {
        "enemyName": "Logic Pig",
        "attackSpeed": 5,
        "timeLimit": 420,
        "bestTime": 60,
        "requirement": "Bitwise operations.",
        "requirementDescription": "Load AL with 0xF0, then perform AND with 0x0F and store the result in AL.",
        "codeTemplate": "",
        "correctCode": "MOV AL, 0xF0\nAND AL, 0x0F"
      },
      {
        "enemyName": "Jump Jackal",
        "attackSpeed": 5,
        "timeLimit": 600,
        "bestTime": 60,
        "requirement": "Basic jump conditions.",
        "requirementDescription": "Set up a loop that counts down from 5 to 0 using DEC and JNZ.",
        "codeTemplate": "",
        "correctCode": "MOV CX, 5\nLOOP_START:\nDEC CX\nJNZ LOOP_START"
      },
      {
        "enemyName": "Recursive Devourer",
        "attackSpeed": 5,
        "timeLimit": 720,
        "bestTime": 60,
        "requirement": "Use CALL and RET for subroutine.",
        "requirementDescription": "Create a subroutine that increments AX by 1 and call it from the main program.",
        "codeTemplate": "",
        "correctCode": "CALL IncrementAX\nHLT\n\nIncrementAX:\nINC AX\nRET"
      },
      {
        "enemyName": "Rotate Orc",
        "attackSpeed": 5,
        "timeLimit": 900,
        "bestTime": 60,
        "requirement": "Rotate operations.",
        "requirementDescription": "Load AL with 0b10101010 and perform a 2-bit rotate left.",
        "codeTemplate": "",
        "correctCode": "MOV AL, 0b10101010\nROL AL, 2"
      },
      {
        "enemyName": "Recursive Deadman",
        "attackSpeed": 5,
        "timeLimit": 1200,
        "bestTime": 60,
        "requirement": "Basic jump conditions.",
        "requirementDescription": "Set up a loop that counts down from 5 to 0 using DEC and JNZ.",
        "codeTemplate": "",
        "correctCode": "MOV CX, 5\nLOOP_START:\nDEC CX\nJNZ LOOP_START"
      }
    ]
  }
  '; //Example hardcoded json

$data = json_decode($json, true);

if (isset($data['Enemies']) && is_array($data['Enemies'])) { // Check if 'Enemies' key exists and is an array
    $enemies = $data['Enemies'];

    foreach ($enemies as $enemy) {
        $enemyName = $enemy['enemyName'];
        $attackSpeed = $enemy['attackSpeed'];
        $timeLimit = $enemy['timeLimit'];
        $bestTime = $enemy['bestTime'];
        $requirement = $enemy['requirement'];
        $requirementDescription = $enemy['requirementDescription'];
        $codeTemplate = $enemy['codeTemplate'];
        $correctCode = $enemy['correctCode'];

        // Prepare and execute the SQL query
        $stmt = $conn->prepare("INSERT INTO enemies (enemyName, attackSpeed, timeLimit, bestTime, requirement, requirementDescription, codeTemplate, correctCode) VALUES (?, ?, ?, ?, ?, ?, ?, ?)");
        $stmt->bind_param("siiissss", $enemyName, $attackSpeed, $timeLimit, $bestTime, $requirement, $requirementDescription, $codeTemplate, $correctCode);

        if ($stmt->execute()) {
            echo "New record created successfully for: " . $enemyName . "<br>";
        } else {
            echo "Error: " . $stmt->error . "<br>";
        }

        $stmt->close();
    }
} else {
    echo "Invalid JSON data or 'Enemies' key not found.";
}

$conn->close();
?>