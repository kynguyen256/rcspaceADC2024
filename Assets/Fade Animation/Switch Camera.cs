using System;
using UnityEngine;
using Unity.Collections;

public class SwitchCamera : MonoBehaviour
{
   public GameObject Camera1;
   public GameObject Camera2;
   public int manager;

    void Start()
    {
        manager = 0;
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
        }
        else
        {
            Cam1();
            manager = 0;
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
