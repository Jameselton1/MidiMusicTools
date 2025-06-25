using MidiMusicTools.Enum;
namespace MidiMusicTools.Interfaces
{
	public interface IRootNoteGenerator
	{
		Queue<Note> GenerateRootNotes(int totalBeats, List<Note> scale);
	}
}