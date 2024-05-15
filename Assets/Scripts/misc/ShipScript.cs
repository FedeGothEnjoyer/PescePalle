using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipScript : MonoBehaviour
{
    [SerializeField] float increment = 0.3f;
    [SerializeField] float time = 10f;
    float currentTime;

    // Start is called before the first frame update
    void Start()
    {
		if (CurrentData.shipFallen)
		{
            transform.localPosition = new Vector3(0, -1, 10);
            currentTime = time;
            return;
		}
		else
		{
            Camera.main.GetComponent<CameraShake>().Shake(time);
		}
        CurrentData.shipFallen = true;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime <= time)
            transform.position -= new Vector3(0, increment, 0) * Time.deltaTime;
    }
}
