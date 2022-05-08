namespace Kaya
{
	public static class Info
	{
		public class Version
		{
			public readonly int Major;
			public readonly int Minor;
			public readonly int Patch;
			public readonly int Revision;

			public Version(int major, int minor, int patch, int revision)
			{
				this.Major = major;
				this.Minor = minor;
				this.Patch = patch;
				this.Revision = revision;
			}
		}

		public static readonly Version AssetVersion = new Version(0, 0, 1, 0);
	}
}