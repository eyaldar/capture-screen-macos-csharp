using System.Runtime.InteropServices;
using ObjCRuntime;
using ScreenshotMac.Models;

namespace ScreenshotMac;

public static class Interop
{
    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/ApplicationServices")]
    private static extern IntPtr CGWindowListCopyWindowInfo(CGWindowListOption option, uint relativeToWindow);
        
    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/ApplicationServices")]
    public static extern IntPtr CGWindowListCreateImage(CGRect screenBounds, CGWindowListOption windowOption, uint windowId, CGWindowImageOption imageOption);
    
    [DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
    public static extern IntPtr CGDisplayCreateImage(uint displayId);
    
    public static NSArray? GetWindowInfoList(CGWindowListOption option, uint relativeToWindow)
    {
        var ptr = CGWindowListCopyWindowInfo(option, relativeToWindow);
        return Runtime.GetNSObject<NSArray>(ptr);
    }
    
    public static int GetForegroundWindowProcessId()
    {
        var foregroundApp = NSWorkspace.SharedWorkspace.FrontmostApplication;
        return foregroundApp.ProcessIdentifier;
    }

    public static WindowInfo? GetActiveWindowInfo(int processId)
    {
        var windowInfoParser = new WindowInfoParser();
        NSArray? windowInfoList = GetWindowInfoList(CGWindowListOption.OnScreenOnly, 0);

        for (uint i = 0; i < windowInfoList?.Count; i++)
        {
            var window = Runtime.GetNSObject<NSDictionary>(windowInfoList.ValueAt(i));
            var windowInfo = windowInfoParser.FetchWindowData(window);

            if (windowInfo == null)
            {
                continue;
            }

            var winInfo = windowInfo.Value;

            if (processId == winInfo.ProcessId)
            {
                return winInfo;
            }
        }

        return null;
    }
}