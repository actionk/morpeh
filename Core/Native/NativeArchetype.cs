﻿#if MORPEH_BURST
namespace Morpeh.Native {
    using Unity.Collections.LowLevel.Unsafe;

    public struct NativeArchetype {
        internal NativeFastList<int> entitiesBitMap;

        [NativeDisableUnsafePtrRestriction]
        internal unsafe int* lengthPtr;
    }
}
#endif
