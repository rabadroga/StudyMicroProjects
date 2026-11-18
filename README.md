# BeginCsh

[Русский](#русский) | [English](#english)

---

## Русский

### О проекте
Учебный репозиторий для изучения **C#** и платформы **.NET**. Проект показывает постепенную эволюцию от базовых синтаксических упражнений к структурированным консольным микро-проектам с разделением на слои (`Domain`, `Application`, `ConsoleUI`), использованием `record`-типов, типизированных результатов операций (`Outcomes`) и модульного тестирования на **xUnit**.

### Стек и конфигурация
- **Платформа:** `.NET 10.0` (`net10.0`)
- **Язык:** `C#` (`Nullable` и `ImplicitUsings` включены, `AnalysisLevel: latest-recommended`)
- **Тестирование:** `xUnit` (`2.9.3`), `Microsoft.NET.Test.Sdk`
- **Решение:** [`StudyMicroProjects.slnx`](StudyMicroProjects.slnx)

### Структура проекта
- [`src/Program.cs`](src/Program.cs) — точка входа в консольное приложение (сейчас запускает демонстрационный сценарий для `WarehouseBinGrid`).
- [`src/1Base/InputHelper.cs`](src/1Base/InputHelper.cs) — общий помощник для чтения и валидации пользовательского ввода из консоли.
- [`src/LearningBasics/`](src/LearningBasics) — базовая практика по C#: работа со строками, числами (`decimal`, `double`), циклами, коллекциями (`List<T>`), кортежами и базовыми алгоритмами.
- [`src/ProductOperations/`](src/ProductOperations) — первая версия консольного расчёта стоимости товара (ввод артикула, количества, цены и расчёт НДС 16%).
- [`src/ProductOperationsV2/`](src/ProductOperationsV2) — переработанная версия расчёта заказа на базе `record Order` с округлением и отдельным выводом чека (`OrderReceiptPrinter`).
- [`src/PackageDimensionsValidator/`](src/PackageDimensionsValidator) — проверка габаритов ручки и коробки (`ValidateDirect` и `ValidateWithRotation` с учётом поворота) и детализацией превышений по осям.
- [`src/TieredPriceCalculator/`](src/TieredPriceCalculator) — калькулятор скидок с плоской и прогрессивной шкалами (`flat` / `progressive`), учётом статуса клиента (`Retail`, `Wholesaler`, `Designer`), промокодов и ограничением максимальной скидки.
- [`src/WarehouseInventorySimulator/`](src/WarehouseInventorySimulator) — симулятор складского учёта с консольным меню (`ConsoleMenu`), журналом операций (`OperationLog`), учётом остатков (`InventoryItem`, `Warehouse`) и типизированными исходами операций.
- [`src/1Base/WarehouseBinGrid/`](src/1Base/WarehouseBinGrid) — система адресного хранения на стеллаже (`Rack` 4×6: полки `A`–`D`, ячейки `1`–`6`): ручное и автоматическое размещение (`Deposit` / `AutoDeposit`), списание (`Withdraw`), блокировка/разблокировка ячеек, статистика (`RackStatistic`) и консольная визуализация сетки (`SimpleGridRenderer`).
- [`tests/BeginCsh.Tests/`](tests/BeginCsh.Tests) — модульные тесты на `xUnit` для `WarehouseInventorySimulator` и `WarehouseBinGrid`.
- [`docs/`](docs) — рабочие заметки и трекер ошибок.

### Требования и запуск
- **.NET SDK** 10.0+

Сборка всего решения:
```bash
dotnet build StudyMicroProjects.slnx
```

Запуск консольного приложения:
```bash
dotnet run --project src/beginCsh.csproj
```

Запуск модульных тестов:
```bash
dotnet test StudyMicroProjects.slnx
```

---

## English

### About the Project
A learning repository for **C#** and the **.NET** ecosystem. It demonstrates a progression from fundamental syntax exercises to structured console micro-projects with layered architecture (`Domain`, `Application`, `ConsoleUI`), `record` types, typed operation outcomes (`Outcomes`), and **xUnit** unit tests.

### Tech Stack & Configuration
- **Target Framework:** `.NET 10.0` (`net10.0`)
- **Language:** `C#` (`Nullable` and `ImplicitUsings` enabled, `AnalysisLevel: latest-recommended`)
- **Testing:** `xUnit` (`2.9.3`), `Microsoft.NET.Test.Sdk`
- **Solution:** [`StudyMicroProjects.slnx`](StudyMicroProjects.slnx)

### Project Structure
- [`src/Program.cs`](src/Program.cs) — console application entry point (currently runs a demo scenario for `WarehouseBinGrid`).
- [`src/1Base/InputHelper.cs`](src/1Base/InputHelper.cs) — shared helper for reading and validating console input.
- [`src/LearningBasics/`](src/LearningBasics) — foundational C# exercises: strings, numeric types (`decimal`, `double`), loops, collections (`List<T>`), tuples, and basic algorithms.
- [`src/ProductOperations/`](src/ProductOperations) — initial version of the product cost calculator (SKU, quantity, price input, and 16% VAT calculation).
- [`src/ProductOperationsV2/`](src/ProductOperationsV2) — refactored order calculator using `record Order`, rounding, and a dedicated receipt printer (`OrderReceiptPrinter`).
- [`src/PackageDimensionsValidator/`](src/PackageDimensionsValidator) — package fit validator (`ValidateDirect` and `ValidateWithRotation`) with per-axis exceedance details.
- [`src/TieredPriceCalculator/`](src/TieredPriceCalculator) — discount calculator supporting flat and progressive tiers, customer categories (`Retail`, `Wholesaler`, `Designer`), promo codes, and a maximum discount cap.
- [`src/WarehouseInventorySimulator/`](src/WarehouseInventorySimulator) — warehouse inventory simulator featuring a console menu (`ConsoleMenu`), operation audit log (`OperationLog`), stock management (`InventoryItem`, `Warehouse`), and typed operation outcomes.
- [`src/1Base/WarehouseBinGrid/`](src/1Base/WarehouseBinGrid) — bin-level rack storage system (`Rack` 4×6: shelves `A`–`D`, cells `1`–`6`) supporting manual and automatic placement (`Deposit` / `AutoDeposit`), withdrawals (`Withdraw`), bin blocking/unblocking, rack statistics (`RackStatistic`), and a console grid renderer (`SimpleGridRenderer`).
- [`tests/BeginCsh.Tests/`](tests/BeginCsh.Tests) — `xUnit` unit tests covering `WarehouseInventorySimulator` and `WarehouseBinGrid`.
- [`docs/`](docs) — study notes and error tracker.

### Prerequisites & Running
- **.NET SDK** 10.0+

Build the solution:
```bash
dotnet build StudyMicroProjects.slnx
```

Run the console application:
```bash
dotnet run --project src/beginCsh.csproj
```

Run unit tests:
```bash
dotnet test StudyMicroProjects.slnx
```
