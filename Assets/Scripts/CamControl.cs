using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    public static CamControl Instance { get; private set; }
    public CamMover n, s, e, w;
    public Vector2 axis { get; private set; }
    private float x, y;
    public float augSpeed;
    public float gain, div;
    private void Awake()
    {
        Instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        if (e.touched)
        {
            x += augSpeed;
            x = Mathf.Clamp(x, 0, 1);
        }
        else if (w.touched)
        {
            x -= augSpeed;

            x = Mathf.Clamp(x, -1, 0);
        }
        else
        {
            if (x != 0)
            {
                x /= div;
                if (Mathf.Abs(x) < gain)
                    x = 0;
            }
        }

        if (n.touched)
        {
            y += augSpeed;

            y = Mathf.Clamp(y, 0, 1);
        }
        else if (s.touched)
        {
            y -= augSpeed;
            y = Mathf.Clamp(y, -1, 0);
        }
        else
        {
            if (y != 0)
            {
                y /= div;
                if (Mathf.Abs(y) < gain)
                    y = 0;
            }
        }
        axis = new Vector2(x, y);
    }
}
