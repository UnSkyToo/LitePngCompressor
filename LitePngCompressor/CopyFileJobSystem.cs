using System;

namespace LitePngCompressor
{
    internal struct CopyFileItemEntity
    {
        internal string InputFilePath;
        internal string OutputFilePath;
    }

    internal class CopyFileJobSystem : JobSystem<CopyFileItemEntity>
    {
        internal event Action<CopyFileItemEntity, bool> OnExecuted;

        public CopyFileJobSystem(CopyFileItemEntity[] Entities, int ThreadCount)
            : base(Entities, ThreadCount)
        {
        }

        protected override void OnExecute(CopyFileItemEntity Entity)
        {
            var Code = PathHelper.CopyFile(Entity.InputFilePath, Entity.OutputFilePath);
            OnExecuted?.Invoke(Entity, Code);
        }
    }
}