using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Typewriter.CodeModel.Configuration;
using Typewriter.CodeModel.Implementation;
using Typewriter.Generation;
using Typewriter.Metadata.Roslyn;

namespace Typewriter.CLI
{
    public class Typewriter
    {
        private ILoggerFactory loggerFactory;
        private ILogger logger;

        public Typewriter(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory;
            logger = loggerFactory.CreateLogger("");
            Log.Logger = logger;
        }

        public void Generate(string solutionPath, string projectPath, IEnumerable<string> templatePaths, BuildOptions buildOptions)
        {
            Stopwatch globalStopWatch = Stopwatch.StartNew();

            var solution = new Solution(solutionPath, buildOptions, loggerFactory);
            
            IEnumerable<Project> projects = !String.IsNullOrWhiteSpace(projectPath) ? solution.GetProject(projectPath).Solution.Projects : null;
            foreach (var templatePath in templatePaths)
            {
                var template = new Template(templatePath);
                var included = (template.Settings as SettingsImpl).IncludedProjects;
                if (projects == null)
                {
                    var includedProjects = (template.Settings as SettingsImpl).IncludedProjects;
                    projects = solution.GetProjects(includedProjects);
                }
                RenderTemplateForAllProjects(template, projects.Where(p=> included.Contains(p.Name)));
            }

            globalStopWatch.Stop();
            logger.LogInformation($"Done in {TimeSpan.FromMilliseconds(globalStopWatch.ElapsedMilliseconds).TotalSeconds} seconds");
        }

        private void RenderTemplateForAllProjects(Template template, IEnumerable<Project> projects)
        {
            foreach (var project in projects)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                logger.LogInformation($"Rendering template {template.TemplatePath}");
                RenderProject(template, project);
                stopwatch.Stop();
                logger.LogDebug($"Rendered in {TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds).TotalSeconds} seconds");
            }
        }

        private static void RenderProject(Template template, Microsoft.CodeAnalysis.Project project)
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
