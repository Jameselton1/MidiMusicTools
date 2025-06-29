using MidiMusicTools.Enum;
using MidiMusicTools.Models;
using MidiMusicTools.Abstracts;

namespace MidiMusicTools.Services
{
	/// <summary>
	/// Generates a melody track by selecting random notes from a power chord
	/// built on the root note for each beat. Each beat can have a random number of subdivisions.
	/// </summary>
	public class MelodyTrackGenerator : TrackGeneratorBase
	{
		private int NOTES_AT_ONCE = 1;

		/// <summary>
		/// Generates a single beat for the melody track.
		/// Each subdivision contains one random note from a power chord.
		/// </summary>
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
					// Create a chord with this beat's root note
					List<Note> chordNotes = ChordData.GetChordNotes(ChordType.Power, rootNote, songProps.Scale);
					// Choose a random note from the chord
					int noteIndex = random.Next(chordNotes.Count);
					Note melodyNote = chordNotes[noteIndex];
					// Calculate Midi note value based on octave and note
					subdivision.MidiNotes.Add((int)melodyNote + MusicConstants.NUM_OF_NOTES * MidiOctave);
				}
				beat.Subdivisions.Add(subdivision);
			}

			return beat;
		}

		/// <summary>
		/// Returns the default MIDI instrument for melody (Electric Guitar).
		/// </summary>
		protected override int DefaultMidiInstrument() => 28;

		/// <summary>
		/// Returns the default octave for melody.
		/// </summary>
		protected override int DefaultOctave() => 6;
	}
}