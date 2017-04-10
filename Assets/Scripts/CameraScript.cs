using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	public GameObject player1;
	//public Transform player1;
	public Transform player2;
	public Transform player3;
	public Transform player4;

	private const float DISTANCE_MARGIN = 1.0f;

	private Vector3 middlePoint;
	private float distanceFromMiddlePoint;
	private float distanceBetweenPlayers12;
	private float distanceBetweenPlayers34;
	private float distanceBetweenPlayersTotal;
	private float cameraDistance;
	private float aspectRatio;
	private float fov;
	private float tanFov;

	void Start() {
		/*
		// Find aspect ratio of screen
		aspectRatio = Screen.width / Screen.height;
		tanFov = Mathf.Tan (Mathf.Deg2Rad * Camera.main.fieldOfView / 2.0f);

		// Find each player in the game;
		*/
	}

	void Update () {
		player1 = GameObject.Find ("Xander");
		this.transform.parent = player1.transform;

		/*
		// Position the camera in the center
		Vector3 newCameraPos = Camera.main.transform.position;
		newCameraPos.x = middlePoint.x;
		Camera.main.transform.position = newCameraPos;

		// Find the middle point between players
		Vector3 vectorBetweenPlayers12 = player2.position - player1.position;
		Vector3 vectorBetweenPlayers34 = player3.position - player4.position;
		middlePoint = vectorBetweenPlayers12 + 0.5f * vectorBetweenPlayers34;

		// Calculate the new distance
		distanceBetweenPlayersTotal = vectorBetweenPlayers12.magnitude / vectorBetweenPlayers34.magnitude;
		cameraDistance = (distanceBetweenPlayersTotal / 2.0f / aspectRatio) / tanFov;

		// Set camera to new position
		Vector3 dir = (Camera.main.transform.position - middlePoint).normalized;
		Camera.main.transform.position = middlePoint + dir * (cameraDistance + DISTANCE_MARGIN);
		*/
	}

}