namespace ScreenshotMac
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: ScreenshotMac <mode> <outputPath>");
                return;
            }
            
            var mode = GetCaptureMode(args[0]);
            var outputPath = args[1];
            
            NSApplication.Init();
            
            var screenshotProvider = new ScreenshotProvider();
            
            using var image = screenshotProvider.Capture(mode);
                
            if (image == null)
            {
                Console.WriteLine("Failed to capture active window");
                return;
            }
                        
            SaveScreenshot(image, outputPath);

            NSApplication.SharedApplication.Terminate(NSApplication.SharedApplication);
        }
        
        static CaptureMode GetCaptureMode(string mode)
        {
            return mode switch
            {
                "primary" => CaptureMode.PrimaryScreen,
                "active" => CaptureMode.ActiveWindow,
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode,"Invalid mode specified. Valid modes are 'primary' and 'active'")
            };
        }

        static void SaveScreenshot(CGImage image, string filePath)
        {
            var folderPath = Path.GetDirectoryName(filePath);

            if (folderPath == null) return;
            
            // Ensure the directory exists
            Directory.CreateDirectory(folderPath);

            // Save the image as a PNG file
            using var stream = File.OpenWrite(filePath);
            var properties = new NSDictionary();
            var imageFormat = NSBitmapImageFileType.Png;

            // Create an NSBitmapImageRep from the CGImage
            using var bitmapRep = new NSBitmapImageRep(image);
            // Get the representation data
            var data = bitmapRep.RepresentationUsingTypeProperties(imageFormat, properties);

            // Save the data to the stream
            data.AsStream().CopyTo(stream);
        }
    }
}
