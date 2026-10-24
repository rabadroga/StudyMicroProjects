namespace BeginCsh.WarehouseInventorySimulator;

public sealed record OperationLog
(
    string Sku,
    OperationType OperationType,
    int Amount, 
    int Quantity,
    bool IsLowStock, //могу ли я вынести это в логику рекорда как лямбда выражение?
    DateTimeOffset DateTime
);