using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative
{
    public class BlockedSlotSeries
    {
        private List<BlockedSlot> _BlockedSlots = new List<BlockedSlot>();

        [DataMember]
        public List<BlockedSlot> BlockedSlots
        {
            get
            {
                return this._BlockedSlots;
            }
            set
            {
                this._BlockedSlots = value;
            }
        }
    }
}
