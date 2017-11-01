using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityNet.Client.Core
{
    public class IdGenerator
    {
        public const uint Minimum = 0;
        public const uint Maximum = uint.MaxValue;

        private uint seed = 0;
        private uint current;
        private Queue<uint> discardIds = new Queue<uint>();

        public static bool IsValid(uint id)
        {
            return ((id >= Minimum) && (id <= Maximum)) ? true : false;
        }

        public IdGenerator() :
            this(0)
        {
        }

        public IdGenerator(uint seed)
        {
            this.seed = this.current = seed;
        }

        public bool Generate(out uint id)
        {
            if(!GetDiscardId(out id))
            {
                if(id < Maximum)
                    id = current++;
                else
                    return false;
            }

            return true;
        }

        public void Discard(uint id)
        {
            discardIds.Enqueue(id);
        }

        private bool GetDiscardId(out uint id)
        {
            id = 0;

            if(discardIds.Count <= 0)
                return false;

            id = discardIds.Dequeue();
            if(!IsValid(id))
                return false;

            return true;
        }
    }
}
