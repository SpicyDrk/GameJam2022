using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgHandler : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    GameObject[] bgs;


    private Vector2 offset = Vector2.zero;
    private Vector2 bgSize = new Vector2(18f, 10f);
    
    void Start()
    {
        bgs = GameObject.FindGameObjectsWithTag("bg");
    }

    void FixedUpdate()
    {
        var yMax = bgs[0].transform.position.y;
        var yMin = bgs[0].transform.position.y;
        var xMin = bgs[0].transform.position.x;
        var xMax = bgs[0].transform.position.x;
        //get bounds of the bgs;
        foreach(var bg in bgs)
        {
            if(bg.transform.position.x > xMax)
            {
                xMax = bg.transform.position.x;
            }
            if (bg.transform.position.y > yMax)
            {
                yMax = bg.transform.position.y;
            }
            if (bg.transform.position.y < yMin)
            {
                yMin = bg.transform.position.y;
            }
            if (bg.transform.position.x < xMin)
            {
                xMin = bg.transform.position.x;
            }
        }

        if (player.transform.position.y > yMax)
        {
            foreach (var bg in bgs)
            {
                
                if (bg.transform.position.y < (player.transform.position.y - bgSize.y- 1f))
                {
                    bg.transform.position = bg.transform.position + new Vector3(0f, bgSize.y * 3f);
                }
            }
        }
        if (player.transform.position.y < yMin)
        {
            foreach (var bg in bgs)
            {

                if (bg.transform.position.y > (player.transform.position.y + bgSize.y + 1f))
                {
                    bg.transform.position = bg.transform.position - new Vector3(0f, bgSize.y * 3f);
                }
            }
        }
        if (player.transform.position.x < xMin)
        {
            foreach (var bg in bgs)
            {

                if (bg.transform.position.x > (player.transform.position.x + bgSize.x + 1f))
                {
                    bg.transform.position = bg.transform.position - new Vector3(bgSize.x * 3f, 0f);
                }
            }
        }

        if (player.transform.position.x > xMax)
        {
            foreach (var bg in bgs)
            {

                if (bg.transform.position.x < (player.transform.position.x - bgSize.x - 1f))
                {
                    bg.transform.position = bg.transform.position + new Vector3(bgSize.x * 3f, 0f);
                }
            }
        }
    }
}
