using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LifeScript2 : MonoBehaviour
{
    public Grid grid;
    public Tilemap main;
    public Tilemap temp;
    public Tile life;
    public Tile dead;
    public bool gameOn = false;

    private Vector3Int maxDistance = new Vector3Int(5,5,0);
    private Vector3Int minDistance= new Vector3Int(-5,5,0);

    private float deltaTime = 0;
    private float fps = 5;


    private Vector3Int mouseInGrid;

    //arrays
    private bool[,] mainArray;
    private bool[,] tempArray;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
    void FixedUpdate()
    {
        if(gameOn)
        {
            deltaTime += Time.deltaTime;
            if(deltaTime > 1/fps)
            {
                UpdateMap();
                deltaTime = 0;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                mouseInGrid = grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                main.SetTile(mouseInGrid, life);
                minDistance.x = Mathf.Min(mouseInGrid.x, minDistance.x - 1);
                minDistance.y = Mathf.Min(mouseInGrid.y, minDistance.y - 1);
                maxDistance.x = Mathf.Max(mouseInGrid.x, maxDistance.x + 1);
                maxDistance.y = Mathf.Max(mouseInGrid.y, maxDistance.y + 1);
            }
            else if (Input.GetKey(KeyCode.Mouse1))
            {
                mouseInGrid = grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                main.SetTile(mouseInGrid, dead);
                minDistance.x = Mathf.Min(mouseInGrid.x, minDistance.x - 1);
                minDistance.y = Mathf.Min(mouseInGrid.y, minDistance.y - 1);
                maxDistance.x = Mathf.Max(mouseInGrid.x, maxDistance.x + 1);
                maxDistance.y = Mathf.Max(mouseInGrid.y, maxDistance.y + 1);
            }
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            GameSwitch();
        }
    }
    void MainTilemapToArray()
    {
        mainArray = new bool[maxDistance.x-minDistance.x, maxDistance.y - minDistance.y];
        for (int x = 0; x < maxDistance.x-minDistance.x; x++)
        {
            for (int y = 0; y < maxDistance.y - minDistance.y; x++)
            {
                if(main.GetSprite(new Vector3Int(x, y, 0)) == life.sprite)
                {
                    mainArray[x, y] = true;
                }
            }
        }
    }
    private int adjectentLife;
    public void UpdateMap()
    {
        
        for (int x = 0; x < maxDistance.x - minDistance.x; x++)
        {
            for (int y = 0; y < maxDistance.y - minDistance.y; x++)
            {
                adjectentLife = CountAdjectent(x,y);
                if (mainArray[x,y] && (adjectentLife==2 || adjectentLife==3))
                {
                    tempArray[x, y] = true;
                    if (adjectentLife <2 || adjectentLife > 3)
                    {
                        //temp.SetTile(new Vector3Int(x, y, 0), dead);
                    }
                    else
                    {
                        temp.SetTile(new Vector3Int(x, y, 0), life);
                        minDistance.x = Mathf.Min(x, minDistance.x - 1);
                        minDistance.y = Mathf.Min(y, minDistance.y - 1);
                        maxDistance.x = Mathf.Max(x, maxDistance.x + 1);
                        maxDistance.y = Mathf.Max(y, maxDistance.y + 1);
                    }
                }
                else
                {
                    if (adjectentLife == 3)
                    {
                        temp.SetTile(new Vector3Int(x, y, 0), life);
                        minDistance.x = Mathf.Min(x, minDistance.x - 1);
                        minDistance.y = Mathf.Min(y, minDistance.y - 1);
                        maxDistance.x = Mathf.Max(x, maxDistance.x + 1);
                        maxDistance.y = Mathf.Max(y, maxDistance.y + 1);
                    }
                    else
                    {
                        //temp.SetTile(new Vector3Int(x, y, 0), dead);
                        
                    }
                }
            }
        }
        main.ClearAllTiles();
        //update main map
        for (int x = minDistance.x; x <= maxDistance.x; x++)
        {
            for (int y = minDistance.y; y <= maxDistance.x; y++)
            {
                main.SetTile(new Vector3Int(x, y, 0), temp.GetTile(new Vector3Int(x, y, 0)));
            }
        }
    }
    private int CountAdjectent(int x0, int y0)
    {
        int value=0;
        for(int x = -1;x<=1;x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                try
                {
                    if (mainArray[x0 + x, y0 + y])
                    {
                        value++;
                    }
                }
                catch
                {

                }
            }
        }
        return value;
    }
    public void GameSwitch()
    {
        gameOn = !gameOn;
    }
}
