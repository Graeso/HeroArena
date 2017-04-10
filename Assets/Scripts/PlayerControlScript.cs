using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class PlayerControlScript : MonoBehaviour {
	public GameObject playerPrefab;

	const int maxPlayers = 4;

	private static Vector3 p1SpawnSpoint = new Vector3 (-4.46f, .15f, .06f);
	private static Vector3 p2SpawnSpoint = new Vector3 (-1.46f, .15f, -.46f);
	private static Vector3 p3SpawnSpoint = new Vector3 (-4.69f, .15f, -2.7f);
	private static Vector3 p4SpawnSpoint = new Vector3 (-1.39f, .15f, -2.88f);

	List<Vector3> playerPositions = new List<Vector3> () {
		p1SpawnSpoint,
		p2SpawnSpoint,
		p3SpawnSpoint,
		p4SpawnSpoint,
	};

	List<PlayerControlScript> players = new List<PlayerControlScript> (maxPlayers);

	void Start() {
		InputManager.OnDeviceDetached += OnDeviceDetached;
	}

	void Update() {
		var inputDevice = InputManager.ActiveDevice;

		if (JoinButtonWasPressedOnDevice (inputDevice)) {
			if (ThereIsNoPlayerUsingDevice (inputDevice)) {
				CreatePlayer (inputDevice);
			}
		}
	}

	bool JoinButtonWasPressedOnDevice (InputDevice inputDevice) {
		return inputDevice.Action1.WasPressed || inputDevice.Action2.WasPressed || inputDevice.Action3.WasPressed || inputDevice.Action4.WasPressed;
	}

	PlayerControlScript FindPlayerUsingDevice (InputDevice inputDevice) {
		var playerCount = players.Count;
		for (var i = 0; i < playerCount; i++) {
			var player = players[i];
			if (XanderScript.S.Device == inputDevice) {
				return player;
			}
		}

		return null;
	}

	bool ThereIsNoPlayerUsingDevice (InputDevice inputDevice) {
		return FindPlayerUsingDevice (inputDevice) == null;
	}

	void OnDeviceDetached (InputDevice inputDevice) {
		var player = FindPlayerUsingDevice (inputDevice);
		if (player != null) {
			RemovePlayer (player);
		}
	}

	PlayerControlScript CreatePlayer (InputDevice inputDevice) {
		if (players.Count < maxPlayers) {
			var playerPosition = playerPositions[0];
			playerPositions.RemoveAt( 0 );
			var gameObject = (GameObject) Instantiate( playerPrefab, playerPosition, Quaternion.identity);
			var player = gameObject.GetComponent<PlayerControlScript>();
			XanderScript.S.Device = inputDevice;
			players.Add (player);

			return player;
		}

		return null;
	}

	void RemovePlayer (PlayerControlScript player) {
		playerPositions.Insert( 0, player.transform.position );
		players.Remove (player);
		XanderScript.S.Device = null;
		Destroy( player.gameObject );
	}

	/*
	void OnGUI() {
		const float h = 22.0f;
		var y = 10.0f;

		GUI.Label (new Rect(10, y, 300, y + h), "Active players: " + players.Count + "/" + maxPlayers);
		y += h;

		if (players.Count < maxPlayers) {
			GUI.Label (new Rect(10, y, 300, y + h), "Press a button to join!");
			y += h;
		}
	}
	*/
}
