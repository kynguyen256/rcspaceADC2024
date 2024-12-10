using System;
using UnityEngine;
using Unity.Collections;

public class SwitchCamera : MonoBehaviour
{
   public GameObject Camera1;
   public GameObject Camera2;
   public int manager;
   // Used to scale everything according to which camera is selected
   public static int cameraNumber;

    void Start()
    {
        manager = 1;
        ManageCamera();
    }

    public void ChangeCamera()
    {
        GetComponent<Animator>().SetTrigger("Change");
    }
    public void ManageCamera()
    {
        if(manager == 0)
        {
            Cam2();
            manager = 1;
            cameraNumber = 2;
        }
        else
        {
            Cam1();
            manager = 0;
            cameraNumber = 1;
        }
    }
   void Cam1()
   {
    Camera1.SetActive(true);
    Camera2.SetActive(false);
   }

   void Cam2()
   {
    Camera2.SetActive(true);
    Camera1.SetActive(false);
   }
}
