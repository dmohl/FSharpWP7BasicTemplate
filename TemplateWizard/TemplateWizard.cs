using System.Collections.Generic;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using VSLangProj;

namespace TemplateWizard
{
    public class TemplateWizard : IWizard
    {
        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
            var currentProject = (VSProject)project.Object;
            if (project.Name == "App")
            {
                var mainReference = ((VSProject)currentProject.DTE.Solution.Item(0));
                mainReference.References.AddProject(project);
            }
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void RunFinished()
        {
        }
    }
}
