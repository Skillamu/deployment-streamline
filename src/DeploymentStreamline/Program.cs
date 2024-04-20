using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace DeploymentStreamline
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Already logged into Azure from the CLI? (y/n): ");
            var input = Console.ReadKey(true).Key.ToString().ToLower();
            Console.WriteLine(input);

            while (input != "y" && input != "n")
            {
                Console.Clear();
                Console.Write("Already logged into Azure from the CLI? (y/n): ");
                input = Console.ReadKey(true).Key.ToString().ToLower();
                Console.WriteLine(input);
            }

            if (input == "n")
            {
                var azLoginProcess = new Process();
                azLoginProcess.StartInfo.FileName = "cmd.exe";
                azLoginProcess.StartInfo.Arguments = $"/c az login";
                azLoginProcess.StartInfo.RedirectStandardOutput = true;

                azLoginProcess.Start();

                var azLoginProcessOutput = azLoginProcess.StandardOutput.ReadToEnd();

                azLoginProcess.WaitForExit();

                Console.WriteLine("\n\n" + azLoginProcessOutput);
            }

            Console.Write("Absolute path to the deployment config file to use: ");
            var filePath = Console.ReadLine();

            var deploymentConfigJson = File.ReadAllText(filePath);
            Console.WriteLine(deploymentConfigJson);

            var deploymentConfig = JsonSerializer.Deserialize<DeploymentConfig>(deploymentConfigJson);

            var deploymentNameSuffix = Regex.Replace(DateTime.Now.ToLocalTime().ToString(), "[^0-9]", "");

            var command = $"az deployment group create --name {deploymentConfig.Name}-{deploymentNameSuffix} --resource-group {deploymentConfig.ResourceGroup} --template-file {deploymentConfig.TemplateFile} --parameters {deploymentConfig.ParametersFile}";

            var directoryPath = Path.GetDirectoryName(filePath);

            var process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = $"/c cd {directoryPath}&{command}";
            process.StartInfo.RedirectStandardOutput = true;

            process.Start();

            var processOutput = process.StandardOutput.ReadToEnd();

            process.WaitForExit();

            Console.WriteLine(processOutput);

            Console.WriteLine("Press any key to close the window...");
            Console.ReadKey(true);
        }
    }
}
