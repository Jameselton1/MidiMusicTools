using NAudio.Midi;

namespace MidiMusicTools.Models
{
	/// <summary>
	/// Represents a single track in the song, with its instrument and beats.
	/// </summary>
	public struct Track
	{
		/// <summary>The MIDI instrument number for this track.</summary>
		public int MidiInstrument { get; set; }
		/// <summary>List of beats in this track.</summary>
		public List<Beat> Beats { get; set; }

		public Track() { }
	}
}