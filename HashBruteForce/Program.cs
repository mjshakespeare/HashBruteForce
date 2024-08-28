using System.CommandLine;
using HashBruteForce;

var panPrefixOption = new Option<string>(
    "--panprefix",
    "The PAN prefix to use.");
        
var panLengthOption = new Option<int>(
    "--panlength",
    getDefaultValue: () => 14,
    "The length of the PAN.");
        
var targetHashOption = new Option<string>(
    "--targethash",
    "The target hash value.");
       
var rootCommand = new RootCommand
{
    panPrefixOption,
    panLengthOption,
    targetHashOption
};
        
rootCommand.Description
    = "A simple CLI tool to handle PAN parameters.";

rootCommand.SetHandler(
    (panPrefix, panLength, targetHash) =>
    {
        using var hashAttemptPool
            = new PanHashCracker();

        var hashBytes
            = Convert
                .FromBase64String(
                    targetHash);
        
        var hash
            = hashAttemptPool
                .CrackHash(
                    panPrefix,
                    panLength,
                    hashBytes);

        Console.WriteLine(hash);
    }, 
    panPrefixOption,
    panLengthOption,
    targetHashOption);

rootCommand.Invoke(args);