using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ShipNewDay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DialougeManager.instance.dialougeEnded.AddListener(NewDay);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void NewDay()
    {
        Debug.Log("New day");
        FoodManager.foodTaken = 2;
        FindObjectOfType<PlayerMovement>().ChangingRoom(new Vector2(-6.2f, 0.24f), false, "Ship4 5", true, "Ship4 5");
    }
}
