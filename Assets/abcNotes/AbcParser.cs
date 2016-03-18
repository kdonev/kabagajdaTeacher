using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace abcNotes
{
	public class AbcParser
	{
		private StreamReader _sr;

		private string _currentLine;
		private int _lineIdx;

		AbcHeader _header;

		AbcHeader _currentHeader;
		AbcTune _currentTune;

		AbcTuneLine _currentTuneLine;

		AbcNote _currentNote;

		List<AbcTune> _tunes;

		public AbcParser ()
		{
		}

		public List<AbcTune> Parse(TextAsset text)
		{
			_tunes = new List<AbcTune> ();

			using (MemoryStream ms = new MemoryStream (text.bytes, false))
			{
				using (StreamReader sr = new StreamReader (ms))
				{
					_header = new AbcHeader ();
					_currentHeader = _header;

					_sr = sr;

					while (ReadNextLine ())
						ParseLine ();
				}

				_sr = null;
			}

			return _tunes;
		}

		private bool ReadNextLine()
		{
			_currentLine = _sr.ReadLine ();
			if (_currentLine != null)
				_currentLine.Trim ();

			return _currentLine != null;
		}

		void ParseLine()
		{
			if (IsEmptyLine ()) 
			{
				HandleEmptyLine ();
			}
			else if (IsFieldLine ()) 
			{
				ParseField ();
			}
			else
			{
				if (_currentTune != null)
					ParseTuneLine ();
			}

		}

		char GetNextChar()
		{
			char result = '\0';
			if (_lineIdx < _currentLine.Length)
			{
				result = _currentLine [_lineIdx];
				++_lineIdx;
			}

			return result;
		}

		char PeakNextChar()
		{
			char result = '\0';
			if (_lineIdx < _currentLine.Length)
			{
				result = _currentLine [_lineIdx];
			}

			return result;
		}

		void ParseTuneLine()
		{
			_lineIdx = 0;

			_currentTuneLine = new AbcTuneLine ();
			_currentTune.AddLine (_currentTuneLine);

			while (_lineIdx < _currentLine.Length)
			{
				char ch = PeakNextChar();
				if ((ch >= 'A' && ch <= 'G') ||
				    (ch >= 'a' && ch <= 'g'))
				{
					ParseNote ();
				}
				else
				{
					if (_currentNote != null)
						_currentNote.beamWithNext = false;
					++_lineIdx;
				}
			}

			// last note is not beamed
			_currentNote.beamWithNext = false;
		}

		void ParseNote ()
		{
			_currentNote = new AbcNote ();
			_currentTuneLine.AddNote (_currentNote);

			_currentNote.length = _currentTune.UnitNoteLength;
			_currentNote.beamWithNext = true;

			string note = new string(GetNextChar (), 1);

			char nextCh = PeakNextChar ();
			if (nextCh == ',' || nextCh == '\'')
			{
				note += nextCh;
				++_lineIdx;
				nextCh = PeakNextChar ();
			}

			_currentNote.note = note;

			if (nextCh >= '1' && nextCh <= '9')
			{
				int nom = ParseInt ();
				_currentNote.length *= nom;
				nextCh = PeakNextChar ();
			}

			if (nextCh == '/')
			{
				++_lineIdx;
				nextCh = PeakNextChar ();

				if (nextCh >= '1' && nextCh <= '9')
				{
					int denom = ParseInt ();
					_currentNote.length /= denom;
				}
				else
				{
					_currentNote.length /= 2;
				}
			}
		}

		int ParseInt()
		{
			char ch = PeakNextChar ();
			int result = 0;

			while (ch >= '0' && ch <= '9')
			{
				result *= 10;
				result += ch - '0';
				++_lineIdx;
				ch = PeakNextChar ();
			}

			return result;
		}

		bool IsEmptyLine()
		{
			return _currentLine.Length == 0;
		}

		bool IsFieldLine()
		{
			if (_currentLine.Length < 2)
				return false;

			if (_currentLine [1] != ':')
				return false;

			char fieldName = _currentLine [0];

			return (fieldName >= 'A' && fieldName <= 'Z' ||
				    fieldName >= 'a' && fieldName <= 'z');
		}

		void ParseField()
		{
			char fieldName = _currentLine [0];

			switch (fieldName) 
			{
			case 'X':
				ParseX ();
				break;

			case 'L':
				ParseL ();
				break;

			case 'T':
				ParseT ();
				break;
			}
		}

		void HandleEmptyLine ()
		{
			_currentTune = null;
			_currentHeader = null;
		}

		void ParseT()
		{
			if (_currentTune != null)
			{
				_currentTune.Title = GetFieldParam ();
			}
		}

		void ParseX()
		{
			// add new tune
			AbcTune newTune = new AbcTune(_header);

			_tunes.Add (newTune);

			_currentTune = newTune;
			_currentHeader = newTune;
		}

		string GetFieldParam()
		{
			return _currentLine.Substring (2).Trim ();
		}

		void ParseL()
		{
			if (_currentHeader == null)
				return;
			
			string fieldParam = GetFieldParam ();

			if (fieldParam.StartsWith ("1/")) {
				int l = int.Parse (fieldParam.Substring (2));

				float noteLength = 1F / (float)l;

				_currentHeader.UnitNoteLength = noteLength;
			}
		}
	}
}

