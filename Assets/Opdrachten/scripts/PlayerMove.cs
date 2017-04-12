using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerMove : NetworkBehaviour {
	[SyncVar]
	public int score = 0;

	public TextMesh scoreText;
	public GameObject bulletPrefab;

	private GameObject mainCamera;
	private Vector3 startPosition;
	private Quaternion startRotation;
	private float timer = 0;

	void Start() {
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		startPosition = transform.position;
		startRotation = transform.rotation;
	}
	
	void Update () {
		RpcSetScore(this.score);
		scoreText.transform.LookAt(scoreText.transform.position - mainCamera.transform.position);

		if (!isLocalPlayer)
			return;

		float h = Input.GetAxis("Horizontal") * 0.6f;
		float v = Input.GetAxis("Vertical") * 0.32f;

		transform.Rotate(new Vector3(0, h, 0));
		transform.position += transform.forward * v;

		if (Input.GetKeyDown(KeyCode.Space))
			CmdFire();

		if (transform.position.y < -4) {
			CmdResetScore();
			transform.position = startPosition;
			transform.rotation = startRotation;
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		}
		
		if (timer < 0) {
			CmdResetScale();
		} else {
			timer -= Time.deltaTime;
		}
	}

	public override void OnStartLocalPlayer() {
		GetComponent<MeshRenderer>().material.color = Color.red;
	}

	public void addScore(int score) {
		this.score += score;
	}

	public void narrowDown() {
		RpcNarrowDown();
	}

	[ClientRpc]
	private void RpcSetScore(int score) {
		scoreText.text = "Score: " + score;
		if (score >= 5)
			scoreText.text = "Gewonnen!";
	}
		

	[ClientRpc]
	private void RpcResetScale() {
		transform.localScale = new Vector3(1f, 1f, 1f);
	}

	[Command]
	private void CmdResetScale() {
		transform.localScale = new Vector3(1f, 1f, 1f);
		RpcResetScale();
	}

	[ClientRpc]
	private void RpcNarrowDown() {
		timer = 10;
		transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
	}

	[Command]
	private void CmdResetScore() {
		this.score = 0;
	}

	[Command]
	private void CmdFire() {
		var bullet = (GameObject)Instantiate(bulletPrefab, transform.position + transform.up, Quaternion.identity);

		bullet.GetComponent<Rigidbody>().velocity = transform.forward * 18;
		bullet.GetComponent<BulletHit>().sender = this.gameObject;

		NetworkServer.Spawn(bullet);

		Destroy(bullet, 2.0f);
	}
}
