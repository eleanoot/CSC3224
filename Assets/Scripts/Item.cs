using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    
    

    protected Text message;

    // Keep a reference to the Player. 
    protected Player player;

    protected abstract void Pickup();

    // Only necessary for active items to implement, as passive items will always be active. 
    public virtual void OnUse()
    {
        Debug.Log("default on use");
        return;
    }

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

    void OnTriggerEnter2D()
    {
        Pickup();
        if (message != null)
            message.text = "";
        // Destroy this picked up item. 
        if (itemType == ItemType.Passive)
            Destroy(gameObject);
        // Remove all items from board so only one can be picked up. 
        if (Stats.RoomCount % 5 == 0)
        {
            GameObject[] items = GameObject.FindGameObjectsWithTag("Item");

            foreach (GameObject i in items)
            {
                Destroy(i);
            }
        }
        
    }
    

    protected void DisplayText(bool display)
    {
        if (display)
        {
            message.text = itemDesc;
        }
        else
        {
            message.text = "";
        }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // Only add text panels if this is an item room. Prevents bugs with placeable active items. 
        if (Stats.RoomCount % 5 == 0)
        {
            message = Instantiate(ItemManager.instance.flavourText) as Text;
            message.transform.SetParent(GameObject.Find("Canvas").transform, false);
            message.text = "";

            // Transform this label from its space on the canvas to be relevant to its item. 
            message.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position + new Vector3(0, 1, 0));
        }
        

        player = GameObject.Find("Player").GetComponent<Player>();

        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
