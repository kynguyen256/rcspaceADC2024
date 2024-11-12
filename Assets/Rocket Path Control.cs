using UnityEngine;
using System;
using Unity.Collections;

public class RocketPathControl : MonoBehaviour
{
    // Game Objects to reference
    public GameObject pathPoint;
    public GameObject referencePath;
    public GameObject simManObject;
    SimulationManager simManager;
    
    // Variables
    public float posX;
    public float posY;
    public float posZ;
    public Vector3 pos = new Vector3();

    public float prevPosX; // previous position vars
    public float prevPosY;
    public float prevPosZ;
    public Vector3 prevPos = new Vector3();

    public double distance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        simManager = simManObject.GetComponent<SimulationManager>();
        // init previous position to first position vars
        prevPosX = (float)simManager.getData(SimulationManager.globalTime,1);
        prevPosY = (float)simManager.getData(SimulationManager.globalTime,2);
        prevPosZ = (float)simManager.getData(SimulationManager.globalTime,3);
    }

    // Update is called once per frame
    void Update()
    {
        posX = (float)simManager.getData(SimulationManager.globalTime,1);
        posY = (float)simManager.getData(SimulationManager.globalTime,2);
        posZ = (float)simManager.getData(SimulationManager.globalTime,3);
        pos = new Vector3(posX/100, posY/100, posZ/100);

        transform.position = pos;

        distance = Distance(posX,posY,posZ,prevPosX,prevPosY,prevPosZ);
        Debug.Log("Distance: " + distance);

        prevPosX = posX;
        prevPosY = posY;
        prevPosZ = posZ;

        GameObject pointClone = Instantiate(pathPoint, pos, referencePath.transform.rotation);
    }

    static double Distance(float x, float y, float z, float px, float py, float pz) {
        double dist;
        double deltaX = Math.Pow(x - px,2);
        double deltaY = Math.Pow(y - py,2);
        double deltaZ = Math.Pow(z - pz,2);
        dist = Math.Sqrt(deltaX + deltaY + deltaZ);
        return dist;
    }
}
