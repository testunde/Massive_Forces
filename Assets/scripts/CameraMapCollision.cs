using UnityEngine;
using System.Collections;

public class CameraMapCollision : MonoBehaviour {
	
	public GameObject obj;
	public GameObject map;
	public Vector3 colPoint=new Vector3(0f,0f,0f);
	
	void Start(){
		obj=gameObject;
		map=GameObject.Find("Terrain");
	}

	void OnCollisionEnter(Collision col){
		if(col.gameObject.Equals(map)){
			obj=col.gameObject;
			float x=0f,y=0f,z=0f,cc=0f;
			int c=0;
			foreach(ContactPoint element in col.contacts){
				x+=col.contacts[c].point.x;
				y+=col.contacts[c].point.y;
				z+=col.contacts[c].point.z;
				c++;
			}
			cc=(float)c;
			colPoint=new Vector3(x/cc,y/cc,z/cc);
		}
	}
}
