// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Extensions;

/// <summary>
/// Сlass with extension methods for <see cref="Type"/> class.
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// Method for checking if a type is simple:
    /// Boolean, Byte, SByte, Int16, UInt16, Int32,
    /// UInt32, Int64, UInt64, IntPtr, UIntPtr, Char,
    /// Double, Single, Decimal, String, or any Enum.
    /// </summary>
    /// <param name="type">Type checked.</param>
    /// <returns>Result of checking.</returns>
    public static bool IsSimple(this Type type) =>
        // Primitive types: Boolean, Byte, SByte,
        // Int16, UInt16, Int32, UInt32, Int64, UInt64,
        // IntPtr, UIntPtr, Char, Double, Single
        type.IsPrimitive ||
        // Not primitive types:  Decimal, String
        type.IsEquivalentTo(typeof(decimal)) ||
        type.IsEquivalentTo(typeof(string)) ||
        // Enums
        type.IsEnum;

    /// <summary>
    /// Method for checking if a type is an integer number:
    /// Byte, SByte, Int16, UInt16, Int32, UInt32, Int64, UInt64
    /// </summary>
    /// <param name="type">Type checked.</param>
    /// <returns>Result of checking.</returns>
    public static bool IsIntegerNumber(this Type type) =>
        type.IsEquivalentTo(typeof(byte)) ||
        type.IsEquivalentTo(typeof(short)) ||
        type.IsEquivalentTo(typeof(ushort)) ||
        type.IsEquivalentTo(typeof(int)) ||
        type.IsEquivalentTo(typeof(uint)) ||
        type.IsEquivalentTo(typeof(long)) ||
        type.IsEquivalentTo(typeof(ulong));

    /// <summary>
    /// Method for checking if a type is a number:
    /// Byte, SByte, Int16, UInt16, Int32, UInt32, Int64, UInt64
    /// Single, Double, Decimal
    /// </summary>
    /// <param name="type">Type checked.</param>
    /// <returns>Result of checking.</returns>
    public static bool IsNumber(this Type type) =>
        type.IsIntegerNumber() ||
        type.IsEquivalentTo(typeof(float)) ||
        type.IsEquivalentTo(typeof(double)) ||
        type.IsEquivalentTo(typeof(decimal));
}
