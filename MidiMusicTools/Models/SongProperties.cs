using MidiMusicTools.Enum;
using MidiMusicTools.Services;

namespace MidiMusicTools.Models
{
	/// <summary>
	/// Holds all the properties that define the structure and theory of a generated song.
	/// </summary>
	public struct SongProperties
	{
		/// <summary>The root note (0-11, C-B) for the song's scale.</summary>
		public int Root { get; set; }
		/// <summary>The musical mode (e.g., Major, Minor, Dorian, etc.).</summary>
		public Mode Mode { get; set; }
		/// <summary>List of sections (e.g., Verse, Chorus) in the song.</summary>
		public List<SectionType> Sections { get; set; } = new();
		/// <summary>List of track properties (type, instrument, octave) for each track.</summary>
		public List<TrackProperties> TrackStructure { get; set; } = new();
		/// <summary>The time signature of the song.</summary>
		public TimeSignature TimeSignature { get; set; }
		/// <summary>Number of phrases per section.</summary>
		public int PhrasesPerSection { get; set; }
		/// <summary>Number of bars per phrase.</summary>
		public int BarsPerPhrase { get; set; }

		/// <summary>
		/// The scale for the song, generated from the root and mode.
		/// </summary>
		public readonly List<Note> Scale => NoteGeneratorHelpers.GenerateScale(Root, Mode);

		/// <summary>
		/// The total number of beats in the song, calculated from structure.
		/// </summary>
		public readonly int TotalBeats => Sections.Count * PhrasesPerSection * BarsPerPhrase * TimeSignature.Numerator;

		public SongProperties() { }
	}
}