using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    // The time it will take the object to move in seconds
    public float moveTime;
    // The layer on which the collision will be checked.
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    // To make movement more efficient. 
    private float inverseMoveTime;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();

        inverseMoveTime = 1f / moveTime;
    }

    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        // Start position to move from.
        Vector2 start = transform.position;

        // Calculate end positon based on the direction parameters passed in.
        Vector2 end = start + new Vector2(xDir, yDir);

        // Disable the collider for now so the linecast doesn't hit itself.
        boxCollider.enabled = false;

        // Cast a line from the start point to the end point checking the collisions.
        hit = Physics2D.Linecast(start, end, blockingLayer);

        // Reenable collider.
        boxCollider.enabled = true;

        // Check if anything was hit.
        if (hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }

        return false;
    }

    //Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
    protected IEnumerator SmoothMovement(Vector3 end)
    {
        //Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
        //Square magnitude is used instead of magnitude because it's computationally cheaper.
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        //While that distance is greater than a very small amount (Epsilon, almost zero):
        while (sqrRemainingDistance > float.Epsilon)
        {
            //Find a new position proportionally closer to the end, based on the moveTime
            Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);

            //Call MovePosition on attached Rigidbody2D and move it to the calculated position.
            rb2D.MovePosition(newPostion);

            //Recalculate the remaining distance after moving.
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            //Return and loop until sqrRemainingDistance is close enough to zero to end the function
            yield return null;
        }
    }

    // Generic parameter T to specify type we expect the unit to interact with if blocked. 
    protected virtual void AttemptMove<T> (int xDir, int yDir) where T : Component
    {
        // Store whatever linecast hits when move is called. 
        RaycastHit2D hit;

        bool canMove = Move(xDir, yDir, out hit);

        // If nothing was hit, nothing to do here. 
        if (hit.transform == null)
            return;

        // Get a component reference to the component attached to the object hit. 
        T hitComponent = hit.transform.GetComponent<T>();

        // Object is blocked and has hit something it can interact.
        if (!canMove && hitComponent != null)
            OnCantMove(hitComponent);
    }

    // Will be overridden in inheriting classes.
    protected abstract void OnCantMove<T>(T component)
            where T : Component;
}
