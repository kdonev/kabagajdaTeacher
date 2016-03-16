using UnityEngine;
using System.Collections;

public class NotePlayer : MonoBehaviour {

	public int Note;

	private int _lastNote;

	private static readonly bool[][] FingerPositions = {
		//            R F1   R F2   R F3   R F4   R F5   L F1   L F2   L F3   L F4   L F5
		new bool[] { false, false, false, false, false, false, false, false, false, false },   // _G
		new bool[] { false, false, false, false, false, false, false, false, false, true  },   // _A
		new bool[] { false, false, false, false, false, false, false, false, true , false },   // _B
		new bool[] { false, false, false, false, false, false, false, true , false, false },   // C
		new bool[] { false, false, false, false, false, false, true , false, false, false },   // D
		new bool[] { false, false, false, true , false, false, false, false, false, false },   // E
		new bool[] { false, true , false, true , false, false, false, false, false, false },   // F
		new bool[] { false, false, true , false, false, false, false, false, false, false },   // #F
		new bool[] { false, true , true , false, false, false, false, false, false, false },   // G
		new bool[] { true , false, false, false, false, false, false, false, false, false },   // A
	};

	private HandController _leftHand;
	private HandController _rightHand;

	// Use this for initialization
	void Start () {
		_leftHand = GameObject.Find ("LeftHand").GetComponent<HandController>();
		_rightHand = GameObject.Find ("RightHand").GetComponent<HandController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Note != _lastNote) {
			_lastNote = Note;

			int n = Note;

			_rightHand.f1Up = FingerPositions [n] [0];
			_rightHand.f2Up = FingerPositions [n] [1];
			_rightHand.f3Up = FingerPositions [n] [2];
			_rightHand.f4Up = FingerPositions [n] [3];
			_rightHand.f5Up = FingerPositions [n] [4];

			_leftHand.f1Up = FingerPositions [n] [5];
			_leftHand.f2Up = FingerPositions [n] [6];
			_leftHand.f3Up = FingerPositions [n] [7];
			_leftHand.f4Up = FingerPositions [n] [8];
			_leftHand.f5Up = FingerPositions [n] [9];
		}
	}
}
