using UnityEngine;
using System.Collections;
using Scripts;

namespace Database{
	public class Acom_Move : A_Command {
		private Vector3 target;
		
		public Acom_Move(Vector3 target) : base(){
			this.target=target;
			this.name="move to";
		}
		
		public override void begin(){
			
		}
		
		public override void abort(IngameItem item){
			
		}
		
		public override void finish(IngameItem item){
			
		}
	}
}
