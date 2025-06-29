using MidiMusicTools.Enum;

namespace MidiMusicTools.Services
{
	/// <summary>
	/// Helper methods for generating musical scales based on root and mode.
	/// </summary>
	public static class NoteGeneratorHelpers
	{
		/// <summary>
		/// Generates a list of notes within the scale that corresponds to the given root and mode.
		/// </summary>
		/// <param name="root">The root note (0-11, C-B).</param>
		/// <param name="mode">The musical mode (e.g., Major, Minor, Dorian).</param>
		/// <returns>List of notes in the scale.</returns>
		public static List<Note> GenerateScale(int root, Mode mode)
		{
			int[] template = ScaleData.GetScaleTemplate(mode);
			var scale = new List<Note>();

			for (int i = 0; i < template.Length; i++)
			{
				int index = (root + template[i]) % 12;
				scale.Add(
						(Note)System.Enum.GetValues(typeof(Note)).GetValue(index)
				);
			}
			return scale;
		}
	}
}