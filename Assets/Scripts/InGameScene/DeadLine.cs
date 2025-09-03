using System.Collections.Generic;
using UnityEngine;

public class DeadLine : MonoBehaviour
{
    private int staytime = 0;

    [SerializeField]
    private GameManager gameManager;

    private List<GameObject> collidedList;

    void Awake()
    {
        collidedList = new List<GameObject>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        collidedList.Add(other.gameObject);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (collidedList.Contains(other.gameObject))
            collidedList.Remove(other.gameObject);
    }

    void Update()
    {
        if (collidedList.Count > 0)
        {
            staytime += 1;
        }
        else
        {
            staytime = 0;
        }

        if (staytime >= 180)
        {
            gameManager.GameOver();
        }
    }
}
