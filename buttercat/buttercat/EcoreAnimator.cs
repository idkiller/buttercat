

using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace buttercat
{
    public class AnimatorLoop
    {
        const string LibEcore = "libecore.so.1";

        delegate bool EcoreTaskCallback(IntPtr data);

        [DllImport(LibEcore)]
        static extern IntPtr ecore_animator_add(EcoreTaskCallback func, IntPtr data);
        [DllImport(LibEcore)]
        static extern IntPtr ecore_animator_del(IntPtr animator);
        [DllImport(LibEcore)]
        static extern void ecore_animator_thaw(IntPtr animator);
        [DllImport(LibEcore)]
        static extern void ecore_animator_freeze(IntPtr animator);
        [DllImport(LibEcore)]
        static extern void ecore_animator_frametime_set(double frametime);
        [DllImport(LibEcore)]
        static extern double ecore_animator_frametime_get();

        IntPtr animator;
        TaskCompletionSource<bool> joinTaskCompletion;
        double frametime;

        EcoreTaskCallback callback;

        public AnimatorLoop()
        {
            frametime = ecore_animator_frametime_get();
            callback = NativeCallback;
        }

        ~AnimatorLoop()
        {
            if (animator != IntPtr.Zero)
            {
                ecore_animator_del(animator);
            }
        }

        public bool IsRunning { get; set; }

        public event EventHandler Processed;

        public double FrameTime
        {
            get => frametime;
            set
            {
                if (frametime != value)
                {
                    frametime = value;
                    ecore_animator_frametime_set(frametime);
                }
            }
        }

        public void Start()
        {
            IsRunning = true;
            animator = ecore_animator_add(callback, IntPtr.Zero);
        }

        public Task Join()
        {
            if (joinTaskCompletion != null && !joinTaskCompletion.Task.IsCompleted )
            {
                joinTaskCompletion.TrySetCanceled();
            }

            joinTaskCompletion = new TaskCompletionSource<bool>();
            IsRunning = false;

            return joinTaskCompletion.Task;
        }

        protected virtual void OnProcess()
        {
            Processed?.Invoke(this, EventArgs.Empty);
        }

        bool NativeCallback(IntPtr data)
        {
            OnProcess();

            if (!IsRunning)
            {
                joinTaskCompletion?.TrySetResult(true);
            }
            return IsRunning;
        }
    }
}