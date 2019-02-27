using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemText : MonoBehaviour
{

    private Item item;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        item.SendMessage("DisplayText", true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        item.SendMessage("DisplayText", false);
    }

    // Start is called before the first frame update
    void Start()
    {
        item = GetComponentInParent<Item>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
