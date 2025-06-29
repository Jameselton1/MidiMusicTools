# Website
Try out the library at [iwantmidi.com](https://iwantmidi.com) now!

## Overview

**MidiMusicTools** is a C# library for generating algorithmic music using music theory concepts and exporting it as MIDI files. It uses the [NAudio MIDI library](https://github.com/naudio/NAudio) for MIDI file creation and manipulation. The library is designed to create multi-instrumental music, with support for melodies, chords, and basslines, all adhering to musical scales and modes.

## Features

- **Song Structure Generation:** Automatically generates song structures with sections (e.g., verse, chorus), phrases, bars, beats, and subdivisions.
- **Track Types:** Supports melody, chord, and bassline tracks, each with their own note generation logic.
- **Scale and Mode Support:** Generates notes based on a root note and musical mode (major, minor, dorian, etc.).
- **Chord Generation:** Supports different chord types (triad, power, seventh) and generates chord notes from the scale.
- **Randomisation:** Includes random generators for song properties and root notes for variety.
- **MIDI Export:** Converts generated songs into standard MIDI files for use in DAWs such as FL Studio or Logic Pro.

## Usage

### 1. Generate a Song

Use a song generator (e.g., `RandomSongGenerator`) to create a song:

```csharp
var generator = new RandomSongGenerator();
var song = generator.GenerateSong();
```

### 2. Convert Song to MIDI

Use the `SongToMidiConverter` to convert the song to a MIDI event collection and export it:

```csharp
var converter = new SongToMidiConverter();
var midiEvents = converter.GenerateMidi(song);
converter.Export("output.mid", midiEvents);
```

### 3. Import to DAW

Open the generated `output.mid` file in your favourite DAW to listen, edit, or arrange the music.

## Project Structure

- **Abstracts/**: Base classes for song and track generators.
- **Enum/**: Enumerations for notes, modes, chord types, etc.
- **Interfaces/**: Interfaces for extensibility.
- **Models/**: Core data structures (Song, Track, Beat, etc.).
- **Services/**: Implementations for generators and MIDI conversion.

## Extending

You can create your own song or track generators by inheriting from `SongGeneratorBase` or `TrackGeneratorBase` and implementing the required methods.

## Example

```csharp
// Generate a random song and export as MIDI
var generator = new RandomSongGenerator();
var song = generator.GenerateSong();

var converter = new SongToMidiConverter();
var midiEvents = converter.GenerateMidi(song);
converter.Export("mySong.mid", midiEvents);
```

## Requirements

- .NET 8.0 or later
- [NAudio.Midi](https://www.nuget.org/packages/NAudio.Midi/)

## Licence

MIT Licence. See [LICENSE](LICENSE) for details.

## Credits

- [NAudio](https://github.com/naudio/NAudio) for MIDI support.
- Music theory resources from [LedgerNote](https://ledgernote.com/columns/music-theory/musical-modes-explained/)

