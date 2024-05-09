using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class algaOffset : MonoBehaviour
{
    void Start()
    {
        GetComponent<Animator>().SetFloat("offset", Random.Range(0, 1f));
        GetComponent<Animator>().speed = Random.Range(0.4f, 0.8f);
    }
}
