using Xunit;

namespace Laziness.Test
{
    public class UnitTest1
    {
        [Fact]
        public void TestLazyMixinCounter()
        {
            var sample = new Sample();

            Assert.False(sample.IsInitialized);
            sample.Counter.Add();
            Assert.True(sample.IsInitialized);
            sample.Counter.Add();
            sample.Counter.Add();
            sample.Counter.Add();
            Assert.Equal(4, sample.Counter.Count);

            sample.Reset();

            Assert.False(sample.IsInitialized);
            sample.Counter.Add();
            Assert.True(sample.IsInitialized);
            sample.Counter.Add();
            sample.Counter.Add();
            sample.Counter.Add();
            Assert.Equal(4, sample.Counter.Count);
        }

        [Fact]
        public void TestGetValueOrDefault()
        {
            var sample = new LazyMixin<Counter>();
            Assert.Null(sample.GetValueOrDefault());

            var v = sample.Value;
            Assert.Equal(v, sample.GetValueOrDefault());
        }
    }

    class Sample
    {
        public Counter Counter => _counter.Value;
        private LazyMixin<Counter> _counter;

        public bool IsInitialized => _counter.IsInitialized;
        public void Reset() => _counter.Dispose();
    }

    class Counter
    {
        public int Count { get; private set; }

        public void Add() => Count++;
    }
}
