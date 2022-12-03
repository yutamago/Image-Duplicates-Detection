namespace ImageDuplicatesDetection.interfaces;

public interface IPhotoComparisonService
{
    /// <summary>
    /// Compares two images for similarity and decides whether they are equal.
    /// </summary>
    /// <param name="filePathImageA">Full file path to image A.</param>
    /// <param name="filePathImageB">Full file path to image B.</param>
    /// <returns>Returns true, if the images are considered equal.</returns>
    Task<bool> CompareAsync(string filePathImageA, string filePathImageB);
}