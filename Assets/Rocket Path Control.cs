using UnityEngine;
using System;
using Unity.Collections;

public class RocketPathControl : MonoBehaviour
{
    // Game Objects to reference
    public GameObject pathPoint; // Prefab sphere we will use to "draw" the path
    public GameObject referencePath; // In this case the rocket itself because that is the position data we want the path to follow
    //public GameObject simManObject; // The Simulation Manager
    //SimulationManager simManager; // We need to create an object in this script so that we can call Simulation Manager's methods

    private GameObject stage1Points;
    private GameObject stage2Points;
    
    // Variables
    public float posX;
    public float posY;
    public float posZ;
    public Vector3 scaledPos = new Vector3();
    public float directionX;
    public float directionY;
    public float directionZ;
    public Vector3 direction = new Vector3();

    public float prevPosX; // previous position vars
    public float prevPosY;
    public float prevPosZ;
    public Vector3 prevPos = new Vector3();

    public double distance;
    public int frameCounter = 0;
    public int skipPoints;

    public static int currentCamera = 1;
    public Vector3 camera1Scale = new Vector3(100,100,300);
    public Vector3 camera2Scale = new Vector3(10,10,30);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //simManager = simManObject.GetComponent<SimulationManager>();
        // init previous position to first position vars
        prevPosX = (float)SimulationManager.getData(SimulationManager.globalTime,1);
        prevPosY = (float)SimulationManager.getData(SimulationManager.globalTime,2);
        prevPosZ = (float)SimulationManager.getData(SimulationManager.globalTime,3);
        prevPos = new Vector3(posX, posY, posZ);
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate new position
        posX = (float)SimulationManager.getData(SimulationManager.globalTime,1);
        posY = (float)SimulationManager.getData(SimulationManager.globalTime,2);
        posZ = (float)SimulationManager.getData(SimulationManager.globalTime,3);
        scaledPos = new Vector3(posX/100, posY/100, posZ/100);
        // Go to new position
        transform.position = scaledPos;

        // METHOD 1.0 THIS WORKS NOW (technically more fancy, and hey, it uses the velocity data)
        
        // Calculate new rotation (based on the deretion of velocity vector)
        directionX = (float)SimulationManager.getData(SimulationManager.globalTime,4);
        directionY = (float)SimulationManager.getData(SimulationManager.globalTime,5);
        directionZ = (float)SimulationManager.getData(SimulationManager.globalTime,6);
        direction = new Vector3(directionX, directionY, directionZ);
        // Orient rotation to point in the direction of velocity vector (b/c it is tanget to the curve)
        transform.rotation = Quaternion.LookRotation(direction);
        
        // METHOD 2.0 (but not as cool, so we won't use it)
        //transform.rotation = Quaternion.LookRotation(scaledPos - prevPos/100);

        if (currentCamera != SwitchCamera.cameraNumber)
        {
            currentCamera = SwitchCamera.cameraNumber;
            if (currentCamera == 1) {
                transform.localScale = new Vector3(100,100,300);
            } else {
                transform.localScale = new Vector3(10,10,30);
            }
        }
        
        // Output total distance traveled (eventually will be added to UI)
        distance += Distance(posX,posY,posZ,prevPosX,prevPosY,prevPosZ);
        // Debug.Log("Distance: " + distance + "km");

        // Update previous position to current position
        prevPosX = posX;
        prevPosY = posY;
        prevPosZ = posZ;
        prevPos = new Vector3(posX, posY, posZ);
        if (frameCounter == skipPoints)
        {
            //GameObject pointClone = Instantiate(pathPoint, scaledPos, referencePath.transform.rotation);
            frameCounter = 0;
        }
        frameCounter++;
    }

    public static double Distance(float x, float y, float z, float px, float py, float pz) {
        double dist;
        double deltaX = Math.Pow(x - px,2);
        double deltaY = Math.Pow(y - py,2);
        double deltaZ = Math.Pow(z - pz,2);
        dist = Math.Sqrt(deltaX + deltaY + deltaZ);
        return dist;
    }
}
