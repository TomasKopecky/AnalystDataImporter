using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using UtfUnknown;

namespace AnalystDataImporter.Utilities
{
    public static class EncodingDetector
    {
        public static Encoding DetectEncoding(string filePath, int sampleSize = 4096) // Default sample size is 4KB
        {
            try
            {
                byte[] buffer = new byte[sampleSize];
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    fileStream.Read(buffer, 0, sampleSize);
                }

                // Detect encoding from bytes
                DetectionResult result = CharsetDetector.DetectFromBytes(buffer);

                // Get the best detection result
                DetectionDetail resultDetected = result.Detected;

                if (resultDetected != null && resultDetected.Encoding != null)
                {
                    return resultDetected.Encoding;
                }
                else
                {
                    Debug.WriteLine("Encoding detection failed, defaulting to UTF-8");
                    return null;
                }
            }
            catch (IOException ex)
            {
                Debug.WriteLine($"IO Exception: {ex.Message}");
                return null; // Default to UTF-8 in case of exception
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"General Exception: {ex.Message}");
                return null; // Default to UTF-8 in case of exception
            }
        }
    }
}
