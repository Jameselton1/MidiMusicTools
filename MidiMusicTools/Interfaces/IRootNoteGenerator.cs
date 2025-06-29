using MidiMusicTools.Enum;

namespace MidiMusicTools.Interfaces
{
	/// <summary>
	/// Interface for generating a sequence of root notes for a song.
	/// </summary>
	public interface IRootNoteGenerator
	{
		/// <summary>
		/// Generates a queue of root notes for the song, based on the total number of beats and the scale.
		/// </summary>
		/// <param name="totalBeats">Total number of beats in the song.</param>
		/// <param name="scale">The scale to select notes from.</param>
		/// <returns>A queue of root notes.</returns>
		Queue<Note> GenerateRootNotes(int totalBeats, List<Note> scale);
	}
}