using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private bool isMoving = false;
    private Vector2 lastInput = Vector2.zero; // Stores last direction
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // Set default animation state
        animator.Play("idletree");
    }

    private void Update()
    {
        // Get input regardless of movement state
        Vector2 input = Vector2.zero;

        // Manual check to prevent diagonal movement
        if (Input.GetKey(KeyCode.W))
            input.y = 1;
        else if (Input.GetKey(KeyCode.S))
            input.y = -1;
        else if (Input.GetKey(KeyCode.A))
            input.x = -1;
        else if (Input.GetKey(KeyCode.D))
            input.x = 1;

        // Always update the animation parameters
        if (input != Vector2.zero)
        {
            // Save the last non-zero input
            lastInput = input;
        }

        // Update animator parameters - these are used by both blend trees
        animator.SetFloat("moveX", lastInput.x);
        animator.SetFloat("moveY", lastInput.y);

        // Switch between idle and walk trees based on movement state
        if (input != Vector2.zero && !isMoving)
        {
            Vector3 targetPos = transform.position + new Vector3(input.x, input.y, 0f);
            StartCoroutine(Move(targetPos));
        }
    }

    IEnumerator Move(Vector3 targetPos)
    {
        // Start walking
        isMoving = true;
        animator.Play("walktree");

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Return to idle
        transform.position = targetPos;
        isMoving = false;
    }
}