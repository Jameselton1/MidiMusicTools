using MidiMusicTools.Enum;
using NAudio.Midi;
using System;
using System.ComponentModel.DataAnnotations;

namespace MidiMusicTools {

  public static class MusicConstants {
    public static int NUM_OF_NOTES = System.Enum.GetValues(typeof(Note)).Length;
  }
  
  public struct Song {
    public Track[] Tracks;

    public Mode Mode;
    public Note[] Scale;
    public char[] Sections;
    public char[] TrackStructure;
    public TimeSignature TimeSignature;
    public int PhrasesPerSection;
    public int BarsPerPhrase;
  }

  public struct TimeSignature {
    public int BeatsPerBar;
    public int Denominator;
  }
  
  public struct Track {
    public IList<MidiEvent> Events;
    public int Instrument;
    public char Type;
    public List<Note[,]> notes;
  }

  public class SongGenerator {

    public Song NewSong() {
      var random = new Random();
      var song = new Song();

      song = CreateSongProperties(song);

      // Generate track properties
      var tracks = new Track[song.TrackStructure.Length];
      for (int i = 0; i < tracks.Length; i++) {
        tracks[i] = new Track();
        tracks[i].Type = song.TrackStructure[i];

        switch (song.TrackStructure[i]) {
          case 'C': tracks[i].Instrument = random.Next(32); break;
          case 'M': tracks[i].Instrument = random.Next(8) + 80; break;
          case 'B': tracks[i].Instrument = random.Next(8) + 32; break;
        }
      }

      // Generate Notes
      for (int i = 0; i < song.Sections.Length; i++) {
        for (int t = 0; t < tracks.Length; t++) {
          var notes = new List<Note[,]>();
          int[,] rootNotes = GenerateSectionRootNotes(song.BarsPerPhrase, song.TimeSignature.BeatsPerBar);
          for (int p = 0; p < song.PhrasesPerSection; p++) {
            for (int b = 0; b < song.BarsPerPhrase; b++) {
              for (int bt = 0; bt < song.TimeSignature.BeatsPerBar; bt++) {
                /*
                * Dimension 1: subdivisions
                * Dimension 2: num of notes played simultaneously
                */
                Note[,] barNotes = null;
                switch (tracks[t].Type) {
                  case 'M':
                    barNotes = new Note[random.Next(3) + random.Next(2), 1];
                    for (int j = 0; j < barNotes.Length; j++) {
                      barNotes[j, 0] = song.Scale[random.Next(song.Scale.Length)];
                    }
                    break;
                  case 'C':
                    int[] chordFormula = ChordData.GetChordFormula(ChordType.Triad);
                    barNotes = new Note[1, chordFormula.Length];
                    for (int c = 0; c < chordFormula.Length; c++) {
                      int index = (rootNotes[b, bt] + chordFormula[i]) % song.Scale.Length;
                      barNotes[0, c] = song.Scale[index];
                    }
                    break;
                  case 'B':
                    barNotes = new Note[random.Next(2) + 1, 1];
                    for (int j = 0; j < barNotes.Length; j++) {
                      barNotes[j, 0] = (j == 0) ? song.Scale[rootNotes[j, i]] : song.Scale[random.Next(song.Scale.Length)];
                    }
                    break;
                }
                notes.Add(barNotes);
              }
            }
          }
          tracks[t].notes = notes;
        }
      }
      song.Tracks = tracks;
      return song;
    }

    private int[,] GenerateSectionRootNotes(int barsPerPhrase, int beatsPerBar) {
      var random = new Random();
      var rootNotes = new int[barsPerPhrase, beatsPerBar];
      for (int i  = barsPerPhrase; i < barsPerPhrase; i++) {
        for (int j = beatsPerBar; j < beatsPerBar; j++) {
          rootNotes[i, j] = random.Next(7);
        }
      }
      return rootNotes;
    }

    private Song CreateSongProperties(Song song) {
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
    private Note[] CreateScale(int root, Mode mode) {
      int[] template = ScaleData.GetScaleTemplate(mode);
      Note[] scale = new Note[template.Length];

      for (int i = 0; i < template.Length; i++) {
        int index = (root + template[i]) % 12;
        scale[i] = (Note)System.Enum.GetValues(typeof(Note)).GetValue(index);
      }
      return scale;
    }

  }

  public class MidiMusicGenerator {
    // MIDI File Types:
    // 0 = Flattening: all tracks are combined into 1 track
    // 1 = Exploding:  all tracks are kept separate
    private int midiFileType = 1;

    private static int deltaTicksPerBar = 768;
    private int deltaTicksPerQuarterNote = deltaTicksPerBar / 4;

    public MidiEventCollection GenerateMidi() {
      MidiEventCollection events = new MidiEventCollection(midiFileType, deltaTicksPerQuarterNote);
      
      MidiEvent midiEvent = new MidiEvent(2, 0, MidiCommandCode.NoteOn);
      events.AddEvent(midiEvent, 0);
    }
  }
}