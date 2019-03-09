// Asset modified from 'ProgressBarPack', UPLN
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[ExecuteInEditMode]

public class ProgressBar : MonoBehaviour
{

    [Header("Bar Setting")]
    public Color BarColor;   
    public Color BarBackGroundColor;
    public Sprite BarBackGroundSprite;
    [Range(1f, 100f)]
    public int Alert = 100;
    public Color BarAlertColor;


    private Image bar;
    private float nextPlay;
    private static float barValue;
    public float BarValue
    {
        get { return barValue; }

        set
        {
            value = Mathf.Clamp(value, 0, 100);
            barValue = value;
            UpdateValue(barValue);

        }
    }

        

    private void Awake()
    {
        bar = transform.Find("Bar").GetComponent<Image>();
    }

    private void Start()
    {

        bar.color = BarColor;

        UpdateValue(barValue);


    }

    void UpdateValue(float val)
    {
        bar.fillAmount = val / 100;

        // TODO: currently not changing colour on full.
        if (Alert >= val)
        {
            bar.color = BarAlertColor;
        }
        else
        {
            bar.color = BarColor;
        }

    }


    //private void Update()
    //{
    //    if (!Application.isPlaying)
    //    {           
    //        UpdateValue(50);

    //        bar.color = BarColor;          
    //    }
    //    else
    //    {
    //        if (Alert >= barValue)
    //        {
    //            nextPlay = Time.time + RepeatRate;
    //            audiosource.PlayOneShot(sound);
    //        }
    //    }
    //}

}
