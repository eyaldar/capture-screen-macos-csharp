namespace ScreenshotMac.Models;

public readonly record struct WindowInfo(uint WindowNumber, string OwnerName, string WindowName, string ProcessName, CGRect Bounds, int ProcessId);