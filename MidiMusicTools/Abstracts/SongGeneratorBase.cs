using MidiMusicTools.Models;
using MidiMusicTools.Enum;
using MidiMusicTools.Services;
using MidiMusicTools.Interfaces;

namespace MidiMusicTools.Abstracts
{
	public abstract class SongGeneratorBase : ISongGenerator
	{
		public Song GenerateSong()
		{
			var song = new Song();
			// Figure out song properties
			var songProps = BuildSongProperties();
			var rootNoteGenerator = BuildRootNoteGenerator();
			// Generate Notes
			song.Tracks = GenerateTrackNotes(songProps, rootNoteGenerator);

			return song;
		}

		protected List<Track> GenerateTrackNotes(SongProperties songProps, IRootNoteGenerator rootNoteGenerator)
		{
			// Generate root notes, which NoteGenerator generates notes around
			Queue<Note> rootNotes = rootNoteGenerator.GenerateRootNotes(songProps.TotalBeats, songProps.Scale);

			// Generate Notes
			var listTracks = new List<Track>();
			foreach (TrackType trackType in songProps.TrackStructure)
			{
				ITrackGenerator generator = trackType switch
				{
					TrackType.Melody => new MelodyTrackGenerator(),
					TrackType.Chord => new ChordTrackGenerator(),
					TrackType.Bassline => new BasslineTrackGenerator(),
					_ => throw new NotSupportedException($"TrackType {trackType} not supported")
				};

				listTracks.Add(generator.GenerateTrack(songProps, rootNotes));
			}
			return listTracks;
		}

		protected abstract SongProperties BuildSongProperties();

		protected abstract IRootNoteGenerator BuildRootNoteGenerator();
	}
}