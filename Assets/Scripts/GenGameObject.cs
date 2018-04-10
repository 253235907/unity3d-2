using UnityEngine;  
using System.Collections;  
using System.Collections.Generic;  
using Com.Mygame;  

public class GenGameObject : MonoBehaviour {  

	Stack<GameObject> priests_start = new Stack<GameObject>();  
	Stack<GameObject> priests_end = new Stack<GameObject>();  
	Stack<GameObject> devils_start = new Stack<GameObject>();  
	Stack<GameObject> devils_end = new Stack<GameObject>();  

	GameObject[] boat = new GameObject[2];  
	GameObject boat_obj;  
	public float speed = 100f;  

	GameSceneController my;  

	Vector3 shoreStartPos = new Vector3(-20, 0, 0);  
	Vector3 shoreEndPos = new Vector3(20, 0, 0);  
	Vector3 boatStartPos = new Vector3(-8, 0, 0);  
	Vector3 boatEndPos = new Vector3(8, 0, 0);  

	float gap = 2.2f;  
	Vector3 priestStartPos = new Vector3(-19f, 3.6f, 0);  
	Vector3 priestEndPos = new Vector3(14f, 3.6f, 0);  
	Vector3 devilStartPos = new Vector3(-26f, 3.6f, 0);  
	Vector3 devilEndPos = new Vector3(21f, 3.6f, 0);  


	void Start () {  
		my = GameSceneController.GetInstance();  
		my.setGenGameObject(this);  
		loadSrc();  
	}  

	void loadSrc() {  
		// shore  
		Instantiate(Resources.Load("Prefabs/Shore"), shoreStartPos, Quaternion.identity);  
		Instantiate(Resources.Load("Prefabs/Shore"), shoreEndPos, Quaternion.identity);  
		// boat  
		boat_obj = Instantiate(Resources.Load("Prefabs/Boat"), boatStartPos, Quaternion.identity) as GameObject;  
		// priests & devils  
		for (int i = 0; i < 3; ++i) {  
			priests_start.Push(Instantiate(Resources.Load("Prefabs/Priest")) as GameObject);  
			devils_start.Push(Instantiate(Resources.Load("Prefabs/Devil")) as GameObject);  
		}  
		// light  
		Instantiate(Resources.Load("Prefabs/Light"));  
	}  

	int boatCapacity() {  
		int capacity = 0;  
		for (int i = 0; i < 2; ++i) {  
			if (boat[i] == null) capacity++;  
		}  
		return capacity;  
	}  

	public void priestStartOnBoat() {  
		if (priests_start.Count != 0 && boatCapacity() != 0 && my.state == State.BSTART)  
			getOnTheBoat(priests_start.Pop());  
	}  

	public void priestEndOnBoat() {  
		if (priests_end.Count != 0 && boatCapacity() != 0 && my.state == State.BEND)  
			getOnTheBoat(priests_end.Pop());  
	}  

	public void devilStartOnBoat() {  
		if (devils_start.Count != 0 && boatCapacity() != 0 && my.state == State.BSTART)  
			getOnTheBoat(devils_start.Pop());  
	}  

	public void devilEndOnBoat() {  
		if (devils_end.Count != 0 && boatCapacity() != 0 && my.state == State.BEND)  
			getOnTheBoat(devils_end.Pop());  
	}  

	void setCharacterPositions(Stack<GameObject> stack, Vector3 pos) {  
		GameObject[] array = stack.ToArray();  
		for (int i = 0; i < stack.Count; ++i) {  
			array[i].transform.position = new Vector3(pos.x+ gap*i, pos.y, pos.z);  
		}  
	}  

	void getOnTheBoat(GameObject obj) {  
		if (boatCapacity() != 0) {  
			obj.transform.parent = boat_obj.transform;  
			if (boat[0] == null) {  
				boat[0] = obj;  
				obj.transform.localPosition = new Vector3(-0.3f, 1.8f, 0);  
			} else {  
				boat[1] = obj;  
				obj.transform.localPosition = new Vector3(0.3f, 1.8f, 0);  
			}  
		}  
	}  

	public void moveBoat() {  
		if (boatCapacity() != 2) {  
			if (my.state == State.BSTART) {  
				my.state = State.BSEMOVING;  
			}  
			else if (my.state == State.BEND) {  
				my.state = State.BESMOVING;  
			}  
		}  
	}  

	public void getOffTheBoat(int side) {  
		if (boat[side] != null) {  
			boat[side].transform.parent = null;  
			if (my.state == State.BEND) {  
				if (boat[side].tag == "Priest") {  
					priests_end.Push(boat[side]);  
				}  
				else if (boat[side].tag == "Devil") {  
					devils_end.Push(boat[side]);  
				}  
			}  
			else if (my.state == State.BSTART) {  
				if (boat[side].tag == "Priest") {  
					priests_start.Push(boat[side]);  
				}  
				else if (boat[side].tag == "Devil") {  
					devils_start.Push(boat[side]);  
				}  
			}  
			boat[side] = null;  
		}  
	}  


	void check() {  
		int pOnb = 0, dOnb = 0;  
		int priests_s = 0, devils_s = 0, priests_e = 0, devils_e = 0;  

		if (priests_end.Count == 3 && devils_end.Count == 3) {  
			my.state = State.WIN;  
			return;  
		}  

		for (int i = 0; i < 2; ++i) {  
			if (boat[i] != null && boat[i].tag == "Priest") pOnb++;  
			else if (boat[i] != null && boat[i].tag == "Devil") dOnb++;  
		}  
		if (my.state == State.BSTART) {  
			priests_s = priests_start.Count + pOnb;  
			devils_s = devils_start.Count + dOnb;  
			priests_e = priests_end.Count;  
			devils_e = devils_end.Count;  
		}  
		else if (my.state == State.BEND) {  
			priests_s = priests_start.Count;  
			devils_s = devils_start.Count;  
			priests_e = priests_end.Count + pOnb;  
			devils_e = devils_end.Count + dOnb;  
		}  
		if ((priests_s != 0 && priests_s < devils_s) || (priests_e != 0 && priests_e < devils_e)) {  
			my.state = State.LOSE;  
		}  
	}  	

	void Update() {  
		setCharacterPositions(priests_start, priestStartPos);  
		setCharacterPositions(priests_end, priestEndPos);  
		setCharacterPositions(devils_start, devilStartPos);  
		setCharacterPositions(devils_end, devilEndPos);  

		if (my.state == State.BSEMOVING) {  
			boat_obj.transform.position = Vector3.MoveTowards(boat_obj.transform.position, boatEndPos, speed*Time.deltaTime);  
			if (boat_obj.transform.position == boatEndPos) {  
				my.state = State.BEND;  
			}  
		}  
		else if (my.state == State.BESMOVING) {  
			boat_obj.transform.position = Vector3.MoveTowards(boat_obj.transform.position, boatStartPos, speed*Time.deltaTime);  
			if (boat_obj.transform.position == boatStartPos) {  
				my.state = State.BSTART;  
			}  
		}  
		else check();  
	}  
}  