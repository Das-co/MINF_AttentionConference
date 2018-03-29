using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlPanel : MonoBehaviour {

    public GameObject controlPanelCanvas;
    private bool controlPanelCanvasActive;
    public bool runSequence;
    public Text iniBtn;

    // Use this for initialization
    void Start () {
        controlPanelCanvas.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("k"))
        {
            print("Key Down " + controlPanelCanvas.activeSelf);

            if (controlPanelCanvas.activeSelf == true)
            {
                print("Deactivated ControlPanel");
                controlPanelCanvas.SetActive(false);
            }else if (controlPanelCanvas.activeSelf == false)
            {
                print("Activated ControlPanel");
                controlPanelCanvas.SetActive(true);
            }
        }
    }

    public void LoadSpecificScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void RunSequence()
    {
        runSequence = !runSequence;
        if (runSequence == true)
        {
            iniBtn.text = "Stop";
        } else if (runSequence == false)
        {
            iniBtn.text = "Initialize";
        }

        print("Run sequence");
    }
}
