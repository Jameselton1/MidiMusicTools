using MidiMusicTools.Enum;
using MidiMusicTools.Models;
using MidiMusicTools.Abstracts;

namespace MidiMusicTools.Services
{
	public class MelodyTrackGenerator() : TrackGeneratorBase
	{
		private int NOTES_AT_ONCE = 1;

		// Generates a melody by:
		// 1 note playing at a time
		// random number of subdivisions
		// notes selected from within a chord, based on the current root note
		protected override Beat GenerateBeat()
		{
			Note rootNote = rootNotes.Dequeue();

			int numOfSubdivisions = random.Next(2) + 1;

			Beat beat = new Beat();

			for (int s = 0; s < numOfSubdivisions; s++)
			{
				var subdivision = new Subdivision();

				for (int n = 0; n < NOTES_AT_ONCE; n++)
				{
					// Figure out the index of the note we want by:
					// Create a chord with this beat's root note
					List<Note> chordNotes = ChordData.GetChordNotes(ChordType.Power, rootNote, scale);
					// Choose a random note from the chord
					int noteIndex = random.Next(chordNotes.Count);
					Note melodyNote = chordNotes[noteIndex];
					subdivision.MidiNotes.Add((int)melodyNote + MusicConstants.NUM_OF_NOTES * MidiOctave);
				}
				beat.Subdivisions.Add(subdivision);
			}

			return beat;
		}

		protected override int GenerateMidiInstrument()
		{
			return 28;
		}
		protected override int GenerateMidiOctave()
		{
			return 6;
		}
	}
}