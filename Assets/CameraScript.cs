using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Transform kameraPozice;
    private Camera kameraZoom;
    private Rigidbody rigidbody;
    private float zoomKamery=10;
    private float deltaZoom;

    private Vector2 mouseMove;
    private Vector2 mousePosition;
    private float moveSensitivity=0.8F;
    // Start is called before the first frame update
    void Start()
    {
        kameraPozice = GetComponent<Transform>();
        kameraZoom = GetComponent<Camera>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //zoom
        deltaZoom = Input.mouseScrollDelta.y;
        zoomKamery -= deltaZoom*(1+zoomKamery/50);
        if (zoomKamery > 200)
        {
            zoomKamery = 200;
        }
        else if (zoomKamery < 10)
        {
            zoomKamery = 10;
        }
        kameraZoom.orthographicSize = zoomKamery;

        //moving
        mousePosition = new Vector2((Input.mousePosition.x-Screen.width/2)/(Screen.width/2), (Input.mousePosition.y - Screen.height / 2) / (Screen.height / 2));
        Debug.Log(mousePosition);
    }
    private void FixedUpdate()
    {
        //x movement
        mouseMove = new Vector2(0, 0);
        if (Mathf.Abs(mousePosition.x) > moveSensitivity)
        {
            if (mousePosition.x > 0)
            {
                mouseMove.x = (mousePosition.x - moveSensitivity) / (1- moveSensitivity);
            }
            else
            {
                mouseMove.x = (mousePosition.x + moveSensitivity) / (1 - moveSensitivity);
            }
        }

        //y movement
        if (Mathf.Abs(mousePosition.y) > moveSensitivity)
        {
            if (mousePosition.y > 0)
            {
                mouseMove.y = (mousePosition.y - moveSensitivity) / (1 - moveSensitivity);
            }
            else
            {
                mouseMove.y = (mousePosition.y + moveSensitivity) / (1 - moveSensitivity);
            }
        }
        rigidbody.AddForce(mouseMove * 50);
    }
}
