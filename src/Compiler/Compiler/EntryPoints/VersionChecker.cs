extern alias wpf;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DotNetForHtml5.Compiler
{
    //[LoadInSeparateAppDomain]
    //[Serializable]
    public class VersionChecker : Task // AppDomainIsolatedTask
    {
        //Context for the note below: When changing versions of CSHTML5 or OpenSilver (and possibly in some other scenarios related to having multiple instances of VS at the same time), msbuild sometimes forgets to update the compiler dll it uses for compilation but still uses the correct .targets file, which leads to errors like "The required parameter XXX was not set for the Task YYY", which are too blurry for the users to understand that they need to restart VS or kill msbuild.
        //Note: IsVersionChecked below is required if msbuild uses a version of the compiler that dates after the VersionChecker was added while using older Targets file:
        //      Since the .targets file won't call the VersionChecker, we need to use a Task that already existed and was called to see if the versionChecker was called, so if IsVersionChecked is true there, it means that we did call VersionChecker; otherwise, it means we didn't call it and should cause an error (I arbitrarily picked BeforeXamlPreprocessor).
        [Output]
        public bool IsVersionChecked { get; set; }

        [Required]
        public string TargetsVersion { get; set; }

        public override bool Execute()
        {
            return Execute(TargetsVersion, new LoggerThatUsesTaskOutput(this));
        }

        public bool Execute(string targetsVersion, ILogger logger)
        {
            string operationName = "C#/XAML for HTML5: VersionChecker";
            try
            {
                using (var executionTimeMeasuring = new ExecutionTimeMeasuring())
                {
                    //------- DISPLAY THE PROGRESS -------
                    logger.WriteMessage(operationName + " started.");

                    string compilerVersion = VersionInformation.GetPackageIdAndVersion();
                    bool isSuccess = targetsVersion == compilerVersion;
                   
                    if(!isSuccess)
                    {
                        throw new Exception("PLEASE RESTART VISUAL STUDIO TO FIX THESE COMPILATION ERRORS (Alternatively, you may kill the 'msbuild.exe' process without restarting Visual Studio).");
                    }

                    //------- DISPLAY THE PROGRESS -------
                    logger.WriteMessage(operationName + (isSuccess ? " completed in " + executionTimeMeasuring.StopAndGetTimeInSeconds() + " seconds." : " failed."));
                    IsVersionChecked = isSuccess;
                    return isSuccess;
                }
            }
            catch (Exception ex)
            {
                logger.WriteError(operationName + " failed: " + ex.ToString());
                return false;
            }
        }
    }
}
