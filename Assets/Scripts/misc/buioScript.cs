using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buioScript : MonoBehaviour
{
	SpriteMask mask;
	float maskSize;
	private void Start()
	{
		mask = GetComponent<SpriteMask>();
		maskSize = mask.alphaCutoff;
	}

	void Update()
    {
		mask.alphaCutoff = Mathf.Sin(Time.time*3f) * 0.1f + maskSize;
    }
}
