using MidiMusicTools.Abstracts;
using MidiMusicTools.Interfaces;
using MidiMusicTools.Models;
using MidiMusicTools.Enum;
using MidiMusicTools.Services;

namespace TestMidiTools.Services
{
    /// <summary>
    /// Generates a random song structure, sections, tracks, and root notes.
    /// Used for procedural music generation.
    /// </summary>
    public class RandomSongGenerator : SongGeneratorBase
    {
        private Random random = new Random();

        /// <summary>
        /// Builds random song properties (root, mode, sections, track structure, etc.).
        /// </summary>
        protected override SongProperties BuildSongProperties()
        {
            var songProps = new SongProperties();

            // Assign values
            songProps.Root = GenerateRoot();
            songProps.Mode = GenerateMode();
            songProps.Sections = GenerateSections();
            songProps.TrackStructure = GenerateTrackProperties();

            songProps.BarsPerPhrase = 4;
            songProps.PhrasesPerSection = 2;

            songProps.TimeSignature = new TimeSignature
            {
                Numerator = 4,
                Denominator = 4
            };

            return songProps;
        }

        /// <summary>
        /// Returns a random root note generator.
        /// </summary>
        protected override IRootNoteGenerator BuildRootNoteGenerator()
        {
            return new RandomRootNoteGenerator();
        }

        private int GenerateRoot() => random.Next(MusicConstants.NUM_OF_NOTES);

        private Mode GenerateMode()
        {
            Array modeValues = System.Enum.GetValues(typeof(Mode));
            Mode randomMode = (Mode)modeValues.GetValue(random.Next(modeValues.Length));
            return randomMode;
        }

        private List<SectionType> GenerateSections()
        {
            // Example: Alternates Verse and Chorus
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

        private List<TrackProperties> GenerateTrackProperties()
        {
            return new List<TrackProperties>()
            {
                new TrackProperties() {
                    Type = TrackType.Melody,
                    Instrument = -1,
                    Octave = -1
                },
                new TrackProperties() {
                    Type = TrackType.Chord,
                    Instrument = -1,
                    Octave = -1
                },
                new TrackProperties() {
                    Type = TrackType.Bassline,
                    Instrument = -1,
                    Octave = -1
                },
            };
        }
    }
}