using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Entered");
        TutoManager.Instance.Next();
        Destroy(gameObject);
    }
}
