using UnityEngine;
using System.Collections;
using Scripts;

namespace Database{
	public class Acom_Move : A_Command {
		private Vector3 target;
		
		public Acom_Move(IngameObject obj,Vector3 target) : base(obj){
			this.target=target;
			this.name="move to";
		}
		
		public override void begin(){
			
		}
		
		public override void abort(){
			
		}
		
		public override void finish(){
			
		}
	}
}
