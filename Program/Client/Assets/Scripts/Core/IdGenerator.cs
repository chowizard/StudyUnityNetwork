using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityNet.Client.Core
{
    public class IdGenerator
    {
        public const int Minimum = 0;
        public const int Maximum = int.MaxValue;

        private int seed = 0;
        private int current;
        private Queue<int> discardIds = new Queue<int>();

        public static bool IsValid(int id)
        {
            return ((id >= Minimum) && (id <= Maximum)) ? true : false;
        }

        public IdGenerator() :
            this(0)
        {
        }

        public IdGenerator(int seed)
        {
            this.seed = this.current = seed;
        }

        public bool Generate(out int id)
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

        public void Discard(int id)
        {
            discardIds.Enqueue(id);
        }

        private bool GetDiscardId(out int id)
        {
            id = -1;

            if(discardIds.Count <= 0)
                return false;

            id = discardIds.Dequeue();
            if(!IsValid(id))
                return false;

            return true;
        }
    }
}
