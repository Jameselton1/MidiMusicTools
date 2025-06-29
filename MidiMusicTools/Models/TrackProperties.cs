using MidiMusicTools.Enum;

namespace MidiMusicTools.Models
{
	/// <summary>
	/// Defines the type and MIDI settings for a single track in the song.
	/// </summary>
	public struct TrackProperties
	{
		/// <summary>The type of track (Melody, Chord, Bassline).</summary>
		public TrackType Type;
		/// <summary>The MIDI octave for this track (-1 for random/default).</summary>
		public int Octave;
		/// <summary>The MIDI instrument for this track (-1 for random/default).</summary>
		public int Instrument;

		public TrackProperties() { }
	}
}