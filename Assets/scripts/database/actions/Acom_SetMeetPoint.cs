using UnityEngine;
using System.Collections;
using Scripts;

namespace Database{
	public class Acom_SetMeetPoint : A_Command {
		
		public Acom_SetMeetPoint() : base(){
			this.name="set meeting point";
		}
		
		public override void begin(){
			//only if the button in action matrix gets called
		}
		
		public override void abort(IngameItem item){
			//only if the button in action matrix gets called
		}
		
		public override void finish(IngameItem item){
			//only if the button in action matrix gets called
			//AND for mouse right click!
		}
		
		public void setPoint(Vector3 coords){
			obj.setTargetPos(coords);
			finish(obj);
		}
	}
}
