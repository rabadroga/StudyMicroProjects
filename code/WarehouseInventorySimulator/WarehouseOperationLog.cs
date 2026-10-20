namespace BeginCsh.WarehouseInventorySimulator;

public sealed record WarehouseOperationLog
(
    string Sku,
    OperationType OperationType,
    int amount, 
    int Quantity,
    bool IsLowStock, //могу ли я вынести это в логику рекорда как лямбда выражение?
    DateTimeOffset DateTime
);
public enum OperationType //стоит ли выносить енум в отдельный файл?
{
    Shipment,
    Restock
}