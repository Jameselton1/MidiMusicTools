using MidiMusicTools.Enum;
namespace MidiMusicTools.Models
{
	/// <summary>
	/// Provides music-related constants for the library.
	/// </summary>
	public static class MusicConstants
	{
		/// <summary>
		/// The number of notes in the chromatic scale.
		/// </summary>
		public static int NUM_OF_NOTES = System.Enum.GetValues(typeof(Note)).Length;
	}
}