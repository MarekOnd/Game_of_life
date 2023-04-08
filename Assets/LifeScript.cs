using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LifeScript : MonoBehaviour
{
    public Grid grid;
    public Tilemap main;
    public Tilemap temp;
    public Tile life;
    public Tile dead;
    public bool gameOn = false;

    private Vector3Int maxDistance = new Vector3Int(0,0,0);
    private Vector3Int minDistance= new Vector3Int(0,0,0);

    private float deltaTime = 0;
    private float fps = 5;


    private Vector3Int mouseInGrid;

    private void OnMouseDown()
    {
        Debug.Log("ahoj");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (gameOn)
        {
            deltaTime += Time.deltaTime;
            if (deltaTime > 1 / fps)
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
                minDistance.x = Mathf.Min(mouseInGrid.x - 1, minDistance.x);
                minDistance.y = Mathf.Min(mouseInGrid.y - 1, minDistance.y);
                maxDistance.x = Mathf.Max(mouseInGrid.x + 1, maxDistance.x);
                maxDistance.y = Mathf.Max(mouseInGrid.y + 1, maxDistance.y);
            }
            else if (Input.GetKey(KeyCode.Mouse1))
            {
                mouseInGrid = grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                main.SetTile(mouseInGrid, dead);

            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                main.ClearAllTiles();
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameSwitch();
        }
        if(Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            if(fps<20)
            {
                fps++;
            }
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            if (fps > 1)
            {
                fps--;
            }
        }
    }
    void FixedUpdate()
    {
        
    }

    private int adjectentLife;
    public void UpdateMap()
    {
        temp.ClearAllTiles();
        for (int x = minDistance.x; x <= maxDistance.x; x++)
        {
            for (int y = minDistance.y; y <= maxDistance.x; y++)
            {
                adjectentLife = CountAdjectent(new Vector3Int(x, y, 0));
                if (main.GetSprite(new Vector3Int(x, y, 0)) == life.sprite)
                {
                    if (adjectentLife <2 || adjectentLife > 3)
                    {
                        //temp.SetTile(new Vector3Int(x, y, 0), dead);
                    }
                    else
                    {
                        temp.SetTile(new Vector3Int(x, y, 0), life);
                        minDistance.x = Mathf.Min(x-1, minDistance.x);
                        minDistance.y = Mathf.Min(y-1, minDistance.y);
                        maxDistance.x = Mathf.Max(x+1, maxDistance.x );
                        maxDistance.y = Mathf.Max(y+1, maxDistance.y );
                    }
                }
                else
                {
                    if (adjectentLife == 3)
                    {
                        temp.SetTile(new Vector3Int(x, y, 0), life);
                        minDistance.x = Mathf.Min(x - 1, minDistance.x);
                        minDistance.y = Mathf.Min(y - 1, minDistance.y);
                        maxDistance.x = Mathf.Max(x + 1, maxDistance.x);
                        maxDistance.y = Mathf.Max(y + 1, maxDistance.y);
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
    private int CountAdjectent(Vector3Int location)
    {
        int value=0;
        for(int x = -1;x<=1;x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if(main.GetSprite(new Vector3Int(location.x+x,location.y+y,0))==life.sprite && (x!=0||y!=0))
                {
                    value++;
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
