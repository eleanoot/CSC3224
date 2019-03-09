using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseBasic : Enemy
{
    public float movementTime;

    public Tilemap obstaclesTilemap;

    public LayerMask unitsMask;
    
    private List<Path> bestPath = new List<Path>();
    private Animator anim;

    void Awake()
    {
        Tilemap[] tilemaps = FindObjectsOfType<Tilemap>();
        obstaclesTilemap = tilemaps[0];
        anim = GetComponent<Animator>();
        // right, up, left, down, 
        attackTargets = new List<Vector2Int> { new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(-1, 0), new Vector2Int(0, -1) };

        actionTime = 0.5f;
    }

    void Update()
    {
        Attack();

    }


    protected override void Attack()
    {
        if (attackTimer <= 0f)
            attackTimer = actionTime;

        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                HappyPath();
                
                Path nextMove = bestPath[0];
                bestPath.RemoveAt(0);
               // Debug.Log(string.Format("currently at: {0}, {1}", transform.position.x, transform.position.y));
               Move(nextMove.x, nextMove.y);
                //Debug.Log(string.Format("next move: {0}, {1}", nextMove.x, nextMove.y));
                attackTimer = 0f;
            }
        }
        // Use A* pathfinding to determine which tile to move to- aiming for the player. 

        // Move to chosen tile. 
        
    }

    private void Move(float x, float y)
    {
            float xDir = x - transform.position.x;
            float yDir = y - transform.position.y;
            // Debug.Log(string.Format("move dir {0}, {1}", xDir, yDir));
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

            // TO-DO: some kind of resolution for if player and mouse move to same space at the same time.
            if (x == target.transform.position.x && y == target.transform.position.y)
            {
                // Action on hit depends if the target is still the player or not. 
                if (target.transform != GameObject.FindGameObjectWithTag("Player").transform)
                {
                    target.gameObject.SendMessage("IsHit");
                }
                else if (Stats.TakeDamage(damageDealt))
                    StartCoroutine(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().IsHit()); // only flash the sprite if damage has actually been taken: prevent overlap of player being left invisible on final damage hit. 
                
            }
            else
            {
                Vector2 startTile = transform.position;
                Vector2 targetTile = new Vector2(x, y);

                StartCoroutine(SmoothMovement(targetTile));
            }

          
        


    }

    // Smoothly move the enemy from one tile to the next like the player.
    private IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        float inverseMoveTime = 1 / movementTime;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPos = Vector3.MoveTowards(transform.position, end, inverseMoveTime * Time.deltaTime);
            transform.position = newPos;
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            // Wait until the next frame to continue execution. 
            yield return null;
        }
    }


    private List<Path> GetAdjacentSquares(Path p)
    {
        List<Path> ret = new List<Path>();

        foreach (Vector2Int d in attackTargets)
        {
            float x = p.x + d.x;
            float y = p.y + d.y;
           // Debug.Log(string.Format("new pos {0}, {1}", x, y));
            if ( x > -5 && x < 4 && y > -5 && y < 4 && !CheckForCollision(d, new Vector2(x,y)))
            {
                ret.Add(new Path(p.g + 1, ManhattanDistance(new Vector2(x, y), new Vector2(target.position.x, target.position.y)), p, x, y));
            }
        }

        
        return ret;
    }

    public static int ManhattanDistance(Vector2 a, Vector2 b)
    {
        return Mathf.RoundToInt(Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y));
    }

    // Need to check for collisions with both enemies and obstacles - not the outer walls as already covered by movement range check. 
    // Obstacles tested by tilemap check, enemie by raycast.
    private bool CheckForCollision(Vector2 start, Vector2 end)
    {
        bool hasObstacle = getCell(obstaclesTilemap, end) != null;

       this.GetComponent<BoxCollider2D>().enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(start, end, unitsMask);
        this.GetComponent<BoxCollider2D>().enabled = true;
        if (hit.transform != null && (hit.transform != target.transform))
        {
            // TO-DO: fix enemy going back and forth when player is right next to an enemy tile
          // Debug.Log(string.Format("enemy at {0}, {1}", end.x, end.y));
           return true;
        }
        else if (hasObstacle)
            return true;
        return false;
    }

    private static Path FindLowestFScore(List<Path> openList)
    {
        int lowestF = int.MaxValue;
        Path lowestCell = null;
        foreach (Path p in openList)
        {
            if (p.f < lowestF)
            {
                lowestF = p.f;
                lowestCell = p;
            }
        }
        return lowestCell;
    }

    private void HappyPath()
    {
        Path destinationSquare = new Path(0, 0, null, target.position.x, target.position.y);
        //Debug.Log(string.Format("destination square: {0}, {1}", destinationSquare.x, destinationSquare.y));
        List<Path> evaluationList = new List<Path>();
        List<Path> closedPathList = new List<Path>();
        evaluationList.Add(new Path(0, 0, null, gameObject.transform.position.x, gameObject.transform.position.y));
        Path currentSquare = null;
        while (evaluationList.Count > 0)
        {
            currentSquare = FindLowestFScore(evaluationList);
            closedPathList.Add(currentSquare);
            //Debug.Log(string.Format("closed square added: {0}, {1}", currentSquare.x, currentSquare.y));
            evaluationList.Remove(currentSquare);
            // The target has been located or infinite loop break for now
            if (DestinationFound(closedPathList, destinationSquare) || evaluationList.Count >= 64)
            {
                bestPath = buildPath(currentSquare);
                break;
            }
            List<Path> adjacentSquares = GetAdjacentSquares(currentSquare);
            foreach (Path p in adjacentSquares)
            {
                if (closedPathList.Contains(p))
                    continue; // skip this one, we already know about it
                if (!evaluationList.Contains(p))
                {
                    evaluationList.Add(p);
                }
                else if (p.h + currentSquare.g + 1 < p.f)
                    p.parent = currentSquare;
            }
        }

        

    }

    private bool DestinationFound(List<Path> closed, Path dest)
    {
        foreach(Path p in closed )
        {
            if (dest.x == p.x && dest.y == p.y)
                return true;

        }
        return false;
    }

    // Simply used because at the end of our loop we have a path with parents in the reverse order. This reverses the list so it's from Enemy to Player
    private List<Path> buildPath(Path p)
    {
        List<Path> bestPath = new List<Path>();
        Path currentLoc = p;
        bestPath.Insert(0, currentLoc);
        while (currentLoc.parent != null)
        {
            currentLoc = currentLoc.parent;
            if (currentLoc.parent != null)
                bestPath.Insert(0, currentLoc);
        }
        return bestPath;
    }

    /* TILEMAP/MOVEMENT UTILS */
    // Determine if the given cell position is part of this tilemap or not.
    private TileBase getCell(Tilemap tilemap, Vector2 cellWorldPos)
    {
        return tilemap.GetTile(tilemap.WorldToCell(cellWorldPos));
    }
}

// Added class to make holding path information simpler. 
class Path : object
{
    public int g;         // Steps from A to this
    public int h;         // Steps from this to B
    public Path parent;   // Parent node in the path
    public float x;         // x coordinate
    public float y;         // y coordinate
    public Path(int _g, int _h, Path _parent, float _x, float _y)
    {
        g = _g;
        h = _h;
        parent = _parent;
        x = _x;
        y = _y;
    }
    public int f // Total score for this
    {
        get
        {
            return g + h;
        }
    }
}

