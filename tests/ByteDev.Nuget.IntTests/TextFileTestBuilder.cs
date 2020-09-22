using System;
using System.IO;

namespace ByteDev.Nuget.IntTests
{
    public class TextFileTestBuilder
    {
        private string _filePath = @"C:\Temp\" + Path.GetRandomFileName();
        private long _size;
        private string _text = string.Empty;

        public static TextFileTestBuilder InFileSystem => new TextFileTestBuilder();

        public TextFileTestBuilder WithFilePath(string filePath)
        {
            _filePath = filePath;
            return this;
        }

        public TextFileTestBuilder WithSize(long size)
        {
            _size = size < 0 ? 0 : size;
            return this;
        }

        public TextFileTestBuilder WithText(string text)
        {
            _text = text;
            return this;
        }

        public FileInfo Build()
        {
            if(_size > 0 && !string.IsNullOrEmpty(_text))
                throw new ArgumentException("Size cannot be set to >0 and text set to non null/empty.");

            if (string.IsNullOrEmpty(_filePath))
                throw new ArgumentException("File path was not set. A file path must be provided.");

            using (var streamWriter = File.CreateText(_filePath))
            {
                if (!string.IsNullOrEmpty(_text))
                {
                    streamWriter.Write(_text);
                }
                else
                {
                    streamWriter.WriteFillerText(_size);
                }
            }

            return new FileInfo(_filePath);
        }
    }

    internal static class TextWriterExtensions
    {
        public static void WriteFillerText(this TextWriter source, long size)
        {
            for (long l = 0; l < size; l++)
            {
                source.Write("A");
            }
        }
    }
}