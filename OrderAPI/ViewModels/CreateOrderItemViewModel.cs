﻿namespace OrderAPI.ViewModels
{
    public class CreateOrderItemViewModel
    {
        public string ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }

    }
}
