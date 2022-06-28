using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Principal;
using System.Security.Cryptography;
using System.Threading;


using System.Net.NetworkInformation;


namespace Adrack.DataUtils
{
    public class AsyncStreamHashAlgorithm
    {
        protected readonly HashAlgorithm _hashAlgorithm;
        protected byte[] _hash;
        protected bool _cancel;
        protected int _bufferSize = 4096;
        public delegate void FileHashingProgressHandler(object sender, FileHashingProgressArgs e);
        public event FileHashingProgressHandler FileHashingProgress = null;

        public AsyncStreamHashAlgorithm(HashAlgorithm hashAlgorithm)
        {
            _hashAlgorithm = hashAlgorithm;
        }

        public byte[] ComputeHash(Stream stream)
        {
            _cancel = false;
            _hash = null;
            int bufferSize = _bufferSize; // this makes it impossible to change the buffer size while computing

            long totalBytesRead = 0;

            long size = stream.Length;
            byte[] readAheadBuffer = new byte[bufferSize];
            int readAheadBytesRead = stream.Read(readAheadBuffer, 0, readAheadBuffer.Length);

            totalBytesRead += readAheadBytesRead;

            do
            {
                int bytesRead = readAheadBytesRead;
                byte[] buffer = readAheadBuffer;

                readAheadBuffer = new byte[bufferSize];
                readAheadBytesRead = stream.Read(readAheadBuffer, 0, readAheadBuffer.Length);

                totalBytesRead += readAheadBytesRead;

                if (readAheadBytesRead == 0)
                    _hashAlgorithm.TransformFinalBlock(buffer, 0, bytesRead);
                else
                    _hashAlgorithm.TransformBlock(buffer, 0, bytesRead, buffer, 0);
                if (FileHashingProgress != null)
                    FileHashingProgress(this, new FileHashingProgressArgs(totalBytesRead, size, _hashAlgorithm));
            } while (readAheadBytesRead != 0 && !_cancel);

            if (FileHashingProgress != null)
                FileHashingProgress(this, new FileHashingProgressArgs(totalBytesRead + 1, size, _hashAlgorithm));

            if (_cancel)
            {
                return _hash = null;
            }

            return _hash = _hashAlgorithm.Hash;
        }

        public int BufferSize
        {
            get { return _bufferSize; }
            set { _bufferSize = value; }
        }

        public byte[] Hash
        {
            get { return _hash; }
        }

        public virtual void Cancel()
        {
            _cancel = true;
        }

        public override string ToString()
        {
            StringBuilder hashText = new StringBuilder();
            for (int i = 0; i < Hash.Length; i++)
            {
                hashText.Append(Hash[i].ToString("x2").ToLower());
            }
            return hashText.ToString();
        }
    }

    public class FileHashingProgressArgs : EventArgs
    {
        public long TotalBytesRead { get; set; }
        public long Size { get; set; }
        public HashAlgorithm hash;

        public FileHashingProgressArgs(long totalBytesRead, long size, HashAlgorithm _hash)
        {
            TotalBytesRead = totalBytesRead;
            Size = size;
            hash = _hash;
        }
    }


    public class ASyncFileHashAlgorithm
    {
        protected HashAlgorithm hashAlgorithm;
        protected byte[] hash;
        protected bool cancel = false;
        public static int bufferSize = 65535;
        public delegate void FileHashingProgressHandler(object sender, FileHashingProgressArgs e);
        public event FileHashingProgressHandler FileHashingProgress = null;

        public static bool terminateAll = false;
        public ASyncFileHashAlgorithm(HashAlgorithm hashAlgorithm)
        {
            this.hashAlgorithm = hashAlgorithm;
        }


        public byte[] ComputeHash(Stream stream)
        {
            try
            {
                cancel = false;
                hash = null;
                int _bufferSize = bufferSize; // this makes it impossible to change the buffer size while computing

                byte[] readAheadBuffer, buffer;
                int readAheadBytesRead, bytesRead;
                long size, totalBytesRead = 0;

                size = stream.Length;
                readAheadBuffer = new byte[_bufferSize];
                readAheadBytesRead = stream.Read(readAheadBuffer, 0, readAheadBuffer.Length);

                totalBytesRead += readAheadBytesRead;
                int counter = 0;
                do
                {
                    if (terminateAll) return null;
                    bytesRead = readAheadBytesRead;
                    buffer = readAheadBuffer;

                    readAheadBuffer = new byte[_bufferSize];
                    readAheadBytesRead = stream.Read(readAheadBuffer, 0, readAheadBuffer.Length);

                    totalBytesRead += readAheadBytesRead;

                    if (readAheadBytesRead == 0)
                        hashAlgorithm.TransformFinalBlock(buffer, 0, bytesRead);
                    else
                        hashAlgorithm.TransformBlock(buffer, 0, bytesRead, buffer, 0);

                    if (FileHashingProgress != null)
                        FileHashingProgress(this, new FileHashingProgressArgs(totalBytesRead, size, hashAlgorithm));
                    /*counter++;
                    if (counter % 3==0)
                    {
                        Thread.Sleep(1);
                    }*/

                } while (readAheadBytesRead != 0 && !cancel);

                if (FileHashingProgress != null)
                    FileHashingProgress(this, new FileHashingProgressArgs(totalBytesRead + 1, size, hashAlgorithm));
                if (cancel)
                    return hash = null;

                stream.Close();
                return hash = hashAlgorithm.Hash;
            }
            catch (Exception e)
            {

                if (FileHashingProgress != null)
                    FileHashingProgress(null, new FileHashingProgressArgs(0, 0, hashAlgorithm));

                throw e;

            }
        }



