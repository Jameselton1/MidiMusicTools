namespace MidiMusicTools.Models
{
	/// <summary>
	/// Represents a musical time signature (e.g., 4/4, 3/4).
	/// </summary>
	public struct TimeSignature
	{
		/// <summary>
		/// The number of beats in a bar (numerator).
		/// </summary>
		public int Numerator { get; set; }
		/// <summary>
		/// The note value that represents one beat (denominator).
		/// </summary>
		public int Denominator { get; set; }
	}
}