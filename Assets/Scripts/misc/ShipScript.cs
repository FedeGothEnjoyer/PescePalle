using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipScript : MonoBehaviour
{
    [SerializeField] float increment = 0.3f;
    [SerializeField] float time = 10f;
    float currentTime;
    static ShipScript instance;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;
        if (instance != null) Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
            SceneManager.activeSceneChanged += Deactivate;
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime <= time)
            transform.position -= new Vector3(0, increment, 0) * Time.deltaTime;
    }
    private void Deactivate(Scene s1, Scene s2)
    {
        currentTime = time;
        gameObject.transform.position = new Vector3(0, 0, 0);
    }
}
