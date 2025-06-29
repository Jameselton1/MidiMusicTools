using MidiMusicTools.Enum;
using MidiMusicTools.Models;
using MidiMusicTools.Interfaces;
using System.Linq;
using MidiMusicTools.Services;

namespace MidiMusicTools.Abstracts
{
	/// <summary>
	/// Abstract base class for generating MIDI tracks.
	/// Concrete subclasses implement specific note generation logic for each beat.
	/// </summary>
	public abstract class TrackGeneratorBase : ITrackGenerator
	{
		// Queue of root notes for the track.
		protected Queue<Note> rootNotes = new Queue<Note>();
		// Properties describing the song structure and settings.
		protected SongProperties songProps = new SongProperties();
		
		protected Random random = new Random();
		protected int MidiOctave;

		/// <summary>
		/// Generates a MIDI track using the provided song properties, root notes, instrument, and octave.
		/// </summary>
		/// <param name="sp">Song properties.</param>
		/// <param name="rn">Queue of root notes for the track.</param>
		/// <param name="instrument">MIDI instrument number (-1 for random/default).</param>
		/// <param name="octave">MIDI octave (-1 for random/default).</param>
		/// <returns>A generated Track object.</returns>
		public Track GenerateTrack(SongProperties sp, Queue<Note> rn, int instrument = -1, int octave = -1)
		{
			var track = new Track();

			rootNotes = rn;
			songProps = sp;

			MidiOctave = (octave == -1) ? DefaultOctave() : octave;
			track.Beats = GenerateBeats();
			track.MidiInstrument = (instrument == -1) ? DefaultMidiInstrument() : instrument;
			return track;
		}

		/// <summary>
		/// Generates all beats for the track, organized by section and structure.
		/// </summary>
		/// <returns>List of all beats in the track.</returns>
		protected List<Beat> GenerateBeats()
		{
			var beats = new List<Beat>();

			// Store generated beats for each unique section type.
			var sectionBeats = new Dictionary<SectionType, List<Beat>>();
			foreach (SectionType sectionType in songProps.Sections.Distinct())
			{
				sectionBeats[sectionType] = GenerateSection();
			}

			// Add beats for each section in the original order.
			foreach (SectionType sectionType in songProps.Sections)
			{
				beats.AddRange(sectionBeats[sectionType]);
			}

			return beats;
		}

		/// <summary>
		/// Generates the total number of beats for a section type (phrases × bars × beats per bar)
		/// by repeatedly invoking the provided generator function.
		/// </summary>
		/// <typeparam name="T">Type of item to generate.</typeparam>
		/// <param name="count">Number of times to call the generator.</param>
		/// <param name="generator">Function that generates a list of items.</param>
		/// <returns>List of generated items.</returns>
		protected List<T> GenerateMany<T>(int count, Func<List<T>> generator)
		{
			var result = new List<T>();
			for (int i = 0; i < count; i++)
			{
				result.AddRange(generator());
			}
			return result;
		}

		/// <summary>
		/// Generates all beats for a section by generating phrases.
		/// </summary>
		/// <returns>List of beats in the section.</returns>
		protected List<Beat> GenerateSection()
		{
			return GenerateMany(songProps.PhrasesPerSection, () => GeneratePhrase());
		}

		/// <summary>
		/// Generates all beats for a phrase by generating bars.
		/// </summary>
		/// <returns>List of beats in the phrase.</returns>
		protected List<Beat> GeneratePhrase()
		{
			return GenerateMany(songProps.BarsPerPhrase, () => GenerateBar());
		}

		/// <summary>
		/// Generates all beats for a bar.
		/// </summary>
		/// <returns>List of beats in the bar.</returns>
		protected List<Beat> GenerateBar()
		{
			var beats = new List<Beat>();

			for (int i = 0; i < songProps.TimeSignature.Numerator; i++)
			{
				beats.Add(GenerateBeat());
			}

			return beats;
		}

		/// <summary>
		/// Generates a single beat. Must be implemented by subclasses.
		/// </summary>
		/// <returns>A generated Beat object.</returns>
		protected abstract Beat GenerateBeat();

		/// <summary>
		/// Returns the default MIDI instrument number for the track.
		/// Must be implemented by subclasses.
		/// </summary>
		protected abstract int DefaultMidiInstrument();

		/// <summary>
		/// Returns the default MIDI octave for the track.
		/// Must be implemented by subclasses.
		/// </summary>
		protected abstract int DefaultOctave();
	}
}