using System;

namespace Unicon2.Unity.ViewModels
{
    public abstract class DisposableBindableBase : ViewModelBase, IDisposable
    {
        private const string DISPOSED_MESSAGE = "Object is no longer usable as long it was disposed";


        /// <summary>
        /// Initializes an instance 
        /// </summary>
        protected DisposableBindableBase() : this(new object())
        {
        }

        /// <summary>
        /// Initializes an instance 
        /// </summary>
        /// <param name="lockObject"></param>
        protected DisposableBindableBase(object lockObject)
        {
            LockObject = lockObject ?? throw new ObjectDisposedException(DISPOSED_MESSAGE);
        }


        /// <summary>
        /// Gets a value which specifies whether the object has already disposed or not
        /// </summary>
        protected bool IsDisposed { get; set; }

        /// <summary>
        /// Gets or sets synchronization object
        /// </summary>
        protected object LockObject { get; set; }


        /// <summary>
        /// Provides basic implementation of Disposable pattern
        /// </summary>
        /// <param name="disposing">A value which specifies whether this method is called from Dispose method or from finalize method</param>
        protected void Dispose(bool disposing)
        {
            if (IsDisposed) return;
            if (disposing)
            {
                lock (LockObject)
                    OnDisposing();
            }
            IsDisposed = true;
        }

        /// <summary>
        /// Does actual explicit disposal of available managed resources
        /// </summary>
        protected virtual void OnDisposing()
        {
            /*None*/
        }


        /// <summary>
        /// Throws <see cref="ObjectDisposedException"/> exception in case the object has already been disposed
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(DISPOSED_MESSAGE);
        }


        /// <summary>
        ///  Performs application-defined tasks associated with freeing, releasing, or
        ///   resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
