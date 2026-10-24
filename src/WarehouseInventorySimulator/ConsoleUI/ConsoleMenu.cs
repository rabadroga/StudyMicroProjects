using System.Diagnostics;

namespace BeginCsh.WarehouseInventorySimulator;

public class ConsoleMenu
{
    private readonly Warehouse _warehouse;
    private bool _isShiftActive = true;

    public ConsoleMenu(Warehouse warehouse) => _warehouse = warehouse;
    public void Run()
    {
        OutputPrinter.PrintWelcome();

        while (_isShiftActive)
        {
            var choice = InputReader.ChoiceInput();
            HandleChoice(choice);
        }

        if (_warehouse.Logs.Count == 0)
        {
            OutputPrinter.PrintEmptyLogReport();
        }
        else
            foreach (var log in _warehouse.Logs)
            {
                OutputPrinter.PrintLogReport(log);
            }
    }

    private void HandleChoice(ChoiceValue choice)
    {
        switch (choice)
        {
            case ChoiceValue.Creation:
                string creationSku = InputReader.SkuInput();
                int creationQuantity = InputReader.QuantityInput();
                int creationWarningThreshold = InputReader.WarningThresholdInput();
                var creationOutcome = _warehouse.CreateItem(creationSku, creationQuantity, creationWarningThreshold);
                OutputPrinter.PrintCreationOutcome(creationOutcome);
                break;

            case ChoiceValue.Shipment:
                string shipmentSku = InputReader.SkuInput();
                int shipmentAmount = InputReader.AmountInput();
                var shipmentOutcome = _warehouse.ShipItem(shipmentSku, shipmentAmount);
                OutputPrinter.PrintShipmentOutcome(shipmentOutcome);
                break;

            case ChoiceValue.Restockment:
                string restockmentSku = InputReader.SkuInput();
                int restockmentAmount = InputReader.AmountInput();
                var restockmentOutcome = _warehouse.RestockItem(restockmentSku, restockmentAmount);
                OutputPrinter.PrintRestockmentOutcome(restockmentOutcome);
                break;

            case ChoiceValue.Exit:
                _isShiftActive = false;
                break;

            default: throw new UnreachableException("Охвачены не все возможные исходы!");
        }

    }
}