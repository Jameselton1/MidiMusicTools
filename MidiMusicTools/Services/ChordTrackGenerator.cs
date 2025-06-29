using MidiMusicTools.Enum;
using MidiMusicTools.Models;
using MidiMusicTools.Abstracts;

namespace MidiMusicTools.Services
{
	/// <summary>
	/// Generates a chord track by building triad chords on the root note for each beat.
	/// Each beat contains one subdivision with all chord notes played together.
	/// </summary>
	public class ChordTrackGenerator : TrackGeneratorBase
	{
		private int NUM_OF_SUBDIVISIONS = 1;

		/// <summary>
		/// Generates a single beat for the chord track.
		/// Builds a triad chord and adds all notes to the subdivision.
		/// </summary>
		protected override Beat GenerateBeat()
		{
			var beat = new Beat();

			Note rootNote = rootNotes.Dequeue();
			List<Note> chordNotes = ChordData.GetChordNotes(ChordType.Triad, rootNote, songProps.Scale);

			for (int s = 0; s < NUM_OF_SUBDIVISIONS; s++)
			{
				var subdivision = new Subdivision();
				for (int c = 0; c < chordNotes.Count; c++)
				{
					// Calculate Midi note value based on octave and note
					subdivision.MidiNotes.Add((int)chordNotes[c] + MusicConstants.NUM_OF_NOTES * MidiOctave);
				}
				beat.Subdivisions.Add(subdivision);
			}
			return beat;
		}

		/// <summary>
		/// Returns the default MIDI instrument for chords (Acoustic Grand Piano).
		/// </summary>
		protected override int DefaultMidiInstrument() => 1;

		/// <summary>
		/// Returns the default octave for chords.
		/// </summary>
		protected override int DefaultOctave() => 4;
	}
}