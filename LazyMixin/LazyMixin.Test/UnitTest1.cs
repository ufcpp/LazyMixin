using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Laziness.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestLazyMixinCounter()
        {
            var sample = new Sample();

            Assert.IsFalse(sample.IsInitialized);
            sample.Counter.Add();
            Assert.IsTrue(sample.IsInitialized);
            sample.Counter.Add();
            sample.Counter.Add();
            sample.Counter.Add();
            Assert.AreEqual(4, sample.Counter.Count);

            sample.Reset();

            Assert.IsFalse(sample.IsInitialized);
            sample.Counter.Add();
            Assert.IsTrue(sample.IsInitialized);
            sample.Counter.Add();
            sample.Counter.Add();
            sample.Counter.Add();
            Assert.AreEqual(4, sample.Counter.Count);
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
