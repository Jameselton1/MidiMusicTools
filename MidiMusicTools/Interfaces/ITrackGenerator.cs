using MidiMusicTools.Enum;
using MidiMusicTools.Models;

namespace MidiMusicTools.Interfaces
{
	public interface ITrackGenerator
	{
		Track GenerateTrack(SongProperties songProps, Queue<Note> rootNotes, int instrument = -1, int octave = -1);
	}
}