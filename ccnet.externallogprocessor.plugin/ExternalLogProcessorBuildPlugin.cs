// -----------------------------------------------------------------------
// <copyright file="ExternalLogProcssorBuildPlugin.cs" company="Ace Olszowka">
// Copyright (c) 2018 Ace Olszowka.
// </copyright>
// -----------------------------------------------------------------------

namespace ccnet.externallogprocessor.plugin
{
    using Exortech.NetReflector;
    using ThoughtWorks.CruiseControl.WebDashboard.Dashboard;
    using ThoughtWorks.CruiseControl.WebDashboard.Dashboard.GenericPlugins;

    /// <summary>
    /// A CruiseControl.NET Build Plugin that will pass the entire contents
    /// of the build log as Standard Input to a specified program and then
    /// take the output of that program and display it in the dashboard.
    /// </summary>
    [ReflectorType("externalLogProcessorBuildPlugin")]
    public class ExternalLogProcessorBuildPlugin : ProjectConfigurableBuildPlugin
    {
        private readonly IActionInstantiator actionInstantiator;

        public ExternalLogProcessorBuildPlugin(IActionInstantiator actionInstantiator)
        {
            this.actionInstantiator = actionInstantiator;
        }

        /// <summary>
        /// Gets or sets the property that will be used as the Link Description in CruiseControl.NET
        /// </summary>
        [ReflectorProperty("description")]
        public string ConfiguredLinkDescription { get; set; }
            = "No Description Configured. Check your dashboard.config";

        /// <summary>
        /// Gets or sets the name of the action (which will be the Link Name) in CruiseControl.NET
        /// </summary>
        [ReflectorProperty("actionName")]
        public string ActionName { get; set; }
            = "NoActionSetCheckDashboardConfig";

        /// <summary>
        /// Gets or sets the fully qualified path to the external log processor application.
        /// </summary>
        [ReflectorProperty("externalLogProcessor")]
        public string ExternalLogProcessor { get; set; }
            = string.Empty;

        public override INamedAction[] NamedActions
        {
            get
            {
                ExternalLogProcessorBuildAction action = (ExternalLogProcessorBuildAction)actionInstantiator.InstantiateAction(typeof(ExternalLogProcessorBuildAction));
                action.ExternalLogProcessor = ExternalLogProcessor;
                return new INamedAction[] { new ImmutableNamedAction(ActionName, action) };
            }
        }

        /// <summary>
        /// Gets the Link Description that will be displayed by CruiseControl.NET. See <see cref="ConfiguredLinkDescription"/>.
        /// </summary>
        public override string LinkDescription
        {
            get { return ConfiguredLinkDescription; }
        }
    }
}
