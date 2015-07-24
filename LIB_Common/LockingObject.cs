using System;
using System.Threading;

namespace LIB_Common
{
    /// <summary>
    /// LockingObject SmartRWLocker的Lock方法返回的锁对象。仅仅通过using来使用该对象，如：using(this.smartLocker.Lock(AccessMode.Read)){...}
    /// </summary>
    public class LockingObject : IDisposable
    {
        private ReaderWriterLock readerWriterLock;
        private AccessMode accessMode = AccessMode.Read;

        #region Ctor
        public LockingObject(ReaderWriterLock _lock, AccessMode _lockMode)
        {
            readerWriterLock = _lock;
            accessMode = _lockMode;

            if (accessMode == AccessMode.Read)
            {
                readerWriterLock.AcquireReaderLock(-1);
            }
            else
            {
                readerWriterLock.AcquireWriterLock(-1);
            }
        }
        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            if (accessMode == AccessMode.Read)
            {
                readerWriterLock.ReleaseReaderLock();
            }
            else
            {
                readerWriterLock.ReleaseWriterLock();
            }
        }

        #endregion
    }
}
