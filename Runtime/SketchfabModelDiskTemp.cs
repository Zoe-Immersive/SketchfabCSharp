using System;
using System.IO;
using System.Threading;

public class SketchfabModelDiskTemp
{
    public string AbsolutePath { get; private set; }
    public float FlushIntervalSeconds { get; private set; }

    private SemaphoreSlim m_TempDirectoryLock = new SemaphoreSlim(1, 1);

    private Timer m_Timer;

    public SketchfabModelDiskTemp(string _diskTempDirectoryPath, float _flushIntervalSeconds)
    {
        AbsolutePath = _diskTempDirectoryPath;
        FlushIntervalSeconds = _flushIntervalSeconds;

        if (!Directory.Exists(AbsolutePath))
        {
            Directory.CreateDirectory(AbsolutePath);
        }

        Flush();

        m_Timer = new Timer(OnTimerTimeout, null, 0, (int)(FlushIntervalSeconds * 1000.0));
    }

    public void Lock()
    {
        m_TempDirectoryLock.Wait();
    }

    public void Unlock()
    {
        m_TempDirectoryLock.Release();
    }

    private void OnTimerTimeout(object stateInfo)
    {
        m_TempDirectoryLock.Wait();

        try
        {
            Flush();
        }
        catch (Exception _exception)
        {
            throw _exception;
        }
        finally
        {
            m_TempDirectoryLock.Release();
        }
    }

    private void Flush()
    {
        string[] tempDirectories = Directory.GetDirectories(AbsolutePath);
        if (tempDirectories.Length != 0)
        {
            foreach (string tempDirectoryPath in tempDirectories)
            {
                Directory.Delete(tempDirectoryPath, true);
            }
        }
    }
}
