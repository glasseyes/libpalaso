﻿using System;
using SIL.WritingSystems;
using System.IO;



//TODO 1: Set up teamcity build configurations
// a: copy of libpalaso using glasseyes github, and with LanguageData.exe artifact
// b: check source files - run LanguageData -c, fail if new files, artifact LastModified.txt
// c: pushbutton to run LanguageData -g, artifact LanguageData.txt to be used by libpalaso
// then set up dependencies (a) on latest build of (c) and (c) on latest build of (a)
//TODO 2: Write tests for LanguageData and classes

namespace LanguageData
{
    class Program
    {
		static int Main(string[] args)
        {
			var options = new Options();
			var isValid = CommandLine.Parser.Default.ParseArgumentsStrict (args, options);
			//Console.WriteLine ("Parsing is valid: {0}", isValid);
			if (isValid)
			{
				// consume Options instance properties
				if (options.ShowHelp)
				{
					Console.WriteLine(options.GetUsage ());
					return 0;
				}
				if (String.IsNullOrEmpty(options.InputDir))
				{
					options.InputDir = Path.Combine ("..", "..", "SIL.WritingSystems", "Resources");
				}
				if (!Directory.Exists(options.InputDir))
				{
					Console.WriteLine ("Input directory does not exist");
					return 1;
				}
				else if (!File.Exists(Path.Combine(options.InputDir, "LanguageIndex.txt")) ||
					!File.Exists(Path.Combine(options.InputDir, "ianaSubtagRegistry.txt")) ||
					!File.Exists(Path.Combine(options.InputDir, "TwoToThreeCodes.txt")))
				{
					Console.WriteLine ("Input directory does not contain the source files LanguageIndex.txt, ianaSubtagRegistry.txt and TwoToThreeCodes.txt");
					return 1;
				}
				if ((options.OutputFile != "LanguageDataIndex.txt") && File.Exists (options.OutputFile))
				{
					Console.WriteLine ("The file {0} already exists.", options.OutputFile);
					return 1;
				}
				if (options.Verbose)
				{
					Console.WriteLine("Input directory: {0}", options.InputDir);
					Console.WriteLine("Output file: {0}", options.OutputFile);
					Console.WriteLine("Getting new files: {0}", options.GetFresh);
				}

			}
			else
			{
				// Display the default usage information
				Console.WriteLine("command line parsing failed");
				Console.WriteLine(options.GetUsage());
				return 1;
			}

            GetAndCheckSources getcheck = new GetAndCheckSources ();
			getcheck.GetOldSources (options.InputDir);
			if (options.GetFresh || options.CheckFresh)
			{
				if (!getcheck.GetNewSources ())
				{
					Console.WriteLine("Failed to download files - aborting");
					return 2;
				}
				bool newfiles = getcheck.CheckSourcesAreDifferent ();
				if (newfiles)
				{
					getcheck.WriteNewFiles (".");
					if (options.CheckFresh)
					{
						return 99;
					}
				} 
			}
			if (!options.CheckFresh) {
                Sldr.Initialize(true);
                LanguageIndex langIndex = new LanguageIndex (getcheck.GetFileStrings (options.GetFresh));
				langIndex.WriteIndex (options.OutputFile);
                Sldr.Cleanup();
            }
			return 0;
        }
    }
}
