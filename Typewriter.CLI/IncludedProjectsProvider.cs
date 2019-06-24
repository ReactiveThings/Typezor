using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Typewriter.CodeModel.Configuration;

namespace Typewriter.CLI
{
    public class IncludedProjectsProvider
    {
        private readonly ILogger logger;

        public IncludedProjectsProvider(ILogger logger)
        {
            this.logger = logger;
        }
        public ICollection<string> GetIncludedProjects(SettingsImpl settings, string projectPath, IEnumerable<Project> projects)
        {
            var includedProjects = settings.IncludedProjects;
            if (projects == null)
            {
                if (settings.ShouldIncludeAllProjects)
                {
                    includedProjects = null; // include all projects
                }
                if (settings.ShouldIncludeCurrentProject && !settings.ShouldIncludeAllProjects)
                {
                    throw new ArgumentException("IncludeCurrentProject is not supported. Please use IncludeProject");
                }
                if (settings.ShouldIncludeReferencedProjects)
                {
                    throw new ArgumentException("IncludeReferencedProjects is not supported. Please use IncludeProject");
                }
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
                    throw new ArgumentException("IncludeAllProjects is not supported when project parameter is provided");
                }
            }
            return includedProjects;
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
    }
}
