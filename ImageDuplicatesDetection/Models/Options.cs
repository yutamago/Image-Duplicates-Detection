namespace ImageDuplicatesDetection;

public struct Options
{
    public string? FileA = null;
    public string? FileB = null;
    public string Algorithm = nameof(AlgorithmOptions.Average);

    public Options()
    {
    }
}