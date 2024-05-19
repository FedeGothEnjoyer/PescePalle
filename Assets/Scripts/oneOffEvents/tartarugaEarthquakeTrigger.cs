using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tartarugaEarthquakeTrigger : MonoBehaviour
{
    DialougeManager dialogue;
    void Start()
    {
        dialogue = FindFirstObjectByType<DialougeManager>();
    }

    void Update()
    {
        if(dialogue.sentences.Count == 1)
		{
            Camera.main.GetComponent<CameraShake>().Shake(1000f);
            Destroy(this);
		}
    }
}
