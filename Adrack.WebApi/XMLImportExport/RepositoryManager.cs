using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Adrack.WebApi.XMLImportExport
{


    public class FoundEvent
    {
        private FileSystemInfo m_info;
        public FoundEvent(FileSystemInfo info)
        {
            m_info = info;
        }
        public FileSystemInfo Info
        {
            get { return m_info; }
        }
    }

    public class ThreadEvent
    {
        private Boolean m_success;
        private String m_errorMsg;


        public ThreadEvent(Boolean success, String errorMsg)
        {
            m_success = success;
            m_errorMsg = errorMsg;
        }


        public Boolean Success
        {
            get { return m_success; }
        }

        public String ErrorMsg
        {
            get { return m_errorMsg; }
        }
    }

    public class RepositoryManager
    {
        public static int FILE_NOT_FOUND=0;
        public static int FILE_LOCKED=1;
        public static int FILE_INVALID=2;
        public static int FILE_PRESENT=3;

        public delegate void FoundInfoEventHandler(FoundEvent e);
        public static event FoundInfoEventHandler FoundInfo;

        public delegate void ThreadEndedEventHandler(ThreadEvent e);
        public static event ThreadEndedEventHandler ThreadEnded;

        private static Thread m_thread = null;
        private static Boolean m_stop = false;
        private static RepositoryParams m_pars = null;
        private static Byte[] m_containingBytes = null;


        // ----- Public Methods -----

        public static Boolean Start(RepositoryParams pars)
        {
            Boolean success = false;

            if (m_thread == null)
            {
                // Perform a reset of all variables,
                // to ensure that the state of the searcher is the same on every new start:
                ResetVariables();

                // Remember the parameters:
                m_pars = pars;

                // Start searching for FileSystemInfos that match the parameters:
                m_thread = new Thread(new ThreadStart(SearchThread));
                m_thread.Start();

                success = true;
            }

            return success;
        }

        public static void Stop()
        {
            // Stop the thread by setting a flag:
            m_stop = true;
        }


        // ----- Private Methods -----

        private static void ResetVariables()
        {
            m_thread = null;
            m_stop = false;
            m_pars = null;
            m_containingBytes = null;
        }

        private static void SearchThread()
        {
            Boolean success = true;
            String errorMsg = "";

            // Search for FileSystemInfos that match the parameters:
            if ((m_pars.SearchDir.Length >= 3) && (Directory.Exists(m_pars.SearchDir)))
            {
                if (m_pars.FileNames.Count > 0)
                {
                    // Convert the string to search for into bytes if necessary:

                    if (m_pars.ContainingChecked)
                    {
                        if (m_pars.ContainingText != "")
                        {
                            try
                            {
                                m_containingBytes = m_pars.Encoding.GetBytes(m_pars.ContainingText);
                            }
                            catch (Exception)
                            {
                                success = false;
                                errorMsg = "The string\r\n" + m_pars.ContainingText + "\r\ncannot be converted into bytes.";
                            }
                        }
                        else
                        {
                            success = false;
                            errorMsg = "The string to search for must not be empty.";
                        }
                    }

                    if (success)
                    {
                        // Get the directory info for the search directory:
                        DirectoryInfo dirInfo = null;
                        try
                        {
                            dirInfo = new DirectoryInfo(m_pars.SearchDir);
                        }
                        catch (Exception ex)
                        {
                            success = false;
                            errorMsg = ex.Message;
                        }

                        if (success)
                        {
                            // Search the directory (maybe recursively),
                            // and raise events if something was found:
                            SearchDirectory(dirInfo);
                        }
                    }
                }
                else
                {
                    success = false;
                    errorMsg = "Please enter one or more filenames to search for.";
                }
            }
            else
            {
                success = false;
                errorMsg = "The directory\r\n" + m_pars.SearchDir + "\r\ndoes not exist.";
            }

            // Remember the thread has ended:
            m_thread = null;

            // Raise an event:
            if (ThreadEnded != null)
            {
                ThreadEnded(new ThreadEvent(success, errorMsg));
            }
        }

        private static void SearchDirectory(DirectoryInfo dirInfo)
        {
            if (!m_stop)
            {
                try
                {
                    foreach (String fileName in m_pars.FileNames)
                    {
                        FileSystemInfo[] infos = dirInfo.GetFileSystemInfos(fileName);

                        foreach (FileSystemInfo info in infos)
                        {
                            if (m_stop)
                            {
                                break;
                            }

                            if (MatchesRestrictions(info))
                            {
                                // We have found a matching FileSystemInfo, so let's raise an event:
                                if (FoundInfo != null)
                                {
                                    FoundInfo(new FoundEvent(info));
                                }
                            }
                        }
                    }

                    if (m_pars.IncludeSubDirsChecked)
                    {
                        DirectoryInfo[] subDirInfos = dirInfo.GetDirectories();
                        foreach (DirectoryInfo subDirInfo in subDirInfos)
                        {
                            if (m_stop)
                            {
                                break;
                            }

                            // Recursion:
                            SearchDirectory(subDirInfo);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private static Boolean MatchesRestrictions(FileSystemInfo info)
        {
            Boolean matches = true;

            if (matches && m_pars.NewerThanChecked)
            {
                matches = (info.LastWriteTime >= m_pars.NewerThanDateTime);
            }

            if (matches && m_pars.OlderThanChecked)
            {
                matches = (info.LastWriteTime <= m_pars.OlderThanDateTime);
            }

            if (matches && m_pars.ContainingChecked)
            {
                matches = false;
                if (info is FileInfo)
                {
                    matches = FileContainsBytes(info.FullName, m_containingBytes);
                }
            }

            return matches;
        }

        public static int checkServerRepositoryItem(string directory, string hash, long size)
        {
            string path = directory + "\\" + hash;
            FileInfo info = new FileInfo(path);
            if (!info.Exists) return FILE_NOT_FOUND;
            if (IsFileLocked(info)) return FILE_LOCKED;
            if (info.Length != size)
                return FILE_INVALID;
            return FILE_PRESENT;
        }

        /*public static List<RepositoryItem> getRepositoryItems(string directory)
        {
            List<RepositoryItem> list = new List<RepositoryItem>();
            getRepositoryItemsRec(directory, list);
            return list;
        }*/

        public static List<RepositoryItem> getRepositoryItems(PluginFileInfo[] infoArray)
        {
            List<RepositoryItem> list = new List<RepositoryItem>();
            for (int i = 0; i < infoArray.Length; i++)
            {
                RepositoryItem item = new RepositoryItem(infoArray[i]);
                list.Add(item);
            }
            return list;
        }


        
        protected static bool IsFileLocked(FileInfo file)
        {
            
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }


       /* static void getRepositoryItemsRec(string directory,List<RepositoryItem> list)
        {
            DirectoryInfo info = new DirectoryInfo(directory);
            FileInfo [] files=info.GetFiles();

            
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Length>0)
                    if (!IsFileLocked(files[i]))
                        list.Add(new RepositoryItem(files[i]));
            }

            DirectoryInfo[] dirs = info.GetDirectories();
            for (int i = 0; i < dirs.Length; i++)
            {
                getRepositoryItemsRec(dirs[i].FullName, list);
            }
            
        }*/


        private static Boolean FileContainsBytes(String path, Byte[] compare)
        {
            Boolean contains = false;

            Int32 blockSize = 4096;
            if ((compare.Length >= 1) && (compare.Length <= blockSize))
            {
                Byte[] block = new Byte[compare.Length - 1 + blockSize];

                try
                {
                    FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);

                    // Read the first bytes from the file into "block":
                    Int32 bytesRead = fs.Read(block, 0, block.Length);

                    do
                    {
                        // Search "block" for the sequence "compare":
                        Int32 endPos = bytesRead - compare.Length + 1;
                        for (Int32 i = 0; i < endPos; i++)
                        {
                            // Read "compare.Length" bytes at position "i" from the buffer,
                            // and compare them with "compare":
                            Int32 j;
                            for (j = 0; j < compare.Length; j++)
                            {
                                if (block[i + j] != compare[j])
                                {
                                    break;
                                }
                            }

                            if (j == compare.Length)
                            {
                                // "block" contains the sequence "compare":
                                contains = true;
                                break;
                            }
                        }

                        // Search completed?
                        if (contains || (fs.Position >= fs.Length))
                        {
                            break;
                        }
                        else
                        {
                            // Copy the last "compare.Length - 1" bytes to the beginning of "block":
                            for (Int32 i = 0; i < (compare.Length - 1); i++)
                            {
                                block[i] = block[blockSize + i];
                            }

                            // Read the next "blockSize" bytes into "block":
                            bytesRead = compare.Length - 1 + fs.Read(block, compare.Length - 1, blockSize);
                        }
                    }
                    while (!m_stop);

                    fs.Close();
                }
                catch (Exception)
                {
                }
            }

            return contains;
        }
 
    }
}
