using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;

namespace QueryFilter.Tests
{
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute(int count = 3)
            : base(() => new Fixture { RepeatCount = count, }.Customize(new AutoMoqCustomization()))
        {
        }
    }
}
