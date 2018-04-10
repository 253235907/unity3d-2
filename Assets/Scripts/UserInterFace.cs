using UnityEngine;  
using System.Collections;  
using Com.Mygame;  

public class UserInterFace : MonoBehaviour {  

	GameSceneController my;  
	IUserActions action;  

	float width, height;  

	float castw(float scale) {  
		return (Screen.width - width) / scale;  
	}  

	float casth(float scale) {  
		return (Screen.height - height) / scale;  
	}  

	void Start() {  
		my = GameSceneController.GetInstance();  
		action = GameSceneController.GetInstance() as IUserActions;  
	}  

	void OnGUI() {  
		width = Screen.width / 12;  
		height = Screen.height / 12;  
		print (my.state);  
		if (my.state == State.WIN) {  
			GUI.Button (new Rect (castw (2f), casth (6f), width, height), "YOU WIN!");  
			if (GUI.Button (new Rect (castw (2f), casth (2f), width, height), "Restart")) {
				action.restart ();  
			}	 
		}  
		else if (my.state == State.LOSE) {  
			GUI.Button (new Rect (castw (2f), casth (6f), width, height), "Lose!");
				if(GUI.Button(new Rect(castw(2f), casth(2f), width, height), "Restart")) {
					action.restart();  
				}   
		}  
		else {  
			if (GUI.RepeatButton(new Rect(10, 10, 120, 20), my.getBaseCode().gameName)) {  
				GUI.TextArea(new Rect(10, 40, Screen.width - 20, Screen.height/2), my.getBaseCode().gameRule);  
			}  
			else if (my.state == State.BSTART || my.state == State.BEND) {  
				if (GUI.Button(new Rect(castw(2f), casth(6f), width, height), "Go")) {  
					action.moveBoat();  
				}  
				if (GUI.Button(new Rect(castw(10.5f), casth(4f), width, height), "Devil On")) {  
					action.devilSOnB();  
				}  
				if (GUI.Button(new Rect(castw(4.3f), casth(4f), width, height), "Priest On")) {  
					action.priestSOnB();  
				}  
				if (GUI.Button(new Rect(castw(1.1f), casth(4f), width, height), "Devil On")) {  
					action.devilEOnB();  
				}  
				if (GUI.Button(new Rect(castw(1.3f), casth(4f), width, height), "Priest On")) {  
					action.priestEOnB();  
				}  
				if (GUI.Button(new Rect(castw(2.5f), casth(1.3f), width, height), "Off")) {  
					action.offBoatL();  
				}  
				if (GUI.Button(new Rect(castw(1.7f), casth(1.3f), width, height), "Off")) {  
					action.offBoatR();  
				}  
			}  
		}  
	}  
}  