// See https://aka.ms/new-console-template for more information
using System.Drawing.Imaging;
using System.Drawing;

#region Static variables
string subDir;
int i = 0;
#endregion

subDir = DateTime.Now.ToString("dd-MMM-yyyy");
if (!Directory.Exists(subDir))
{
    Directory.CreateDirectory(subDir);
}
SetInterval(TimeSpan.FromSeconds(5)).ConfigureAwait(false);
Thread.Sleep(TimeSpan.FromMinutes(240));

/// <summary>
/// USed to Save the ScreenSHot
/// </summary>
void SaveScreenShot()
{
    try
    {
        using var bitmap = new Bitmap(1920, 1080);
        using (var g = Graphics.FromImage(bitmap))
        {
            g.CopyFromScreen(0, 0, 0, 0,
            bitmap.Size, CopyPixelOperation.SourceCopy);
        }
        i++;
        string str = DateTime.Now.ToString().Replace(' ', '_').Replace('/', '_').Replace(':', '_');
        string screenshotName = string.Format("{0}_{1}.png", "Screenshot", str);
        bitmap.Save(subDir + "/" + screenshotName, ImageFormat.Png);       
        Console.WriteLine("Number of images captured:{0}", i);
    }
    catch (Exception ex)
    {
        throw;
    }
}


/// <summary>
/// Similar to Windows SetTimeOut Function
/// </summary>
async Task SetInterval(TimeSpan timeout)
{
    await Task.Delay(timeout).ConfigureAwait(false);
    SaveScreenShot();
    await SetInterval(timeout).ConfigureAwait(false); ;
}