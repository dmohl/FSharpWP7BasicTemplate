using System;
using System.Collections.Generic;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using VSLangProj;
using System.Diagnostics;

namespace TemplateWizard
{
    public class TemplateWizard : IWizard
    {
        protected DTE Dte { get; set; }
        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            Dte = (DTE) automationObject;
        }

        public void ProjectFinishedGenerating(Project project)
        {
            try
            {
                Project appProject = null;
                Project appHostProject = null;
                for (int i = 1; i <= Dte.Solution.Projects.Count; i++)
                {
                    switch (Dte.Solution.Projects.Item(i).Name)
                    {
                        case "App":
                            appProject = Dte.Solution.Projects.Item(i);
                            break;
                        case "AppHost":
                            appHostProject = Dte.Solution.Projects.Item(i);
                            break;
                    }
                }

                if (appHostProject != null && appProject != null)
                {
                    ((VSProject)appHostProject.Object).References.AddProject(appProject);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Error: {0}\r\nStack Trace: {1}", ex.Message, ex.StackTrace));
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
