namespace Shared.Utility
{
    public static class EnumUtil
    {
        public static TEnum[] ToArray<TEnum>() where TEnum : struct, Enum
        {
            return (TEnum[])Enum.GetValues(typeof(TEnum));
        }
    }
}