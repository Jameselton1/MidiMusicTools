using MidiMusicTools.Enum;
using MidiMusicTools.Models;
using MidiMusicTools.Abstracts;

namespace MidiMusicTools.Services
{
    /// <summary>
    /// Generates a bassline track by selecting random notes from a chord
    /// built on the root note for each beat. Each beat contains one subdivision and one note.
    /// </summary>
    public class BasslineTrackGenerator : TrackGeneratorBase
    {
        int NUM_OF_SUBDIVISIONS = 1;
        int NOTES_AT_ONCE = 1;

        /// <summary>
        /// Generates a single beat for the bassline track.
        /// Selects a random note from a chord based on the current root note.
        /// </summary>
        protected override Beat GenerateBeat()
        {
            Note rootNote = rootNotes.Dequeue();
            var beat = new Beat();

            for (int s = 0; s < NUM_OF_SUBDIVISIONS; s++)
            {
                var subdivision = new Subdivision();
                for (int n = 0; n < NOTES_AT_ONCE; n++)
                {
                    // Create a chord from the root note and select a random note from it
                    List<Note> chordNotes = ChordData.GetChordNotes(ChordType.Power, rootNote, songProps.Scale);
                    int noteIndex = random.Next(chordNotes.Count);
                    Note basslineNote = chordNotes[noteIndex];
                    // Convert note to MIDI value and add to subdivision
                    subdivision.MidiNotes.Add((int)basslineNote + MusicConstants.NUM_OF_NOTES * MidiOctave);
                }
                beat.Subdivisions.Add(subdivision);
            }

            return beat;
        }

        /// <summary>
        /// Returns the default MIDI instrument for bass (Electric Bass).
        /// </summary>
        protected override int DefaultMidiInstrument() => 34;

        /// <summary>
        /// Returns the default octave for the bassline.
        /// </summary>
        protected override int DefaultOctave() => 3;
    }
}