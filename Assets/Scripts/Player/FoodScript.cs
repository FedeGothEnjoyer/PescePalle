using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FoodScript : MonoBehaviour
{
    [SerializeField] GameObject effect;
    public UnityEvent foodTaken;

	private void Start()
	{
        foodTaken = new UnityEvent();
	}

	private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            PlayerMovement.foodPositions.Add(transform.position);
            FoodManager.foodTaken += 1;
            Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

	private void OnDestroy()
	{
        foodTaken.Invoke();
    }
}
