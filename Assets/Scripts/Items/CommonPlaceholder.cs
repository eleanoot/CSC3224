using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonPlaceholder : Item
{
    protected override void Pickup()
    {
        Debug.Log("Picked up");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
