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

		SongPlayer sp = GameObject.Find ("SongPlayer").GetComponent<SongPlayer>();

		sp.PlaySong (_songs [0]);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
