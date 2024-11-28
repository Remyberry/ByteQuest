using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMove : MonoBehaviour
{
    public float moveSpeed = 2f; // Speed of downward movement
    public float tilemapHeight = 10f; // Height of the Tilemap in Unity units

    private Vector3 startPosition; // Initial position of the Tilemap

    private void Start()
    {
        // Record the starting position of the Tilemap
        startPosition = transform.position;
    }

    private void Update()
    {
        // Move the Tilemap downward
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;

        // Check if the Tilemap has moved out of bounds
        if (transform.position.y <= startPosition.y - tilemapHeight)
        {
            // Reset the Tilemap's position to create a looping effect
            transform.position = startPosition;
        }
    }
}
