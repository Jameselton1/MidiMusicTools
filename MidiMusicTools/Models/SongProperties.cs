using MidiMusicTools.Enum;
using MidiMusicTools.Services;
namespace MidiMusicTools.Models
{
	public struct SongProperties
	{
		public int Root { get; set; }
		public Mode Mode { get; set; }
		public List<SectionType> Sections { get; set; } = new();
		public List<TrackType> TrackStructure { get; set; } = new();
		public TimeSignature TimeSignature { get; set; }
		public int PhrasesPerSection { get; set; }
		public int BarsPerPhrase { get; set; }

		public readonly List<Note> Scale => NoteGeneratorHelpers.GenerateScale(Root, Mode);
		public readonly int TotalBeats => Sections.Count * PhrasesPerSection * BarsPerPhrase * TimeSignature.Nominator;

		public SongProperties()
		{ 
			
		}

	}
}