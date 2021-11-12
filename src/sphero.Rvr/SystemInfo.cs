using System.Linq;
using System.Text;

namespace sphero.Rvr;

public record SystemInfo(byte BoardRevision, ProcessorInfo[] Processors)
{
    protected virtual bool PrintMembers(StringBuilder builder)
    {
        builder.Append($"{nameof(BoardRevision)} = {BoardRevision}");
        if (Processors?.Length > 0)
        {
            builder.AppendJoin(", ", Processors.Select(p => p.ToString()));
        }
        return true;
    }
}