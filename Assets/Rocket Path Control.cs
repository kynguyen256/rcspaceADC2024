using UnityEngine;

public class RocketPathControl : MonoBehaviour
{
    // Game Objects to reference
    public GameObject pathPoint; // Prefab sphere we will use to "draw" the path
    public GameObject referencePath; // In this case the rocket itself because that is the position data we want the path to follow
    public GameObject simManObject; // The Simulation Manager
    SimulationManager simManager; // We need to create an object in this script so that we can call Simulation Manager's methods
    
    // Variables
    public float posX;
    public float posY;
    public float posZ;
    public Vector3 pos = new Vector3();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        simManager = simManObject.GetComponent<SimulationManager>();
    }

    // Update is called once per frame
    void Update()
    {
        posX = (float)simManager.getData(SimulationManager.globalTime,1);
        posY = (float)simManager.getData(SimulationManager.globalTime,2);
        posZ = (float)simManager.getData(SimulationManager.globalTime,3);
        pos = new Vector3(posX/100, posY/100, posZ/100);

        transform.position = pos;

        GameObject pointClone = Instantiate(pathPoint, pos, referencePath.transform.rotation);
    }
}
