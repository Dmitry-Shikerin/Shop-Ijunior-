using System;
using System.Collections.Generic;

namespace Магазин_товаров
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string CommandShowListProducts = "1";
            const string CommandBuyProduct = "2";
            const string CommandShowBuyerListProducts = "3";

            Seller seller = new Seller();
            Buyer buyer = new Buyer();
            Shop shop = new Shop();

            bool isWork = true;

            Console.WriteLine("Добро пожаловать в магазин ");

            while (isWork)
            {
                Console.WriteLine($"{CommandShowListProducts} - Показать список товаров Продавца");
                Console.WriteLine($"{CommandBuyProduct} - Купить товар");
                Console.WriteLine($"{CommandShowBuyerListProducts} - Показать список товаров покупателя");
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case CommandShowListProducts:
                        seller.ShowProducts();
                        break;

                    case CommandBuyProduct:
                        shop.TradeProduct(seller, buyer);
                        break;

                    case CommandShowBuyerListProducts:
                        buyer.ShowProducts();
                        break;
                }

                Console.ReadLine();
                Console.Clear();
            }
        }
    }

    class Product
    {
        public Product(string name, int price)
        {
            Name = name;
            Price = price;
        }

        public string Name { get; private set; }
        public int Price { get; private set; }
    }

    class Human
    {
        protected List<Product> Products = new List<Product>();

        public Human(int money)
        {
            Money = money;
        }

        public int Money { get; protected set; }

        public void ShowProducts()
        {
            if (Products.Count != 0)
            {
                for (int i = 0; i < Products.Count; i++)
                {
                    Console.WriteLine($"{i + 1} {Products[i].Name} {Products[i].Price}");
                }
            }
            else
            {
                Console.WriteLine("Список товаров пуст");
            }
        }
    }

    class Seller : Human
    {
        public Seller() : base(0)
        {
            FillProducts();
        }

        public bool TryToGetProduct(out Product product)
        {
            bool haveProduct = false;

            if (Products.Count > 0)
            {
                ShowProducts();

                Console.WriteLine("Введите id товара");
                int productId = ReadInt() - 1;

                if (productId < 0 || productId >= Products.Count)
                {
                    Console.WriteLine("Некорректный ввод");
                    product = null;
                    return haveProduct;
                }

                product = Products[productId];
                haveProduct = true;
                return haveProduct;
            }
            else
            {
                Console.WriteLine("У продавца закончились продукты");
                product = null;
            }

            return haveProduct;
        }

        public void SellProduct(Product product)
        {
            Money += product.Price;
            Products.Remove(product);
        }

        private void FillProducts()
        {
            Products.Add(new Product("Яблоко", 15));
            Products.Add(new Product("Груша", 17));
            Products.Add(new Product("Авокадо", 25));
            Products.Add(new Product("Арбуз", 150));
        }

        private int ReadInt()
        {
            int number = 0;

            bool isNumber = false;

            while (isNumber == false)
            {
                isNumber = int.TryParse(Console.ReadLine(), out number);

                if (isNumber == false)
                {
                    Console.WriteLine("Ошибка. Введите число.");
                }
            }

            return number;
        }
    }
    class Buyer : Human
    {
        public Buyer() : base(1000) { }

        public bool CanPay(int productPrice)
        {
            return Money > productPrice;
        }

        public void BuyProduct(Product product)
        {
            if (product != null)
            {
                Products.Add(product);
                Money -= product.Price;
            }
        }
    }

    class Shop
    {
        public void TradeProduct(Seller seller, Buyer buyer)
        {
            if (seller.TryToGetProduct(out Product product) == false)
                return;

            if (buyer.CanPay(product.Price))
            {
                seller.SellProduct(product);
                buyer.BuyProduct(product);
            }
            else
            {
                Console.WriteLine("У вас недостаточно денег");
            }
        }
    }
}
