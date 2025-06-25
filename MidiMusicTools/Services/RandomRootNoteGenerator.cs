using MidiMusicTools.Enum;
using MidiMusicTools.Interfaces;

namespace MidiMusicTools.Services
{
	public class RandomRootNoteGenerator() : IRootNoteGenerator
	{
		private Random random = new Random();

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