using MidiMusicTools.Enum;
using MidiMusicTools.Models;
using MidiMusicTools.Interfaces;
using System.Linq;
using MidiMusicTools.Services;
namespace MidiMusicTools.Abstracts
{
	// Abstract Note Generator class, concrete classes are responsible for generating notes on a beat.
	public abstract class TrackGeneratorBase : ITrackGenerator
	{
		protected Queue<Note> rootNotes = new Queue<Note>();
		protected SongProperties songProps = new SongProperties();
		protected List<Note> scale = new List<Note>();
		protected Random random = new Random();
		protected int MidiOctave;

		public Track GenerateTrack(SongProperties sp, Queue<Note> rn, int instrument = -1, int octave = -1)
		{
			var track = new Track();

			rootNotes = rn;
			songProps = sp;
			scale = songProps.Scale;
			
		  MidiOctave = (octave == -1) ? GenerateMidiOctave() : octave;
			track.Beats = GenerateBeats();
			track.MidiInstrument = (instrument == -1) ? GenerateMidiInstrument() : instrument;
			return track;
		}

		protected List<Beat> GenerateBeats()
		{
			var beats = new List<Beat>();

			// 1. Store generated beats for each unique section type
			var sectionBeats = new Dictionary<SectionType, List<Beat>>();
			foreach (SectionType sectionType in songProps.Sections.Distinct())
			{
				sectionBeats[sectionType] = GenerateSection();
			}

			// 2. Add beats for each section in the original order
			foreach (SectionType sectionType in songProps.Sections)
			{
				beats.AddRange(sectionBeats[sectionType]);
			}

			return beats;
		}

		// Generic generator logic
		protected List<T> GenerateMany<T>(int count, Func<List<T>> generator)
		{
			var result = new List<T>();
			for (int i = 0; i < count; i++)
			{
				result.AddRange(generator());
			}
			return result;
		}

		protected List<Beat> GenerateSection()
		{
			return GenerateMany(songProps.PhrasesPerSection, () => GeneratePhrase());
		}

		protected List<Beat> GeneratePhrase()
		{
			return GenerateMany(songProps.BarsPerPhrase, () => GenerateBar());
		}

		protected List<Beat> GenerateBar()
		{
			var beats = new List<Beat>();

			for (int i = 0; i < songProps.TimeSignature.Numerator; i++)
			{
				beats.Add(GenerateBeat());
			}

			return beats;
		}

		protected abstract Beat GenerateBeat();
		protected abstract int GenerateMidiInstrument();
		protected abstract int GenerateMidiOctave();
	}
}