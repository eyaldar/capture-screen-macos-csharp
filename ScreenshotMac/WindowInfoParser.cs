using System.Diagnostics;
using ScreenshotMac.Models;

namespace ScreenshotMac;

public class WindowInfoParser
{
    public WindowInfo? FetchWindowData(NSDictionary? windowInfoDict)
    {
        if (windowInfoDict == null)
            return null;
        
        var processName = "";
        var processId = 0;
        var windowName = windowInfoDict.ObjectForKey((NSString)"kCGWindowName").ToString() ?? "";
        var windowNumber = uint.Parse(windowInfoDict.ObjectForKey((NSString)"kCGWindowNumber").ToString() ?? "0");
        var ownerName = windowInfoDict.ObjectForKey((NSString)"kCGWindowOwnerName").ToString() ?? "";
        var processIdNumber = windowInfoDict.ObjectForKey((NSString)"kCGWindowOwnerPID") as NSNumber;
        var boundsDict = windowInfoDict.ObjectForKey((NSString)"kCGWindowBounds") as NSDictionary;
        CGRect.TryParse(boundsDict, out var rect);

        if (processIdNumber != null)
        {
            processId = processIdNumber.Int32Value;
            processName = Process.GetProcessById(processId).ProcessName;
        }
        
        return new WindowInfo(windowNumber, ownerName, windowName, processName, rect, processId);
    }
}