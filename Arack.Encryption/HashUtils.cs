// ***********************************************************************
// Assembly         : Arack.Encryption
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="HashUtils.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Arack.Encryption
{
    /// <summary>
    ///     Hashing progress arguments
    ///     Implements the <see cref="EventArgs" />
    /// </summary>
    /// <seealso cref="EventArgs" />
    public class HashingProgressArgs : EventArgs
    {
        /// <summary>
        ///     The hash
        /// </summary>
        public HashAlgorithm hash;

        /// <summary>
        ///     Initializes a new instance of the <see cref="HashingProgressArgs" /> class.
        /// </summary>
        /// <param name="totalBytesRead">The total bytes read.</param>
        /// <param name="size">The size.</param>
        /// <param name="_hash">The hash.</param>
        public HashingProgressArgs(long totalBytesRead, long size, HashAlgorithm _hash)
        {
            TotalBytesRead = totalBytesRead;
            Size = size;
            hash = _hash;
        }

        /// <summary>
        ///     Gets or sets the total bytes read.
        /// </summary>
        /// <value>The total bytes read.</value>
        public long TotalBytesRead { get; set; }

        /// <summary>
        ///     Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        public long Size { get; set; }
    }

    /// <summary>
    ///     Thread based stream hashing algorithm with callback
    /// </summary>
    public class StreamHashing
    {
        /// <summary>
        ///     Delegate HashingProgressHandler
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        public delegate void HashingProgressHandler(object sender, HashingProgressArgs e);

        /// <summary>
        ///     The hash algorithm
        /// </summary>
        protected readonly HashAlgorithm _hashAlgorithm;

        /// <summary>
        ///     The buffer size
        /// </summary>
        protected int _bufferSize = 4096;

        /// <summary>
        ///     The cancel
        /// </summary>
        protected bool _cancel;

        /// <summary>
        ///     The hash
        /// </summary>
        protected byte[] _hash;

        /// <summary>
        ///     Initializes a new instance of the <see cref="StreamHashing" /> class.
        /// </summary>
        /// <param name="hashAlgorithm">The hash algorithm.</param>
        public StreamHashing(HashAlgorithm hashAlgorithm)
        {
            _hashAlgorithm = hashAlgorithm;
        }

        /// <summary>
        ///     Gets or sets the size of the buffer.
        /// </summary>
        /// <value>The size of the buffer.</value>
        public int BufferSize
        {
            get { return _bufferSize; }
            set { _bufferSize = value; }
        }

        /// <summary>
        ///     Gets the hash.
        /// </summary>
        /// <value>The hash.</value>
        public byte[] Hash => _hash;

        /// <summary>
        ///     Occurs when [file hashing progress].
        /// </summary>
        public event HashingProgressHandler FileHashingProgress;

        /// <summary>
        ///     Computes the hash.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>System.Byte[].</returns>
        public byte[] ComputeHash(Stream stream)
        {
            _cancel = false;
            _hash = null;
            var bufferSize = _bufferSize; // this makes it impossible to change the buffer size while computing

            long totalBytesRead = 0;

            var size = stream.Length;
            var readAheadBuffer = new byte[bufferSize];
            var readAheadBytesRead = stream.Read(readAheadBuffer, 0, readAheadBuffer.Length);

            totalBytesRead += readAheadBytesRead;

            do
            {
                var bytesRead = readAheadBytesRead;
                var buffer = readAheadBuffer;

                readAheadBuffer = new byte[bufferSize];
                readAheadBytesRead = stream.Read(readAheadBuffer, 0, readAheadBuffer.Length);

                totalBytesRead += readAheadBytesRead;

                if (readAheadBytesRead == 0)
                    _hashAlgorithm.TransformFinalBlock(buffer, 0, bytesRead);
                else
                    _hashAlgorithm.TransformBlock(buffer, 0, bytesRead, buffer, 0);
                if (FileHashingProgress != null)
                    FileHashingProgress(this, new HashingProgressArgs(totalBytesRead, size, _hashAlgorithm));
            } while (readAheadBytesRead != 0 && !_cancel);

            if (FileHashingProgress != null)
                FileHashingProgress(this, new HashingProgressArgs(totalBytesRead + 1, size, _hashAlgorithm));

            if (_cancel) return _hash = null;

            return _hash = _hashAlgorithm.Hash;
        }

        /// <summary>
        ///     Cancels this instance.
        /// </summary>
        public virtual void Cancel()
        {
            _cancel = true;
        }

        /// <summary>
        ///     Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            var hashText = new StringBuilder();
            for (var i = 0; i < Hash.Length; i++) hashText.Append(Hash[i].ToString("x2").ToLower());
            return hashText.ToString();
        }
    }

    /// <summary>
    ///     Thread based file hashing algorithm with callback
    /// </summary>
    public class FileHashing
    {
        /// <summary>
        ///     Delegate HashingProgressHandler
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        public delegate void HashingProgressHandler(object sender, HashingProgressArgs e);

        /// <summary>
        ///     The buffer size
        /// </summary>
        public static int bufferSize = 65535;

        /// <summary>
        ///     The terminate all
        /// </summary>
        public static bool terminateAll = false;

        /// <summary>
        ///     The cancel
        /// </summary>
        protected bool cancel;

        /// <summary>
        ///     The hash
        /// </summary>
        protected byte[] hash;

        /// <summary>
        ///     The hash algorithm
        /// </summary>
        protected HashAlgorithm hashAlgorithm;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FileHashing" /> class.
        /// </summary>
        /// <param name="hashAlgorithm">The hash algorithm.</param>
        public FileHashing(HashAlgorithm hashAlgorithm)
        {
            this.hashAlgorithm = hashAlgorithm;
        }

        /// <summary>
        ///     Gets the hash.
        /// </summary>
        /// <value>The hash.</value>
        public byte[] Hash => hash;

        /// <summary>
        ///     Occurs when [file hashing progress].
        /// </summary>
        public event HashingProgressHandler FileHashingProgress;

        /// <summary>
        ///     Computes the hash.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>System.Byte[].</returns>
        public byte[] ComputeHash(Stream stream)
        {
            try
            {
                cancel = false;
                hash = null;
                var _bufferSize = bufferSize; // this makes it impossible to change the buffer size while computing

                byte[] readAheadBuffer, buffer;
                int readAheadBytesRead, bytesRead;
                long size, totalBytesRead = 0;

                size = stream.Length;
                readAheadBuffer = new byte[_bufferSize];
                readAheadBytesRead = stream.Read(readAheadBuffer, 0, readAheadBuffer.Length);

                totalBytesRead += readAheadBytesRead;

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
                        FileHashingProgress(this, new HashingProgressArgs(totalBytesRead, size, hashAlgorithm));
                    /*counter++;
                    if (counter % 3==0)
                    {
                        Thread.Sleep(1);
                    }*/
                } while (readAheadBytesRead != 0 && !cancel);

                if (FileHashingProgress != null)
                    FileHashingProgress(this, new HashingProgressArgs(totalBytesRead + 1, size, hashAlgorithm));
                if (cancel)
                    return hash = null;

                stream.Close();
                return hash = hashAlgorithm.Hash;
            }
            catch (Exception e)
            {
                if (FileHashingProgress != null)
                    FileHashingProgress(null, new HashingProgressArgs(0, 0, hashAlgorithm));

                throw e;
            }
        }

        /// <summary>
        ///     Cancels this instance.
        /// </summary>
        public void Cancel()
        {
            cancel = true;
        }

        /// <summary>
        ///     Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            var hex = "";
            foreach (var b in Hash)
                hex += b.ToString("x2");

            return hex;
        }
    }
}