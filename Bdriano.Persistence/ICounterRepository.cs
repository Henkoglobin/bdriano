using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Bdriano.Persistence {
	public interface ICounterRepository {
		Task<long> GetCurrentAsync();
		Task IncrementAsync();
	}

	public class FileCounterRepository : ICounterRepository {
		private FileInfo StorageFile {
			get {
				var fileInfo = new FileInfo("counter.txt");

				if (!fileInfo.Exists) {
					using var writer = new StreamWriter(fileInfo.Create(), Encoding.ASCII);

					writer.Write("1");
				}

				return fileInfo;
			}
		}

		public async Task<long> GetCurrentAsync() {
			using var reader = new StreamReader(StorageFile.OpenRead());

			var content = await reader.ReadToEndAsync();
			
			return Int64.Parse(content);
		}

		public async Task IncrementAsync() {
			await using var stream = StorageFile.Open(FileMode.Open);
			
			using var reader = new StreamReader(stream, Encoding.ASCII, true, 4096, true);

			var value = Int64.Parse(await reader.ReadToEndAsync()) + 1;
			stream.Seek(0, SeekOrigin.Begin);

			await using var writer = new StreamWriter(stream, Encoding.ASCII);

			await writer.WriteAsync(value.ToString());
		}
	}

	public static class CounterRepositoryExtensions {
		public static async Task<long> GetCurrentAndIncrementAsync(this ICounterRepository repository) {
			var value = await repository.GetCurrentAsync();
			await repository.IncrementAsync();

			return value;
		}
	}
}
