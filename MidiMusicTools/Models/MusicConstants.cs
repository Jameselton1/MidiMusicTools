using MidiMusicTools.Enum;
namespace MidiMusicTools.Models
{
	public static class MusicConstants
	{
		public static int NUM_OF_NOTES = System.Enum.GetValues(typeof(Note)).Length;
	}
}