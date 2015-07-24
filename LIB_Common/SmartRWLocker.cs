using System;
using System.Threading;

namespace LIB_Common
{
    /// <summary>
    /// SmartRWLocker 简化了ReaderWriterLock的使用。通过using来使用Lock方法返回的对象，如：using(this.smartLocker.Lock(AccessMode.Read)){...}
    /// </summary>   
    public class SmartRWLocker
    {
        private readonly ReaderWriterLock readerWriterLock = new ReaderWriterLock();

        #region LastRequireReadTime
        private DateTime lastRequireReadTime = DateTime.Now;
        public DateTime LastRequireReadTime
        {
            get { return lastRequireReadTime; }
        }
        #endregion

        #region LastRequireWriteTime
        private DateTime lastRequireWriteTime = DateTime.Now;
        public DateTime LastRequireWriteTime
        {
            get { return lastRequireWriteTime; }
        }
        #endregion

        #region Lock
        public LockingObject Lock(AccessMode accessMode)
        {
            if (accessMode == AccessMode.Read)
            {
                lastRequireReadTime = DateTime.Now;
            }
            else
            {
                lastRequireWriteTime = DateTime.Now;
            }

            return new LockingObject(readerWriterLock, accessMode);
        }
        #endregion
    }

    /// <summary>
    /// AccessMode 访问锁定资源的方式。
    /// </summary>
    public enum AccessMode
    {
        Read  = 0,
        Write = 1
    }
}
