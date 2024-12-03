using UnityEngine;
using System;
using Unity.Collections;

public class VelocityVector : MonoBehaviour
{
    public GameObject simManObject; // The Simulation Manager i guess ??
    SimulationManager simManager; // We need to create an object in this script so that we can call Simulation Manager's methods
    public float directionX;
    public float directionY;
    public float directionZ;
    public Vector3 direction = new Vector3();
    public float len; 
    public Vector3 scaler = new Vector3();

    void Start() 
    {
        simManager = simManObject.GetComponent<SimulationManager>();
    }

    void Update() 
    {
        Debug.Log("this is working somewhat");
        directionX = (float)simManager.getData(SimulationManager.globalTime,4);
        directionY = (float)simManager.getData(SimulationManager.globalTime,5);
        directionZ = (float)simManager.getData(SimulationManager.globalTime,6);
        direction = new Vector3(directionX, directionY, directionZ);
        // transform.rotation = Quaternion.LookRotation(direction); 
        
        len = direction.magnitude;
        scaler = new Vector3(len,(float)0.5,(float)0.5);
        transform.localScale = scaler;
        Debug.Log(scaler);
    }

}
