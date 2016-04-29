using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scripts;

namespace Scripts {
	public class GraphXYfast {
		private static InputModul inputMod=InputModul.getInstance();
		private Vector2[,] nodes;
		private List<GameObject> drawing=new List<GameObject>();
		private float space;
		private float xMin,yMin;	//..of possible terrain position
		private float xMax,yMax;
		private int xCount,yCount;	//..of points
		
		public GraphXYfast(){
		}
		
		public void generate(float space){
			this.space=space;
			processGridSize();
			// GameObject cube=checkingObject();
			
			for(int x=0;x<xCount;x++){
				for(int y=0;y<yCount;y++){
					nodes[x,y]=new Vector2(x*space+xMin,y*space+yMin);
				}
			}
			
			// MonoBehaviour.Destroy(cube);
		}
		
		private void processGridSize(){
			//set center of terrain as start values
			xMin=inputMod.terrain.transform.position.x;
			yMin=inputMod.terrain.transform.position.y;
			
			while(inputMod.getCoordsAtXZ(xMin-space,yMin).y>0f)
				xMin-=space;
			while(inputMod.getCoordsAtXZ(xMin,yMin-space).y>0f)
				yMin-=space;
			
			xCount=0;
			yCount=0;
			while(inputMod.getCoordsAtXZ(space*(xCount+1)+xMin,yMin).y>0f)
				xCount++;
			while(inputMod.getCoordsAtXZ(xMin,space*(yCount+1)+yMin).y>0f)
				yCount++;
			
			nodes=new Vector2[xCount,yCount];
		}
		
		//generate cube with scale of 'space' for checking for node generation
		// private GameObject checkingObject(){
			// GameObject cube=GameObject.CreatePrimitive(PrimitiveType.Cube);
			// MonoBehaviour.Destroy(cube.GetComponent<MeshRenderer>());
			// cube.GetComponent<BoxCollider>().isTrigger=true;
			// cube.AddComponent<GraphCheckCube>();
			// cube.layer=2;	//to the cube ignores the raycast
			// cube.transform.localScale=new Vector3(space,100f,space);
			// cube.name="Checking Object";
			// return cube;
		// }
		
