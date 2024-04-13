using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] Vector2 targetPos;

    SpriteRenderer render;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = transform.position;
        render = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isInputEnable = FindAnyObjectByType<InputBlock>().isInputEnble;

		if (Input.GetMouseButtonDown(0) && isInputEnable)
		{
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            render.flipX = targetPos.x < transform.position.x;
        }

        var movement = Vector2.ClampMagnitude(Vector2.Lerp(transform.position, targetPos, speed * Time.deltaTime) - (Vector2)transform.position, maxSpeed*Time.deltaTime);
        transform.position = transform.position + (Vector3)movement;
    }
}
