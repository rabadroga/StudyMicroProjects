namespace BeginCsh.WarehouseInventorySimulator;

public sealed record ShipmentHistoryLog
(
    string Sku,
    OperationType OperationType,
    int amount, 
    int Quantity,
    bool IsLowStock, //могу ли я вынести это в логику рекорда как лямбда выражение?
    DateTimeOffset DateTime
);
/*
сделать коммит, что добавил методы списания и добавления внутри сущности
создал готовый рекорд для записи логов смены

потом
создать класс управления складов
со словарем, готовыми методами
и тд, что в чате

начать реализацию методов
*/

public enum OperationType
{
    Shipment,
    Receipt
}