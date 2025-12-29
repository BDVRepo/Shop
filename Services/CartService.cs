using Shop.Models;

namespace Shop.Services;

public class CartService
{
    private List<CartItem> _items = new();

    public event Action? OnChange;

    public List<CartItem> GetItems() => _items;

    public void AddItem(Product product, int quantity = 1, string? size = null)
    {
        var existingItem = _items.FirstOrDefault(x => x.ProductId == product.Id && x.Size == size);
        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            _items.Add(new CartItem
            {
                ProductId = product.Id,
                Product = product,
                Quantity = quantity,
                Size = size,
                UnitPrice = product.Price
            });
        }
        NotifyStateChanged();
    }

    public void RemoveItem(int productId, string? size = null)
    {
        var item = _items.FirstOrDefault(x => x.ProductId == productId && x.Size == size);
        if (item != null)
        {
            _items.Remove(item);
            NotifyStateChanged();
        }
    }

    public void UpdateQuantity(int productId, int quantity, string? size = null)
    {
        var item = _items.FirstOrDefault(x => x.ProductId == productId && x.Size == size);
        if (item != null)
        {
            if (quantity <= 0)
            {
                RemoveItem(productId, size);
            }
            else
            {
                item.Quantity = quantity;
                NotifyStateChanged();
            }
        }
    }

    public void Clear()
    {
        _items.Clear();
        NotifyStateChanged();
    }

    public decimal GetTotal()
    {
        return _items.Sum(x => x.UnitPrice * x.Quantity);
    }

    public int GetItemCount()
    {
        return _items.Sum(x => x.Quantity);
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}

public class CartItem
{
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Size { get; set; }
}

