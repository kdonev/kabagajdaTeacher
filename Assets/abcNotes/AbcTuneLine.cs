using System;
using System.Collections.Generic;

namespace abcNotes
{
	public class AbcTuneLine
	{
		private List<AbcNote> _notes = new List<AbcNote>();

		public int NotesCount{
			get{
				return _notes.Count;
			}
		}

		public AbcNote this[int idx]
		{
			get {
				return _notes [idx];
			}
		}

		public void AddNote(AbcNote note)
		{
			_notes.Add (note);
		}

		public AbcTuneLine ()
		{
		}
	}
}

