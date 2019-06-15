using System.Collections.Generic;
using Typewriter.Configuration;

namespace Typewriter.CodeModel.Configuration
{
    public class SettingsImpl : Settings
    {
        //private readonly Document _projectItem;

        public SettingsImpl()
        {
            //_projectItem = projectItem;
        }

        private List<string> _includedProjects;

        public override Settings IncludeProject(string projectName)
        {
            if (_includedProjects == null)
                _includedProjects = new List<string>();

            _includedProjects.Add(projectName);
            //ProjectHelpers.AddProject(_projectItem, _includedProjects, projectName);
            return this;
        }

        public override Settings IncludeReferencedProjects()
        {
            if (_includedProjects == null)
                _includedProjects = new List<string>();

            //ProjectHelpers.AddReferencedProjects(_includedProjects, _projectItem);
            return this;
        }

        public override Settings IncludeCurrentProject()
        {
            if (_includedProjects == null)
                _includedProjects = new List<string>();

            //ProjectHelpers.AddCurrentProject(_includedProjects, _projectItem);
            return this;
        }

        public override Settings IncludeAllProjects()
        {
            if (_includedProjects == null)
                _includedProjects = new List<string>();

            //ProjectHelpers.AddAllProjects(_projectItem, _includedProjects);
            return this;
        }

        public ICollection<string> IncludedProjects
        {
            get
            {
                if (_includedProjects == null)
                {
                    IncludeCurrentProject();
                    IncludeReferencedProjects();
                }

                return _includedProjects;
            }
        }
    }
}
