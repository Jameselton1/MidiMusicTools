using MidiMusicTools.Enum;
using MidiMusicTools.Models;
using MidiMusicTools.Abstracts;

namespace MidiMusicTools.Services
{
	public class BasslineTrackGenerator() : TrackGeneratorBase
	{
		int NUM_OF_SUBDIVISIONS = 1;
		int NOTES_AT_ONCE = 1;

		protected override Beat GenerateBeat()
		{
			Note rootNote = rootNotes.Dequeue();
			var beat = new Beat();

			for (int s = 0; s < NUM_OF_SUBDIVISIONS; s++)
			{
				var subdivision = new Subdivision();
				for (int n = 0; n < NOTES_AT_ONCE; n++)
				{
					// Get the note we want by: 
					// Create Chord from beat root note
					List<Note> chordNotes = ChordData.GetChordNotes(ChordType.Power, rootNote, scale);
					// select random note from chord
					int noteIndex = random.Next(chordNotes.Count);
					Note basslineNote = chordNotes[noteIndex];
					subdivision.MidiNotes.Add((int)basslineNote + MusicConstants.NUM_OF_NOTES * MidiOctave);
				}
				beat.Subdivisions.Add(subdivision);
			}

			return beat;
		}

		protected override int GenerateMidiInstrument()
		{
			return 34;
		}

		protected override int GenerateMidiOctave()
		{
			return 3;
		}
	}
}