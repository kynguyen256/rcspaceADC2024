using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;

public class CommunicationLink
{
    // Instance variables (Sorry Ms. Kikuchi, I'm making some of them public. No mutators/accessor methods)
    public string name; // Why do we need private names???? 
    // I'm sorry Raif, that was becaus I'm used to coding for the College Board
    private int availibilityColumn;
    public bool isAvailible;
    private bool wasAvailible;
    public bool changedAvailibility;
    private int distanceColumn;
    private double distance; 
    private int antennaDiameter;
    public double linkBudget;
    
    public TMP_Text DSNLinkObj;

    // Making a constructor (feels like AP CSA)
    public CommunicationLink(string name, int availibilityColumn, int distanceColumn, int antennaDiameter)
    {
        this.name = name;
        this.availibilityColumn = availibilityColumn;
        isAvailible = false;
        wasAvailible = false;
        this.distanceColumn = distanceColumn;
        distance = double.MaxValue;
        this.antennaDiameter = antennaDiameter;
    }

    public void updateData(int time)
    {
        // Check to see if satellite is availible by checing the availibity collumn of Artemis data
        isAvailible = (SimulationManager.getData(time, availibilityColumn)==1);
        // If satellite is availible, update distance
        if (isAvailible)
        {
            distance = SimulationManager.getData(time, distanceColumn);
        }
        else
        {
            distance = double.MaxValue;
        }
        // Update linkBudget
        linkBudget = calculateLinkBudget();
        DSNLinkObj = GameObject.Find(name).GetComponent<TMP_Text>();
        DSNLinkObj.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = $"{Math.Round(linkBudget, 2)}kb/s";
        
        if(linkBudget > 0)
        {
            DSNLinkObj.transform.GetChild(1).gameObject.GetComponent<RawImage>().color = Color.green;
        } else {
            DSNLinkObj.transform.GetChild(1).gameObject.GetComponent<RawImage>().color = Color.red;
        }

        // Check to see if there has been a change in availibility
        changedAvailibility = (isAvailible != wasAvailible);
        // Set wasAvailible to current availibility (to compare next frame if a change)
        wasAvailible = isAvailible;
    }

    // Yup, that's right, I made a whole method devoted to the link budget equation 
    public double calculateLinkBudget()
    {
        // Here goes nothing...
        double calculation = Math.Pow(10, 0.1*(10 + 9 - 19.43 + 10*Math.Log10(0.55*(Math.Pow((Math.PI*antennaDiameter)/0.136363636,2.0))) - 20*Math.Log10((4000*Math.PI*distance)/0.136363636) - (-228.6) - 10*Math.Log10(222)))/1000;
        // "we must limit the rate at which Orion transmits data to 10 Mbps" according to the ADC handbook
        if (calculation > 10000)
        {
            return 10000;
        }
        else
        {
            return calculation;
        }
    }

    // For Ms. Kikuchi :)
    public string toString()
    {
        return $"{name} has a link budget of: {linkBudget}kb/s.";
    }

    // This is to distinguish it.
    public string priorityToString()
    {
        return $"Thus priority satetlite is {name} and has a link budget of: {linkBudget}kb/s.";
    }
}
