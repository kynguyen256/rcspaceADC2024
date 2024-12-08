using UnityEngine;
using System;
using Unity.Collections;

public class VelocityVector : MonoBehaviour
{
    // NOTE : changed SimulationMangar's methods to static that need to be used by these other scrips, elminating these
    // schenanigans we had to do
    //public GameObject simManObject; // The Simulation Manager i guess ??
   //SimulationManager simManager; // We need to create an object in this script so that we can call Simulation Manager's methods
    public float directionX;
    public float directionY;
    public float directionZ;
    public Vector3 direction = new Vector3();
    public float len; 
    public Vector3 scaler = new Vector3();

    public bool gone = true;

    void Start() 
    {
        //simManager = simManObject.GetComponent<SimulationManager>();
    }

    void Update() 
    {
        // this is working somewhat
        directionX = (float)SimulationManager.getData(SimulationManager.globalTime,4);
        directionY = (float)SimulationManager.getData(SimulationManager.globalTime,5);
        directionZ = (float)SimulationManager.getData(SimulationManager.globalTime,6);
        direction = new Vector3(directionX, directionY, directionZ);
        // transform.rotation = Quaternion.LookRotation(direction); 
        
        len = (float)0.1 * direction.magnitude;
        scaler = new Vector3(len,(float)0.5,(float)0.5);
        transform.localScale = scaler;
        //Debug.Log(scaler);
    }

    public void disappear() {
        if (gone == true) {
            gameObject.SetActive(false);
            gone = false;
        } else {
            gameObject.SetActive(true);
            gone = true;
        }
    }
}
