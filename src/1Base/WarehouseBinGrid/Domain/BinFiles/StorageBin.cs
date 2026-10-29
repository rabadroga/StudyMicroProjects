using System.Diagnostics;

namespace BeginCsh.WarehouseBinGrid;

internal class StorageBin
{
    internal BinCoordinate Coordinate { get; }
    internal int MaxQuantity { get; }
    internal BinContent Content { get; private set; } = new BinContent.Empty();

    /*internal BinStatus Status => Content switch
    {
        BinContent.Empty => BinStatus.Empty,
        BinContent.Occupied => BinStatus.Occupied,
        BinContent.Blocked => BinStatus.Blocked,
        _ => throw new UnreachableException()
    };*/ //пока здесь оставлю, если не пригодится в аппликейш, то удалю енум статуса

    internal StorageBin(BinCoordinate coordinate, int maxQuantity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxQuantity);

        Coordinate = coordinate != default ? coordinate : throw new ArgumentException("Пустые координаты", nameof(coordinate)); //это же не избыточно?

        MaxQuantity = maxQuantity;
    }

    internal DepositOutcome Deposit(string sku, int amount)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sku);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        switch (Content)
        {
            case BinContent.Empty:
                if (amount > MaxQuantity)
                {
                    return new DepositOutcome.QuantityExcess(sku, MaxQuantity, amount, 0);
                }
                BinContent.Occupied newOccupiedContent = new BinContent.Occupied(sku, amount);
                Content = newOccupiedContent;
                return new DepositOutcome.Success(newOccupiedContent);

            case BinContent.Occupied occupied when occupied.Sku == sku:
                if ((occupied.Quantity + amount) > MaxQuantity)
                {
                    return new DepositOutcome.QuantityExcess(sku, MaxQuantity, amount, occupied.Quantity);
                }
                BinContent.Occupied updatedOccupiedContent  = occupied with { Quantity = occupied.Quantity + amount };
                Content = updatedOccupiedContent;
                return new DepositOutcome.Success(updatedOccupiedContent);

            case BinContent.Occupied otherOccupied:
                return new DepositOutcome.OccupiedWithOtherSku(sku, otherOccupied);
            case BinContent.Blocked blocked:
                return new DepositOutcome.Blocked(sku, blocked);

            default:
                throw new UnreachableException();
        }
    }

    internal WithdrawOutcome Withdraw(string sku, int amount)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sku);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        switch (Content)
        {
            case BinContent.Occupied occupied when occupied.Sku == sku:
                if (amount > occupied.Quantity)
                {
                    return new WithdrawOutcome.InsufficientStock(occupied, amount);
                }

                int newQuantity = occupied.Quantity - amount;

                if (newQuantity == 0)
                {
                    Content = new BinContent.Empty();
                    return new WithdrawOutcome.SuccessEmpty(sku, amount);
                }
                else
                {
                    BinContent.Occupied occupiedContent = occupied with { Quantity = newQuantity };
                    Content = occupiedContent;
                    return new WithdrawOutcome.Success(occupiedContent);
                }

            case BinContent.Occupied:
                return new WithdrawOutcome.ItemNotFound(sku);
            case BinContent.Empty:
                return new WithdrawOutcome.Empty(sku);
            case BinContent.Blocked blocked:
                return new WithdrawOutcome.Blocked(blocked);

            default:
                throw new UnreachableException();
        }
    }

    internal BlockOutcome Block(string reason)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reason);

        switch (Content)
        {
            case BinContent.Empty:
                Content = new BinContent.Blocked(reason);
                return new BlockOutcome.SuccessEmpty();

            case BinContent.Occupied occupied:
                Content = new BinContent.Blocked(reason, occupied);
                return new BlockOutcome.SuccessOccupied(Content);

            case BinContent.Blocked:
                return new BlockOutcome.AlreadyBlocked();

            default:
                throw new UnreachableException();
        }
    }

    internal UnblockOutcome UnBlock()
    {
        switch (Content)
        {
            case BinContent.Blocked blocked:
                if (blocked.OccupiedContent is not null)
                {
                   BinContent.Occupied newBlockedOccupiedContent = blocked.OccupiedContent;
                   Content = newBlockedOccupiedContent;
                    return new UnblockOutcome.SuccessOccupied(newBlockedOccupiedContent);
                }
                else
                {
                    Content = new BinContent.Empty();
                    return new UnblockOutcome.SuccessEmpty();
                }

            case BinContent.Occupied or BinContent.Empty:
                return new UnblockOutcome.NotBlocked();

            default:
                throw new UnreachableException();
        }
    }
}