using Shared.Events.Cammon;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{

    public class PaymentFailedEvent : IEvent
    {
        public Guid OrderId { get; set; }
        public string Reason { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }



    }
}