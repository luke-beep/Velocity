using Windows.Storage;
using Windows.System;

namespace Velocity.Helpers;

public static class FileExtension
{
    public static async Task OpenFolder(string path)
    {
        var folder = await StorageFolder.GetFolderFromPathAsync(path);
        if (folder != null)
        {
            await Launcher.LaunchFolderAsync(folder);
        }
    }

    public static async Task OpenFile(string path)
    {
        try
        {
            var file = await StorageFile.GetFileFromPathAsync(path);
            if (file != null)
            {
                await Launcher.LaunchFileAsync(file);
            }
        }
        catch (Exception)
        {
            await OpenFolder(path);
            throw;
        }
    }
}