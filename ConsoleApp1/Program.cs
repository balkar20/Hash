using System.Diagnostics;
var path = "C:\\Work\\PasswordHashUtility\\PasswordHashUtility\\bin\\Release\\net6.0\\win-x64\\PasswordHashUtility.exe";

var process = new Process
{
    StartInfo = new ProcessStartInfo
    {
        FileName = path,
        Arguments = "VFedorenko Test!123",
        UseShellExecute = false,
        RedirectStandardOutput = true,
        CreateNoWindow = true,
        WindowStyle = ProcessWindowStyle.Hidden,
        RedirectStandardError = true,
        RedirectStandardInput = true
    }
};

process.Start();

while (!process.StandardOutput.EndOfStream)
{
    //get hash
    string hash = process.StandardOutput.ReadLine();

    //Write hash to console
    Console.WriteLine(hash);
}
Console.ReadLine();

