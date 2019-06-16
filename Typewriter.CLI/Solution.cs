using Buildalyzer;
using Buildalyzer.Environment;
using Buildalyzer.Workspaces;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Typewriter.CLI
{
    public class Solution
    {
        private readonly BuildOptions options;
        private readonly AnalyzerManager manager;
        private readonly AdhocWorkspace workspace = new AdhocWorkspace();

        public Solution(string solutionPath, BuildOptions options)
        {
            manager = new AnalyzerManager(solutionPath);
            this.options = options;
        }

        public IEnumerable<Project> GetProjects(ICollection<string> projectNames = null)
        {
            var projects = manager.Projects
                .Where(p => projectNames == null || projectNames.Contains(p.Value.ProjectInSolution.ProjectName))
                .Select(p => p.Value);

            foreach (var projectAnalyzer in projects)
            {
                var project = GetProject(projectAnalyzer);
                if (project != null)
                {
                    yield return project;
                }
                else
                {
                    yield return Build(projectAnalyzer).First().AddToWorkspace(workspace);
                }
            }
        }

        private Project GetProject(ProjectAnalyzer projectAnalyzer)
        {
            return workspace.CurrentSolution.Projects.Where(p => p.Id.Id == projectAnalyzer.ProjectGuid).SingleOrDefault();
        }

        private AnalyzerResults Build(ProjectAnalyzer projectAnalyzer)
        {
            EnvironmentOptions environmentOptions = new EnvironmentOptions();
            environmentOptions.DesignTime = options.DesignTime;
            if (!this.options.CleanBeforeCompile)
            {
                environmentOptions.TargetsToBuild.Remove("Clean");
            }

            return projectAnalyzer.Build(environmentOptions);
        }
    }
}
