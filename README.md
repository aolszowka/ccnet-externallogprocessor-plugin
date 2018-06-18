# ccnet-externallogprocessor-plugin
A Simple CruiseControl.NET Web Dashboard Plugin To Parse A Log Externally

This plugin will pass the entire CruiseControl.NET Integration Log as Standard Input to the program configured along with the configured arguments.

The external program is then run, and the output of the external application is then pushed back as an HTML Fragment.

Using this plugin you can extend CruiseControl.NET's reporting functionality much more flexibly  instead of having to write your log parsers in .NET 2.0 (or really in .NET at all!)

### Requirements
* This plugin is built for the CruiseControl.NET 1.4.4SP1 Web Dashboard. See the section Hacking on how to upgrade this.
* This plugin is built against .NET 2.0 for maximum compatibility with CruiseControl.NET 1.4.4SP1
* This plugin will run an application local to the Web Dashboard Instance, ensure you understand the implications.

### How To Install
* Grab the source and compile.
    * While this project is targeting .NET 2.0 Visual Studio 2017 was used to develop this, and as such some syntatical sugar options available only in Visual Studio 2017 were used.
* Copy the binary over to the CruiseControl.NET Dashboard installation folder. (Default is ```C:\Program Files (x86)\CruiseControl.NET\webdashboard\bin```)
* Modify your ```dashboard.config``` (Default is ```C:\Program Files (x86)\CruiseControl.NET\webdashboard\dashboard.config```) to add the following to the buildPlugins section

```xml
<externalLogProcessorBuildPlugin
  description="My Sample External Parser"
  actionName="SampleExternalParser"
  externalLogProcessor="C:\Program Files (x86)\CruiseControl.NET\webdashboard\SampleExternalParser\sample.externallogprocessor.exe"
/>
```

Take a peek at the [Sample.dashboard.config](sample.externallogprocessor/Sample.dashboard.config) for more examples including how to pass arguments to the external parser program.

* Restart the IIS AppPool hosting the Dashboard to pick up the configuration change.

Once you've done the above navigate to a build and you will notice on the left hand navigation menu a new link is available, named the same as the ```description``` above. Clicking on it will execute the program and return you the output. In this screenshot the ```sample.externalprocessor.exe``` is being ran.

Futhermore you'll notice that a new aspx action has been assigned, the same as the ```actionName``` defined in the configuration.

### Common Issues
I personally have experienced some really funky behavior with the Web Dashboard while developing this plugin. A lot of the issues I think were due to the fact that there is a lot of caching happening that I do not fully understand. When I rolled this out to "production" I no longer encountered the issues. If you see something, or know whats going on please drop a line in the issues page.

### Hacking
This project is licensed under the MIT License and you are encouraged to fork this project to suit your needs. I also happily take pull requests for functionality that does not require a bump in the version of CruiseControl.NET. Bonus points for including unit tests.

#### Upgrading Supported CruiseControl.NET Version
The most common change will probably be upgrading to a much more recent version of CruiseControl.NET. While I have not tested it and its been a long time since I've actively followed CruiseControl.NET's plugin architecture. It should, in theory, be as simple as pulling the following files from your CruiseControl.NET instance and updating the references in the project:

* NetReflector.dll
* ThoughtWorks.CruiseControl.Core.dll
* ThoughtWorks.CruiseControl.WebDashboard.dll

However this plugin is tightly coupled to a number of CruiseControlNET classes that very well could have changed (IE ```ProjectConfigurableBuildPlugin```). At very least you have an example of something that worked at least back in 1.4.4SP1.


### Known Issues
Check [Issues/known-issue](https://github.com/aolszowka/ccnet-externallogprocessor-plugin/labels/known-issue) for more.


### TODO
Check the [Issues/enhacement](https://github.com/aolszowka/ccnet-externallogprocessor-plugin/labels/enhancement) for more.


## Copyright & License
* ccnet-externallogprocessor-plugin Copyright 2018 Ace Olszowka [MIT License](LICENSE.txt)
* CruiseControl.NET Copyright 2005 ThoughtWorks, Inc [ThoughtWorks Open Source Software License](https://raw.githubusercontent.com/ccnet/CruiseControl.NET/0ced9ffb9f651474dd09a38e756064c8ebd5e220/license.txt)
