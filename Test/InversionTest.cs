using System;
using System.Threading.Tasks;
using TATask.StringTools;
using Xunit;

namespace TATaskTest
{
    public class InversionTest
    {
        [Fact]
        public async Task SmallText_AlgorithmicInvert_ShouldInvert()
        {
            var input = "abc ABC 123";
            var inverter = new AlgorithmicTool();

            var output = await inverter.Invert(input);

            Assert.Equal("321 CBA cba", output);
        }

        [Fact]
        public async Task EvenLengthText_AlgorithmicInvert_ShouldInvert()
        {
            var input = "123abc";
            var inverter = new AlgorithmicTool();

            var output = await inverter.Invert(input);

            Assert.Equal("cba321", output);
        }

        [Fact]
        public async Task PalindromeText_AlgorithmicInvert_ShouldBeRevertedAndMatchSource()
        {
            var input = "dogeeseseegod";
            var inverter = new AlgorithmicTool();

            var output = await inverter.Invert(input);

            Assert.Equal(input, output);
        }

        [Fact]
        public async Task UnicodeText_AlgorithmicInvert_ShouldInvert()
        {
            var input = "Kogu töö ja ükski mäng teeb Jackist nüri poisi";
            var inverter = new AlgorithmicTool();

            var output = await inverter.Invert(input);

            Assert.Equal("isiop irün tsikcaJ beet gnäm ikskü aj ööt ugoK", output);
        }

        [Fact]
        public async Task BothInverters_SameText_InvertedShouldBeTheSame()
        {
            var input = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

            var algorithmicInverter = new AlgorithmicTool();
            var algorithmicOutput = await algorithmicInverter.Invert(input);

            var collectionInvetret = new CollectionTool();
            var collectionOutput = await collectionInvetret.Invert(input);

            Assert.Equal(algorithmicOutput, collectionOutput);
        }
    }
}
