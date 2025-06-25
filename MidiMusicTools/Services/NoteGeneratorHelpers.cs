using MidiMusicTools.Enum;

namespace MidiMusicTools.Services
{
	public static class NoteGeneratorHelpers
	{
		// Uses basic maths to create a list of notes within the scale which corresponds to the root and mode
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