using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class titoliDiCoda : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] SpriteRenderer background;
    [SerializeField] SpriteRenderer background2;
    [SerializeField] float backgroundY;
    [SerializeField] float finalY;
    [SerializeField] float fadeTime;
    RectTransform t;

    void Start()
    {
        t = GetComponent<RectTransform>();
        Destroy(PlayerMovement.active);
        StartCoroutine(Scroll());
    }

    IEnumerator Scroll()
	{
        while(t.position.y < finalY)
		{
            t.position += new Vector3(0, speed * Time.deltaTime,0);
            if(t.position.y > backgroundY)
			{
                background.color = new Color(1, 1, 1, (t.position.y-backgroundY)/(finalY-backgroundY));
			}
            yield return null;
		}

        yield return new WaitForSeconds(3f);
        float timer = 0f;
		while (background2.color.a < 1f)
		{
            timer += Time.deltaTime;
            background2.color = new Color(background2.color.r, background2.color.g, background2.color.b, timer / fadeTime);
            yield return null;
		}

        SceneManager.LoadScene("Menù");
	}
}
