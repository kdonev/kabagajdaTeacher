using UnityEngine;
using System.Collections;
using abcNotes;
using System.Collections.Generic;
using UnityEngine.UI;

public class Songs : MonoBehaviour {

	List<AbcTune> _songs;
	Dropdown _dp;

	// Use this for initialization
	void Start () {
		TextAsset songsAsset = Resources.Load ("songs_data") as TextAsset;

		AbcParser parser = new AbcParser ();
		_songs = parser.Parse (songsAsset);

		SongPlayer sp = GameObject.Find ("SongPlayer").GetComponent<SongPlayer>();

		_dp = GameObject.Find ("SongsDropdown").GetComponent<Dropdown> ();

		_dp.options = CreateSongsList ();
		_dp.onValueChanged.AddListener (delegate {
			sp.PlaySong(_songs[_dp.value]);	
		});

		sp.PlaySong (_songs [0]);
	}

	void Destroy()
	{
		_dp.onValueChanged.RemoveAllListeners ();
	}

	List<Dropdown.OptionData> CreateSongsList()
	{
		List<Dropdown.OptionData> result = new List<Dropdown.OptionData> (_songs.Count);

		foreach (AbcTune t in _songs)
		{
			result.Add (new Dropdown.OptionData (t.Title));
		}

		return result;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
