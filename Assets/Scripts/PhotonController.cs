using System;
using System.Collections;
using System.Collections.Generic;
using Photon;
using UnityEngine;

/**
 * Custom Photon Event Codes
 * Custom Events start from 101 since [1..100] is reserved by Photon
 */
public enum NetworkEventCodes : byte {
	ParticipantJoined = 101
}

public class PhotonController : PunBehaviour {
	public string GameVersion;
	public Transform RemoteAvatarSlot;
    public GameObject vRCam;

	public byte MaxPlayers = 2;

    public Transform TrackingSpace;
	public GameObject LocalAvatarPrefab;
	public GameObject RemoteAvatarPrefab;
    [SerializeField]
    private bool master;

	void Start () {
		// Players are grouped by game version, if two clients have another version than the other,
		// the won't be able to see each other!
		PhotonNetwork.ConnectUsingSettings (GameVersion);
	}

	void OnEnable () {
		PhotonNetwork.OnEventCall += OnEvent;
	}

	void OnDisable () {
		PhotonNetwork.OnEventCall -= OnEvent;
	}

	private void OnEvent (byte eventcode, object content, int senderid) {
		string eventName = "UNKNOWN";

		if (Enum.IsDefined (typeof (NetworkEventCodes), eventcode)) {
			eventName = Enum.GetName (typeof (NetworkEventCodes), eventcode);
		}
		
		Debug.Log (
			"[PhotonController]: Recieved event: [" +
			"eventName=" + eventName + 
			", eventCode=" + eventcode + 
			", senderId=" + senderid + 
			"]"
		);

		if (eventcode == (byte) NetworkEventCodes.ParticipantJoined) {
			GameObject go = null;
            
			Debug.Log ("[PhotonController]: Executing VR avatar instantiation");

			if (PhotonNetwork.player.ID == senderid) {
				Debug.Log ("[PhotonController]: Setup local avatar for sending");
				
                if (master == true) {
                    go = Instantiate (LocalAvatarPrefab, TrackingSpace);
                    print("spawn as presenter");
                    go.tag = "Target";
                }
                else {
                    go = Instantiate(LocalAvatarPrefab, RemoteAvatarSlot);
                    go.tag = "Player";
                    print("spawn as audience");
                    vRCam.transform.position = RemoteAvatarSlot.position;
                }
            } else {
				Debug.Log ("[PhotonController]: Instantiated remote avatar");
				if (RemoteAvatarPrefab) {
                    if (master == false) {
                        go = Instantiate(RemoteAvatarPrefab, RemoteAvatarSlot);
                        go.tag = "Target";
                        print("spawn as audience");
                        vRCam.transform.position = RemoteAvatarSlot.position;
                    }
                    else {
                        go = Instantiate(RemoteAvatarPrefab, TrackingSpace);
                        go.tag = "Player";
                        print("spawn as audience");
                    }
                }
			}

			if (go != null) {
				PhotonView pView = go.GetComponent<PhotonView> ();

				if (pView != null) {
					pView.viewID = (int) content;
				}
			}
		}
	}
	
	public override void OnConnectedToMaster () {
		Debug.Log ("[PhotonController]: Connected to master");
		PhotonNetwork.JoinRandomRoom ();
	}

	public override void OnJoinedLobby () {
		Debug.Log ("[PhotonController]: Lobby joined");
		PhotonNetwork.JoinRandomRoom ();
	}
	
	public void OnPhotonRandomJoinFailed () {
		Debug.Log ("[PhotonController]: Random join failed");
		PhotonNetwork.CreateRoom (
			null,
			new RoomOptions () {
				MaxPlayers = MaxPlayers
			},
			null
		);
        master = true;
	}
	
	public override void OnFailedToConnectToPhoton (DisconnectCause cause) {
		Debug.LogError ("[PhotonController]: Failed to connect to Photon: " + cause);
	}
	
	public override void OnJoinedRoom () {
		Debug.Log ("[PhotonController]: Joined room");
		int viewId = PhotonNetwork.AllocateViewID ();

		PhotonNetwork.RaiseEvent (
			(byte) NetworkEventCodes.ParticipantJoined,
			viewId,
			true,
			new RaiseEventOptions () {
				CachingOption = EventCaching.AddToRoomCache,
				Receivers = ReceiverGroup.All
			}
		);
	}
}
