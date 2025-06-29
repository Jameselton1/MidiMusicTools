using MidiMusicTools.Enum;
using MidiMusicTools.Interfaces;

namespace MidiMusicTools.Services
{
	/// <summary>
	/// Generates a random sequence of root notes from the given scale for the entire song.
	/// </summary>
	public class RandomRootNoteGenerator : IRootNoteGenerator
	{
		private Random random = new Random();

		/// <summary>
		/// Generates a queue of random root notes from the scale, one for each beat.
		/// </summary>
		/// <param name="totalBeats">Total number of beats in the song.</param>
		/// <param name="scale">The scale to select notes from.</param>
		/// <returns>A queue of randomly selected root notes.</returns>
		public Queue<Note> GenerateRootNotes(int totalBeats, List<Note> scale)
		{
			var rootNotes = new Queue<Note>();

			for (int i = 0; i < totalBeats; i++)
			{
				// Random note from scale
				int index = random.Next(scale.Count);
				rootNotes.Enqueue(scale[index]);
			}
			return rootNotes;
		}
	}
}