using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TATask.Contracts;

namespace TATask.StringTools
{
    public class CollectionTool : IStringTool
    {
        public Task<string> Invert(string sourceString)
        {
            var queue = new Stack<char>();
            foreach(char c in sourceString)
            {
                queue.Push(c);
            }
            return Task.FromResult(new string(queue.ToArray()));
        }
    }
}