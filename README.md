# LazyMixin

Simple mixin-like implementation for lazy initialization without thread-safety.

## Installation

You can donwload from NuGet, [LazyMixin](https://www.nuget.org/packages/LazyMixin/).

PM> Install-Package LazyMixin

requires Visual Studio 2015.

## How to use

The `LazyMixin<T>` struct is simple and efficient in memory usage, but has tight restrictions because of the C# struct (.NET value type) charactaristics.

 - Do not use for a property. It makes a copied value.
 - Do not use for a readonly field. It also makes a copied value.

for instance:

    // Do not
    public LazyMixin<T> X { get; }

    // Do not
    private readonly LazyMixin<T> _x;

    // Recommended usage
    public T X => _x.Value;
    private LazyMixin<T> _x;

It is thus highly recommended to use this struct with the LazyMixinAnalyzer analyzer.
This analyzer is included within the LazyMixin Nuget package so that you can automatically use it.

The analyzer provides:

Error on using `LazyMixin<T>` for a property

    public LazyMixin<T> X { get; } // error: DoNotUseLazyForProperty

Error on using `LazyMixin<T>` for a read-only field

    private readonly LazyMixin<T> _x; // error: DoNotUseLazyForReadonlyField

Code Fix to encapsulate a `LazyMixin<T>` field with a property

    private LazyMixin<T> _x; // info: AccessViaProperty

the fix result is:

    public T X => _x.Value;
    
    private LazyMixin<T> _x;

## Sample

There are some samples in test codes

- [LazyMixin.Test/UnitTest1.cs](https://github.com/ufcpp/LazyMixin/blob/master/LazyMixin/LazyMixin.Test/UnitTest1.cs)
- [LazyMixinAnalyzer.Test/DoNotUseLazyForPropertyUnitTests.cs](https://github.com/ufcpp/LazyMixin/blob/master/LazyMixin/LazyMixinAnalyzer/LazyMixinAnalyzer.Test/DoNotUseLazyForPropertyUnitTests.cs)
- [LazyMixinAnalyzer.Test/DoNotUseLazyForReadonlyFieldUnitTests.cs](https://github.com/ufcpp/LazyMixin/blob/master/LazyMixin/LazyMixinAnalyzer/LazyMixinAnalyzer.Test/DoNotUseLazyForReadonlyFieldUnitTests.cs)
- [LazyMixinAnalyzer.Test/AccessViaPropertyUnitTests.cs](https://github.com/ufcpp/LazyMixin/blob/master/LazyMixin/LazyMixinAnalyzer/LazyMixinAnalyzer.Test/AccessViaPropertyUnitTests.cs)

## License

under [MIT License](http://opensource.org/licenses/MIT)

