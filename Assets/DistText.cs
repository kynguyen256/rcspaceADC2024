using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DistanceDisplay : MonoBehaviour
{
    public string text;
    public TMP_Text Dist;
    public float distVal = 0;

    public TextAsset shotDatasheet;
    public static string [] shotData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shotData = shotDatasheet.text.Split(new string[] { ",", "\n"}, System.StringSplitOptions.None);
        Debug.Log(shotData);
    }

    // Update is called once per frame
    void Update()
    {
        distVal = (float)getShotData(SimulationManager.globalTime,1); 
        Debug.Log("Distance: " +distVal);
        Dist.SetText("Distance: " + distVal); 
    }

    public static double getShotData(int time, int column)
    {
        return Convert.ToDouble(shotData[(time-6)*2 + column]);
        // Note: 6 shifts data columns to align with time (kinda) and 16 is the number of columns
    }
}

