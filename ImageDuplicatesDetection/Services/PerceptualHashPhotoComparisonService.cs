using ImageDuplicatesDetection.interfaces;

namespace ImageDuplicatesDetection.Services;

public class PerceptualHashPhotoComparisonService : IPhotoComparisonService
{
    public Task<bool> CompareAsync(string filePathImageA, string filePathImageB)
    {
        throw new NotImplementedException();
    }
}