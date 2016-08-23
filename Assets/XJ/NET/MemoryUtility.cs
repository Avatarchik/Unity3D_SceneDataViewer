namespace XJ.NET
{
    // Unity3D に含まれる Mono では PrivateMemorySize64 や、
    // WorkingSet64 は、常に 0 が返ります。
    // Unity3D のバージョン 5.4 までこの仕様を確認しています。

    public static class MemoryUtility
    {
        private static long currentMemory = 0;
        private static long maxMemory = 0;

        public static long CurrentMemory
        {
            get { return MemoryUtility.currentMemory; }
        }

        public static long MaxMemory
        {
            get { return MemoryUtility.maxMemory; }
        }

        public static void Update()
        {
            MemoryUtility.currentMemory = 
                System.Diagnostics.Process.GetCurrentProcess().WorkingSet64;

            MemoryUtility.maxMemory =
                MemoryUtility.currentMemory > MemoryUtility.maxMemory ?
                MemoryUtility.currentMemory : MemoryUtility.maxMemory;
        }

        public static float ByteToMByte(long byteValue)
        {
            return byteValue / 1048576f;
        }
    }
}