using UnityEngine;
using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;

public class SimulationManager : MonoBehaviour
{
    // Data & Objects
    public TextAsset artemisDatasheet;
    public static string [] artemisData;

    //Variables
    public static int globalTime = 8;
    public int minutesPerFrame = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Only time in the entire program where we have to splice the data. 
        // All other objects will call upon this array to get data from, using method below.
        artemisData = artemisDatasheet.text.Split(new string[] { ",", "\n"}, System.StringSplitOptions.None);
    }

    // Update is called once per frame
    void Update()
    {
        // This simulation manager runs the global time.
        // Currently it just increases globalTime each frame, but we will implement UI controls later
        globalTime += minutesPerFrame;
        Debug.Log("globalTime: " + globalTime);
    }

    // Method used by many other scrips to use the data contained in artemisData
    // Yes, there are some significant short cuts here, but hey, it works
    public static double getData(int time, int column)
    {
        return Convert.ToDouble(artemisData[(time-6)*14 + column]);
        // Note: 6 shifts data columns to align with time (kinda) and 14 is the number of collumns
    }
}
