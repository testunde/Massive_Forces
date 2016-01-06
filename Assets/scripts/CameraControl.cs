using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	
	private Vector3 input;
	private GameObject camera;
	
	void Start () {
		camera=gameObject;
	}
	
	void Update () {
		//input = new Vector3(Input.GetAxis("Horizontal"),0f,Input.GetAxis("Vertical"));
		Transform camTr=camera.transform;
		float sinX=Mathf.Sin(Mathf.Deg2Rad*camTr.eulerAngles.x);
		Vector3 forw=new Vector3(transform.forward.x+sinX,0f,transform.forward.z+sinX);	//x and z coords depends on y-rotation!
		input=Input.GetAxis("Horizontal")*transform.right+Input.GetAxis("Vertical")*forw;
		camTr.position+=input*.5f;
	}
}