        public byte[] Hash
        {
            get
            { return hash; }
        }

        public void Cancel()
        {
            cancel = true;
        }

        public override string ToString()
        {
            string hex = "";
            foreach (byte b in Hash)
                hex += b.ToString("x2");

            return hex;
        }
    }


    public class DMUtils
    {

        
        public static Thread HashFileAsync(ASyncFileHashAlgorithm hasher, string fileName, ASyncFileHashAlgorithm.FileHashingProgressHandler handler)
        {

            try
            {
                Stream stream = (Stream)File.Open(fileName, FileMode.Open, FileAccess.ReadWrite);

                hasher.FileHashingProgress += handler;

                Thread t = new Thread(
                    delegate () { hasher.ComputeHash(stream); }
                );
                t.Priority = ThreadPriority.Lowest;
                //t.Start();
                return t;
            }
            catch (Exception e)
            {
                
                return null;
            }

        }

        public static byte[] HashFile2(string fileName)
        {
            //  Console.Write("Starting...");
            Stream stream = (Stream)File.Open(fileName, FileMode.Open, FileAccess.ReadWrite);

            //hasher.FileHashingProgress += OnFileHashingProgress;
            ASyncFileHashAlgorithm hasher = new ASyncFileHashAlgorithm(System.Security.Cryptography.SHA1CryptoServiceProvider.Create());
            return hasher.ComputeHash(stream);

        }

        static Object debugLock = new Object();

        static bool debugLogInWriting = false;
        public static void DebugLog(string msg)
        {
            

        }

        
        public static void ErrorLog(string msg)
        {
            DebugLog("ERROR:" + msg);
        }

        
        private static string GetMacAddress()
        {
            const int MIN_MAC_ADDR_LENGTH = 12;
            string macAddress = string.Empty;
            long maxSpeed = -1;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                DebugLog(
                    "Found MAC Address: " + nic.GetPhysicalAddress() +
                    " Type: " + nic.NetworkInterfaceType);

                string tempMac = nic.GetPhysicalAddress().ToString();
                if (nic.Speed > maxSpeed &&
                    !string.IsNullOrEmpty(tempMac) &&
                    tempMac.Length >= MIN_MAC_ADDR_LENGTH)
                {
                    DebugLog("New Max Speed = " + nic.Speed + ", MAC: " + tempMac);
                    maxSpeed = nic.Speed;
                    macAddress = tempMac;
                }
            }

            return macAddress;
        }
        

        
        public static void DebugLogTicket(string message, string ticket)
        {

            if (ticket.Length > 1000)
                ticket = ticket.Substring(0, 1000) + "...";
            DebugLog(message + " - " + ticket);

        }

      
        public static void NotifyLogClient(int compId, int messageId, string message, params Object[] erParams)
        {
        
        }

        public static void WarningLogClient(int compId, int messageId, string message, params Object[] erParams)
        {
          
        }


        public static void DebugLogClientMsg(int compId, int eventCode, string message, params Object[] erParams)
        {
          
        }


        public static string GetParams(params Object[] erParams)
        {
            string[] paramsStr = { };
            if (erParams == null) erParams = paramsStr;

            string paramsString = "";

            StringBuilder sb = new StringBuilder();

            foreach (string str in erParams)
            {
                sb.Append(str).Append(";");
            }

            paramsString = sb.ToString();
            return paramsString;
        }
        


        public static void ErrorLogClient(int compId, int errorCode, string message, params Object[] paramsList)
        {
            
        }





        static SemaphoreSlim lockEntered = new SemaphoreSlim(1);
        static int lockCounter = 0;


        public static bool inLock()
        {
         
            return lockCounter > 0;
        }

        public static void enterLock(string funcName)
        {
         
            lockCounter++;
            //DebugLog("Enter DM wait" + funcName);// + " Lock: "+lockCounter);*/
            lockEntered.Wait(1000);
            //DebugLog("Exit DM wait " + funcName);
        }

        public static void releaseLock(string funcName)
        {
            
            lockCounter--;
            
            lockEntered.Release();
        }
        
        public static bool CheckHost(string hostAddr)
        {
            try
            {
                if (Uri.CheckHostName(hostAddr) != UriHostNameType.Unknown)
                {

                    Dns.GetHostEntry(hostAddr);
                    return true;
                }

            }
            catch
            {
                return false;
            }
            return false;
        }

        public static string LocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    //    break;
                }
            }
            return localIP;
        }

    }
}