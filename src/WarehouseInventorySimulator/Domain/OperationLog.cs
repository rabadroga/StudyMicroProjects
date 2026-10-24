namespace BeginCsh.WarehouseInventorySimulator;

public sealed record OperationLog
(
    string Sku,
    OperationType OperationType,
    int Amount, 
    int Quantity,
    bool IsLowStock,
    DateTimeOffset DateTime
);