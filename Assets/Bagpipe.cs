using System;

namespace AssemblyCSharp
{
	public class Bagpipe
	{
		public static readonly string[] VisibleNotes = {
			"_G", "_A", "_B", "C", "D", "E", "F", "#F", "G", "A" };

		public static readonly string[] RealNotes = {
			"_A", "_B", "_#C", "D", "E", "#F", "G", "#G", "A", "B" };

		public static readonly bool[][] FingerPositions = {
		//    R F1   R F2   R F3   R F4   R F5   L F1   L F2   L F3   L F4   L F5
			{ false, false, false, false, false, false, false, false, false, false },   // _G
			{ false, false, false, false, false, false, false, false, false, true  },   // _A
			{ false, false, false, false, false, false, false, false, true , false },   // _B
			{ false, false, false, false, false, false, false, true , false, false },   // C
			{ false, false, false, false, false, false, true , false, false, false },   // D
			{ false, false, false, true , false, false, false, false, false, false },   // E
			{ false, true , false, true , false, false, false, false, false, false },   // F
			{ false, false, true , false, false, false, false, false, false, false },   // #F
			{ false, true , true , false, false, false, false, false, false, false },   // G
			{ true , false, false, false, false, false, false, false, false, false },   // A
		};

		public Bagpipe ()
		{
		}
	}
}

