﻿namespace OrderAPI.Models.Entites
{
    public class OrderItem
    {
        public Guid OrderItemId { get; set; }
        public Guid ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }

    }
}
