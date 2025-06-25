using MidiMusicTools.Enum;
using MidiMusicTools.Models;
using MidiMusicTools.Abstracts;

namespace MidiMusicTools.Services
{
	public class ChordTrackGenerator() : TrackGeneratorBase
	{
		private int NUM_OF_SUBDIVISIONS = 1;

		// Generate a beat, the notes of which make up a chord.
		protected override Beat GenerateBeat()
		{
			var beat = new Beat();

			Note rootNote = rootNotes.Dequeue();
			List<Note> chordNotes = ChordData.GetChordNotes(ChordType.Triad, rootNote, scale);

			for (int s = 0; s < NUM_OF_SUBDIVISIONS; s++)
			{
				var subdivision = new Subdivision();
				for (int c = 0; c < chordNotes.Count; c++)
				{
					subdivision.MidiNotes.Add((int)chordNotes[c] + MusicConstants.NUM_OF_NOTES * MidiOctave);
				}
				beat.Subdivisions.Add(subdivision);
			}
			return beat;
		}

		protected override int GenerateMidiInstrument()
		{
			return 1;
		}
		protected override int GenerateMidiOctave()
		{
			return 4;
		}
	}
}