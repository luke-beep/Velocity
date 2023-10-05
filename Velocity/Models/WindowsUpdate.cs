namespace Velocity.Models;

public class WindowsUpdate
{
    public string? Title
    {
        get;
        init;
    }

    public string? Description
    {
        get;
        init;
    }

    public bool IsDownloaded
    {
        get;
        set;
    }

    public bool IsInstalled
    {
        get;
        set;
    }
}