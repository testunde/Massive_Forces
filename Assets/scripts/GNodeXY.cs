using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scripts;

namespace Scripts {
	public class GNodeXY {
		private Vector2 coords;
		private List<GNodeXY> neighbors=new List<GNodeXY>();
		
		public GNodeXY(Vector2 coords){
			this.coords=coords;
		}
		public GNodeXY(float x,float y) : this(new Vector2(x,y)){	//overload
		}
		
		public void addNeighbor(GNodeXY node){
			if(node.getV2Coords()!=coords)
				neighbors.Add(node);
		}
		public void addNeighbors(List<GNodeXY> nodes){
			foreach(GNodeXY n in nodes)
				addNeighbor(n);
		}
		
		public List<GNodeXY> getNeighbors(){
			return new List<GNodeXY>(neighbors);
		}
		
		public Vector2 getV2Coords(){
			return coords;
		}
		
		//y value gets set to 0f
		public Vector3 getV3Coords(){
			return new Vector3(coords.x,0f,coords.y);
		}
	}
}
