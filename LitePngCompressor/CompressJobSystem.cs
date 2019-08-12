using System;

namespace LitePngCompressor
{
    internal struct CompressItemEntity
    {
        internal string InputFilePath;
        internal string OutputFilePath;
    }

    internal class CompressJobSystem : JobSystem<CompressItemEntity>
    {
        internal event Action<CompressItemEntity, bool> OnExecuted;

        public CompressJobSystem(CompressItemEntity[] Entities, int ThreadCount)
            : base(Entities, ThreadCount)
        {
        }

        protected override void OnExecute(CompressItemEntity Entity)
        {
            var Code = PngQuant.Compress(Entity.InputFilePath, Entity.OutputFilePath);
            OnExecuted?.Invoke(Entity, Code);
        }
    }
}