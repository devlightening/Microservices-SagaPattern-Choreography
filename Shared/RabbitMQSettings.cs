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
        public const string Order_PaymentCompletedEventQueue = "order.paymentcompleteevent.queue";
        public const string Order_PaymentFailedEventQueue = "order.paymentfailedevent.queue"; 
        public const string Stock_PaymentFailedEventQueue = "stock.paymentfailedevent.queue";


    }
}
