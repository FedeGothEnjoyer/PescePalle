using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenùScript : MonoBehaviour
{
    [SerializeField] List<string> taneGiorni;
    [SerializeField] RectTransform title;
    [SerializeField] float animationStrength;
    [SerializeField] float animationSpeed;
    private float y;

    private void Awake()
    {

    }

    private void Start()
	{
        y = title.position.y;
        DataSystem.LoadData();
        if (CurrentData.day == -1)
		{
            GameObject.Find("Continue").GetComponent<Button>().interactable = false;
		}
	}

	private void Update()
	{
        title.position = new Vector2(title.position.x, Mathf.Sin(Time.time*animationSpeed)*animationStrength+y);
	}

	public void StartNewGame()
    {
        SceneManager.LoadScene("Tana");
        CurrentData.day = 1;
    }

    public void StartTutorial()
    {
        SceneManager.LoadScene("Tuto");
        CurrentData.day = 1;
    }

    public void ContinueGame()
    {
        DataSystem.LoadData();
        if(CurrentData.day == -1)
        {
            return;
        }
        SceneManager.LoadScene(taneGiorni[CurrentData.day-1]);
    }

    public void QuitGame()
    {
        Debug.Log("Game quitted.");
        Application.Quit();
    }
}
