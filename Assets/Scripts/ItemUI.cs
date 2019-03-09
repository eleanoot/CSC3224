using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    private Image activeImage;

    private ProgressBar chargeBar;

    // Start is called before the first frame update
    void Awake()
    {
        activeImage = GetComponent<Image>();
        chargeBar = GetComponentInChildren<ProgressBar>();
        UpdateItem();
    }

    public void UpdateItem()
    {
        if (Stats.ActiveItem != null)
        {
            activeImage.color = Color.white;
            activeImage.sprite = Stats.ActiveItem.GetComponent<SpriteRenderer>().sprite;
            chargeBar.BarValue = Stats.CurrentCharge * (100/Stats.ActiveCharge);
        }
        else // If we don't have an item, make the panel clear. 
        {
            activeImage.color = Color.clear;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
