using System;

namespace abcNotes
{
	public class AbcHeader
	{
		private string sourceFile;

		private float unitNoteLength = 1.0F / 8.0F;

		private float notesPerMinute = 30F;

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

		public float NotesPerMinute
		{
			get {
				return notesPerMinute;
			}

			set {
				notesPerMinute = value;
			}
		}

		public float NoteLengthInSeconds
		{
			get {
				return 60F / notesPerMinute;
			}
		}

		internal AbcHeader ()
		{
		}

		protected AbcHeader(AbcHeader other)
		{
			this.sourceFile = other.sourceFile;
			this.unitNoteLength = other.unitNoteLength;
			this.notesPerMinute = other.notesPerMinute;
		}
	}
}

