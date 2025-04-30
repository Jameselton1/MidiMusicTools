using MidiMusicTools.Enum;
using NAudio.Midi;

namespace MidiMusicTools
{

  public static class MusicConstants
  {
    public static int NUM_OF_NOTES = System.Enum.GetValues(typeof(Note)).Length;
  }

  public struct Song
  {
    public Track[] Tracks;
    public Mode Mode;
    public Note[] Scale;
    public char[] Sections;
    public char[] TrackStructure;
    public TimeSignature TimeSignature;
    public int PhrasesPerSection;
    public int BarsPerPhrase;
  }

  public struct TimeSignature
  {
    public int BeatsPerBar;
    public int Denominator;
  }

  public struct Track
  {
    public List<MidiEvent> Events = new List<MidiEvent>();
    public int Instrument;
    public char Type;
    public List<Note[,]> notes;

		public Track()
		{
		}
	}

  public class SongGenerator
  {
    public Song NewSong()
    {
      var random = new Random();
      var song = new Song();

      song = CreateSongProperties(song);

      // Generate track properties
      var tracks = new Track[song.TrackStructure.Length];
      for (int i = 0; i < tracks.Length; i++)
      {
        tracks[i] = new Track();
        tracks[i].Type = song.TrackStructure[i];

        switch (song.TrackStructure[i])
        {
          case 'C': tracks[i].Instrument = random.Next(32); break;
          case 'M': tracks[i].Instrument = random.Next(8) + 80; break;
          case 'B': tracks[i].Instrument = random.Next(8) + 32; break;
        }
      }

      // Generate Notes
      for (int i = 0; i < song.Sections.Length; i++)
      {
        for (int t = 0; t < tracks.Length; t++)
        {
          var notes = new List<Note[,]>();
          int[,] rootNotes = GenerateSectionRootNotes(song.BarsPerPhrase, song.TimeSignature.BeatsPerBar);
          for (int p = 0; p < song.PhrasesPerSection; p++)
          {
            for (int b = 0; b < song.BarsPerPhrase; b++)
            {
              for (int bt = 0; bt < song.TimeSignature.BeatsPerBar; bt++)
              {
                /*
                * Dimension 1: subdivisions
                * Dimension 2: num of notes played simultaneously
                */
                Note[,] beatNotes = null;
                int numOfSubdivisions = 0;
                int noteDensity = 0;
                switch (tracks[t].Type)
                {
                  case 'M':
                    numOfSubdivisions = random.Next(3) + random.Next(2);
                    noteDensity = 1;

                    beatNotes = new Note[numOfSubdivisions, noteDensity];

                    for (int s = 0; s < numOfSubdivisions; s++)
                    {
                      beatNotes[s, 0] = song.Scale[random.Next(song.Scale.Length)];
                    }
                    break;
                  case 'C':
                    int[] chordFormula = ChordData.GetChordFormula(ChordType.Triad);
                    numOfSubdivisions = 1;
                    noteDensity = chordFormula.Length;

                    beatNotes = new Note[numOfSubdivisions, noteDensity];

                    for (int c = 0; c < chordFormula.Length; c++)
                    {
                      int index = (rootNotes[b, bt] + chordFormula[c]) % song.Scale.Length;
                      beatNotes[0, c] = song.Scale[index];
                    }
                    break;
                  case 'B':
                    numOfSubdivisions = random.Next(2) + 1;
                    noteDensity = 1;

                    beatNotes = new Note[numOfSubdivisions, noteDensity];

                    for (int s = 0; s < numOfSubdivisions; s++)
                    {
                      beatNotes[s, 0] = (s == 0) ? song.Scale[rootNotes[s, 0]] : song.Scale[random.Next(song.Scale.Length)];
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
      var random = new Random();
      var rootNotes = new int[barsPerPhrase, beatsPerBar];
      for (int i = barsPerPhrase; i < barsPerPhrase; i++)
      {
        for (int j = beatsPerBar; j < beatsPerBar; j++)
        {
          rootNotes[i, j] = random.Next(7);
        }
      }
      return rootNotes;
    }

    private Song CreateSongProperties(Song song)
    {
      Random random = new Random();

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
      song.BarsPerPhrase = 4;
      song.PhrasesPerSection = 4;
      song.TimeSignature.BeatsPerBar = 4;
      song.TimeSignature.Denominator = 4;
      song.Mode = randomMode;
      song.Scale = CreateScale(root, song.Mode);
      song.Sections = randomSections.ToString().ToCharArray();
      song.TrackStructure = randomTrackStructure.ToString().ToCharArray();
      return song;
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
  
  public class SongToMidiConverter
  {
    // MIDI File Types:
    // 0 = Flattening: all tracks are combined into 1 track
    // 1 = Exploding:  all tracks are kept separate
    private int midiFileType = 1;
    private static int deltaTicksPerBar = 768;
    private int deltaTicksPerQuarterNote = deltaTicksPerBar / 4;

    // MidiEventCollection contains a list of tracks. 
    // A track contains a list of MidiEvents.
    // This means I need to create a list of MidiEvents for 
    // each track and then add each to MidiEventCollections
    public MidiEventCollection GenerateMidi(Song song)
    {
      Track[] tracks = song.Tracks;
      tracks = AddChangeInstrumentsEvent(tracks);

      // generate note events
      int timeElapsed = 0;
      for (int t = 0; t < tracks.Length; t++)
      {
        List<int[,]> midiNotes = GetMidiNotesFromTrack(tracks[t]);
        foreach (int[,] notes in midiNotes)
        {
          for (int x = 0; x < notes.GetLength(0); x++)
          {
            int[] barNotes = new int[notes.GetLength(0)];
            for (int y = 0; y < notes.GetLength(1); y++)
            {
              barNotes[y] = notes[x, y];
            }
            int noteLength = (deltaTicksPerQuarterNote / notes.Length);
            tracks[t] = AddNoteEvent(tracks[t], barNotes, timeElapsed, noteLength);
            timeElapsed += noteLength;
          }
        }
      }

      var events = new MidiEventCollection(midiFileType, deltaTicksPerQuarterNote);
      foreach (Track track in tracks)
      {
        events.AddTrack(track.Events);
      }
      events.PrepareForExport();
      return events;
    }

    public void Export(string exportFilePath, MidiEventCollection events)
    {
      MidiFile.Export(exportFilePath, events);
    }

    private Track[] AddChangeInstrumentsEvent(Track[] tracks)
    {
      // Change instruments and then return values.
      for (int i = 0; i < tracks.Length; i++)
      {
        // Change instrument midievent
        int timeInTicks = 0;
        int channel = 1;
        int instrument = 50;
        var instrumentEvent = new PatchChangeEvent(timeInTicks, channel, instrument);
        tracks[0].Events.Add(instrumentEvent);
      }
      return tracks;
    }

    private Track AddNoteEvent(Track track, int[] notes, int startTime, int noteLength)
    {
      int channel = 0;
      int velocity = 100;
      foreach (int i in notes)
      {
        var noteEvent = new NoteOnEvent(startTime, channel, notes[i], velocity, noteLength);
        track.Events.Add(noteEvent);
        track.Events.Add(noteEvent.OffEvent);
      }
      return track;
    }

    private List<int[,]> GetMidiNotesFromTrack(Track track)
    {
      return new List<int[,]>();
    }
  }
}
