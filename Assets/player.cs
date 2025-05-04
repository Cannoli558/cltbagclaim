using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private bool isMoving = false;
    private Vector2 lastInput; // Stores last direction
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isMoving)
        {
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

            // If input was given
            if (input != Vector2.zero)
            {
                lastInput = input;

                // Match exactly your Animator parameters
                animator.SetFloat("moveX", lastInput.x);
                animator.SetFloat("moveY", lastInput.y);

                // Move character
                Vector3 targetPos = transform.position + new Vector3(lastInput.x, lastInput.y, 0f);
                StartCoroutine(Move(targetPos));
            }
        }
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
    }
}
