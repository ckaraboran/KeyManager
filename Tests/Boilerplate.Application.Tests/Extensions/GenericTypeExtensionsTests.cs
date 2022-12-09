using System.Diagnostics.CodeAnalysis;
using Boilerplate.Application.Extensions;

namespace Boilerplate.Application.Tests.Extensions;

public class GenericTypeExtensionsTests
{
    [Fact]
    public void Given_Type_When_GetGenericTypeName_Then_Returns_GenericTypeName()
    {
        var a = new NormalTypeClass();
        Assert.Equal("NormalTypeClass", a.GetGenericTypeName());
    }

    [Fact]
    public void Given_GenericType_When_GetGenericTypeName_Then_Returns_GenericTypeName()
    {
        var a = new GenericTypeClass<string>();
        Assert.Equal("GenericTypeClass<String>", a.GetGenericTypeName());
    }

    private class NormalTypeClass
    {
    }

    [SuppressMessage("SonarLint", "S2326", Justification = "Created For Test Purposes")]
    private class GenericTypeClass<T>
    {
    }
}