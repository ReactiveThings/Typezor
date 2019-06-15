using Buildalyzer;
using Buildalyzer.Workspaces;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Typewriter.CLI
{
    public class Solution
    {
        private AnalyzerManager manager;
        AdhocWorkspace workspace = new AdhocWorkspace();

        public Solution(string solutionPath)
        {
            manager = new AnalyzerManager(solutionPath);
        }

        public IEnumerable<Project> GetProjects(ICollection<string> projectNames)
        {
            var projects = manager.Projects
                .Where(p => projectNames.Count == 0 || projectNames.Contains(p.Value.ProjectInSolution.ProjectName))
                .Select(p => p.Value);

            foreach (var analyzer in projects)
            {
                yield return analyzer.AddToWorkspace(workspace);
            }
        }
    }
}
