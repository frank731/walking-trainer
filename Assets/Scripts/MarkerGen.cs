using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerGen : MonoBehaviour
{
    public GameObject marker;
    private int x = 0;
    public void Start()
    {
        for(int i = 0; i < 40; i++)
        {
            GameObject mark = Instantiate(marker, new Vector2(x, 10.34f), transform.rotation);
            mark.GetComponent<TMPro.TextMeshPro>().text = x.ToString() + "m";
            x += 10;
        }
    }
}
