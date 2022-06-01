// See https://aka.ms/new-console-template for more information
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;

#region Static variables
string subDir;
Bitmap currentImage = null;
Bitmap previousImage = null;
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
/// Used to Save the ScreenShot
/// </summary>
void SaveScreenShot()
{
    try
    {
        currentImage = new Bitmap(1920, 1080);
        using (var g = Graphics.FromImage(currentImage))
        {
            g.CopyFromScreen(0, 0, 0, 0,
            currentImage.Size, CopyPixelOperation.SourceCopy);
        }
        string str = DateTime.Now.ToString().Replace(' ', '_').Replace('/', '_').Replace(':', '_');
        string screenshotName = string.Format("{0}_{1}.png", "Screenshot", str);
        if (previousImage== null || (previousImage!=null && !CompareMemCmp(currentImage, previousImage)))
        {
            currentImage.Save(subDir + "/" + screenshotName, ImageFormat.Png);
            previousImage = currentImage;
            i++;
            Console.WriteLine("Number of images captured:{0}", i);
        }   
    }
    catch (Exception ex)
    {
        throw;
    }
}

[DllImport("msvcrt.dll")]
static extern int memcmp(IntPtr b1, IntPtr b2, long count);
 bool CompareMemCmp(Bitmap b1, Bitmap b2)
{
    if ((b1 == null) != (b2 == null)) return false;
    if (b1.Size != b2.Size) return false;

    var bd1 = b1.LockBits(new Rectangle(new Point(0, 0), b1.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
    var bd2 = b2.LockBits(new Rectangle(new Point(0, 0), b2.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

    try
    {
        IntPtr bd1scan0 = bd1.Scan0;
        IntPtr bd2scan0 = bd2.Scan0;

        int stride = bd1.Stride;
        int len = stride * b1.Height;

        return memcmp(bd1scan0, bd2scan0, len) == 0;
    }
    finally
    {
        b1.UnlockBits(bd1);
        b2.UnlockBits(bd2);
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