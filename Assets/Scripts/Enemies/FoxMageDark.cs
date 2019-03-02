using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxMageDark : Enemy
{
    public GameObject magic;
    public float magicSpeed;


    void Awake()
    {
        //hp = 3.0;
        //damageDealt = 0.5f;
        // north east, north west, south west, south east
        attackTargets = new List<Vector2Int> { new Vector2Int(1, 1), new Vector2Int(-1, 1), new Vector2Int(-1, -1), new Vector2Int(1, -1) };
        actionTime = 2f;
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
                // Disable the enemy's box collider for now so the projectiles don't collide with itself.
                float angle = 0f;
                foreach (Vector2 n in attackTargets)
                {
                    GameObject magicInst = Instantiate(magic, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
                    magicInst.transform.SetParent(transform);
                    magicInst.GetComponent<Rigidbody2D>().AddForce(n * magicSpeed);
                    magicInst.GetComponent<Project>().SetDamage(damageDealt);
                    angle += 90f;
                }

                attackTimer = 0f;
            }
        }

    }

}
