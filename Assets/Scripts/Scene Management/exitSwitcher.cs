using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exitSwitcher : MonoBehaviour
{
    [SerializeField] string secondExit;
    [SerializeField] ChangeScene exit;

	private void Start()
	{
		GetComponent<FoodScript>().foodTaken.AddListener(Switch);
	}

	private void Switch()
	{
		exit.target = secondExit;
	}

}
