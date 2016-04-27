using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scripts;

namespace Scripts {
	public class GraphXY {
		private static InputModul inputMod=InputModul.getInstance();
		private List<GNodeXY> nodes=new List<GNodeXY>();
		private List<GameObject> drawing=new List<GameObject>();
		private float space;
		private float xMin,xMax,yMin,yMax;	//..of possible terrain position
		
		public GraphXY(){
		}
		
		public void generate(float space){
			this.space=space;
			nodes.Clear();
			findMin();
			GameObject cube=checkObject();
			
			float tempX=xMin;
			float tempY=yMin;
			for(int y=0;yMax<yMin;y++){
				tempY=yMin+(y*space);
				for(int x=0; (xMax<xMin) ? (true) : (tempX<=xMax) ;x++){
					Vector3 tempCoord=inputMod.getCoordsAtXZ(tempX,tempY);
					if(tempCoord.y<0){	//if current position out of terrain
						if(xMax<xMin){
							xMax=tempX-space;
						}else if(yMax<yMin){
							yMax=tempY-space;
						}
						break;
					}else{	//genereate nodes
						//TODO: ignore buildings and neutrals!
						GNodeXY tempNode=new GNodeXY(tempCoord.x,tempCoord.z);
						nodes.Add(tempNode);
						List<GNodeXY> neighbors=new List<GNodeXY>();//getNodesInRadius(tempNode);
						tempNode.addNeighbors(neighbors);
						foreach(GNodeXY n in neighbors)
							n.addNeighbor(tempNode);
					}
					tempX=xMin+((x+1)*space);
				}
				tempX=xMin;
			}
			
			MonoBehaviour.Destroy(cube);
		}
		
		//WARNING: unsets the max values!
		public void findMin(){
			//set center of terrain as start values
			this.xMin=inputMod.terrain.transform.position.x;
			this.yMin=inputMod.terrain.transform.position.y;
			
			while(inputMod.getCoordsAtXZ(this.xMin-this.space,this.yMin).y>0f){
				this.xMin-=this.space;
			}
			while(inputMod.getCoordsAtXZ(this.xMin,this.yMin-this.space).y>0f){
				this.yMin-=this.space;
			}
			
			//set smaller then min values so they are marked as 'not set'
			this.xMax=xMin-1f;
			this.yMax=yMin-1f;
		}
		
		//generate cube with scale of 'space' for checking for node generation
		private GameObject checkObject(){
			GameObject cube=GameObject.CreatePrimitive(PrimitiveType.Cube);
			MonoBehaviour.Destroy(cube.GetComponent<MeshRenderer>());
			cube.GetComponent<BoxCollider>().isTrigger=true;
			cube.AddComponent<GraphCheckCube>();
			cube.layer=2;	//to the cube ignores the raycast
			cube.transform.localScale=new Vector3(space,100f,space);
			cube.name="Check Object";
			return cube;
		}
		
		//used algorithm: breadth-first search (Breitensuche)
		// public bool pathExists(GNodeXY source,GNodeXY target){
			// List<GNodeXY> visited=new List<GNodeXY>();
			// Queue<GNodeXY> Q=new Queue<GNodeXY>();
			// Q.Enqueue(source);
			// visited.Add(source);
			// while(Q.Count>0){
				// GNodeXY u=Q.Dequeue();
				// foreach(GNodeXY v in u.getNeighbors()){
					// if(v.getV2Coords()==target.getV2Coords())
						// return true;	//target was found
					// if(!visited.Contains(v)){
						// Q.Enqueue(v);
						// visited.Add(v);
					// }
				// }
			// }
			// return false;	//while loop runs out without found the target
		// }
		
