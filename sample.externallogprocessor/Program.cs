// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Ace Olszowka">
// Copyright (c) 2018 Ace Olszowka.
// </copyright>
// -----------------------------------------------------------------------

namespace sample.externallogprocessor
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;

    /// <summary>
    /// This Sample Program shows how you can externally process the CruiseControl.NET Log
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            // Be aware that any errors won't show up to an end user unless
            // you write it out to Standard Out!
            try
            {
                StringBuilder htmlFragment = new StringBuilder();

                // You can also have args passed in
                if (args.Any())
                {
                    foreach (string arg in args)
                    {
                        htmlFragment.AppendLine($"<p>Argument: {arg}</p>");
                    }
                }

                // Read the Log off the Standard Input into a XDocument
                string xmlLogString = Console.In.ReadToEnd();
                XDocument ccnetLog = XDocument.Parse(xmlLogString);

                // Now Grab the Intergration Label and Status
                string intergrationLabel = ccnetLog.Descendants("CCNetLabel").FirstOrDefault()?.Value;
                string intergrationStatus = ccnetLog.Descendants("CCNetIntegrationStatus").FirstOrDefault()?.Value;

                // Print out an XML Fragment to the Console with our Result
                htmlFragment.AppendLine($"Last Intergration Label <b>{intergrationLabel}</b> Status was <b>{intergrationStatus} </b>");

                Console.WriteLine(htmlFragment);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong parsing the log externally. {ex.ToString()}");
            }
        }
    }
}
