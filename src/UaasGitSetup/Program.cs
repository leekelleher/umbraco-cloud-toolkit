using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;

namespace UaasGitSetup
{
	class Program
	{
		const int EXITCODE_ERROR = 2;

		class ScmInfo
		{
			public int Type { get; set; }
			public string GitUrl { get; set; }
		}

		static void Main(string[] args)
		{
			var success = false;

			Console.Write("Enter your UaaS project name: ");
			var projectName = Console.ReadLine();

			if (string.IsNullOrWhiteSpace(projectName))
				Environment.Exit(EXITCODE_ERROR);

			Console.Write("Enter your UaaS username: ");
			var username = Console.ReadLine();

			if (string.IsNullOrWhiteSpace(username))
				Environment.Exit(EXITCODE_ERROR);

			Console.Write("Enter your UaaS password: ");
			var password = Console.ReadLine();

			if (string.IsNullOrWhiteSpace(password))
				Environment.Exit(EXITCODE_ERROR);

			var workingDirectory = Path.Combine(Directory.GetCurrentDirectory(), projectName);
			if (!Directory.Exists(workingDirectory))
				Directory.CreateDirectory(workingDirectory);

			var environments = new Dictionary<string, string>()
			{
				{ "dev", "dev-" },
				{ "stage", "stage-" },
				{ "live", "" }
			};
			var format = "https://{0}{1}.scm.umbraco.io/scm/info";
			var serializer = new JavaScriptSerializer();

			Console.WriteLine();
			Console.WriteLine("Discovering SCM info for environments...");
			Console.WriteLine();

			var tmpGitUrls = new Dictionary<string, string>();

			foreach (var environment in environments)
			{
				using (var client = new WebClient() { Credentials = new NetworkCredential(username, password) })
				{
					var url = string.Format(format, environment.Value, projectName);

					Console.WriteLine("Downloading SCM info for '{0}' environment...", environment.Key);

					var response = client.DownloadString(url);

					if (!string.IsNullOrWhiteSpace(response))
					{
						var scmInfo = serializer.Deserialize<ScmInfo>(response);

						if (scmInfo != null)
						{
							tmpGitUrls.Add(environment.Key, scmInfo.GitUrl);

							if (environment.Key == "dev")
							{
								git(workingDirectory, "clone {0} {1}", scmInfo.GitUrl, ".");
								Console.WriteLine("Cloning Git repo for '{0}' environment:\r\n{1}", environment.Key, scmInfo.GitUrl);

								Console.WriteLine("Swapping Git repo 'origin' with '{0}' remote", environment.Key);
								git(workingDirectory, "remote rename {0} {1}", "origin", environment.Key);

								success = true;
								continue;
							}

							git(workingDirectory, "remote add {0} {1}", environment.Key, scmInfo.GitUrl);
							Console.WriteLine("Adding Git repo remote for '{0}' environment:\r\n{1}", environment.Key, scmInfo.GitUrl);
						}
					}
				}

				Console.WriteLine();
			}

			Console.WriteLine("Cloned Git repo success? {0}", success);

			if (success)
			{
				Console.WriteLine("Fetching latest commits from all Git repo environments");
				git(workingDirectory, "fetch {0}", "--all");
			}

			foreach (var tmpGitUrl in tmpGitUrls)
			{
				Console.WriteLine("Updating Git repo remote URL for '{0}'", tmpGitUrl.Key);
				git(workingDirectory, "remote set-url {0} {1}", tmpGitUrl.Key, tmpGitUrl.Value);
			}

			// Console.Read();
		}

		static void git(string workingDirectory, string format, params string[] args)
		{
			if (string.IsNullOrWhiteSpace(workingDirectory))
				workingDirectory = Directory.GetCurrentDirectory();

			RunCommandLine(workingDirectory, "git.exe", format, args);
		}

		static void RunCommandLine(string workingDirectory, string exe, string format, params string[] args)
		{
			if (string.IsNullOrWhiteSpace(workingDirectory))
				workingDirectory = Directory.GetCurrentDirectory();

			var psi = new ProcessStartInfo(exe)
			{
				Arguments = string.Format(format, args),
				WorkingDirectory = workingDirectory,
				UseShellExecute = false,
				RedirectStandardOutput = true,
				CreateNoWindow = true
			};

			var p = Process.Start(psi);
			if (p == null)
				throw new Exception(string.Format("Fatal error: failed to lauch \"{0} {1}\"!", exe, psi.Arguments));

			var stdout = p.StandardOutput.ReadToEnd();

			// TODO: [LK] Add a verbose option?
			System.Console.ForegroundColor = ConsoleColor.DarkYellow;
			System.Console.WriteLine(stdout);
			System.Console.ResetColor();

			p.WaitForExit();
		}
	}
}