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

            var format = "https://{0}{1}.scm.s1.umbraco.io/scm/info";

            var serializer = new JavaScriptSerializer();
            var usernameEscaped = username.Replace("@", "%40");

            Console.WriteLine();
            Console.WriteLine("Discovering SCM info for environments...");
            Console.WriteLine();

            var tmpGitUrls = new Dictionary<string, string>();

            foreach (var environment in environments)
            {
                try
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

                                var uri = new UriBuilder(scmInfo.GitUrl)
                                {
                                    UserName = usernameEscaped,
                                    Password = password
                                };
                                var gitUrl = uri.Uri.ToString();

                                if (environment.Key == "dev")
                                {
                                    Console.WriteLine("Cloning Git repo for '{0}' environment:\r\n{1}", environment.Key, scmInfo.GitUrl);
                                    git(workingDirectory, "clone {0} {1}", gitUrl, ".");

                                    Console.WriteLine("Swapping Git repo 'origin' with '{0}' remote", environment.Key);
                                    git(workingDirectory, "remote rename {0} {1}", "origin", environment.Key);

                                    success = true;
                                    continue;
                                }

                                Console.WriteLine("Adding Git repo remote for '{0}' environment:\r\n{1}", environment.Key, scmInfo.GitUrl);
                                git(workingDirectory, "remote add {0} {1}", environment.Key, gitUrl);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\r\n==============================");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("==============================\r\n");
                    continue;
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
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(stdout);
            Console.ResetColor();

            p.WaitForExit();
        }
    }
}