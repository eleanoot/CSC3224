using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    // Prevent the actual item pools from being refilled each time a room is reloaded. 
    public static ItemManager instance = null;

    public GameObject[] allItems; // the actual item prefabs to be instantiated.

    public Text flavourText;

    [SerializeField]
    private static List<Item> common = new List<Item>();

    [SerializeField]
    private static List<Item> rare = new List<Item>();

    [SerializeField]
    private static List<Item> legendary = new List<Item>();


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // Go through all the items and fill the rarity item pools with them. 
            foreach (GameObject i in allItems)
            {
                Item item = i.GetComponent<Item>();
                switch (item.Rarity)
                {
                    case ("Common"):
                        common.Add(item);
                        break;
                    case ("Rare"):
                        rare.Add(item);
                        break;
                    case ("Legendary"):
                        legendary.Add(item);
                        break;
                    default:
                        break; // shouldn't get here, but just in case before we try to remove something from a pool it isnt in.
                }

            }

            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        
    }

    public void RemoveFromPool(Item item)
    {
        switch (item.Rarity)
        {
            case ("Common"):
                common.Remove(item);
                break;
            case ("Rare"):
                rare.Remove(item);
                break;
            case ("Legendary"):
                legendary.Remove(item);
                break;
            default:
                break; // shouldn't get here, but just in case before we try to remove something from a pool it isnt in.
        }

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
