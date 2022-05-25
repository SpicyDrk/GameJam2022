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
        //Instantiate(popper, Vector3.zero, Quaternion.identity);
        SetScreenBounds();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeSinceLastEnemy > .05f)
        {
            var spawnSides = new string[] { "left", "right", "up", "down" };
            var sideSelection = spawnSides[Random.Range(0, spawnSides.Length)];
            var playerLoc = new Vector2(player.transform.position.x, player.transform.position.y);
            Vector3 spawnLoc = Vector3.zero;
            spawnLoc = sideSelection switch
            {
                "left" => new Vector3(playerLoc.x + minX, Random.Range(minY  + 1f, maxY - 1f) + playerLoc.y, 1f),
                "right" => new Vector3(playerLoc.x + maxX, Random.Range(minY + 1f, maxY  - 1f) + playerLoc.y, 1f),
                "up" => new Vector3(Random.Range(minX + 1f, maxX - 1f) + playerLoc.x, playerLoc.y + maxY, 1f),
                "down" => new Vector3(Random.Range(minX + 1f, maxX - 1f) + playerLoc.x, playerLoc.y + minY, 1f),
                _ => new Vector3(player.transform.position.x + minX, player.transform.position.y, 1f),
            };
            Instantiate(popper, spawnLoc, Quaternion.identity);
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
