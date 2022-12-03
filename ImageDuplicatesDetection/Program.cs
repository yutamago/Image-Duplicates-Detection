// See https://aka.ms/new-console-template for more information

using System.IO;
using ImageDuplicatesDetection.interfaces;
using ImageDuplicatesDetection.Services;

namespace ImageDuplicatesDetection;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: path1 path2 [algorithm]");
            Console.WriteLine($"Algorithms: {nameof(AlgorithmOptions.Average)},  {nameof(AlgorithmOptions.Median)}");
            return;
        }
        
        var options = GetOptionsFromArgs(args);
        ValidateOptions(options, out var algorithmTyped);

        IPhotoComparisonService photoComparisonService = algorithmTyped switch
        {
            AlgorithmOptions.Median => new MedianHashPhotoComparisonService(),
            _ => new AverageHashPhotoComparisonService(),
        };

        var result = await photoComparisonService.CompareAsync(options.FileA!, options.FileB!);

        Console.WriteLine(result ? "The images are EQUAL." : "The images are DIFFERENT.");
        return;
    }

    private static Options GetOptionsFromArgs(IReadOnlyList<string> args)
    {
        if (args.Count is < 2 or > 3)
        {
            throw new Exception("Illegal number of arguments.");
        }


        var newOptions = new Options();

        if (args.Count is 2 or 3)
        {
            newOptions.FileA = args[0].Trim();
            newOptions.FileB = args[1].Trim();
        }

        if (args.Count is 3)
        {
            newOptions.Algorithm = args[2].Trim();
        }

        return newOptions;
    }


    private static void ValidateOptions(Options options, out AlgorithmOptions typedAlgorithm)
    {
        if (!File.Exists(options.FileA))
        {
            throw new ArgumentException($"The given path to File A is incorrect: {options.FileA}");
        }

        if (!File.Exists(options.FileB))
        {
            throw new ArgumentException($"The given path to File B is incorrect: {options.FileB}");
        }

        if (!Enum.TryParse(typeof(AlgorithmOptions), options.Algorithm, true, out var algorithmParsed))
        {
            throw new ArgumentException($"The given algorithm is not available: {options.Algorithm}");
        }

        typedAlgorithm = (AlgorithmOptions)algorithmParsed!;
    }
}