using UnityEngine;
using System.Collections;
using Scripts_o;

namespace Scripts_o {
	public class State {
		//just the current state can over hand to other script as reference
		private int st;
		public State(){
			st=0;
		}
		~State(){
		}
		
		public int Get(){
			return st;
		}
		public void Set(int s){
			st=s;
		}
		public bool Equal(int s){
			return s==st;
		}
	}
}
