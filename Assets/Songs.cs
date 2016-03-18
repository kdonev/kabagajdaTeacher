using UnityEngine;
using System.Collections;
using abcNotes;
using System.Collections.Generic;

public class Songs : MonoBehaviour {

	List<AbcTune> _songs;

	// Use this for initialization
	void Start () {
		TextAsset songsAsset = Resources.Load ("songs_data") as TextAsset;

		AbcParser parser = new AbcParser ();
		_songs = parser.Parse (songsAsset);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
