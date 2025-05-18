using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YT1.Models
{
    public class ShoppingCart
    {
        public List<ShoppingCartItem> CartList { get; set; }
        public ShoppingCart() {
            this.CartList = new List<ShoppingCartItem>();
        }
        public void AddToCart(ShoppingCartItem item, int Quantity)
        {
            var checkExit = this.CartList.FirstOrDefault(x => x.ProductId == item.ProductId && x.Size == item.Size && x.Color == item.Color);
            if (checkExit != null)
            {
                checkExit.Quantity += Quantity;
                checkExit.TotalPrice = checkExit.Quantity * checkExit.Price;
            }
            else
            {
                this.CartList.Add(item);
            }
        }
        public void Remove(int id, string size, string color)
        {
            var checkExit = this.CartList.SingleOrDefault(x => x.ProductId == id && x.Size == size && x.Color == color);
            if (checkExit != null)
            {
                CartList.Remove(checkExit);
            }
        }
        public void UpdateQuantity(int id, int quantity, string size, string color)
        {
            var checkExit = this.CartList.SingleOrDefault(x => x.ProductId == id && x.Size == size && x.Color == color);
            if (checkExit != null)
            {
                checkExit.Quantity = quantity;
                checkExit.TotalPrice = checkExit.Price * checkExit.Quantity;
            }
        }
        public decimal GetTotalPrice()
        {
            return CartList.Sum(x => x.TotalPrice);
        }
        public decimal GetTotalQuantity()
        {
            return CartList.Sum(x => x.Quantity);
        }
        public void ClearCart()
        {
            CartList.Clear();
        }
    }

    public class ShoppingCartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string Alias { get; set; }
        public string ProductImg { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public decimal TotalPrice { get; set; }
    }
}