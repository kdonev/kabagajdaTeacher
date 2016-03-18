using System;

namespace abcNotes
{
	public class AbcHeader
	{
		private string sourceFile;

		private float unitNoteLength = 1.0F / 8.0F;

		public string SourceFile
		{
			get {
				return sourceFile;
			}

			internal set {
				sourceFile = value;
			}
		}

		public float UnitNoteLength
		{
			get {
				return unitNoteLength;
			}

			internal set {
				unitNoteLength = value;
			}
		}

		internal AbcHeader ()
		{
		}

		protected AbcHeader(AbcHeader other)
		{
			this.sourceFile = other.sourceFile;
			this.unitNoteLength = other.unitNoteLength;
		}
	}
}

