using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inherits from base class for objects that can move. 
public abstract class Enemy : MonoBehaviour
{
    // How much HP this enemy has left.
    public double hp;
    // How many hearts this enemy's attack knocks off the player.
    public float damageDealt;
    // The squares this enemy aims for, if any.
    protected List<Vector2Int> attackTargets;
    // The particular unit this enemy aims for, if any direction: by default its the player. 
    protected Transform target;
    // The time it takes this Enemy to act. 
    protected float actionTime;
    protected float attackTimer = 0f;

    // Reference to the sprite renderer to flash the sprite on hit. 
    private Renderer rend;
    

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Will be overridden by functons in the inherited classes that specialise the enemy type.
    protected abstract void Attack();

    // Update is called once per frame
    void Update()
    {
        
    }

    void TakeDamage(float dmg)
    {
        // Debug.Log(string.Format("{0} hit for {1} dmg", gameObject.name, dmg));
        hp -= dmg;
        StartCoroutine(IsHit());
        
    }

    protected void UpdateTarget(Transform newTarget)
    {
        target = newTarget;
    }

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

        if (hp <= 0)
            Destroy(gameObject);
    }
}
