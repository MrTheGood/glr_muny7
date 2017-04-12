using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {
	public Transform objectToFollow;	//Het object dat gevolgt moet worden
	public float distance = 100; 				//De afstand tussen het volg object en het volgende object.

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		//Als het volg object bestaad,
		if (objectToFollow != null) {
			//Bereken de nieuwe positie door de x-waarde te zetten op de x-waarde van het volg object min de afstand.
			Vector3 position = transform.position;
			position.z = objectToFollow.position.z - distance;
			transform.position = position;
		}
	}
}
