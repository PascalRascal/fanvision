using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class compassData : MonoBehaviour {
    public GameObject cam;
    Compass comp;
    Text txt;
	// Use this for initialization
	void Start () {
        comp = Input.compass;
        comp.enabled = true;
        Input.location.Start();

   
    }

    // Update is called once per frame
    void Update () {
        txt = GetComponent<Text>();
        txt.text = comp.trueHeading.ToString() + "\n" + cam.transform.rotation.eulerAngles.ToString();
	}
}

