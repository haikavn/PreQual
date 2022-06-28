using Adrack.DataUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Adrack.WebApi.XMLImportExport
{
    public class PluginFileInfo
    {
        public FileInfo info;
        public string FullName;
        public string pluginInfo;
        public string metadata;
        public string startDate;
        public string endDate;
        public int utcHoursDifference;
    }

    public class RepositoryItem
    {

 
        public static int ITEM_READY = 0;
        public static int ITEM_HASHING = 1;
        public static int ITEM_SENDING = 2;
        public static int ITEM_FINALIZED = 2;

        long hashOperationId;
        int hashComponentId;

       
        string path="";
        string hash = "";

        int state = 0;

        public string metadata;
        public string pluginInfo;
        public string pluginStartDate;
        public string pluginEndDate;
        public int hoursDiff;

        public string displayName = "";
        FileInfo fileInfo = null;

        Object attachedObject=null;

        public void setAttachedObject(Object aobject)
        {
            attachedObject=aobject;
        }

        public Object getAttachedObject()
        {
            return attachedObject;
        }

        public string getHash()
        {
            return hash;
        }

        public int getState()
        {
            //lock (this)
            {
                return state;
            }
        }

        public void setState(int _state)
        {
            //lock (this)
            {
                state = _state;
            }
        }

        public bool isBusy()
        {
            return state != 0;
        }

        public virtual string getItemPath()
        {
            return path;
        }

        public virtual string getItemName()
        {
            return fileInfo.Name;
        }

        public RepositoryItem()
        {
        }
        public RepositoryItem(PluginFileInfo info)
        {
            this.fileInfo = info.info;
            this.path = info.info.FullName;
            this.pluginInfo = info.pluginInfo;
            this.metadata = info.metadata;
            this.pluginStartDate = info.startDate;
            this.pluginEndDate = info.endDate;
            this.hoursDiff = info.utcHoursDifference;
        }

        public RepositoryItem(string path)
        {
            this.fileInfo = new FileInfo(path);
            this.path = path;
            
        }

        public bool Exists()
        {
            return File.Exists(getItemPath());
        }

        public string getItemDisplayPath()
        {
            if (displayName != "")
                return displayName;
            return getItemPath();
        }
        public string getOutputFileName()
        {
            return hash + fileInfo.Extension;
        }
        int percentOld = 0;
        bool isServer = false;
        private void FileHashingProgressHandler(object sender, FileHashingProgressArgs e)
        {
            if (sender == null)
            {
                
                
                setState(ITEM_READY);
                return;
            }

            int percent = (int)Math.Round((double)e.TotalBytesRead / (double)e.Size * 10);
            
            if (e.TotalBytesRead > e.Size)
            {                
                byte[] hashBytes = e.hash.Hash;
                string hex = "";
                foreach (byte b in hashBytes)
                hex += b.ToString("x2");
                hash = hex;
                
                
                setState(ITEM_READY);
            }
                    
            percentOld = percent;
        }
        
        public virtual void calcItemHash(int componentId, bool async=true)
        {
            //starts repository item hashing
            isServer = false;
            hashComponentId = componentId;
            //SHA1.Create
            ASyncFileHashAlgorithm hasher = new ASyncFileHashAlgorithm(SHA1.Create());
            
            Thread t=DMUtils.HashFileAsync(hasher, path, FileHashingProgressHandler);
            if (t!=null)
            {

                if (isServer)
                    hashOperationId = 0;
                else
                    hashOperationId = 1;
                setState(ITEM_HASHING);
                t.Start();

                if (!async)
                    t.Join();
            }
         
            
        }

        

        public long getItemSize()
        {
            return fileInfo.Length;
        }

    }
}
