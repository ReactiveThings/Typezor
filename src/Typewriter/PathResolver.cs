using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;

namespace Typewriter
{
    public static class PathResolver
    {
        public static string ResolveRelative(string path, string projectItem)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            if (Path.IsPathRooted(path) || projectItem == null) return path;

            if (path.StartsWith("~\\"))
            {
                throw new NotSupportedException("'~~\\'project path not supported");
            }
            else if (path.StartsWith("~~\\"))
            {
                throw new NotSupportedException("'~~\\'solution path not supported");
            }
            else
            {
                var folder = Path.GetDirectoryName(projectItem);
                return Path.Combine(folder, path);
            }
        }
    }
}
