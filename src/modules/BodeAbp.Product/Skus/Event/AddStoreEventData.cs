using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Events.Bus;

namespace BodeAbp.Product.Skus.Event
{
    public class AddStoreEventData : IEventData
    {
        public long ProductId { get; set; }
        public int No { get; set; }

        public DateTime EventTime { get; set; }

        public object EventSource { get; set; }
    }
}
