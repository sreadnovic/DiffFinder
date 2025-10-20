using System.Text.Json;

namespace DiffingApiTask.Tests
{
    public class DiffFinderTests
    {
        private IDiffFinder diffFInder;
        private static readonly JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
        
        [SetUp]
        public void Setup()
        {
            diffFInder = new DiffFinder();
        }

        [Test]
        public void GetDiff_NoDifferences_ReturnsEqualsResult()
        {
            string left = "AAAAAA=="; // [0,0,0,0]
            string right = "AAAAAA=="; // [0,0,0,0]
            var result = diffFInder.GetDiff(left, right);
            var expectedObj = new EqualsResult
            {
                diffResultType = "Equals"
            };

            var expectedSerialized = JsonSerializer.Serialize(expectedObj, options);
            var resultSerialized = JsonSerializer.Serialize(result, options);
            Assert.That(resultSerialized, Is.EqualTo(expectedSerialized));
        }

        [Test]
        public void GetDiff_SingleDifference_ReturnsCorrectOffsetAndLength()
        {
            string left = "AAAAAA=="; // [0,0,0,0]
            string right = "AQAAAA=="; // [1,0,0,0]
            var result = diffFInder.GetDiff(left, right);
            var expectedObj = new ContentDoNotMatchResult
            {
                diffResultType = "ContentDoNotMatch",
                diffs = new List<Diff>
                {
                    new Diff { offset = 0, length = 1 }
                }
            };
            var expectedSerialized = JsonSerializer.Serialize(expectedObj, options);
            var resultSerialized = JsonSerializer.Serialize(result, options);
            Assert.That(resultSerialized, Is.EqualTo(expectedSerialized));
        }

        [Test]
        public void GetDiff_MultipleDifferences_ReturnsAllOffsetsAndLengths()
        {
            string left = "AAAAAA=="; // [0,0,0,0]
            string right = "AQABAQ=="; // [1,0,1,1]
            var result = diffFInder.GetDiff(left, right);
            var expectedObj = new ContentDoNotMatchResult
            {
                diffResultType = "ContentDoNotMatch",
                diffs = new List<Diff>
                {
                    new Diff { offset = 0, length = 1 },
                    new Diff { offset = 2, length = 2 }
                }
            };
            var expectedSerialized = JsonSerializer.Serialize(expectedObj, options);
            var resultSerialized = JsonSerializer.Serialize(result, options);
            Assert.That(resultSerialized, Is.EqualTo(expectedSerialized));
        }

        [Test]
        public void GetDiff_MultipleDifferences2_ReturnsAllOffsetsAndLengths()
        {
            string left = "AAAAAA==";    // [0,0,0,0]
            string right = "AQABAQ==";   // [1,0,1,1]
            var result = diffFInder.GetDiff(left, right);
            var expectedObj = new
            {
                diffResultType = "ContentDoNotMatch",
                diffs = new[]
                {
                    new { offset = 0, length = 1 },
                    new { offset = 2, length = 2 }
                }
            };
            var expectedSerialized = JsonSerializer.Serialize(expectedObj, options);
            var resultSerialized = JsonSerializer.Serialize(result, options);
            Assert.That(resultSerialized, Is.EqualTo(expectedSerialized));
        }

        [Test]
        public void GetDiff_DifferencesAtEnd_ReturnsCorrectOffsetAndLength()
        {
            string left = "AAAAAA=="; // [0,0,0,0]
            string right = "AAAAAQ=="; // [0,0,0,1]
            var result = diffFInder.GetDiff(left, right);
            var expectedObj = new ContentDoNotMatchResult
            {
                diffResultType = "ContentDoNotMatch",
                diffs = new List<Diff>
                {
                    new Diff { offset = 3, length = 1 }
                }
            };
            var expectedSerialized = JsonSerializer.Serialize(expectedObj, options);
            var resultSerialized = JsonSerializer.Serialize(result, options);
            Assert.That(resultSerialized, Is.EqualTo(expectedSerialized));
        }
    }
}