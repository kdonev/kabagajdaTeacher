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

		public string GetTextWithHighlight(int idx)
		{
			string result = "";

			while (idx > 0 && _notes [idx].lyrics == "_")
				--idx;

			int i = 0;
			foreach (AbcNote n in _notes)
			{
				if (i == idx)
				{
					result += "<b>";
					result += n.lyrics;
					result += "</b>";
				}
				else
				{
					if (n.lyrics != "_")
					    result += n.lyrics;
				}

				++i;
			}

			return result;
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

