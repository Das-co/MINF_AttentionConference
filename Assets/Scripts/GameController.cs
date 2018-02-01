using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OVRAdapter), typeof(PhotonController))]
public class GameController : MonoBehaviour {
	public string GameVersion;
	
	private OVRAdapter _ovrAdapter;
	public OVRAdapter OvrAdapter {
		get {
			if (!_ovrAdapter) {
				_ovrAdapter = GetComponent<OVRAdapter> ();
			}
			return _ovrAdapter;
		}
	}
	
	private PhotonController _photonController;
	public PhotonController PhotonController {
		get {
			if (!_photonController) {
				_photonController = GetComponent<PhotonController> ();
			}
			return _photonController;
		}
	}
}
