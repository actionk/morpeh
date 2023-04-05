namespace Scellecs.Morpeh.Collections {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Unity.IL2CPP.CompilerServices;

    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UnsafeIntHashMap<T> where T : unmanaged {
        public int length;
        public int capacity;
        public int capacityMinusOne;
        public int lastIndex;
        public int freeIndex;

        public PinnedArray<int> buckets;

        public PinnedArray<T>   data;
        public PinnedArray<IntHashMapSlot> slots;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UnsafeIntHashMap(in int capacity = 0) {
            this.lastIndex = 0;
            this.length    = 0;
            this.freeIndex = -1;

            this.capacityMinusOne = HashHelpers.GetCapacity(capacity);
            this.capacity         = this.capacityMinusOne + 1;

            this.buckets = new PinnedArray<int>(this.capacity);
            this.slots   = new PinnedArray<IntHashMapSlot>(this.capacity);
            this.data    = new PinnedArray<T>(this.capacity);
        }

        ~UnsafeIntHashMap() {
            this.buckets.Dispose();
            this.data.Dispose();
            this.slots.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator() {
            Enumerator e;
            e.hashMap = this;
            e.index   = 0;
            e.current = default;
            return e;
        }

        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        [Il2CppSetOption(Option.DivideByZeroChecks, false)]
        public unsafe struct Enumerator {
            public UnsafeIntHashMap<T> hashMap;

            public int index;
            public int current;

            public bool MoveNext() {
                {
                    var slotsPtr = this.hashMap.slots.ptr;
                    for (; this.index < this.hashMap.lastIndex; this.index++) {
                        if (slotsPtr[this.index].key - 1 < 0) {
                            continue;
                        }

                        this.current =  this.index;
                        this.index++;

                        return true;
                    }
                }

                this.index   = this.hashMap.lastIndex + 1;
                this.current = default;
                return false;
            }

            public int Current => this.current;
        }
    }
}