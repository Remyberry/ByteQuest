using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCont : MonoBehaviour
{
    public float moveSpeed;
    public LayerMask solidsLayer;
    private bool isMoving;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float horizontalInput = UnityEngine.Input.GetAxisRaw("Horizontal");
        float verticalInput = UnityEngine.Input.GetAxisRaw("Vertical");

        if (horizontalInput != 0 || verticalInput != 0) 
        {
            isMoving = true;

            if (Mathf.Abs(horizontalInput) > 0 && Mathf.Abs(verticalInput) > 0)
            {
                if (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput))
                {
                    verticalInput = 0f;
                }
                else
                {
                    horizontalInput = 0f;
                }
            }

            animator.SetFloat("moveX", horizontalInput);
            animator.SetFloat("moveY", verticalInput);

            var targetPos = transform.position;
            targetPos.x += horizontalInput;
            targetPos.y += verticalInput;
            if (isWalkable(new Vector3(targetPos.x, targetPos.y - 0.25f)))
            {
                transform.Translate(new Vector3(horizontalInput * moveSpeed * Time.deltaTime, verticalInput * moveSpeed * Time.deltaTime, 0f));
            }
            animator.SetBool("isMoving", isMoving);
            
        }   
        else
        {
            isMoving = false;
            animator.SetBool("isMoving", isMoving);
        }
        /*//Moves Forward and back along z axis                           //Up/Down
        transform.Translate(Vector3.up * Time.deltaTime * Input.GetAxis("Vertical") * moveSpeed);
        //Moves Left and right along x Axis                               //Left/Right
        transform.Translate(Vector3.right * Time.deltaTime * Input.GetAxis("Horizontal") * moveSpeed);*/

    }

    private bool isWalkable(Vector2 vector) 
    {
        if (Physics2D.OverlapCircle(vector, 0f, solidsLayer) != null)
        {
            return false;
        }
        return true;
    }
}
