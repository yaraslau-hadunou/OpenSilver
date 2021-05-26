using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using OpenSilver.TemplateConfiguration;
using System.Collections.Generic;

namespace OpenSilver.TemplateWizards
{
    class AppCustomizationWizard : IWizard
    {
        public void BeforeOpeningFile(ProjectItem projectItem)
        {
            
        }

        public void ProjectFinishedGenerating(Project project)
        {
            
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
            
        }

        public void RunFinished()
        {
            
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            AppConfigurationWindow window = new AppConfigurationWindow();

            bool? result = window.ShowDialog();
            if (!result.HasValue || !result.Value)
            {
                throw new WizardBackoutException("OpenSilver project creation was cancelled by user");
            }

            replacementsDictionary.Add("$opensilverbuildtype$", window.OpenSilverBuildType);
            replacementsDictionary.Add("$blazorversion$", window.BlazorVersion);
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}
