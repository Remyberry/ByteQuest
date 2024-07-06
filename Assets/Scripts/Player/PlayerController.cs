using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform movePoint;
    private bool isMoving;
    public LayerMask solidsLayer;
    private Vector3 input;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start() {
        movePoint.parent = null;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        animator.SetFloat("moveX", Input.GetAxisRaw("Horizontal"));
        animator.SetFloat("moveY", Input.GetAxisRaw("Vertical"));

        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            isMoving = false;
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                if (!Physics2D.OverlapCircle(movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), 0.2f, solidsLayer))
                {
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                }
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                if (!Physics2D.OverlapCircle(movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), 0.2f, solidsLayer))
                {
                    movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                }
            }

            animator.SetBool("isMoving", isMoving);
        }
        else {
            isMoving = true;
            animator.SetBool("isMoving", isMoving);
        }
    }

}
