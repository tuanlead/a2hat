using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Tools {
    public enum RWLockStatus {
        UNLOCKED,
        READ_LOCK,
        WRITE_LOCK
    }

    /// <summary>
    /// Helps to work with ReaderWriterLock
    /// </summary>
    public class RWLock : IDisposable {
        public delegate ResultType DoWorkFunc<ResultType>();
        public delegate void DoWorkFunc();

        public static int defaultTimeout = 30000;
        private RWLockStatus status = RWLockStatus.UNLOCKED;
        private readonly ReaderWriterLock lockObj;
        private readonly int timeout;
        private LockCookie cookie;
        private bool upgraded = false;

        #region delegate based methods 

        /// <summary>
        /// Runs operation with Writer lock privelege.
        /// </summary>
        /// <typeparam name="ResultType"></typeparam>
        /// <param name="lockObj"></param>
        /// <param name="timeout"></param>
        /// <param name="doWork"></param>
        /// <returns></returns>
        public static ResultType GetWriteLock<ResultType>( ReaderWriterLock lockObj, int timeout, DoWorkFunc<ResultType> doWork ) {
            RWLockStatus status = (lockObj.IsWriterLockHeld ? RWLockStatus.WRITE_LOCK : (lockObj.IsReaderLockHeld ? RWLockStatus.READ_LOCK : RWLockStatus.UNLOCKED));
            LockCookie writeLock = default(LockCookie);

            if ( status == RWLockStatus.READ_LOCK ) {
                writeLock = lockObj.UpgradeToWriterLock( timeout );
            } else if ( status == RWLockStatus.UNLOCKED ) {
                lockObj.AcquireWriterLock( timeout );
            }
            try {
                return doWork();
            } finally {
                if ( status == RWLockStatus.READ_LOCK ) {
                    lockObj.DowngradeFromWriterLock( ref writeLock );
                } else if ( status == RWLockStatus.UNLOCKED ) {
                    lockObj.ReleaseWriterLock();
                }
            }
        }

        /// <summary>
        /// Runs operation with Reader lock privilege.
        /// </summary>
        /// <typeparam name="ResultType"></typeparam>
        /// <param name="lockObj"></param>
        /// <param name="timeout"></param>
        /// <param name="doWork"></param>
        /// <returns></returns>
        public static ResultType GetReadLock<ResultType>( ReaderWriterLock lockObj, int timeout, DoWorkFunc<ResultType> doWork ) {
            bool releaseLock = false;
            if ( !lockObj.IsWriterLockHeld && !lockObj.IsReaderLockHeld ) {
                lockObj.AcquireReaderLock( timeout );
                releaseLock = true;
            }
            try {
                return doWork();
            } finally {
                if ( releaseLock ) {
                    lockObj.ReleaseReaderLock();
                }
            }
        }

        
        /// <summary>
        /// Runs operation with Writer lock privelege.
        /// </summary>
        /// <param name="lockObj"></param>
        /// <param name="timeout"></param>
        /// <param name="doWork"></param>
        /// <returns></returns>
        public static void GetWriteLock( ReaderWriterLock lockObj, int timeout, DoWorkFunc doWork ) {
            RWLockStatus status = (lockObj.IsWriterLockHeld ? RWLockStatus.WRITE_LOCK : (lockObj.IsReaderLockHeld ? RWLockStatus.READ_LOCK : RWLockStatus.UNLOCKED));
            LockCookie writeLock = default(LockCookie);

            if ( status == RWLockStatus.READ_LOCK ) {
                writeLock = lockObj.UpgradeToWriterLock( timeout );
            } else if ( status == RWLockStatus.UNLOCKED ) {
                lockObj.AcquireWriterLock( timeout );
            }
            try {
                doWork();
            } finally {
                if ( status == RWLockStatus.READ_LOCK ) {
                    lockObj.DowngradeFromWriterLock( ref writeLock );
                } else if ( status == RWLockStatus.UNLOCKED ) {
                    lockObj.ReleaseWriterLock();
                }
            }
        }

        /// <summary>
        /// Runs operation with Reader lock privilege.
        /// </summary>
        /// <param name="lockObj"></param>
        /// <param name="timeout"></param>
        /// <param name="doWork"></param>
        /// <returns></returns>
        public static void GetReadLock( ReaderWriterLock lockObj, int timeout, DoWorkFunc doWork ) {
            bool releaseLock = false;
            if ( !lockObj.IsWriterLockHeld && !lockObj.IsReaderLockHeld ) {
                lockObj.AcquireReaderLock( timeout );
                releaseLock = true;
            }
            try {
                doWork();
            } finally {
                if ( releaseLock ) {
                    lockObj.ReleaseReaderLock();
                }
            }
        }

        #endregion

        #region disposable based methods

        public static RWLock GetReadLock( ReaderWriterLock lockObj ) {
            return new RWLock( lockObj, RWLockStatus.READ_LOCK, defaultTimeout );
        }

        public static RWLock GetWriteLock( ReaderWriterLock lockObj ) {
            return new RWLock( lockObj, RWLockStatus.WRITE_LOCK, defaultTimeout );
        }

        public RWLock( ReaderWriterLock lockObj, RWLockStatus status, int timeoutMS ) {
            this.lockObj = lockObj;
            this.timeout = timeoutMS;
            this.Status = status;
        }

        public void Dispose() {
            Status = RWLockStatus.UNLOCKED;
        }

        /// <summary>
        /// 
        /// </summary>
        public RWLockStatus Status {
            get { return status; }
            set {
                if ( status != value ) {
                    if ( status == RWLockStatus.UNLOCKED ) {
                        upgraded = false;
                        if ( value == RWLockStatus.READ_LOCK ) {
                            lockObj.AcquireReaderLock( timeout );
                        } else if ( value == RWLockStatus.WRITE_LOCK ) {
                            lockObj.AcquireWriterLock( timeout );
                        }
                    } else if ( value == RWLockStatus.UNLOCKED ) {
                        lockObj.ReleaseLock();
                    } else if ( value == RWLockStatus.WRITE_LOCK ) { // && status==RWLockStatus.READ_LOCK
                        cookie   = lockObj.UpgradeToWriterLock( timeout );
                        upgraded = true;
                    } else if ( upgraded ) { // value==RWLockStatus.READ_LOCK && status==RWLockStatus.WRITE_LOCK
                        lockObj.DowngradeFromWriterLock( ref cookie );
                        upgraded = false;
                    } else {
                        lockObj.ReleaseLock();
                        status = RWLockStatus.UNLOCKED;
                        lockObj.AcquireReaderLock( timeout );
                    }

                    status = value;
                }
            }
        }
        #endregion
    }
}
