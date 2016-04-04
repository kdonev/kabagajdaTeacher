using System;

namespace abcNotes
{
	public class AbcNote
	{
		public string note;
		public float length;

		public bool beamWithNext;

		public string lyrics;

		public int RepeateFromRow = -1;
		public int RepeateFromNote = -1;
	}
}

