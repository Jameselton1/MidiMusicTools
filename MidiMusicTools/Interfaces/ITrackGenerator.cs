using MidiMusicTools.Enum;
using MidiMusicTools.Models;

namespace MidiMusicTools.Interfaces
{
	/// <summary>
	/// Interface for generating a track (melody, chord, or bassline) for a song.
	/// </summary>
	public interface ITrackGenerator
	{
		/// <summary>
		/// Generates a track based on song properties, root notes, and MIDI settings.
		/// </summary>
		/// <param name="songProps">The song properties.</param>
		/// <param name="rootNotes">Queue of root notes for the track.</param>
		/// <param name="instrument">MIDI instrument number.</param>
		/// <param name="octave">MIDI octave.</param>
		/// <returns>A generated Track.</returns>
		Track GenerateTrack(SongProperties songProps, Queue<Note> rootNotes, int instrument, int octave);
	}
}