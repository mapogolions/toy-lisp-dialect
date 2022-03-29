using Cl.IO;

namespace Cl.Readers
{
    public abstract class ClNumberReader
    {
        protected bool TryReadAtLeastOneNumber(ISource source, out string nums)
        {
            string loop(string acc = "")
            {
                if (source.Eof()) return acc;
                if (!char.IsDigit((char) source.Peek())) return acc;
                return loop($"{acc}{(char) source.Read()}");
            }
            nums = loop();
            return !string.IsNullOrEmpty(nums);
        }
    }
}
