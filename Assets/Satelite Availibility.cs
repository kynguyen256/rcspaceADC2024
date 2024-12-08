using UnityEngine;
using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;

public class CommunicationLink
{
    // Instance variables (Sorry Ms. Kikuchi, I'm making some of them public. No mutators/accessor methods)
    private string name;
    private int availibilityColumn;
    public bool isAvailible;
    private int distanceColumn;
    private double distance; 
    private int antennaDiameter;
    public double linkBudget;

    // Making a constructor (feels like AP CSA)
    public CommunicationLink(string name, int availibilityColumn, int distanceColumn, int antennaDiameter)
    {
        this.name = name;
        this.availibilityColumn = availibilityColumn;
        isAvailible = false;
        this.distanceColumn = distanceColumn;
        distance = double.MaxValue;
        this.antennaDiameter = antennaDiameter;
    }

    public void updateData(int time)
    {
        // Simulation manager reference object

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
        linkBudget = calculateLinkBudget();
    }

    // Yup, that's right, I made a whole method devoted to the link budget equation 
    public double calculateLinkBudget()
    {
        // Here goes nothing...
        double calculation = Math.Pow(0.1*(10 + 9 - 19.43 + 10*Math.Log10(0.55)*(Math.Pow(Math.PI*antennaDiameter,2.0)/0.136363636) - 20*Math.Log10((4000*Math.PI*distance)/0.136363636) - (-228.6) - 10*Math.Log10(22)),10.0)/1000;
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
        return $"{name} is availible: {isAvailible} and has a link budget of: {linkBudget}kb/s.";
    }
}
