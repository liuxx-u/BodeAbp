using Abp.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BodeAbp.Queue.Broker.Storage
{
    public class DefaultFileNamingStrategy : IFileNamingStrategy
    {
        private readonly string _prefix;
        private readonly string _pattern;
        private readonly string _format;
        private readonly Regex _fileNamePattern;

        public DefaultFileNamingStrategy(string prefix, string pattern = @"\d{6}", string format = "{0}{1:000000000}")
        {
            prefix.CheckNotNull("prefix");
            pattern.CheckNotNull("pattern");
            format.CheckNotNull("format");

            _prefix = prefix;
            _pattern = pattern;
            _format = format;

            _fileNamePattern = new Regex("^" + _prefix + _pattern);
        }

        public string GetFileNameFor(string path, int index)
        {
            index.CheckGreaterThan("index", 0, true);

            return Path.Combine(path, string.Format(_format, _prefix, index));
        }
        public string[] GetChunkFiles(string path)
        {
            var files = Directory
                            .EnumerateFiles(path)
                            .Where(x => _fileNamePattern.IsMatch(Path.GetFileName(x)))
                            .OrderBy(x => x, StringComparer.CurrentCultureIgnoreCase)
                            .ToArray();
            return files;
        }
        public string[] GetTempFiles(string path)
        {
            var files = Directory
                            .EnumerateFiles(path)
                            .Where(x => _fileNamePattern.IsMatch(Path.GetFileName(x)) && x.EndsWith(".tmp"))
                            .OrderBy(x => x, StringComparer.CurrentCultureIgnoreCase)
                            .ToArray();
            return files;
        }
    }
}
