using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public virtual void AddItem(Product product, int quantity)
        {
            CartLine line = lineCollection.FirstOrDefault(x => x.Product.ProductID == product.ProductID);
            if (line == null)
            {
                lineCollection.Add(new CartLine {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public virtual void RemoveItem(Product product)
        {
            lineCollection.RemoveAll(x => x.Product.ProductID == product.ProductID);
        }

        public virtual decimal ComputeTotalValue()
        {
            return lineCollection.Sum(x => x.Product.Price*x.Quantity);
        }

        public virtual void Clear() => lineCollection.Clear();

        public virtual IEnumerable<CartLine> Lines => lineCollection;
    }

    public class CartLine
    {
        public int CartLineID { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public int OrderID { get; set; }
    }

    public class CartLineType
    {
        public int Id { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
	    public int OrderID { get; set; }
    }
}
