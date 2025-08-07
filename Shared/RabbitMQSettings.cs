using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public static class RabbitMQSettings
    {
        public const string Stock_OrderCreatedEventQueue = "stock.ordercreatedevent.queue";
        public const string Payment_StockNotReservedEventQueue = "payment.stocknotreservedevent.queue";
        public const string Payment_StockReservedEventQueue = "payment.stockreservedevent.queue";





    }
}
