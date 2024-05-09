using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bancoSpawner : MonoBehaviour
{
    [SerializeField] GameObject banco;
    [SerializeField] Vector2 yRange;
    [SerializeField] float waitTime;
    [SerializeField] float waitTimeMax;
    [SerializeField] bool flipped;
    [SerializeField] float range;
    float timer = 0f;

    // Update is called once per frame
    void Update()
    {
        if(timer <= 0f)
		{
            transform.position = new Vector2(transform.position.x, Random.Range(yRange.x, yRange.y));
            var g = Instantiate(banco, transform.position, Quaternion.identity);
            g.transform.localScale = Vector3.one * Random.Range(0.6f, 1f);
            if (flipped) g.transform.Rotate(new Vector3(0,180,0));
            g.GetComponent<bancoRoutine>().speed = Random.Range(1f, 2f);
            g.GetComponent<bancoRoutine>().flipped = flipped;
            g.GetComponent<bancoRoutine>().range = range;
            g.GetComponent<SpriteRenderer>().color = new Color(g.GetComponent<SpriteRenderer>().color.r, g.GetComponent<SpriteRenderer>().color.g, g.GetComponent<SpriteRenderer>().color.b, Random.Range(0.4f,0.8f));
            timer = Random.Range(waitTime,waitTimeMax);
        }
        timer -= Time.deltaTime;
    }
}
