using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowBest : MonoBehaviour
{
    public Transform farthestStickman;
    public StickmanMendelMachine stickmanMendel;
    void Update()
    {
        try
        {
            transform.position = farthestStickman.position - new Vector3(0, 0, 10);
        }
        catch
        {
           
        }
    }
}