		//used algorithm: breadth-first search (Breitensuche)
		public bool pathExists(Vector2 sourcePos,Vector2 targetPos){
			Tuple<int,int> source=getNearestNodeAt(sourcePos);
			Tuple<int,int> target=getNearestNodeAt(targetPos);
			if(isValid(source) && isValid(target)){
				List<Tuple<int,int>> visited=new List<Tuple<int,int>>();
				Queue<Tuple<int,int>> Q=new Queue<Tuple<int,int>>();
				Q.Enqueue(source);
				visited.Add(source);
				while(Q.Count>0){
					Tuple<int,int> u=Q.Dequeue();
					foreach(Tuple<int,int> v in getNearbyNodes(u)){
						if(v.Equals(target))
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
		
		//used algorithm: a star (A*) with Jump Point Search
		//used JPS algorithm at: http://gamedevelopment.tutsplus.com/tutorials/how-to-speed-up-a-pathfinding-with-the-jump-point-search-algorithm--gamedev-5818
		public List<Vector3> searchPath(Vector2 sourcePos,Vector2 targetPos){
			Tuple<int,int> source=getNearestNodeAt(sourcePos);
			Tuple<int,int> target=getNearestNodeAt(targetPos);
			Queue<Tuple<int,int>> resultNodes=new Queue<Tuple<int,int>>();
			if(isValid(source) && isValid(target)){// && pathExists(source,target)){
				Dictionary<Tuple<int,int>,float> g=new Dictionary<Tuple<int,int>,float>();
				List<Tuple<int,int>> open=new List<Tuple<int,int>>(),close=new List<Tuple<int,int>>();
				Dictionary<Tuple<int,int>,Queue<Tuple<int,int>>> way=new Dictionary<Tuple<int,int>,Queue<Tuple<int,int>>>();
				
				for(int x=0;x<nodes.GetLength(0);x++)
					for(int y=0;y<nodes.GetLength(1);y++)
						way.Add(new Tuple<int,int>(x,y),new Queue<Tuple<int,int>>());
				
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
					Tuple<int,int> min=open[n];
					open.RemoveAt(n);
					
					if(min.Equals(target)){
						resultNodes=way[min];
						break;
					}
					close.Add(min);
					foreach(Tuple<int,int> s in getSuccessorNodes(min,source,target)){
						float eCost=h(min,s);
						if(!(open.Contains(s) || close.Contains(s))){
							open.Add(s);
							float newCost=g[min]+eCost;
							if(g.ContainsKey(s))
								g.Remove(s);
							g.Add(s,newCost);
							
							//save way/ways
							Queue<Tuple<int,int>> newWay=new Queue<Tuple<int,int>>(way[min]);
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
							Queue<Tuple<int,int>> newWay=new Queue<Tuple<int,int>>(way[min]);
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
				Tuple<int,int> n=resultNodes.Dequeue();
				result.Add(V3(nodes[n.Item1,n.Item2]));
			}
			return result;
		}
		public List<Vector3> searchPath(Vector3 sourcePos,Vector3 targetPos){	//overload
			return searchPath(V2(sourcePos),V2(targetPos));
		}
		
		//heurastic function
		private float h(Tuple<int,int> source,Tuple<int,int> target){
			Vector2 sourcePos=nodes[source.Item1,source.Item2];
			Vector2 targetPos=nodes[target.Item1,target.Item2];
			float dx=Mathf.Abs(sourcePos.x-targetPos.x);
			float dy=Mathf.Abs(sourcePos.y-targetPos.y);
			float spaceDiagonal=Mathf.Sqrt(2*Mathf.Pow(space,2));
			
			//diagonal distance
			return space*(dx+dy)+(spaceDiagonal-2*space)*Mathf.Min(dx,dy);
			
			//euclidian distance
			// return Vector2.Distance(sourcePos,targetPos);
			
			//Manhattan distance
			// return space*(dx+dy);
		}
		
		private List<Tuple<int,int>> getSuccessorNodes(Tuple<int,int> current,Tuple<int,int> source,Tuple<int,int> target){	//input and output are coords for the multi-list 'nodes'
			List<Tuple<int,int>> result=new List<Tuple<int,int>>();
			List<Tuple<int,int>> neighbors=getNeighbors(current);
			foreach(Tuple<int,int> n in neighbors){
				//direction from current node to neighbor
				int dX=Mathf.Clamp(n.Item1-current.Item1,-1,1);
				int dY=Mathf.Clamp(n.Item2-current.Item2,-1,1);
				
				//try to find a node to jump to
				Tuple<int,int> jumpPoint=JPS_jump(current.Item1,current.Item2,dX,dY,source,target);
				
				//add to list if a jump point was found
				if(isValid(jumpPoint))
					result.Add(jumpPoint);
			}
			return result;
		}
		
		private Tuple<int,int> JPS_jump(int cX,int cY,int dX,int dY,Tuple<int,int> source,Tuple<int,int>  target){
			// cX, cY - Current Node Position,  dX, dY - Direction
			
			//position of new node we are going to consider (neighbor)
			int nX=cX+dX;
			int nY=cY+dY;
			
			//case if node is blocked (or not available)
			if(!isValidPos(nX,nY))
				return new Tuple<int,int>(-1,-1);
			
			Tuple<int,int> nextNode=new Tuple<int,int>(nX,nY);
			//if target was found
			if(target.Equals(nextNode))
				return nextNode;
			
			//diagonal case
			if(dX!=0 && dY!=0){
				if((isValidPos(nX-dX,nY+dY) && !isValidPos(nX-dX,nY)) || (isValidPos(nX+dX,nX-dX) && !isValidPos(nX,nY-dY)))
					return nextNode;
				
				//check in horizonal and vertical directions for forced neighbors
				//this is a special case for diagonal direction
				if(isValid(JPS_jump(nX,nY,dX,0,source,target)) || isValid(JPS_jump(nX,nY,0,dX,source,target)))
					return nextNode;
			}else{
				//horizonal case
				if(dX!=0){
					if((isValidPos(nX+dX,nY+1) && !isValidPos(nX,nY+1)) || (isValidPos(nX+dX,nY-1) && !isValidPos(nX,nY-1)))
						return nextNode;
				}else{	//vertical case
					if((isValidPos(nX+1,nY+dY) && !isValidPos(nX+1,nY)) || (isValidPos(nX-1,nY+dY) && !isValidPos(nX-1,nY)))
						return nextNode;
				}
			}
			
			//if forced neighbor wasn't found, try next jump point
			return JPS_jump(nX,nY,dX,dY,source,target);
		}
		
		private List<Tuple<int,int>> getNeighbors(Tuple<int,int> n){
			List<Tuple<int,int> result=new List<Tuple<int,int>>();
			
			return result;
		}
		
		//nodes "in radius" are the 8 direclty nearby nodes (if they exists)
		private List<Tuple<int,int>> getNearbyNodes(Tuple<int,int> n){	//input and output are coords for the multi-list 'nodes'
			List<Tuple<int,int>> result=new List<Tuple<int,int>>();
			for(int x=-1;x<=1;x++){
				int xTemp=n.Item1+x;
				if(xTemp>=0 && xTemp<nodes.GetLength(0)){
					for(int y=-1;y<=1;y++){
						int yTemp=n.Item2+y;
						if(yTemp>=0 && yTemp<nodes.GetLength(1)){
							Vector2 pos=nodes[xTemp,yTemp];
							Tuple<int,int> resultNode=new Tuple<int,int>(xTemp,yTemp);
							//only if the coords are not invalid
							if(!n.Equals(resultNode) && isValid(pos))
								result.Add(resultNode);
						}
					}
				}
			}
			return result;
		}
		
		//returns coords for the multi-list 'nodes'
		//result is [-1,-1] if no node is found
		private Tuple<int,int> getNearestNodeAt(Vector2 nPos){	//input is absolute coord on terrain
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
			
			Vector2[] pPos={nodes[x1,y1],nodes[x2,y1],nodes[x1,y2],nodes[x2,y2]};
			Tuple<int,int>[] pN={new Tuple<int,int>(x1,y1),new Tuple<int,int>(x2,y1),new Tuple<int,int>(x1,y2),new Tuple<int,int>(x2,y2)};
			int xFinal=-1,yFinal=-1;
			float dist=Mathf.Infinity;
			for(int i=0;i<pPos.Length;i++){
				if(isValid(pPos[i]) && Vector2.Distance(nPos,pPos[i])<dist){
					dist=Vector2.Distance(nPos,pPos[i]);
					xFinal=pN[i].Item1;
					yFinal=pN[i].Item2;
				}
			}
			
			return new Tuple<int,int>(xFinal,yFinal);
		}
		
		//check if vector/tupel coords are valid,
		private bool isValid(int x,int y){
			return !(n<0 || y<0);
		}
		private bool isValid(Tuple<int,int> n){	//overload
			return isValid(n.Item1,n.Item2);
		}
		private bool isValid(float x,float y){
			return !(x<xMin || y<yMin);
		}
		private bool isValid(Vector2 n){	//overload
			return isValid(n.x,n.y);
		}
		private bool isValidPos(int x,int y){
			return isValid(nodes[x,y]);
		}
		
		public Vector3 getNearestNodePos(Vector3 pos){
			Tuple<int,int> c=getNearestNodeAt(V2(pos));
			return V3(nodes[c.Item1,c.Item2]);
		}
		
		private Vector2 V2(Vector3 v){
			return new Vector2(v.x,v.z);
		}
		private Vector3 V3(Vector2 v){
			return inputMod.getCoordsAtXZ(v);
		}
		
		public float getSpaceValue(){
			return this.space;
		}
		public Vector2 getMaxCoords(){
			return new Vector2(xMin+space*xCount,yMin+space*yCount);
		}
		public Vector2 getMinCoords(){
			return new Vector2(xMin,yMin);
		}
		public int getNodesCount(){
			return nodes.GetLength(0)*nodes.GetLength(1);
		}
		
		public void drawNodes(){
			removeDrawing();
			foreach(Vector2 n in nodes){
				GameObject tempCube=GameObject.CreatePrimitive(PrimitiveType.Cube);
				tempCube.transform.localScale=new Vector3(space/2f,space*2f,space/2f);
				tempCube.transform.position=V3(n);
				tempCube.name="graph drawing";
				drawing.Add(tempCube);
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
