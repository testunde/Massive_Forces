using UnityEngine;
using System.Collections;
using Scripts.Control;

public class CameraControl : MonoBehaviour {
	
	private Vector3 input;
	private GameObject mainCamera;
	public GameObject ground;
	private GameObject cube;
	private float x=0f,y=0f,z=0f;
	public MouseLook mouseLook = new MouseLook();
	
	void Start () {
		mainCamera=gameObject;
		ground=GameObject.Find("Terrain");
		cube=GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube.layer=2;
		mouseLook.Init(mainCamera.transform);
	}
	
	void Update () {
		//input = new Vector3(Input.GetAxis("Horizontal"),0f,Input.GetAxis("Vertical"));
		Transform camTr=mainCamera.transform;
		float sinX=Mathf.Pow(Mathf.Sin(Mathf.Deg2Rad*camTr.eulerAngles.x),2f);	//already multiplied with it self
		float sinY=Mathf.Sin(Mathf.Deg2Rad*camTr.eulerAngles.y);
		float cosY=Mathf.Cos(Mathf.Deg2Rad*camTr.eulerAngles.y);
		Vector3 forw=new Vector3(	//Pythagoras (squareroot of x²+x²) and multiplied with sin/cos of Y-axis
			sinY*(Mathf.Sqrt(Mathf.Pow(transform.forward.x,2f)+sinX)),	//sidewards
			0f,	//eliminate the up/down movement of the transform.forward Verctor3
			cosY*(Mathf.Sqrt(Mathf.Pow(transform.forward.z,2f)+sinX)));	//for-/backwards
		input=Input.GetAxis("Horizontal")*transform.right+	//sidewards
			Input.GetAxis("Vertical")*forw+	//for-/backwards
			+Input.GetAxis("Mouse ScrollWheel")*Vector3.up;	//zoom in worlds y axis
			//Input.GetAxis("Mouse ScrollWheel")*transform.forward;	//zoom in view direction
		camTr.position+=input*.5f;
		
		//Input.GetButtonDown("Fire1")once true if hitted
		//Input.GetMouseButton(0) true of hold down
		RaycastHit hit;
		if (Input.GetMouseButton(0)&&Physics.Raycast(Camera.main.ScreenPointToRay (Input.mousePosition), out hit)&&hit.transform.Equals(ground.transform)){
			Renderer renderer = hit.transform.GetComponent<Renderer>();
			Collider collider = hit.collider as Collider;
			if(!(renderer == null || renderer.sharedMaterial == null || renderer.sharedMaterial.mainTexture == null || collider == null)){
				//Texture2D tex = (Texture2D)renderer.material.mainTexture;
				Vector2 pixelUV = hit.textureCoord;
				x=(pixelUV.x * renderer.material.mainTexture.width-2048f)/8.192f+250f+hit.transform.position.x;
				y=hit.transform.position.y;
				z=(pixelUV.y * renderer.material.mainTexture.height-2048f)/8.192f+250f+hit.transform.position.z;
				cube.transform.position=new Vector3(x,y,z);
				Debug.Log(x + " / " + y + " / " + z);
			}
		}
		
		//rotate view
		if(Input.GetMouseButton(2)){
			mouseLook.LookRotation (mainCamera.transform);
		}
	}
}
