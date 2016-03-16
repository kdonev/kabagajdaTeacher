using UnityEngine;
using System.Collections;

public class HandController : MonoBehaviour {

	public bool f1Up;
	public bool f2Up;
	public bool f3Up;
	public bool f4Up;
	public bool f5Up;

	private bool f1WasUp;
	private bool f2WasUp;
	private bool f3WasUp;
	private bool f4WasUp;
	private bool f5WasUp;

	Animator _f1Animator;
	Animator _f2Animator;
	Animator _f3Animator;
	Animator _f4Animator;
	Animator _f5Animator;

	// Use this for initialization
	void Start () {
		_f1Animator = transform.FindChild ("F1").gameObject.GetComponent<Animator> ();
		_f2Animator = transform.FindChild ("F2").gameObject.GetComponent<Animator> ();
		_f3Animator = transform.FindChild ("F3").gameObject.GetComponent<Animator> ();
		_f4Animator = transform.FindChild ("F4").gameObject.GetComponent<Animator> ();
		_f5Animator = transform.FindChild ("F5").gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		HandleFinger (_f1Animator, f1Up, ref f1WasUp);
		HandleFinger (_f2Animator, f2Up, ref f2WasUp);
		HandleFinger (_f3Animator, f3Up, ref f3WasUp);
		HandleFinger (_f4Animator, f4Up, ref f4WasUp);
		HandleFinger (_f5Animator, f5Up, ref f5WasUp);
	}

	void HandleFinger(Animator f, bool newVal, ref bool oldVal)
	{
		if (oldVal != newVal) 
		{
			oldVal = newVal;
			if (newVal)
				f.SetTrigger ("Up");
			else
				f.SetTrigger ("Down");
		}
	}
}
