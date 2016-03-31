using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public class ActionMatrix {
		public static int width=5,heigth=3;
		private IngameObject obj=null;
		private Action[,] matrix;
		
		public ActionMatrix(){
			matrix=new Action[width,heigth];
			
			//fill with Abl_Empty
			Action empty=new Database.Abl_Empty();
			for(int i=0;i<matrix.GetLength(0);i++){
				for(int j=0;j<matrix.GetLength(1);j++){
					matrix[i,j]=empty;
				}
			}
		}
		
		public ActionMatrix(IngameObject obj,string[,] actions) : this(){
			this.obj=obj;
			Action[,] newActions=new Action[actions.GetLength(0),actions.GetLength(1)];
			
			for(int i=0;i<actions.GetLength(0);i++){
				for(int j=0;j<actions.GetLength(1);j++){
					if(actions[i,j]!=null){
						string[] split=actions[i,j].Split(new char[]{','});
						string ac=split[0];
						Action act=null;
						if(split.GetLength(0)>1)
							act=(Action)System.Activator.CreateInstance(System.Type.GetType("Database."+ac),split[1]);
						else
							act=(Action)System.Activator.CreateInstance(System.Type.GetType("Database."+ac));
						newActions[i,j]=act;
					}
				}
			}
			setMatrix(newActions);
		}
		
		public void setObject(IngameObject obj){
			this.obj=obj;
			foreach(Action ac in matrix)
				ac.setObject(this.obj);
		}
		
		public Action[,] getMatrixCopy(){
			Action[,] result=new Action[width,heigth];
			for(int i=0;i<result.GetLength(0);i++){
				for(int j=0;j<result.GetLength(1);j++){
					result[i,j]=matrix[i,j];
				}
			}
			return result;
		}
		
		public Action getAction(int x,int y){
			return matrix[x,y];
		}
		
		public void setMatrix(Action[,] actions){
			//send a hint if the given matrix is bigger than the defined dimensions
			if(!(actions.GetLength(0)<=width&&actions.GetLength(1)<=heigth))
				Debug.Log("ActionMatrix is too big! ["+this.obj.name+"]");
			
			for(int i=0;i<actions.GetLength(0);i++){
				for(int j=0;j<actions.GetLength(1);j++){
					if(actions[i,j]!=null){
						matrix[i,j]=actions[i,j];
						matrix[i,j].setObject(this.obj);
					}
				}
			}
		}
		
		public void setAction(int x,int y,Action ac){
			matrix[x,y]=ac;
		}
	}
}
