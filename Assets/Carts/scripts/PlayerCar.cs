using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerCar : NetworkBehaviour {
	public WheelCollider frontLeftWheel;
	public WheelCollider frontRightWheel;
	public WheelCollider backLeftWheel;
	public WheelCollider backRightWheel;
	public float maxMotorPower = 450;
	public float maxSteeringAngle = 30;
	public float resetTime = 5f;

	private float resetTimer = 0f;

	// Use this for initialization
	void Start () {

	}

	public override void OnStartLocalPlayer() {
//		GetComponent<MeshRenderer>().material.color = Color.blue;
		Camera.main.GetComponent<SmoothCameraFollow>().target = transform;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (isLocalPlayer) {
			float motorTorque = maxMotorPower * Input.GetAxis("Vertical");
			float steerAngle = maxSteeringAngle * Input.GetAxis("Horizontal");
			CmdMoveCar(motorTorque, steerAngle);
		}
	}

	[Command]
	void CmdMoveCar(float motorTorque, float steerAngle) {
		Vector3 position;
		Quaternion rotation;
		Transform wheel;

		//Set the steer angle and the motortorque of the wheel collider.
		frontLeftWheel.steerAngle = steerAngle;
		frontRightWheel.steerAngle = steerAngle;

		backLeftWheel.motorTorque = motorTorque;
		backRightWheel.motorTorque = motorTorque;

		//Set the wheels themselves.
		wheel = frontLeftWheel.transform.GetChild(0);
		frontLeftWheel.GetWorldPose(out position, out rotation);
		wheel.transform.position = position;
		wheel.transform.rotation = rotation;

		wheel = frontRightWheel.transform.GetChild(0);
		frontRightWheel.GetWorldPose(out position, out rotation);
		wheel.transform.position = position;
		wheel.transform.rotation = rotation;

		wheel = backLeftWheel.transform.GetChild(0);
		backLeftWheel.GetWorldPose(out position, out rotation);
		wheel.transform.position = position;
		wheel.transform.rotation = rotation;

		wheel = backRightWheel.transform.GetChild(0);
		backRightWheel.GetWorldPose(out position, out rotation);
		wheel.transform.position = position;
		wheel.transform.rotation = rotation;

		RpcMoveCar(motorTorque, steerAngle);
		CmdCrashControle();
	}

	[Command]
	private void CmdCrashControle() {
		if (transform.localEulerAngles.z > 80 && transform.localEulerAngles.z < 280) {
			resetTimer += Time.deltaTime;
		} else {
			resetTimer = 0;
		}

		if (resetTimer > resetTime) {
			CmdFlipCar();
			RpcFlipCar();
		}
	}

	[Command]
	private void CmdFlipCar() {
		transform.rotation = Quaternion.LookRotation(transform.forward);
		transform.position += Vector3.up * 0.5f;
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		resetTimer = 0;
	}




	[ClientRpc]
	void RpcMoveCar(float motorTorque, float steerAngle) {
		Vector3 position;
		Quaternion rotation;
		Transform wheel;

		//Set the steer angle and the motortorque of the wheel collider.
		frontLeftWheel.steerAngle = steerAngle;
		frontRightWheel.steerAngle = steerAngle;

		backLeftWheel.motorTorque = motorTorque;
		backRightWheel.motorTorque = motorTorque;

		//Set the wheels themselves.
		wheel = frontLeftWheel.transform.GetChild(0);
		frontLeftWheel.GetWorldPose(out position, out rotation);
		wheel.transform.position = position;
		wheel.transform.rotation = rotation;

		wheel = frontRightWheel.transform.GetChild(0);
		frontRightWheel.GetWorldPose(out position, out rotation);
		wheel.transform.position = position;
		wheel.transform.rotation = rotation;

		wheel = backLeftWheel.transform.GetChild(0);
		backLeftWheel.GetWorldPose(out position, out rotation);
		wheel.transform.position = position;
		wheel.transform.rotation = rotation;

		wheel = backRightWheel.transform.GetChild(0);
		backRightWheel.GetWorldPose(out position, out rotation);
		wheel.transform.position = position;
		wheel.transform.rotation = rotation;
	}

	[ClientRpc]
	private void RpcFlipCar() {
		transform.rotation = Quaternion.LookRotation(transform.forward);
		transform.position += Vector3.up * 0.5f;
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		resetTimer = 0;
	}
}