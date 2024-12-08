using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
    public Slider TimeSlider;
    public TMP_Text TimeText;

    //Variables
    public static int globalTime = 8;
    public int minutesPerFrame = 1;
    public int speedMultiplier; // Speed multiplier implemented so our slider sys works (hopefully!!!!)
    public int skipPoints;

    // Variables/Objects related to satellite availibility
    // Initialize the satalite objects
    // Note : name, availibility collumn, distance collumn, antenna diameter
    CommunicationLink WPSA = new CommunicationLink("WPSA",8,9,12);
    CommunicationLink DS54 = new CommunicationLink("DS54",10,11,34);
    CommunicationLink DS24 = new CommunicationLink("DS24",12,13,34);
    CommunicationLink DS34 = new CommunicationLink("DS34",14,15,34);
    
    void UpdateSpeed()
    {
        speedMultiplier = (int) TimeSlider.value;
        TimeText.text = "Speed: "+speedMultiplier.ToString()+"x";
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;
        double velocityMagnitude = 1;
        // Only time in the entire program where we have to splice the data. 
        // All other objects will call upon this array to get data from, using method below.
        artemisData = artemisDatasheet.text.Split(new string[] { ",", "\n"}, System.StringSplitOptions.None);

        // Traces out the path. Just believe it does. Lots to explain otherwise. Yes, it takes some time at the launch of the program.
        for (int i = 8; i <= 196-((196-8)%skipPoints); i+=(int)(skipPoints/velocityMagnitude+1))
        {
            GameObject pointClone = Instantiate(pointStage1, new Vector3((float)getData(i,1)/100, (float)getData(i,2)/100, (float)getData(i,3)/100), Quaternion.identity);
            velocityMagnitude = Math.Sqrt(getData(i,4)*getData(i,4)+getData(i,5)*getData(i,5)+getData(i,6)*getData(i,6));
            Debug.Log(velocityMagnitude);
        }
        for (int i = 196-((196-8)%skipPoints); i <= 12983; i+=(int)(skipPoints/velocityMagnitude+1))
        {
            GameObject pointClone = Instantiate(pointStage2, new Vector3((float)getData(i,1)/100, (float)getData(i,2)/100, (float)getData(i,3)/100), Quaternion.identity);
            velocityMagnitude = Math.Sqrt(getData(i,4)*getData(i,4)+getData(i,5)*getData(i,5)+getData(i,6)*getData(i,6));
            Debug.Log(velocityMagnitude);
        }
        Debug.Log("Done creating path!");

        TimeSlider = GameObject.Find("TimeMultSlider").GetComponent<Slider>();
        TimeText = GameObject.Find("SpeedText").GetComponent<TMP_Text>();

        Debug.Log(TimeText.text);

        minutesPerFrame = 1;
        speedMultiplier = 0;

        TimeSlider.onValueChanged.AddListener(delegate {
            UpdateSpeed();
        });
    }

    // Update is called once per frame
    void Update()
    {
        // This simulation manager runs the global time.
        // Currently it just increases globalTime each frame, but we will implement UI controls later 
        // which will enable further manipulation of time (pause/play, slider, playback speed,etc.)
        Debug.Log("SpeedMultiplier: " + speedMultiplier.ToString());
        globalTime += 1*speedMultiplier;
        Debug.Log("globalTime: " + globalTime);

        // Alright, lets update those satellites!
        WPSA.updateData(globalTime);
        DS54.updateData(globalTime);
        DS24.updateData(globalTime);
        DS34.updateData(globalTime);
        // And print it out! (if they are availible)
        if (WPSA.isAvailible)
        {
            Debug.Log(WPSA.toString());
        }
        if (DS54.isAvailible)
        {
            Debug.Log(DS54.toString());
        }
        if (DS34.isAvailible)
        {
            Debug.Log(DS34.toString());
        }
        if (DS24.isAvailible)
        {
            Debug.Log(DS24.toString());
        }
    }

    // Method used by many other scrips to use the data contained in artemisData
    // Yes, there are some significant short cuts here, but hey, it works
    public static double getData(int time, int column)
    {
        return Convert.ToDouble(artemisData[(time-6)*16 + column]);
        // Note: 6 shifts data columns to align with time (kinda) and 16 is the number of collumns
    }
}


// Okay, this all didn't really work...
/*
public class CommunicationLink 
{
    // Instance variables
    private int availibilityColumn;
    private bool isAvailible;
    private int distanceColumn;
    private double distance; 

    // Making a constructor (feels like AP CSA)
    public CommunicationLink(int availibilityColumn, int distanceColumn)
    {
        this.availibilityColumn = availibilityColumn;
        isAvailible = false;
        this.distanceColumn = distanceColumn;
        distance = double.MaxValue;
    }

    public void checkData()
    {
        // Simulation manager reference object
        var manager = new SimulationManager();

        // Check to see if satellite is availible by checing the availibity collumn of Artemis data
        isAvailible = (manager.getData(manager.getGlobalTime(), availibilityColumn)==1);
        // If satellite is availible, update distance
        if (isAvailible)
        {
            distance = manager.getData(manager.getGlobalTime(), distanceColumn);
        }
        else
        {
            distance = double.MaxValue;
        }
    }

    // For Ms. Kikuchi :)
    public string toString()
    {
        return $"Availible: {isAvailible} Distance: {distance}km away.";
    }
}
*/