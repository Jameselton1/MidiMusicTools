using MidiMusicTools.Enum;
using NAudio.Midi;

namespace MidiMusicTools
{
	public struct SongProperties
	{
		public int Root;
		public Mode Mode;
		public Note[] Scale;
		public char[] Sections;
		public char[] TrackStructure;
		public TimeSignature TimeSignature;
		public int PhrasesPerSection;
		public int BarsPerPhrase;
	}

	public struct Song
	{
		public Track[] Tracks;
	}
	
	public class SongGenerator
	{
		private readonly Random random = new Random();

		public Song NewSong(SongProperties songProps = default)
		{
			var song = new Song();

			if (songProps.Equals(default(SongProperties)))
			{
				songProps = GenerateRandomSongProperties();
			}

			// Generate track properties
			var tracks = new Track[songProps.TrackStructure.Length];
			for (int i = 0; i < tracks.Length; i++)
			{
				tracks[i] = new Track();
				tracks[i].Type = songProps.TrackStructure[i];

				switch (songProps.TrackStructure[i])
				{
					case 'C': tracks[i].Instrument = random.Next(32); break;
					case 'M': tracks[i].Instrument = random.Next(8) + 80; break;
					case 'B': tracks[i].Instrument = random.Next(8) + 32; break;
				}
			}

			// Generate Notes
			for (int i = 0; i < songProps.Sections.Length; i++)
			{
				int[,] rootNotes = GenerateSectionRootNotes(songProps.BarsPerPhrase, songProps.TimeSignature.BeatsPerBar);
				for (int t = 0; t < tracks.Length; t++)
				{
					var notes = new List<Note[,]>();
					for (int p = 0; p < songProps.PhrasesPerSection; p++)
					{
						for (int b = 0; b < songProps.BarsPerPhrase; b++)
						{
							for (int bt = 0; bt < songProps.TimeSignature.BeatsPerBar; bt++)
							{
								/*
								* Dimension 1: subdivisions
								* Dimension 2: num of notes played simultaneously
								*/
								Note[,] beatNotes = null;
								int numOfSubdivisions = 0;
								int noteDensity = 0;
								int[] chordFormula;
								switch (tracks[t].Type)
								{
									// Melody
									case 'M':
										chordFormula = ChordData.GetChordFormula(ChordType.Power);
										numOfSubdivisions = random.Next(2) + 1;
										noteDensity = 1;

										beatNotes = new Note[numOfSubdivisions, noteDensity];

										for (int s = 0; s < numOfSubdivisions; s++)
										{
											int index = (rootNotes[b, bt] + chordFormula[random.Next(chordFormula.Length)]) % songProps.Scale.Length;
											beatNotes[s, 0] = songProps.Scale[index];
										}
										break;
									// Chord
									case 'C':
										chordFormula = ChordData.GetChordFormula(ChordType.Triad);
										numOfSubdivisions = 1;
										noteDensity = chordFormula.Length;

										beatNotes = new Note[numOfSubdivisions, noteDensity];

										for (int c = 0; c < chordFormula.Length; c++)
										{
											int index = (rootNotes[b, bt] + chordFormula[c]) % songProps.Scale.Length;
											beatNotes[0, c] = songProps.Scale[index];
										}
										break;
									// Bassline
									case 'B':
										numOfSubdivisions = 1;

										int numOfNotesAtATime = 1;

										beatNotes = new Note[numOfSubdivisions, numOfNotesAtATime];

										for (int s = 0; s < numOfSubdivisions; s++)
										{
											beatNotes[s, 0] = (s == 0) ? songProps.Scale[rootNotes[b, bt]] : songProps.Scale[random.Next(songProps.Scale.Length)];
										}
										break;
								}
								notes.Add(beatNotes);
							}
						}
					}
					tracks[t].notes = notes;
				}
			}
			song.Tracks = tracks;
			return song;
		}

		private int[,] GenerateSectionRootNotes(int barsPerPhrase, int beatsPerBar)
		{
			var rootNotes = new int[barsPerPhrase, beatsPerBar];
			for (int i = 0; i < barsPerPhrase; i++)
			{
				for (int j = 0; j < beatsPerBar; j++)
				{
					rootNotes[i, j] = random.Next(7);
				}
			}
			return rootNotes;
		}

		private SongProperties GenerateRandomSongProperties()
		{
			var songProps = new SongProperties();

			// Generate random mode
			int root = random.Next(MusicConstants.NUM_OF_NOTES);
			Array modeValues = System.Enum.GetValues(typeof(Mode));
			Mode randomMode = (Mode)modeValues.GetValue(random.Next(modeValues.Length));

			// Generate random song structure
			Array songStructureValues = System.Enum.GetValues(typeof(SongStructure));
			SongStructure randomSections = (SongStructure)songStructureValues.GetValue(random.Next(songStructureValues.Length));

			//Generate random track structure
			Array trackStructureValues = System.Enum.GetValues(typeof(TrackStructure));
			TrackStructure randomTrackStructure = (TrackStructure)trackStructureValues.GetValue(random.Next(trackStructureValues.Length));

			// Assign values
			songProps.Root = root;
			songProps.Mode = randomMode;
			songProps.Scale = CreateScale(root, songProps.Mode);
			songProps.Sections = randomSections.ToString().ToCharArray();
			songProps.TrackStructure = randomTrackStructure.ToString().ToCharArray();

			songProps.BarsPerPhrase = 4;
			songProps.PhrasesPerSection = 4;

			songProps.TimeSignature.BeatsPerBar = 4;
			songProps.TimeSignature.Denominator = 4;
			return songProps;
		}

		// Return an array of musical notes, based on a scale template (mode) and a root note
		private Note[] CreateScale(int root, Mode mode)
		{
			int[] template = ScaleData.GetScaleTemplate(mode);
			Note[] scale = new Note[template.Length];

			for (int i = 0; i < template.Length; i++)
			{
				int index = (root + template[i]) % 12;
				scale[i] = (Note)System.Enum.GetValues(typeof(Note)).GetValue(index);
			}
			return scale;
		}
	}
}