		//used algorithm: a star (A*)
		public List<Vector3> searchPath(GNodeXY source,GNodeXY target){
			List<GNodeXY> resultNodes=new List<GNodeXY>();
			if(!(source==null || target==null)){// && pathExists(source,target)){
				Dictionary<GNodeXY,float> g=new Dictionary<GNodeXY,float>();
				List<GNodeXY> open=new List<GNodeXY>(),close=new List<GNodeXY>();
				Dictionary<GNodeXY,List<GNodeXY>> way=new Dictionary<GNodeXY,List<GNodeXY>>();
				foreach(GNodeXY n in nodes)
					way.Add(n,new List<GNodeXY>());
				
				g.Add(source,0f);
				open.Add(source);
				while(open.Count>0){
					int n=0;
					float nCost=g[open[0]]+h(open[0],target);
					for(int i=1;i<open.Count;i++){
						float tempCost=g[open[i]]+h(open[i],target);
						if(tempCost<nCost){
							n=i;
							nCost=tempCost;
						}
					}
					GNodeXY min=open[n];
					open.RemoveAt(n);
					
					if(min.getV2Coords()==target.getV2Coords()){
						resultNodes=way[min];
						break;
					}
					close.Add(min);
					foreach(GNodeXY s in min.getNeighbors()){
						float eCost=Vector2.Distance(min.getV2Coords(),s.getV2Coords());
						if(!(open.Contains(s) || close.Contains(s))){
							open.Add(s);
							float newCost=g[min]+eCost;
							if(g.ContainsKey(s))
								g.Remove(s);
							g.Add(s,newCost);
							
							//save way/ways
							way[min].Add(s);
						}else if(g[min]+eCost<g[s]){
							float newCost=g[min]+eCost;
							if(g.ContainsKey(s))
								g.Remove(s);
							g.Add(s,newCost);
							
							//save way/ways
							way[min].Add(s);
							
							if(close.Contains(s)){
								open.Add(s);
								close.Remove(s);
							}
						}
					}
				}
			}
			
			List<Vector3> result=new List<Vector3>();
			//turn node list into Vector3 list
			foreach(GNodeXY n in resultNodes)
				result.Add(inputMod.getCoordsAtXZ(n.getV2Coords()));
			return result;
		}
		//heurastic function (here: set as distance)
		private float h(GNodeXY source,GNodeXY target){
			return Vector2.Distance(source.getV2Coords(),target.getV2Coords());
		}
		
		public float getSpace(){
			return this.space;
		}
		
		public Vector2 getMaxCoords(){
			return new Vector2(xMax,yMax);
		}
		public Vector2 getMinCoords(){
			return new Vector2(xMin,yMin);
		}
		
		public int getNodesCount(){
			return nodes.Count;
		}
		
		//radius=sqrt(space²+space²)+.001f
		private List<GNodeXY> getNodesInRadius(GNodeXY n){
			float radius=Mathf.Sqrt(2*Mathf.Pow(space,2))+.001f;
			List<GNodeXY> result=new List<GNodeXY>();
			foreach(GNodeXY rn in this.nodes){
				Vector2 rnPos=rn.getV2Coords();
				Vector2 nPos=n.getV2Coords();
				if(rnPos!=nPos && Vector2.Distance(rnPos,nPos)<=radius){
					result.Add(rn);
				}
			}
			return result;
		}
		//radius=sqrt(space²+space²)+.001f
		public GNodeXY getNearestNodeInRadius(float nx,float ny){
			float radius=Mathf.Sqrt(2*Mathf.Pow(space,2))+.001f;
			Vector2 nPos=new Vector2(nx,ny);
			GNodeXY result=null;
			foreach(GNodeXY rn in this.nodes){
				Vector2 rnPos=rn.getV2Coords();
				float tempDistance=Vector2.Distance(nPos,rnPos);
				if(tempDistance<=radius && (result==null ? true : tempDistance<Vector2.Distance(nPos,result.getV2Coords()))){
					result=rn;
				}
			}
			return result;
		}
		
		private Vector2 V2(Vector3 v){
			return new Vector2(v.x,v.z);
		}
		
		public void drawNodes(){
			removeDrawing();
			foreach(GNodeXY n in nodes){
				GameObject tempCube=GameObject.CreatePrimitive(PrimitiveType.Cube);
				tempCube.transform.localScale=new Vector3(space/2f,space*2f,space/2f);
				tempCube.transform.position=inputMod.getCoordsAtXZ(n.getV2Coords());
				drawing.Add(tempCube);
			}
		}
		public void removeDrawing(){
			foreach(GameObject go in drawing)
				MonoBehaviour.Destroy(go);
			drawing.Clear();
		}
	}
}
