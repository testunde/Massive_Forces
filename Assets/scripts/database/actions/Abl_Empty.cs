using UnityEngine;
using System.Collections;
using Scripts;

namespace Database{
	public class Abl_Empty : A_Blank {
		
		public Abl_Empty(IngameObject obj) : base(obj){
			this.name="";
			this.disabled=true;
		}
	}
}
