using NAudio.Midi;
namespace MidiMusicTools.Models
{
	public struct Track
	{
		public int MidiInstrument { get; set; }
		public List<Beat> Beats { get; set; }

		public Track() { }
	}
}