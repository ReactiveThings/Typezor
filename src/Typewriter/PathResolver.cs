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
                var folder = Path.GetDirectoryName(projectItem);
                return Path.Combine(folder, path.Substring(2));
            }
            //else if (path.StartsWith("~~\\"))
            //{
            //    var folder = Path.GetDirectoryName(projectItem.Project.Solution.FilePath);
            //    return Path.Combine(folder, path.Substring(3));
            //}
            else
            {
                var folder = Path.GetDirectoryName(projectItem);
                return Path.Combine(folder, path);
            }
        }
    }
}
