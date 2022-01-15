using System.IO;
using System.Linq;
using System.Threading.Tasks;

public static class DirectoryInfoExtensions
{
    public static long Length(this DirectoryInfo _directoryInfo)
    {
        return _directoryInfo.GetFiles("*", SearchOption.AllDirectories).Sum(file => file.Length);
    }

    public static Task<long> AsyncLength(this DirectoryInfo _directoryInfo)
    {
        return  Task.Run(() => _directoryInfo.GetFiles("*", SearchOption.AllDirectories).Sum(file => file.Length));
    }
}
