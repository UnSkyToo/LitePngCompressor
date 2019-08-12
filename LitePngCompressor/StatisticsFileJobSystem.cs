using System;

namespace LitePngCompressor
{
    internal class StatisticsFileJobSystem : JobSystem<string>
    {
        internal event Action<string, bool> OnExecuted;

        public StatisticsFileJobSystem(string[] Entities, int ThreadCount)
            : base(Entities, ThreadCount)
        {
        }

        protected override void OnExecute(string Entity)
        {
            var Code = false;
            
            var OldSizeText = ConfigHelper.GetValue($"{Entity}_Size");
            if (string.IsNullOrWhiteSpace(OldSizeText))
            {
                Code = true;
            }

            if (!long.TryParse(OldSizeText, out long OldSize))
            {
                Code = true;
            }

            var NewSize = PathHelper.GetFileSize(Entity);
            if (OldSize != NewSize)
            {
                ConfigHelper.SetValue($"{Entity}_Size", NewSize.ToString());
                Code = true;
            }

            var OldMd5 = ConfigHelper.GetValue($"{Entity}_MD5");
            if (string.IsNullOrWhiteSpace(OldMd5))
            {
                Code = true;
            }

            var NewMd5 = PathHelper.FileMD5(Entity);
            if (OldMd5 != NewMd5)
            {
                ConfigHelper.SetValue($"{Entity}_MD5", NewMd5);
                Code = true;
            }

            OnExecuted?.Invoke(Entity, Code);
        }
    }
}
