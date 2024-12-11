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
    public Slider SpeedSlider; 
    public Slider TimelineSlider;
    public TMP_Text TimeText;
    public TMP_Text DSNPriority;

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
    // Initilaize satalite object that will reference the priotized satatlite (begin with WPSA)
    CommunicationLink priority;
    // Array of all satellites to be prioritized (created at start of program)
    CommunicationLink[] priorityList;
    
    void UpdateSpeed()
    {
        speedMultiplier = (int) SpeedSlider.value;
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

        // Stuff related to satellites
        priority = WPSA;
        // Create the list (array) of priotized satellites
        CommunicationLink[] priorityList = prioritizeSatellites(priority, WPSA, DS54, DS34, DS24);

        // Stuff related to UI
        SpeedSlider = GameObject.Find("TimeMultSlider").GetComponent<Slider>();
        TimelineSlider = GameObject.Find("TimelineSlider").GetComponent<Slider>();
        TimeText = GameObject.Find("SpeedText").GetComponent<TMP_Text>();
        DSNPriority = GameObject.Find("Priority").GetComponent<TMP_Text>();

        Debug.Log(TimeText.text);

        minutesPerFrame = 1;
        speedMultiplier = 0;

        SpeedSlider.onValueChanged.AddListener(delegate {
            if ((int) SpeedSlider.value != speedMultiplier) 
            {    
                UpdateSpeed();
            }
        });

        TimelineSlider.onValueChanged.AddListener(delegate { 
            // Make sure time DOES NOT INTERFERE HERE!!!
            if ((int) TimelineSlider.value != globalTime) 
            {    
                speedMultiplier = 0;
                SpeedSlider.value = 0;
                TimeText.text = "Speed: "+speedMultiplier.ToString()+"x";
            }

            globalTime = (int) TimelineSlider.value;
        });
    }

    // Update is called once per frame
    void Update()
    {
        // This simulation manager runs the global time.
        // Currently it just increases globalTime each frame, but we will implement UI controls later 
        // which will enable further manipulation of time (pause/play, slider, playback speed,etc.)
        globalTime += 1*speedMultiplier;
        TimelineSlider.value = globalTime;
        // IndexOutOfRange exception was getting annoying, thus:
        if (globalTime > 12982)
        {
            globalTime = 12982;
            SpeedSlider.value = 0;
            speedMultiplier = 0;
            TimeText.text = "Speed: "+speedMultiplier.ToString()+"x";
        }
        Debug.Log("globalTime: " + globalTime);

        // Alright, lets update those satellites!
        WPSA.updateData(globalTime);
        DS54.updateData(globalTime);
        DS24.updateData(globalTime);
        DS34.updateData(globalTime);
        priority.updateData(globalTime);

        // Update priority satellite


        // OLD METHOD
        priority = prioritize(priority, WPSA, DS54, DS34, DS24);


        // NEW METHOD
        //priority = priorityList[globalTime];


        // Let's print it out
        DSNPriority.text = $"Prioritized: {priority.name}";
        GameObject.Find(priority.name).GetComponent<TMP_Text>().transform.GetChild(1).gameObject.GetComponent<RawImage>().color = Color.blue; // make prioritized satellite blue
        //Debug.Log(priority.priorityToString());
    }

    // New priotizing (least transfers)
    public static CommunicationLink[] prioritizeSatellites(CommunicationLink prioritized, CommunicationLink satellite1, CommunicationLink satellite2, CommunicationLink satellite3, CommunicationLink satellite4)
    {
        CommunicationLink[] returnPriorityList = new CommunicationLink[12978];
        for (int i = 0; i <= 12978; i++)
        {
            // Update all of the satellites to the "current time" (within the loop that is)
            prioritized.checkAvailibility(i);
            satellite1.checkAvailibility(i);
            satellite2.checkAvailibility(i);
            satellite3.checkAvailibility(i);
            satellite4.checkAvailibility(i);
            // If we lose priotized satellite, we need to switch satellites
            if (!prioritized.isAvailible)
            {
                // Need to find how long each satellite will last (see public static void method calculateAvailbilityTime below)
                if (satellite1.isAvailible)
                {
                    calculateAvailbilityTime(satellite1, i);
                }
                if (satellite2.isAvailible)
                {
                    calculateAvailbilityTime(satellite2, i);
                }
                if (satellite3.isAvailible)
                {
                    calculateAvailbilityTime(satellite3, i);
                }
                if (satellite4.isAvailible)
                {
                    calculateAvailbilityTime(satellite4, i);
                }
                // With this information we can see which one will last the longest, and set prioritized to that one
                prioritized = findGreatestAvailibilityTime(satellite1, satellite2, satellite3, satellite4);
            }
            returnPriorityList[i] = prioritized;
        }
        return returnPriorityList;
    }

    public static void calculateAvailbilityTime(CommunicationLink satellite, int initialTime)
    {
        // Basically updating availibility time to see how long it will last.
        satellite.availibleTime = 0;
        while (satellite.isAvailible)
        {
            satellite.checkAvailibility(initialTime + satellite.availibleTime);
            satellite.availibleTime++;
        }
    }

    public static CommunicationLink findGreatestAvailibilityTime(CommunicationLink s1, CommunicationLink s2, CommunicationLink s3, CommunicationLink s4)
    {
        if (s1.availibleTime > s2.availibleTime && s2.availibleTime > s3.availibleTime && s3.availibleTime > s4.availibleTime)
        {
            return s1;
        }
        else if (s2.availibleTime > s3.availibleTime && s3.availibleTime > s4.availibleTime)
        {
            return s2;
        }
        else if (s3.availibleTime > s4.availibleTime)
        {
            return s3;
        }
        else
        {
            return s4;
        }
    }


    // Kinda clunky to understand, probably a more "efficient" and easier to understand way to do this
    public static CommunicationLink prioritize(CommunicationLink priorized, CommunicationLink satellite1, CommunicationLink satellite2, CommunicationLink satellite3, CommunicationLink satellite4)
    {
        // Check each satallite for change in availibility, and see if it is beneficial to switch to it
        if (satellite1.isAvailible && satellite1.linkBudget > priorized.linkBudget)
        {
            return satellite1;
        }
        if (satellite2.isAvailible && satellite2.linkBudget > priorized.linkBudget)
        {
            return satellite2;
        }
        if (satellite3.isAvailible && satellite3.linkBudget > priorized.linkBudget)
        {
            return satellite3;
        }
        if (satellite4.isAvailible && satellite4.linkBudget > priorized.linkBudget)
        {
            return satellite4;
        }
        else
        {
            return priorized;
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