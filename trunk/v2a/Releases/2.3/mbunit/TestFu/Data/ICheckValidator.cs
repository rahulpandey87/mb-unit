using System;
using System.Data;

namespace TestFu.Data
{
    /// <summary>
    /// A validator check checks constraints
    /// </summary>
    public interface ICheckValidator
    {
        /// <summary>
        /// Preprocesses the row modifies it to fullfill the constraint
        /// </summary>
        /// <param name="row"></param>
        bool Enforce(DataRow row);
    }
}
