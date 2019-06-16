using System.Collections.Generic;
using Typewriter.CodeModel.Configuration;
using Typewriter.CodeModel.Implementation;
using Typewriter.Generation;
using Typewriter.Metadata.Roslyn;

namespace Typewriter.CLI
{
    public class Typewriter
    {
        public void Generate(string solutionPath, IEnumerable<string> templatePaths, BuildOptions buildOptions)
        {
            var solution = new Solution(solutionPath, buildOptions);
            foreach (var templatePath in templatePaths)
            {
                var template = new Template(templatePath);
                RenderTemplate(solution, template);
            }
        }

        private static void RenderTemplate(Solution solution, Template template)
        {
            var includedProjects = (template.Settings as SettingsImpl).IncludedProjects;

            foreach (var project in solution.GetProjects(includedProjects))
            {
                foreach (var projectFile in project.Documents)
                {
                    var fileMetadata = new RoslynFileMetadata(projectFile, template.Settings, null /*todo*/);
                    if (fileMetadata == null)
                    {
                        // the cs-file was found, but the build-action is not set to compile.
                        continue;
                    }
                    var file = new FileImpl(fileMetadata);

                    template.RenderFile(file);

                    if (template.HasCompileException)
                    {
                        break;
                    }
                }
            }
        }
    }
}
