using UnityEngine;
using System.Collections;

public class BulletHit : MonoBehaviour {
	public GameObject sender;

	void OnCollisionEnter(Collision collision) {
		GameObject hit = collision.gameObject;
		PlayerMove hitPlayer = hit.GetComponent<PlayerMove>();
		if (hitPlayer != null) {
			sender.GetComponent<PlayerMove>().addScore(1);
			sender.GetComponent<PlayerMove>().narrowDown();

			Destroy(gameObject);
			//Destroy(hit);
		}
	}
}
