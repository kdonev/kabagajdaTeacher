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

		private int _multiplet = 0;
		private int _multipletIdx = 0;
		private float _multipletNoteLen;

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
				    (ch >= 'a' && ch <= 'g') ||
					(ch == '^'))
				{
					ParseNote ();
				}
				else if (ch == '(')
				{
					GetNextChar ();
					ch = PeakNextChar ();
					if (ch >= '2' && ch <= '9')
					{
						_multiplet = ch - '0';
						_multipletIdx = 0;
						switch (_multiplet)
						{
						case 2:
							_multipletNoteLen = _currentHeader.UnitNoteLength * 3F / 2F;
							break;

						case 3:
							_multipletNoteLen = _currentHeader.UnitNoteLength * 2F / 3F;
							break;

						case 4:
							_multipletNoteLen = _currentHeader.UnitNoteLength * 3F / 4F;
							break;

						default:
							Debug.Log ("Unsupported multiplet");
							break;
						}
					}
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

			string note = "";
			char nc = GetNextChar ();
			if (nc == '^')
			{
				note = "#";
				nc = GetNextChar ();
			}
			
			note = note + nc;

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

			if (_multiplet > 0)
			{
				_currentNote.length = _multipletNoteLen;
				++_multipletIdx;
				if (_multipletIdx == _multiplet)
					_multiplet = 0;
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

			case 'Q':
				ParseQ ();
				break;

			case 'w':
				Parsew ();
				break;
			}
		}

		void HandleEmptyLine ()
		{
			_currentTune = null;
			_currentHeader = null;
		}

		void SkipSpaces ()
		{
			while (PeakNextChar () == ' ')
				GetNextChar ();
		}

		string ParseSylable()
		{
			string result = "";

			SkipSpaces ();

			char ch = GetNextChar ();
			switch (ch)
			{
			case '-':
				return "-";

			case '\0':
				return "";

			case '_':
				return "_";

			case '*':
				return "*";
			}

			result += ch;

			while (true)
			{
				ch = PeakNextChar ();
			
				if (ch == '_' || ch == '*')
					result += ' ';

				if (ch == '\0' || ch == '_' || ch == '*')
					break;

				if (ch == '~')
					result += ' ';
				else if (ch != '-')
				    result += ch;

				GetNextChar ();

				if (ch == ' ' || ch == '-')
					break;
			}

			return result;
				
		}

		void Parsew()
		{
			if (_currentTuneLine == null)
				return;

			GetFieldParam ();

			int noteIdx = 0;
			while (noteIdx < _currentTuneLine.NotesCount)
			{
				string syl = ParseSylable ();
				if (syl == "")
					break;
				
				_currentTuneLine [noteIdx].lyrics = syl;
				++noteIdx;
			}
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

		void ParseQ()
		{
			if (_currentHeader == null)
				return;
			
			GetFieldParam ();

			float fr = ParseFraction ();
			if (GetNextChar () == '=')
			{
				int bpm = ParseInt ();

				_currentHeader.NotesPerMinute = fr * bpm;
			}
		}

		string GetFieldParam()
		{
			_lineIdx = 0;
			_currentLine = _currentLine.Substring (2).Trim ();
			return _currentLine;
		}

		float ParseFraction()
		{
			int nom = ParseInt ();
			int denom = 1;

			if (PeakNextChar () == '/')
			{
				GetNextChar ();
				denom = ParseInt ();
			}

			return (float)nom / (float)denom;
		}

		void ParseL()
		{
			if (_currentHeader == null)
				return;
			
			GetFieldParam ();
			_currentHeader.UnitNoteLength = ParseFraction ();
		}
	}
}

