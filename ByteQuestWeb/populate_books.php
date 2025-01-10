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

// Get JSON data
$json = file_get_contents('php://input');
$json = '[
    {
        "title": "Book of Byte Quest",
        "content": "Hello?"
    },
    {
        "title": "[L1]Basic Data Movement",
        "content": "Lesson 1: Basic Data Movement\n Objective: Understand how to transfer data between registers, memory, and the stack.\n Instructions Covered:\n\n MOV: Moves data between registers or between memory and registers.\n MOV AX, BX ; AX gets the value in BX\n MOV AL, 5 ; AL gets the value 5\n\n PUSH and POP: Push data onto the stack and pop it back.\n PUSH AX ; Saves AX on the stack\n POP AX ; Retrieves the last value pushed into AX\n\n Exercise Application: You\'ll use MOV to set values in registers and PUSH/POP for managing values on the stack in Exercise 3 (Stack Spider)."
    },
    {
        "title": "[L2]Arithmetic Instructions",
        "content": "Lesson 2: Arithmetic Instructions \nObjective: Learn how to perform simple arithmetic. \n \n \nInstructions Covered: \nADD and SUB: Adds or subtracts values between registers. \nMOV AL, 3 \nADD AL, 5 ; AL = 8 \nSUB AL, 2 ; AL = 6 \n \n \nExercise Application: These are used in Exercise 1 (Byte Bug) and Exercise 2 (Binary Bat)."
    },
    {
        "title": "[L3]Hexadecimal Arithmetic and Bitwise Operations",
        "content": "Lesson 3: Hexadecimal Arithmetic and Bitwise Operations\nObjective: Learn to use hexadecimal values and perform bitwise operations.\n\nInstructions Covered:\nHexadecimal Values: Use 0x prefix to specify hex values, or just use the value if the context expects hex.\nAND, OR, XOR: Perform bitwise operations.\nMOV AL, 0xF0\nAND AL, 0x0F    ; AL becomes 0x00\nSHL (Shift Left): Shifts bits in a register to the left by a specified count.\nExercise Application: In Exercise 5 (Logic Lizard) and Exercise 6 (Shifty Snake), you’ll use AND to filter bits and SHL to multiply a value by powers of 2."
    },
    {
        "title": "[L4]Conditional Jumps",
        "content": "Lesson 4: Conditional Jumps\nObjective: Introduce the concept of jumps and basic program flow control.\n\nInstructions Covered:\nJZ (Jump if Zero): Jump to a specified label if the zero flag is set.\nJNZ (Jump if Not Zero): Jump to a label if the zero flag is not set.\nMOV CX, 5\nDEC CX\nJNZ LOOP       ; Jump back to LOOP label if CX is not zero\nExercise Application: Use these in Exercise 7 (Jump Jackal) to create a loop that counts down."
    },
    {
        "title": "[L5]Looping and Counters",
        "content": "Lesson 5: Looping and Counters\nObjective: Understand how to manage loops and counters.\nInstructions Covered:\nLOOP: Automatically decrements a counter register (CX) and jumps if not zero.\nExercise Application: In Exercise 7 (Jump Jackal), you’ll set up a loop to control program flow and count down until a condition is met."
    },
    {
        "title": "[L6]Stack Operations and Subroutines",
        "content": "Lesson 6: Stack Operations and Subroutines\nObjective: Set up and use basic subroutines and understand stack management.\nInstructions Covered:\nCALL: Call a subroutine (another block of code).\nRET: Return from a subroutine.\n\nCALL mySubroutine\n; ...\nmySubroutine:\n  INC AX\n  RET\nExercise Application: In Exercise 8 (Recursive Raptor), you’ll call a function and return to the main program after it executes."
    },
    {
        "title": "[L7]Recursive Calls and Advanced Stack Handling",
        "content": "Lesson 7: Recursive Calls and Advanced Stack Handling\nObjective: Learn how to manage recursive subroutines.\nInstructions Covered:\nRecursive Subroutines: Use CALL within a subroutine to call itself until a base case is met.\nExercise Application: Exercise 10 (Recursive Rook) involves setting up a recursive call where the subroutine calls itself until AX reaches zero."
    }
]'; //Example hardcoded json
$data = json_decode($json, true);

if (is_array($data)) {
    foreach ($data as $book) {
        $title = $book['title'];
        $content = $book['content'];

        // Prepare and execute the SQL query
        $stmt = $conn->prepare("INSERT INTO books (title, content) VALUES (?, ?)");
        $stmt->bind_param("ss", $title, $content);

        if ($stmt->execute()) {
            echo "New book record created successfully for: " . $title . "<br>";
        } else {
            echo "Error: " . $stmt->error . "<br>";
        }

        $stmt->close();
    }
} else {
    echo "Invalid JSON data.";
}

$conn->close();
?>