using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    // Tilemap references to check for collisions. 
    // CAN'T just use Unity's colliders with the current smooth movement system as will get stuck forever trying to move onto that tile. 
    public Tilemap groundTilemap;
    public Tilemap wallsTilemap;
    public Tilemap obstaclesTilemap;

    // How long the smooth movement will take.
    private float moveTime = 0.1f;
    // Booleans to check if we're currently transitioning and shouldn't be updating yet.
    public bool isMoving = false;
    public bool onCooldown = false;

    // Reference to the animation controller for the player to change walking animation direction.
    private Animator anim;
    // Reference to the sprite renderer to flash the sprite on hit. 
    private Renderer rend;

    void Start()
    {
        anim = GetComponent<Animator>();
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        // If the player is still moving, do nothing this update.
        if (isMoving || onCooldown)
            return;

        // Get the move direction.
        int horizontal = (int)Input.GetAxisRaw("Horizontal");
        int vertical = (int)Input.GetAxisRaw("Vertical");

        // Don't want to be allowed to move diagonally on the grid.
        if (horizontal != 0)
            vertical = 0;

        // If a direction has been provided this update, we're trying to move. 
        if (horizontal != 0 || vertical != 0)
        {
            // Start some delay between movements to prevent going too fast.
            StartCoroutine(ActionCooldown(0.2f));
            MovePlayer(horizontal, vertical);
            
        }
    }

    private void MovePlayer(int xDir, int yDir)
    {
        // Change the walking animation direction depending on where we're turning. 
        // Currently indivudally setting and resetting every time to prevent a delay. 
        if (xDir < 0)
        {
            anim.SetTrigger("turnLeft");
            anim.ResetTrigger("turnRight");
            anim.ResetTrigger("turnUp");
            anim.ResetTrigger("turnDown");
        }
        else if (xDir > 0)
        {
            anim.SetTrigger("turnRight");
            anim.ResetTrigger("turnLeft");
            anim.ResetTrigger("turnUp");
            anim.ResetTrigger("turnDown");
        }
        else if (yDir > 0)
        {
            anim.SetTrigger("turnUp");
            anim.ResetTrigger("turnRight");
            anim.ResetTrigger("turnLeft");
            anim.ResetTrigger("turnDown");
        }
        else if (yDir < 0)
        {
            anim.SetTrigger("turnDown");
            anim.ResetTrigger("turnRight");
            anim.ResetTrigger("turnUp");
            anim.ResetTrigger("turnLeft");
        }
        Vector2 startTile = transform.position;
        Vector2 targetTile = startTile + new Vector2(xDir, yDir);

        // Determine if the target tile is traversable.
        bool hasObstacle = getCell(obstaclesTilemap, targetTile) != null;
        bool hasWall = getCell(wallsTilemap, targetTile) != null;

        // If the tile to move to does not contain a wall or obstacle, it's a valid move. 
        if (!hasObstacle && !hasWall)
        {
            StartCoroutine(SmoothMovement(targetTile));
        }
        else
        {
            StartCoroutine(BlockedMovement(targetTile));
        }

        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Magic")
        {
            StartCoroutine(IsHit());
        }

    }

    /* COROUTINES */
    // 'Flash' the sprite when attacked.
    private IEnumerator IsHit()
    {
        for (int i = 0; i < 5; i++)
        {
            rend.enabled = true;
            yield return new WaitForSeconds(0.1f);
            rend.enabled = false;
            yield return new WaitForSeconds(0.1f);
        }
        rend.enabled = true;
    }


    // Smoothly move the player from one tile to the next. 
    private IEnumerator SmoothMovement(Vector3 end)
    {
        isMoving = true;

        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        float inverseMoveTime = 1 / moveTime;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPos = Vector3.MoveTowards(transform.position, end, inverseMoveTime * Time.deltaTime);
            transform.position = newPos;
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            // Wait until the next frame to continue execution. 
            yield return null;
        }

        // Coroutine has finished moving the player.
        isMoving = false;
    }

    // Moves the player in the intended direction, then 'bounces' back to where they were. 
    // Gives feedback for an invalid move. 
    private IEnumerator BlockedMovement(Vector3 end)
    {
        isMoving = true;

        Vector3 originalPos = transform.position;

        // Update the end position to be a little bit in front of the current position to 'bounce' back from. 
        end = transform.position + ((end - transform.position) / 3);

        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        float inverseMoveTime = (1 / (moveTime * 2));

        // Move forwards.
        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPos = Vector3.MoveTowards(transform.position, end, inverseMoveTime * Time.deltaTime);
            transform.position = newPos;
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            yield return null;
        }

        // Move back to the original position.
        sqrRemainingDistance = (transform.position - originalPos).sqrMagnitude;
        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPos = Vector3.MoveTowards(transform.position, originalPos, inverseMoveTime * Time.deltaTime);
            transform.position = newPos;
            sqrRemainingDistance = (transform.position - originalPos).sqrMagnitude;

            yield return null;
        }

        isMoving = false;
    }

    // Introduce a delay between actions.
    private IEnumerator ActionCooldown(float cooldown)
    {
        onCooldown = true;
        while (cooldown > 0f)
        {
            cooldown -= Time.deltaTime;
            yield return null;
        }
        onCooldown = false;
    }
    
    /* TILEMAP UTILS */
    // Determine if the given cell position is part of this tilemap or not.
    private TileBase getCell(Tilemap tilemap, Vector2 cellWorldPos)
    {
        return tilemap.GetTile(tilemap.WorldToCell(cellWorldPos));
    }
}
