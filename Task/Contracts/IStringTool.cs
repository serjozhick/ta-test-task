using System.Threading.Tasks;

namespace TATask.Contracts
{
    public interface IStringTool
    {
        Task<string> Invert(string sourceString);
    }
}