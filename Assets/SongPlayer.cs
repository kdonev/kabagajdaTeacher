using UnityEngine;
using System.Collections;
using abcNotes;
using UnityEngine.UI;

public class SongPlayer : MonoBehaviour {

	NotePlayer _notePlayer;
	Text _noteLabel;

	AbcTune _song;

	int _lineIdx;
	int _noteIdx;

	float _timeForNextNote;

	// Use this for initialization
	void Start () {
		_notePlayer = GameObject.Find ("NotePlayer").GetComponent<NotePlayer>();
		_noteLabel = GameObject.Find ("CurrentNote").GetComponent<Text>();
	}

	public void PlaySong (AbcTune song)
	{
		_song = song;
		_lineIdx = 0;
		_noteIdx = 0;
	}
	
	// Update is called once per frame
	void Update () {
		var now = Time.time;
		if (_timeForNextNote < now)
		{
			PlayNextNote ();
		}
	}

	void PlayNextNote()
	{
		if (_song == null)
		{
			// finished last note
			_notePlayer.Note = 0;
			return;
		}
		
		AbcTuneLine line = _song [_lineIdx];
		AbcNote note = line [_noteIdx];

		_notePlayer.PlayNote (note.note);
		_timeForNextNote = Time.time + note.length;

		_noteLabel.text = note.note;

		++_noteIdx;
		if (_noteIdx == line.NotesCount)
		{
			_noteIdx = 0;
			++_lineIdx;
			if (_lineIdx == _song.LinesCount)
			{
				// finished
				_song = null;
			}
		}
	}
}
