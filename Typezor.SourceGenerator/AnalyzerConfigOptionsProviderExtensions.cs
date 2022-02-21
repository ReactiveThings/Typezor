using System;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Typezor.SourceGenerator
{
    internal static class AnalyzerConfigOptionsProviderExtensions
    {
        private static string? TryGetValue(this AnalyzerConfigOptions options, string key) =>
            options.TryGetValue(key, out var value) ? value : null;

        private static string? TryGetAdditionalFileMetadataValue(this AnalyzerConfigOptions options, string propertyName) =>
            options.TryGetValue($"build_metadata.AdditionalFiles.{propertyName}");

        public static bool IsClass(this AnalyzerConfigOptionsProvider analyzerConfigOptions, AdditionalText file)
        {
            return new FileInfo(file.Path).Extension == ".cs" && analyzerConfigOptions.IsTypezorFile(file);
        }

        public static bool IsReference(this AnalyzerConfigOptionsProvider analyzerConfigOptions, AdditionalText file)
        {
            var extension = new FileInfo(file.Path).Extension;
            return extension == ".dll" && analyzerConfigOptions.IsTypezorFile(file);
        }

        public static bool IsTemplate(this AnalyzerConfigOptionsProvider analyzerConfigOptions, AdditionalText file)
        {
            var extension = new FileInfo(file.Path).Extension;
            return new[] { ".cshtml", ".razor" }.Contains(extension) && analyzerConfigOptions.IsTypezorFile(file);
        }

        public static bool IsTypezorFile(this AnalyzerConfigOptionsProvider analyzerConfigOptions, AdditionalText file)
        {
            return string.Equals(

                analyzerConfigOptions.GetOptions(file)
                    .TryGetAdditionalFileMetadataValue("Typezor"),
                "true",
                StringComparison.OrdinalIgnoreCase
            );
        }
    }
}