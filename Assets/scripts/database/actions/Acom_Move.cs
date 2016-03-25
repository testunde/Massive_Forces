using UnityEngine;
using System.Collections;
using Scripts;

namespace Database{
	public class Acom_Move : A_Command {
		Vector3 target;
		
		public Acom_Move(Vector3 target){
			this.target=target;
		}
	}
}
