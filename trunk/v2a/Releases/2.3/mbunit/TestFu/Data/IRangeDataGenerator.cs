using System;

namespace TestFu.Data
{
    public interface IRangeDataGenerator : IDataGenerator
    {
        int MinLength { get;set;}
        int MaxLength { get;set;}
    }
}
