using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class SketchfabModelDiskCache
{
    private class DirectoryInfoCreationTimeUtcComparer : IComparer<DirectoryInfo>
    {
        public int Compare(DirectoryInfo x, DirectoryInfo y)
        {
            return x.CreationTimeUtc.CompareTo(y.CreationTimeUtc);
        }
    }

    public string AbsolutePath { get; private set; }
    public long MaxByteSize { get; private set; }
    public long CurrentByteSize { get; private set; }

    public SortedSet<DirectoryInfo> CachedDirectoryInfos { get; private set; }

    private DirectoryInfo m_CacheDirectoryInfo;

    private SemaphoreSlim m_CacheDirectoryLock = new SemaphoreSlim(1, 1);

    public SketchfabModelDiskCache(string _diskCacheDirectoryPath, long _maxCacheByteSize)
    {
        AbsolutePath = _diskCacheDirectoryPath;
        MaxByteSize = _maxCacheByteSize;

        if (!Directory.Exists(AbsolutePath))
        {
            m_CacheDirectoryInfo = Directory.CreateDirectory(AbsolutePath);
        }
        else
        {
            m_CacheDirectoryInfo = new DirectoryInfo(AbsolutePath);
        }

        CachedDirectoryInfos = new SortedSet<DirectoryInfo>(m_CacheDirectoryInfo.GetDirectories(), new DirectoryInfoCreationTimeUtcComparer());

        foreach (DirectoryInfo directoryInfo in CachedDirectoryInfos)
        {
            CurrentByteSize += directoryInfo.Length();
        }

        if (CurrentByteSize > MaxByteSize)
        {
            MakeSpace(CurrentByteSize - MaxByteSize);
        }
    }

    public Task<bool> IsInCache(string _uid)
    {
        return Task.Run(() =>
        {
            m_CacheDirectoryLock.Wait();

            try
            {
                foreach (DirectoryInfo directoryInfo in CachedDirectoryInfos)
                {
                    if (directoryInfo.Name == _uid)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception _exception)
            {
                throw _exception;
            }
            finally
            {
                m_CacheDirectoryLock.Release();
            }
        });
    }

    public Task<SketchfabModel> GetCachedModelMetadata(string _uid)
    {
        return Task.Run(() =>
        {
            try
            {
                m_CacheDirectoryLock.Wait();
                string metadataPath = Path.Combine(m_CacheDirectoryInfo.FullName, _uid, string.Format("{0}_metadata.json", _uid));
                return SketchfabModel.FromJson(File.ReadAllText(metadataPath));
            }
            catch (Exception _exception)
            {
                return null;
            }
            finally
            {
                m_CacheDirectoryLock.Release();
            }
        });
    }

    public void CacheModelMetadata(SketchfabModel _model)
    {
        try
        {
            m_CacheDirectoryLock.Wait();
            string path = Path.Combine(m_CacheDirectoryInfo.FullName, _model.Uid, string.Format("{0}_metadata.json", _model.Uid));
            File.WriteAllText(path, _model.GetJsonString());
        }
        catch (Exception _exception)
        {
            Debug.LogError(_exception);
        }
        finally
        {
            m_CacheDirectoryLock.Release();
        }
    }

    public Task<bool> IsInCache(SketchfabModel _model)
    {
        return IsInCache(_model.Uid);
    }

    public Task AddToCache(DirectoryInfo _modelDirectoryInfo)
    {
        return Task.Run(async () =>
        {
            m_CacheDirectoryLock.Wait();

            try
            {
                long length = await _modelDirectoryInfo.AsyncLength();

                if (length > MaxByteSize)
                {
                    return;
                }

                if (!CachedDirectoryInfos.Contains(_modelDirectoryInfo))
                {
                    long expectedByteSize = CurrentByteSize + length;
                    if (expectedByteSize > MaxByteSize)
                    {
                        MakeSpace(expectedByteSize - MaxByteSize);
                    }

                    CurrentByteSize += length;
                }
                else
                {
                    Directory.Delete(Path.Combine(m_CacheDirectoryInfo.FullName, _modelDirectoryInfo.Name), true);
                }
                DirectoryCopy(_modelDirectoryInfo.FullName, Path.Combine(m_CacheDirectoryInfo.FullName, _modelDirectoryInfo.Name), true);
                CachedDirectoryInfos.Add(_modelDirectoryInfo);
            }
            catch (Exception _exception)
            {
                throw _exception;
            }
            finally
            {
                m_CacheDirectoryLock.Release();
            }
        });
    }

    private void MakeSpace(long _spaceToFree)
    {
        long spaceFreed = 0;
        while (spaceFreed < _spaceToFree &&
            CachedDirectoryInfos.Count > 0)
        {
            DirectoryInfo directoryInfo = CachedDirectoryInfos.First();

            CachedDirectoryInfos.Remove(directoryInfo);

            long directorySize = directoryInfo.Length();
            spaceFreed += directorySize;
            CurrentByteSize -= directorySize;

            Directory.Delete(directoryInfo.FullName, true);
        }
    }

    // This function has been copied and unaltered from
    // https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
    private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
    {
        // Get the subdirectories for the specified directory.
        DirectoryInfo dir = new DirectoryInfo(sourceDirName);

        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException(
                "Source directory does not exist or could not be found: "
                + sourceDirName);
        }

        DirectoryInfo[] dirs = dir.GetDirectories();

        // If the destination directory doesn't exist, create it.
        Directory.CreateDirectory(destDirName);

        // Get the files in the directory and copy them to the new location.
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            string tempPath = Path.Combine(destDirName, file.Name);
            // NOTE: true means to averride the files if existing
            file.CopyTo(tempPath, true);
        }

        // If copying subdirectories, copy them and their contents to new location.
        if (copySubDirs)
        {
            foreach (DirectoryInfo subdir in dirs)
            {
                string tempPath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
            }
        }
    }
}
