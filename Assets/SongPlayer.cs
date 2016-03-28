using UnityEngine;
using System.Collections;
using abcNotes;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SongPlayer : MonoBehaviour {

	NotePlayer _notePlayer;
	Text _noteLabel;
	Text _lyricsLabel;

	AbcTune _song;

	int _lineIdx;
	int _noteIdx;

	bool _playing = false;
	bool _repeat = true;
	bool _finished = false;

	float _timeForNextNote;

	static readonly float[] PlaySpeeds = { 1.4F, 1.2F, 1.0F, 0.8F, 0.6F };

	float _playSpeed = 1.0F;

	// Use this for initialization
	void Start () {
		_notePlayer = GameObject.Find ("NotePlayer").GetComponent<NotePlayer>();
		_noteLabel = GameObject.Find ("CurrentNote").GetComponent<Text>();
		_lyricsLabel = GameObject.Find ("LyricsText").GetComponent<Text>();
	}

	public void PlaySong (AbcTune song)
	{
		_song = song;
		_lineIdx = 0;
		_noteIdx = 0;
		_finished = false;

		EventSystem.current.SetSelectedGameObject (this.gameObject);
		GUI.SetNextControlName ("");
		GUI.FocusControl ("");
		GUIUtility.keyboardControl = 0;
	}
	
	// Update is called once per frame
	void Update () {
		var now = Time.time;
		if (_playing && _timeForNextNote < now)
		{
			PlayNextNote ();
		}

		if (Input.GetKeyDown (KeyCode.Space))
		{
			_playing = !_playing;

			if (_finished)
			{
				_playing = true;
				RestartSong ();
			}
		}
	}

	void RestartSong()
	{
		_lineIdx = 0;
		_noteIdx = 0;
		_finished = false;
	}

	void PlayNextNote()
	{
		if (_finished)
		{
			// finished last note
			if (_repeat)
				RestartSong ();
			else
				_notePlayer.Note = 0;
			
			return;
		}
		
		AbcTuneLine line = _song [_lineIdx];
		AbcNote note = line [_noteIdx];

		_notePlayer.PlayNote (note.note);
		_timeForNextNote = Time.time + note.length * _song.NoteLengthInSeconds * _playSpeed;

		_noteLabel.text = note.note;
		_lyricsLabel.text = line.GetTextWithHighlight (_noteIdx);

		++_noteIdx;
		if (_noteIdx == line.NotesCount)
		{
			_noteIdx = 0;
			++_lineIdx;
			if (_lineIdx == _song.LinesCount)
			{
				_finished = true;
			}
		}
	}

	public void ChangeSpeed(int newSpeed)
	{
		_playSpeed = PlaySpeeds [newSpeed];
	}
}
