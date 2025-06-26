using MidiMusicTools.Abstracts;
using MidiMusicTools.Interfaces;
using MidiMusicTools.Models;
using MidiMusicTools.Enum;

namespace MidiMusicTools.Services
{
	public class RandomPropertiesSongGenerator() : SongGeneratorBase
	{
		private Random random = new Random();

		protected override SongProperties BuildSongProperties()
		{
			var songProps = new SongProperties();

			// Assign values
			songProps.Root = GenerateRoot();
			songProps.Mode = GenerateMode();
			songProps.Sections = GenerateSections();
			songProps.TrackStructure = GenerateTrackTypes();

			songProps.BarsPerPhrase = 4;
			songProps.PhrasesPerSection = 2;

			songProps.TimeSignature = new TimeSignature
			{
				Numerator = 4,
				Denominator = 4
			};
			
			return songProps;
		}

		protected override IRootNoteGenerator BuildRootNoteGenerator()
		{
			return new RandomRootNoteGenerator();
		}

		private int GenerateRoot()
		{
			return random.Next(MusicConstants.NUM_OF_NOTES);
		}

		private Mode GenerateMode()
		{
			Array modeValues = System.Enum.GetValues(typeof(Mode));
			Mode randomMode = (Mode)modeValues.GetValue(random.Next(modeValues.Length));
			return randomMode;
		}

		private List<SectionType> GenerateSections()
		{
			return new List<SectionType>()
			{
				SectionType.Verse,
				SectionType.Chorus,
				SectionType.Verse,
				SectionType.Chorus,
				SectionType.Verse,
				SectionType.Chorus
			};
		}

		private List<TrackType> GenerateTrackTypes()
		{
			return new List<TrackType>()
			{
				TrackType.Melody,
				TrackType.Chord,
				TrackType.Bassline
			};
		}
	}
}