using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public Texture2D coinSpawnTexture;

    public GameObject objectToSpawn;

    public float rayCastHeight;
    public float spacing;
    public float coinPixelSize;

    // Start is called before the first frame update
    void Start()
    {
         
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            SpawnCoinsFromImage();
        }
        
    }

    void SpawnCoinsFromImage()
    {
        if(coinSpawnTexture == null)
        {
            Debug.Log("No Texture!");
            return;
        }

        bool[,] occupiedPixels = new bool[coinSpawnTexture.width , coinSpawnTexture.height];

        int coinCount = 0;

        //goes through rows of 2D array
        for (float y = 0; y < coinSpawnTexture.height; y += coinPixelSize)
        {
            //goes through columns
            for(float x = 0; x < coinSpawnTexture.width; x += coinPixelSize)
            {
                if(CanSpawnCoins(coinSpawnTexture, Mathf.FloorToInt(x), Mathf.FloorToInt(y), occupiedPixels))
                {
                    MarkOccupied(Mathf.FloorToInt(x), Mathf.FloorToInt(y), occupiedPixels);

                    Vector3 spawnPosition = new Vector3(x * spacing, rayCastHeight, y * spacing) + transform.position;
                    
                    if(Physics.Raycast(spawnPosition, Vector3.down, out RaycastHit hit, rayCastHeight * 2))
                    {
                        spawnPosition.y = hit.point.y;
                    }

                    Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

                    Debug.Log("Coins Spawned: " + coinCount);
                }
            }
        }
    }

    //checks to see if theres enough space to spawn coin
    bool CanSpawnCoins(Texture2D image, int startX, int startY, bool[,] occupied)
    {
        int redPixelCount = 0;

        for (int y = 0; y < Mathf.CeilToInt(coinPixelSize); y ++)
        {
            //goes through columns
            for (int x = 0; x < Mathf.CeilToInt(coinPixelSize); x ++)
            {
                int pixelX = startX + x;
                int pixelY = startY + y;

                if(pixelX >= image.width || pixelY >= image.height)
                {
                    continue;
                }

                if (occupied[pixelX, pixelY])
                {
                    return false;
                }

                Color pixelColor = image.GetPixel(pixelX, pixelY);
                if (IsRed(pixelColor))
                {
                    redPixelCount++;
                }
            }
        }


        return redPixelCount >= coinPixelSize;
    }

    void MarkOccupied(int startX, int startY, bool[,] occupied)
    {
        for (int y = 0; y < Mathf.CeilToInt(coinPixelSize); y++)
        {
            //goes through columns
            for (int x = 0; x < Mathf.CeilToInt(coinPixelSize); x++)
            {
                int pixelX = startX + x;
                int pixelY = startY + y;

                if(pixelX >= occupied.GetLength(0) || pixelY >= occupied.GetLength(1))
                {
                    continue ;
                }

                occupied[pixelX, pixelY] = true;
            }
        }
    }

    private bool IsRed(Color color)
    {
        return color.r > 0 && color.g < 1 && color.b < 1;
    }
}
