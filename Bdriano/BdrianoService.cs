using System.Text;

namespace Bdriano {
	public class BdrianoService : IBdrianoService {
		public string Bdrianofy(long steps)
			=> GetPrefix(steps) + "driano";

		private static string GetPrefix(long number) {
			var prefixBuilder = new StringBuilder();
			while (--number >= 0) {
				var letterStartIndex = (number / 26) - 1 < 0 ? 65 : 97;
				var c = (char)(letterStartIndex + number % 26);
				prefixBuilder.Insert(0, c);
				number /= 26;
			}
			return prefixBuilder.ToString();
		}
	}
}