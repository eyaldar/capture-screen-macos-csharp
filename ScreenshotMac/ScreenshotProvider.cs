using ObjCRuntime;
using ScreenshotMac.Models;

namespace ScreenshotMac;

public enum CaptureMode
{
    PrimaryScreen,
    ActiveWindow
}

public class ScreenshotProvider
{
    public CGImage? CaptureScreen(int screenIndex)
    {
        var screen = NSScreen.Screens[0];
        var mainDisplayId = GetDisplayId(screen);
        IntPtr imagePtr = Interop.CGDisplayCreateImage(mainDisplayId);
        var img = Runtime.GetINativeObject<CGImage>(imagePtr, false);

        return img;
    }

    public CGImage? CaptureWindow(WindowInfo windowInfo)
    {
        IntPtr imageRef = Interop.CGWindowListCreateImage(
            windowInfo.Bounds, 
            CGWindowListOption.IncludingWindow, 
            windowInfo.WindowNumber,
            CGWindowImageOption.Default);
        var image = Runtime.GetINativeObject<CGImage>(imageRef, false);
        return image;
    }
    
    public CGImage? Capture(CaptureMode mode)
    {
        switch (mode)
        {
            case CaptureMode.PrimaryScreen:
                return CaptureScreen(0);
            case CaptureMode.ActiveWindow:
                return CaptureActiveWindow();
            default:
                throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
        }
    }
    
    public CGImage? CaptureActiveWindow()
    {
        var activeProcessId = Interop.GetForegroundWindowProcessId();
        var nullableWinInfo = Interop.GetActiveWindowInfo(activeProcessId);

        if (nullableWinInfo == null)
        {
            Console.WriteLine($"Failed to get active window info for process {activeProcessId} - skipping");
            return null;
        }
        
        var winInfo = nullableWinInfo.Value;
        var image = CaptureWindow(winInfo);

        return image;
    }
    
    private uint GetDisplayId(NSScreen screen)
    {
        var screenDictionary = screen.DeviceDescription;
        var screenId = screenDictionary.ObjectForKey(new NSString("NSScreenNumber")) as NSNumber;
        return screenId?.UInt32Value ?? 0;
    }
}