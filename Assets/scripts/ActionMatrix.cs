using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public class ActionMatrix {
		public int width=5,heigth=3;
		private Action[,] matrix;
		
		public ActionMatrix(){
			matrix=new Action[width,heigth];
		}
		
		public ActionMatrix(Action[,] actions){
			if(actions.GetLength(0)<width&&actions.GetLength(1)<heigth)
				matrix=actions;
		}
		
		public Action[,] get(){
			return matrix;
		}
	}
}
