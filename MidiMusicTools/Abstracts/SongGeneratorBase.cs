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
			// Generate root notes, which NoteGenerator generates notes around
			Queue<Note> rootNotes = rootNoteGenerator.GenerateRootNotes(songProps.TotalBeats, songProps.Scale);
			song.Tracks = GenerateTracks(songProps, rootNotes);

			return song;
		}

		protected List<Track> GenerateTracks(SongProperties songProps, Queue<Note> rootNotes)
		{
			// Initialise new track
			var listTracks = new List<Track>();
			// Generate notes for the new track
			foreach (TrackProperties trackProperties in songProps.TrackStructure)
			{
				ITrackGenerator generator = NoteGeneratorHelpers.TrackTypeGeneratorRegistry[trackProperties.Type];
				var rootNotesCopy = new Queue<Note>(rootNotes);
				// Add track to tracklist
				Track track = generator.GenerateTrack(songProps, rootNotesCopy, trackProperties.Instrument, trackProperties.Octave);
				listTracks.Add(track);
			}
			// return tracklist
			return listTracks;
		}

		protected abstract SongProperties BuildSongProperties();

		protected abstract IRootNoteGenerator BuildRootNoteGenerator();
	}
}