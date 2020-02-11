using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    public static CamControl Instance { get; private set; }
    public CamMover n, s, e, w;
    public Vector2 axis { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        if (e.touched)
        {
            axis = new Vector2(1, axis.y);
        }
        else if (w.touched)
        {
            axis = new Vector2(-1, axis.y);
        }
        else
        {
            axis = new Vector2(0, axis.y);
        }

        if (n.touched)
        {
            axis = new Vector2(axis.x, 1);
        }
        else if (s.touched)
        {
            axis = new Vector2(axis.x, -1);
        }
        else
        {
            axis = new Vector2(axis.x, 0);
        }
    }
}
