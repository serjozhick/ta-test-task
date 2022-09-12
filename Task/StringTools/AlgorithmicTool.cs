using System.Text;
using System.Threading.Tasks;
using TATask.Contracts;

namespace TATask.StringTools
{
    public class AlgorithmicTool : IStringTool
    {
        public Task<string> Invert(string sourceString)
        {
            var inputArray = new StringBuilder(sourceString);
            for(int i = 0; i < inputArray.Length/2; i++)
            {
                var charHolder = inputArray[i];
                inputArray[i] = inputArray[^(i+1)];
                inputArray[^(i+1)] = charHolder;
            }
            return Task.FromResult(inputArray.ToString());
        }
    }
}