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
    public GameObject pointStage1; // Prefab used to trace out stage 1
    public GameObject pointStage2; // Prefab used to trace out stage 2

    //Variables
    public static int globalTime = 8;
    public int minutesPerFrame = 10;
    public int skipPoints = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Only time in the entire program where we have to splice the data. 
        // All other objects will call upon this array to get data from, using method below.
        artemisData = artemisDatasheet.text.Split(new string[] { ",", "\n"}, System.StringSplitOptions.None);

        // Traces out the path. Just believe it does. Lots to explain otherwise. Yes, it takes some time at the launch of the program.
        for (int i = 8; i <= 196-((196-8)%skipPoints); i+=skipPoints)
        {
            GameObject pointClone = Instantiate(pointStage1, new Vector3((float)getData(i,1)/100, (float)getData(i,2)/100, (float)getData(i,3)/100), Quaternion.identity);
        }
        for (int i = 196-((196-8)%skipPoints); i <= 12983; i+=skipPoints)
        {
            GameObject pointClone = Instantiate(pointStage2, new Vector3((float)getData(i,1)/100, (float)getData(i,2)/100, (float)getData(i,3)/100), Quaternion.identity);
        }
        Debug.Log("Done creating path!");
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
    public double getData(int time, int column)
    {
        return Convert.ToDouble(artemisData[(time-6)*16 + column]);
        // Note: 6 shifts data columns to align with time (kinda) and 16 is the number of collumns
    }
}
