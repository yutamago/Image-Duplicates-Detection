using System.Collections;
using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using ImageDuplicatesDetection.interfaces;

namespace ImageDuplicatesDetection.Services;

public class AverageHashPhotoComparisonService : IPhotoComparisonService
{
    
    public async Task<bool> CompareAsync(string filePathImageA, string filePathImageB)
    {
        var imageABytes = await File.ReadAllBytesAsync(filePathImageA);
        var imageBBytes = await File.ReadAllBytesAsync(filePathImageB);

        using var imageAStream = new MemoryStream(imageABytes);
        using var imageBStream = new MemoryStream(imageBBytes);
        
        var imageABmp = BitmapFactory.FromStream(imageAStream);
        var imageBBmp = BitmapFactory.FromStream(imageBStream);

        var hashA = CalculateHash(imageABmp);
        var hashB = CalculateHash(imageBBmp);

        for (int i = 0; i < 64; i++)
        {
            if (hashA[i] != hashB[i])
            {
                return false;
            }
        }

        return true;
    }

    private static BitArray CalculateHash(WriteableBitmap? bitmap)
    {
        // Convert to Greyscale
        bitmap = bitmap.Gray();
        
        // Scale down to 8x8
        bitmap = bitmap.Resize(8, 8, WriteableBitmapExtensions.Interpolation.NearestNeighbor);
        
        // Get average gray value
        long sum = 0;
        bitmap.ForEach((_, _, color) =>
        {
            sum += color.R;
            return color;
        });
        var average = (int)(sum / 64);
        
        // If the gray value is larger than the average, a 1 is added to the hash, otherwise a 0.
        var hash = new BitArray(64);
        bitmap.ForEach((x, y, color) =>
        {
            hash[x + (y * 8)] = color.R > average;
            
            return color;
        });
        
        return hash;
    }
}