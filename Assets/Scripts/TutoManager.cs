using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoManager : MonoBehaviour
{
    private static TutoManager _instance;
    public static TutoManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<TutoManager>();
            return _instance;
        }
    }
    public Prefab_Pos monsterTuto;
    public List<Bubble> bubbles; //bool to know if the next bubble is to be printed now or after

    private void Start()
    {
        _instance = this;
        Next();
    }

    public void Next()
    {
        bubbles[0].bubble.SetActive(true);
        Time.timeScale = 0;
    }

    public void Remove()
    {
        bool isFollowed = bubbles[0].isFollowed;
        GameObject.Destroy(bubbles[0].bubble);
        bubbles.RemoveAt(0);
        if (isFollowed)
            Next();
        else
            Time.timeScale = 1;
        //TODO end tuto level when bubbles is empty
    }
}
[System.Serializable]
public struct Bubble
{
    public GameObject bubble;
    public bool isFollowed;

}
