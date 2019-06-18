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
                var settings = (template.Settings as SettingsImpl);
                var includedProjects = settings.IncludedProjects;

                if (projects == null)
                {
                    if (settings.ShouldIncludeAllProjects)
                    {
                        includedProjects = null; // include all projects
                    }
                    if (settings.ShouldIncludeCurrentProject && !settings.ShouldIncludeAllProjects)
                    {
                        logger.LogError("IncludeCurrentProject is not supported. Please use IncludeProject");
                        return;
                    }
                    if (settings.ShouldIncludeReferencedProjects)
                    {
                        logger.LogError("IncludeReferencedProjects is not supported. Please use IncludeProject");
                        return;
                    }
                    projects = solution.GetProjects(includedProjects);
                }
                else
                {
                    if (settings.ShouldIncludeCurrentProject)
                    {
                        logger.LogWarning("IncludeCurrentProject is not supported. Project provided in projectPath parameter will be used");
                        var currentProject = GetCurrentProject(projectPath, projects);
                        includedProjects.Add(currentProject.Name);
                    }
                    if (settings.ShouldIncludeReferencedProjects)
                    {
                        logger.LogWarning("IncludeReferencedProjects is not supported. Referenced Projects for Project provided in projectPath parameter will be used");
                        var currentProject = GetCurrentProject(projectPath, projects);
                        foreach (var referencedProject in GetReferencedProjects(projects, currentProject))
                        {
                            includedProjects.Add(referencedProject.Name);
                        }
                    }
                    if (settings.ShouldIncludeAllProjects)
                    {
                        logger.LogError("IncludeAllProjects is not supported when project parameter is provided");
                        return;
                    }
                }
                RenderTemplateForAllProjects(template, projects.Where(p => includedProjects == null || includedProjects.Contains(p.Name)));
            }

            globalStopWatch.Stop();
            logger.LogInformation($"Done in {TimeSpan.FromMilliseconds(globalStopWatch.ElapsedMilliseconds).TotalSeconds} seconds");
        }

        private static IEnumerable<Project> GetReferencedProjects(IEnumerable<Project> projects, Project currentProject)
        {
            var referencesId = currentProject.ProjectReferences.Select(r => r.ProjectId);
            var referencedProjects = projects.Where(p => referencesId.Contains(p.Id));
            return referencedProjects;
        }

        private static Project GetCurrentProject(string projectPath, IEnumerable<Project> projects)
        {
            return projects.Where(p => p.FilePath.Equals(projectPath, StringComparison.InvariantCultureIgnoreCase)).Single();
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
