using MidiMusicTools.Enum;

namespace MidiMusicTools.Models
{
	/// <summary>
	/// Represents a subdivision of a beat, containing MIDI notes to play.
	/// </summary>
	public class Subdivision
	{
		/// <summary>
		/// MIDI note numbers (0-127) to play at this subdivision.
		/// </summary>
		public List<int> MidiNotes { get; set; } = new List<int>();
	}
}