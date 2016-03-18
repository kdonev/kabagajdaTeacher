using System;
using System.Collections.Generic;

namespace abcNotes
{
	public class AbcTune : AbcHeader
	{
		private readonly AbcHeader _header;

		private readonly List<AbcTuneLine> _lines = new List<AbcTuneLine>();

		private string _title;

		public int LinesCount
		{
			get{
				return _lines.Count;
			}
		}

		public AbcTuneLine this[int idx]
		{
			get {
				return _lines [idx];
			}
		}

		public string Title
		{
			get{
				return _title;
			}

			internal set{
				_title = value;
			}
		}

		public void AddLine(AbcTuneLine line)
		{
			_lines.Add (line);
		}

		internal AbcTune (AbcHeader fromHeader)
			: base(fromHeader)
		{
			_header = fromHeader;
		}
	}
}

