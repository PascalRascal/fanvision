using UnityEngine;
using GeoUtil;
using UnityEngine.UI;
using System.Collections;

public class UTMTest : MonoBehaviour {
    GeoUTMConverter gumc;
    Text txt;

	// Use this for initialization
	void Start () {
        txt = GetComponent<Text>();
        gumc = new GeoUTMConverter();
        Input.location.Start();
        gumc.ToUTM(Input.location.lastData.latitude, Input.location.lastData.longitude);
        txt.text = gumc.X.ToString() + "\n" + gumc.Y.ToString() + "\n" + Input.location.lastData.latitude.ToString() + "\n" + Input.location.lastData.longitude.ToString();
    }

    // Update is called once per frame
    void Update () {

    }
}
