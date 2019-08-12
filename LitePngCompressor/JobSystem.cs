using System;
using System.Threading;

namespace LitePngCompressor
{
    internal class Job<TEntity>
    {
        internal TEntity[] Entities_;
        internal int From_;
        internal int To_;
        internal Exception Ex_;

        internal void Set(TEntity[] Entities, int From, int To)
        {
            Entities_ = Entities;
            From_ = From;
            To_ = To;
            Ex_ = null;
        }
    }

    internal abstract class JobSystem<TEntity>
    {
        private readonly TEntity[] Entities_;
        private readonly Job<TEntity>[] Jobs_;
        private readonly int ThreadCount_;
        private int ThreadRuningCount_;

        internal event Action OnCompleted;

        internal JobSystem(TEntity[] Entities, int ThreadCount)
        {
            Entities_ = Entities;
            ThreadCount_ = ThreadCount;
            var Remainder = Entities_.Length % ThreadCount_;
            var Slice = Entities_.Length / ThreadCount_ + (Remainder == 0 ? 0 : 1);

            Jobs_ = new Job<TEntity>[ThreadCount];
            for (var Index = 0; Index < ThreadCount; ++Index)
            {
                var From = Index * Slice;
                var To = From + Slice;

                if (To > Entities_.Length)
                {
                    To = Entities_.Length;
                }

                Jobs_[Index] = new Job<TEntity>();
                Jobs_[Index].Set(Entities_, From, To);
            }
        }

        internal void Execute()
        {
            ThreadRuningCount_ = ThreadCount_;
            for (var Index = 0; Index < ThreadCount_; ++Index)
            {
                var JobTask = Jobs_[Index];

                if (Jobs_[Index].From_ != Jobs_[Index].To_)
                {
                    ThreadPool.QueueUserWorkItem(QueueOnThread, JobTask);
                }
                else
                {
                    OnTaskDone();
                }
            }
        }

        private void QueueOnThread(object State)
        {
            var JobTask = State as Job<TEntity>;

            try
            {
                for (var Index = JobTask.From_; Index < JobTask.To_; ++Index)
                {
                    OnExecute(JobTask.Entities_[Index]);
                }
            }
            catch (Exception Ex)
            {
                JobTask.Ex_ = Ex;
            }
            finally
            {
                OnTaskDone();
            }
        }

        private void OnTaskDone()
        {
            Interlocked.Decrement(ref ThreadRuningCount_);

            if (ThreadRuningCount_ == 0)
            {
                foreach (var JobTask in Jobs_)
                {
                    if (JobTask.Ex_ != null)
                    {
                        throw JobTask.Ex_;
                    }
                }

                OnCompleted?.Invoke();
            }
        }

        protected abstract void OnExecute(TEntity Entity);
    }
}