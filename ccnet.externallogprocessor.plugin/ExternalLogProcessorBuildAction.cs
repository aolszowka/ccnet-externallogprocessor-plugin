// -----------------------------------------------------------------------
// <copyright file="ExternalLogProcessorBuildAction.cs" company="Ace Olszowka">
// Copyright (c) 2018 Ace Olszowka.
// </copyright>
// -----------------------------------------------------------------------

namespace ccnet.externallogprocessor.plugin
{
    using System;
    using System.Diagnostics;
    using Exortech.NetReflector;
    using ThoughtWorks.CruiseControl.WebDashboard.Dashboard;
    using ThoughtWorks.CruiseControl.WebDashboard.IO;
    using ThoughtWorks.CruiseControl.WebDashboard.MVC;
    using ThoughtWorks.CruiseControl.WebDashboard.MVC.Cruise;

    /// <summary>
    /// A CruiseControl.NET Build Action that will pass the entire contents
    /// of the build log as Standard Input to a specified program and then
    /// take the output of that program and display it in the dashboard.
    /// </summary>
    [ReflectorType("externalLogProcessorBuildAction")]
    public class ExternalLogProcessorBuildAction : ICruiseAction, IConditionalGetFingerprintProvider
    {
        private readonly IFingerprintFactory fingerprintFactory;
        private readonly IBuildRetriever buildRetriever;

        public ExternalLogProcessorBuildAction(IBuildRetriever buildRetriever, IFingerprintFactory fingerprintFactory)
        {
            this.fingerprintFactory = fingerprintFactory;
            this.buildRetriever = buildRetriever;
        }

        /// <summary>
        /// Gets or sets the fully qualified path to the external log processor application.
        /// </summary>
        [ReflectorProperty("externalLogProcessor")]
        public string ExternalLogProcessor { get; set; }
            = string.Empty;

        /// <summary>
        /// Gets or sets the arguments to pass to the external log processor application.
        /// </summary>
        [ReflectorProperty("arguments", Required = false)]
        public string Arguments { get; set; }
            = string.Empty;

        /// <summary>
        /// Grabs the associated log then executes the specified External
        /// Program passing the log as Standard Input. The Standard Output
        /// is then pushed back as an HtmlFragment.
        /// </summary>
        /// <param name="cruiseRequest">The Cruise Request.</param>
        /// <returns><seealso cref="HtmlFragmentResponse"/> with the output.</returns>
        public IResponse Execute(ICruiseRequest cruiseRequest)
        {
            string log = buildRetriever.GetBuild(cruiseRequest.BuildSpecifier).Log;

            string result = "Failed To Execute External Program";

            try
            {
                using (Process externalProgram = new Process())
                {
                    externalProgram.StartInfo.FileName = ExternalLogProcessor;
                    externalProgram.StartInfo.Arguments = Arguments;
                    externalProgram.StartInfo.UseShellExecute = false;
                    externalProgram.StartInfo.RedirectStandardInput = true;
                    externalProgram.StartInfo.RedirectStandardOutput = true;

                    externalProgram.Start();
                    externalProgram.StandardInput.Write(log);
                    externalProgram.StandardInput.Close();
                    result = externalProgram.StandardOutput.ReadToEnd();

                    externalProgram.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                // If we fail for any reason just toally bail out
                result = string.Format("Exception Encountered When Executing Program: {0}", ex.Message);
            }

            return new HtmlFragmentResponse(result);
        }

        // <HACK>
        // I really don't know what this does; I suspect its something to do
        // with telling CruiseControl.NET how to handle caching, but its
        // its unclear. I do know its needed for HtmlFragmentResponse to work
        // though (it needs the IConditionalGetFingerPrintProvider interface)
        // </HACK>
        public ConditionalGetFingerprint GetFingerprint(IRequest request)
        {
            return fingerprintFactory.BuildFromDate(DateTime.Now);
        }
    }
}
