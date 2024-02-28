using System;
using System.Collections.Generic;

namespace Shop
{
	internal class Shop
	{
		static void Main()
		{
			const string CommandShowStockList = "1";
			const string CommandBuyProduct = "2";
			const string CommandShowInventory = "3";
			const string CommandExit = "4";

			Random random = new Random();
			int maxValue = 100000;
			int minValue = 1000;

			Seller seller = new Seller();
			Player player = new Player(random.Next(minValue, maxValue + 1));

			bool isProgramActive = true;

			while (isProgramActive)
			{
				Console.WriteLine("Список команд:\n" +
						$"{CommandShowStockList} - показать список товаров\n" +
						$"{CommandBuyProduct} - купить товар\n" +
						$"{CommandShowInventory} - показать инвентарь\n" +
						$"{CommandExit} - выход из программы");
				Console.Write("Введите команду: ");
				string input = Console.ReadLine();

				switch (input)
				{
					case CommandShowStockList:
						seller.ShowInventory();
						break;

					case CommandBuyProduct:
						seller.Sell(player);
						break;

					case CommandShowInventory:
						player.ShowInventory();
						break;

					case CommandExit:
						isProgramActive = false;
						break;

					default:
						Console.WriteLine("Неизвестная команда!");
						break;
				}

				Console.Write("Нажмите любую кнопку чтобы продолжить: ");
				Console.ReadKey();
				Console.Clear();
			}
		}
	}
}

class Product
{
	public Product(string title, int price)
	{
		Price = price;
		Title = title;
	}

	public int Price { get; private set; }
	public string Title { get; private set; }
}

class Trader
{
	protected Dictionary<Product, int> Inventory = new Dictionary<Product, int>();

	public virtual void ShowInventory()
	{
		if (Inventory.Count > 0)
			foreach (var item in Inventory)
				Console.WriteLine($"{item.Key.Title} в количестве {item.Value} с ценой за штуку {item.Key.Price}");
		else
			Console.WriteLine("Инвентарь пуст");
	}
}

class Player : Trader
{

	public Player(int money) : base()
	{
		Money = money;
	}

	public int Money { get; private set; }

	public void BuyProduct(Product product, int quantity)
	{
		if (Inventory.ContainsKey(product))
			Inventory[product] += quantity;
		else
			Inventory.Add(product, quantity);

		Money -= product.Price * quantity;
		Console.WriteLine($"Вы успешно купили {product.Title}. У вас осталось {Money} денег");
	}

	public override void ShowInventory()
	{
		base.ShowInventory();
		Console.WriteLine($"Деньги: {Money}");
	}

	public bool CanMakePurchase(Product product, int quantity)
	{
		return Money >= product.Price * quantity;
	}
}

class Seller : Trader
{

	public Seller()
	{
		Inventory.Add(new Product("Стол", 7095), 25);
		Inventory.Add(new Product("Лампа", 2604), 30);
		Inventory.Add(new Product("Кофеварка", 4401), 15);
		Inventory.Add(new Product("Стул", 3502), 20);
		Inventory.Add(new Product("Телевизор", 17873), 10);
		Inventory.Add(new Product("Книга", 898), 50);
		Inventory.Add(new Product("Фонарик", 1347), 40);
		Inventory.Add(new Product("Ковёр", 7993), 12);
		Inventory.Add(new Product("Футболка", 1796), 60);
		Inventory.Add(new Product("Кружка", 718), 35);
	}

	public void Sell(Player player)
	{
		Console.WriteLine("Что вы хотите купить?");
		string title = Console.ReadLine();

		if (TryFindProduct(title, out Product findProduct))
		{
			Console.WriteLine("Сколько вы хотите купить?");
			int.TryParse(Console.ReadLine(), out int quantity);

			if (quantity > Inventory[findProduct])
			{
				Console.WriteLine("В магазине нет столько товара.");
			}
			else
			{
				if (player.CanMakePurchase(findProduct, quantity))
				{
					player.BuyProduct(findProduct, quantity);
					Inventory[findProduct] -= quantity;

					if (Inventory[findProduct] == 0)
						Inventory.Remove(findProduct);
				}
				else
				{
					Console.WriteLine("Недостаточно средств!");
				}
			}
		}
		else
		{
			Console.WriteLine("Нет такого товара");
		}
	}

	private bool TryFindProduct(string productName, out Product product)
	{
		product = null;

		foreach (var item in Inventory)
		{
			if (item.Key.Title.ToLower() == productName.ToLower())
			{
				product = item.Key;
				return true;
			}
		}
		return false;
	}
}