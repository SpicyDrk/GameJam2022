using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject popper, cucumber, player;
    private float minX, maxX, minY, maxY, extendedBounds = 3f;

    private float timeSinceLastEnemy = 0f;
    private void Awake()
    {
        Instantiate(popper, Vector3.zero, Quaternion.identity);
        SetScreenBounds();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeSinceLastEnemy > 2f)
        {
            //Instantiate(popper, player.transform.position, Quaternion.identity);
            timeSinceLastEnemy = 0f;
        }
        timeSinceLastEnemy += Time.deltaTime;
    }
    void SetScreenBounds()
    {
        Vector3 bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        maxX = bounds.x + extendedBounds;
        minX = -bounds.x - extendedBounds;
        minY = -bounds.y - extendedBounds;
        maxY = bounds.y + extendedBounds;
    }

}
