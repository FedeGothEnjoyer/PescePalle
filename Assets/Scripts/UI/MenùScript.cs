using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenùScript : MonoBehaviour
{
    [SerializeField] List<SceneAsset> taneGiorni;
    [SerializeField] RectTransform title;
    [SerializeField] float animationStrength;
    [SerializeField] float animationSpeed;
    private float y;

	private void Start()
	{
        y = title.position.y;
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
        SceneManager.LoadScene(taneGiorni[CurrentData.day-1].name);
    }

    public void QuitGame()
    {
        Debug.Log("Game quitted.");
        Application.Quit();
    }
}
