using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Project : MonoBehaviour
{
    private Animator anim;

    private float damageDealt;
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Stats.TakeDamage(damageDealt);
        }
        Destroy(gameObject);

    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        Physics2D.IgnoreCollision(transform.parent.GetComponent<Collider2D>(), GetComponent<Collider2D>());
;    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (Vector3.Distance(transform.parent.position, transform.position) > 2)
        {
            Destroy(gameObject);
        }
        else if (Vector3.Distance(transform.parent.position, transform.position) > 1)
        {
            anim.Play("MagicOrangeFade");
        }

    }

    public void SetDamage(float damage)
    {
        damageDealt = damage;
    }
}
