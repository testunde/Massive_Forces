using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public class ActionMatrix {
		public int width=5,heigth=3;
		private IngameObject obj;
		private Action[,] matrix;
		
		public ActionMatrix(IngameObject obj){
			this.obj=obj;
			matrix=new Action[width,heigth];
			
			//fill with Abl_Empty
			Action empty=new Database.Abl_Empty(obj);
			for(int i=0;i<matrix.GetLength(0);i++){
				for(int j=0;j<matrix.GetLength(1);j++){
					matrix[i,j]=empty;
				}
			}
		}
		
		public ActionMatrix(IngameObject obj,Action[,] actions) : this(obj){
			setMatrix(actions);
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
		
		public void setMatrix(Action[,] actions){
			//send a hint if the given matrix is bigger than the defined dimensions
			if(!(actions.GetLength(0)<width&&actions.GetLength(1)<heigth))
				Debug.Log("ActionMatrix is too big! ["+obj.name+"]");
			
			for(int i=0;i<actions.GetLength(0);i++){
				for(int j=0;j<actions.GetLength(1);j++){
					matrix[i,j]=actions[i,j];
				}
			}
		}
	}
}
