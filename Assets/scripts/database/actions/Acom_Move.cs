using UnityEngine;
using System.Collections;
using Scripts;

namespace Database{
	public class Acom_Move : A_Command {
		private Vector3 target;
		
		public Acom_Move() : base(){
			this.name="move to";
		}
		
		public override void begin(){
			
		}
		
		public override void abort(IngameItem item){
			//remove meeting point
		}
		
		public override void finish(IngameItem item){
			//remove meeting point
		}
		
		public void setPoint(Vector3 coords){
			this.target=coords;
			obj.setTargetPos(coords);
			begin();
		}
	}
}
