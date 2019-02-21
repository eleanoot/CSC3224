using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public enum ItemType
    {
        Passive, // player can have unlimited number and usually always active. 
        Active, // player can only have one and are activated on a keypress. usually need charge time. 
    }

    // The name of this item. 
    [SerializeField]
    protected string itemName;
    // The flavourtext description of this item. 
    [SerializeField]
    protected string itemDesc;
    // The type of this item. 
    [SerializeField]
    protected ItemType itemType;
    // The rarity of this item to identify which pool it comes from. 
    [SerializeField]
    protected string rarity;

    protected abstract void Pickup();

    // Default method for switching between items: only active items should be able to be swapped out, passives will stick around,
    // so not all derivations need to implement this. 
    protected void Switch()
    {
        return;
    }

    public string Rarity
    {
        get
        {
            return rarity;
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Pickup();
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
