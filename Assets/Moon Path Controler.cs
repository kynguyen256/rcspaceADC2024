using UnityEngine;
using System;

public class MoonPathControl : MonoBehaviour
{
    // Game Objects to reference
    public GameObject pathPoint; // Prefab sphere we will use to "draw" the path
    public GameObject referencePath; // In this case the moon itself because that is the position data we want the path to follow
    public TextAsset bonusDatasheet;
    public static string [] moonData;
    
    // Variables
    public float posX;
    public float posY;
    public float posZ;
    public Vector3 pos = new Vector3();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Need to use data from bonus data sheet for moon, so we do it here b/c only moon object needs this data
        moonData = bonusDatasheet.text.Split(new string[] { ",", "\n"}, System.StringSplitOptions.None);
    }

    // Update is called once per frame
    void Update()
    {
        posX = (float)getData(SimulationManager.globalTime,8);
        posY = (float)getData(SimulationManager.globalTime,9);
        posZ = (float)getData(SimulationManager.globalTime,10);
        pos = new Vector3(posX/100, posY/100, posZ/100);

        transform.position = pos;

        // Could implment, but lowers fps:
        /*GameObject pointClone = Instantiate(pathPoint, pos, referencePath.transform.rotation);*/
    }

    public double getData(int time, int column)
    {
        return Convert.ToDouble(moonData[(time-6)*14 + column]);
        // Note: 6 shifts data columns to align with time (kinda) and 14 is the number of collumns
    }
}
