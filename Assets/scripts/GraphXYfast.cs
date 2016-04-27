using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scripts;

namespace Scripts {
	public class GraphXYfast {
		private static InputModul inputMod=InputModul.getInstance();
		private List<List<Vector2>> nodes=new List<List<Vector2>>();
		private List<GameObject> drawing=new List<GameObject>();
		private float space;
		private float xMin,xMax,yMin,yMax;	//..of possible terrain position
		
		public GraphXYfast(){
		}
		
		public void generate(float space){
			this.space=space;
			nodes.Clear();
			findMin();
			// GameObject cube=checkingObject();
			
			float tempX=xMin;
			float tempY=yMin;
			for(int x=0;xMax<xMin;x++){
				tempX=xMin+(x*space);
				nodes.Add(new List<Vector2>());
				for(int y=0; (yMax<yMin) ? (true) : (tempY<=yMax) ;y++){
					Vector3 tempCoord=inputMod.getCoordsAtXZ(tempX,tempY);
					if(tempCoord.y<0){	//if current position out of terrain
						if(yMax<yMin){
							yMax=tempY-space;
						}else if(xMax<xMin){
							xMax=tempX-space;
							nodes.RemoveAt(x);	//remove last list, which would never be filled with y values
						}
						break;
					}else{	//genereate nodes
						// GraphCheckCube gcc=cube.GetComponent<GraphCheckCube>();
						// gcc.hitted=false;
						// cube.transform.position=tempCoord;
						// if(!gcc.hitted)
							nodes[x].Add(new Vector2(tempX,tempY));
					}
					tempY=yMin+((y+1)*space);
				}
				tempY=yMin;	//reset y value
			}
			
			// MonoBehaviour.Destroy(cube);
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
		// private GameObject checkingObject(){
			// GameObject cube=GameObject.CreatePrimitive(PrimitiveType.Cube);
			// MonoBehaviour.Destroy(cube.GetComponent<MeshRenderer>());
			// cube.GetComponent<BoxCollider>().isTrigger=true;
			// cube.AddComponent<GraphCheckCube>();
			// cube.layer=2;	//to the cube ignores the raycast
			// cube.transform.localScale=new Vector3(space,100f,space);
			// cube.name="Check Object";
			// return cube;
		// }
		
		//used algorithm: breadth-first search (Breitensuche)
		public bool pathExists(Vector2 sourcePos,Vector2 targetPos){
			Vector2 source=getNearestNodeAt(sourcePos);
			Vector2 target=getNearestNodeAt(targetPos);
			if(!(source.x<0 || source.y<0 || target.x<0 || target.y<0)){
				List<Vector2> visited=new List<Vector2>();
				Queue<Vector2> Q=new Queue<Vector2>();
				Q.Enqueue(source);
				visited.Add(source);
				while(Q.Count>0){
					Vector2 u=Q.Dequeue();
					foreach(Vector2 v in getNearbyNodes(u)){
						if(v==target)
							return true;	//target was found
						if(!visited.Contains(v)){
							Q.Enqueue(v);
							visited.Add(v);
						}
					}
				}
			}
			return false;	//while loop runs out without found the target
		}
		
		//used algorithm: a star (A*)
		public List<Vector3> searchPath(Vector2 sourcePos,Vector2 targetPos){
			Vector2 source=getNearestNodeAt(sourcePos);
			Vector2 target=getNearestNodeAt(targetPos);
			Queue<Vector2> resultNodes=new Queue<Vector2>();
			if(!(source.x<0 || source.y<0 || target.x<0 || target.y<0)){// && pathExists(source,target)){
				Dictionary<Vector2,float> g=new Dictionary<Vector2,float>();
				List<Vector2> open=new List<Vector2>(),close=new List<Vector2>();
				Dictionary<Vector2,Queue<Vector2>> way=new Dictionary<Vector2,Queue<Vector2>>();
				
				for(int x=0;x<nodes.Count;x++)
					for(int y=0;y<nodes[x].Count;y++)
						way.Add(new Vector2(x,y),new Queue<Vector2>());
				
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
					Vector2 min=open[n];
					open.RemoveAt(n);
					
					if(min==target){
						resultNodes=way[min];
						break;
					}
					close.Add(min);
					foreach(Vector2 s in getNearbyNodes(min)){
						float eCost=h(min,s);
						if(!(open.Contains(s) || close.Contains(s))){
							open.Add(s);
							float newCost=g[min]+eCost;
							if(g.ContainsKey(s))
								g.Remove(s);
							g.Add(s,newCost);
							
							//save way/ways
							Queue<Vector2> newWay=new Queue<Vector2>(way[min]);
							newWay.Enqueue(s);
							if(way.ContainsKey(s))
								way.Remove(s);
							way.Add(s,newWay);
						}else if(g[min]+eCost<g[s]){
							float newCost=g[min]+eCost;
							if(g.ContainsKey(s))
								g.Remove(s);
							g.Add(s,newCost);
							
							//save way/ways
							Queue<Vector2> newWay=new Queue<Vector2>(way[min]);
							newWay.Enqueue(s);
							if(way.ContainsKey(s))
								way.Remove(s);
							way.Add(s,newWay);
							
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
			while(resultNodes.Count>0){
				Vector2 n=resultNodes.Dequeue();
				result.Add(V3(nodes[(int)n.x][(int)n.y]));
			}
			return result;
		}
		public List<Vector3> searchPath(Vector3 sourcePos,Vector3 targetPos){	//overload
			return searchPath(V2(sourcePos),V2(targetPos));
		}
		//heurastic function (here: set as distance)
		private float h(Vector2 source,Vector2 target){
			return Vector2.Distance(nodes[(int)source.x][(int)source.y],nodes[(int)target.x][(int)target.y]);
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
			return nodes.Count*nodes[0].Count;
		}
		
		//nodes in "radius" are the 8 direclty nearby nodes (if they exists)
		private List<Vector2> getNearbyNodes(Vector2 n){	//input and output are coords for the multi-list 'nodes'
			List<Vector2> result=new List<Vector2>();
			for(int x=-1;x<=1;x++){
				int xTemp=(int)n.x+x;
				if(xTemp>=0 && xTemp<nodes.Count){
					for(int y=-1;y<=1;y++){
						int yTemp=(int)n.y+y;
						if(yTemp>=0 && yTemp<nodes[xTemp].Count){
							Vector2 pos=nodes[xTemp][yTemp];
							//only if the coords are not invalid
							if(n!=(new Vector2(xTemp,yTemp)) && !(pos.x<xMin || pos.y<yMin))
								result.Add(new Vector2(xTemp,yTemp));
						}
					}
				}
			}
			return result;
		}
		
		//returns coords for the multi-list 'nodes'
		//result is [-1,-1] if no node is found
		private Vector2 getNearestNodeAt(Vector2 nPos){	//input is absolute coord on terrain
			int x1,x2,y1,y2;
			if((nPos.x-xMin)%space==0){
				x1=(int)((nPos.x-xMin)/space);
				x2=x1;
			}else{
				x1=(int)((nPos.x-xMin)/space);
				x2=x1+1;
			}
			if((nPos.y-yMin)%space==0){
				y1=(int)((nPos.y-yMin)/space);
				y2=y1;
			}else{
				y1=(int)((nPos.y-yMin)/space);
				y2=y1+1;
			}
			
			Vector2[] pPos={nodes[x1][y1],nodes[x2][y1],nodes[x1][y2],nodes[x2][y2]};
			Vector2[] pN={new Vector2(x1,y1),new Vector2(x2,y1),new Vector2(x1,y2),new Vector2(x2,y2)};
			int xFinal=-1,yFinal=-1;
			float dist=Mathf.Infinity;
			for(int i=0;i<pPos.Length;i++){
				if(!(pPos[i].x<xMin || pPos[i].y<yMin) && Vector2.Distance(nPos,pPos[i])<dist){
					dist=Vector2.Distance(nPos,pPos[i]);
					xFinal=(int)pN[i].x;
					yFinal=(int)pN[i].y;
				}
			}
			
			return new Vector2(xFinal,yFinal);
		}
		
		public Vector3 getNearestNodePos(Vector3 pos){
			Vector2 c=getNearestNodeAt(V2(pos));
			return V3(nodes[(int)c.x][(int)c.y]);
		}
		
		public Vector2 V2(Vector3 v){
			return new Vector2(v.x,v.z);
		}
		private Vector3 V3(Vector2 v){
			return inputMod.getCoordsAtXZ(v);
		}
		
		public void drawNodes(){
			removeDrawing();
			foreach(List<Vector2> nList in nodes){
				foreach(Vector2 n in nList){
					GameObject tempCube=GameObject.CreatePrimitive(PrimitiveType.Cube);
					tempCube.transform.localScale=new Vector3(space/2f,space*2f,space/2f);
					tempCube.transform.position=V3(n);
					tempCube.name="graph drawing";
					drawing.Add(tempCube);
				}
			}
		}
		public void drawNodes(List<Vector3> nodeList){
			removeDrawing();
			foreach(Vector3 n in nodeList){
				GameObject tempCube=GameObject.CreatePrimitive(PrimitiveType.Cube);
				tempCube.transform.localScale=new Vector3(space/2f,space*2f,space/2f);
				tempCube.transform.position=n;
				tempCube.name="graph drawing";
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
