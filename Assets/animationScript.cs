using UnityEngine;
using System.Collections;

public class animationScript : MonoBehaviour {

    public Animation anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animation>();
        anim.Play();
	}
	
	// Update is called once per frame
	void Update () {
        if (anim.isPlaying)
        {
            Debug.Log("Animation should be playing");
        }
	}
}
