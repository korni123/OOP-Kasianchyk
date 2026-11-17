# Лабораторна робота №9

**Тема:** HashSet/Queue/Stack: практичні сценарії.

**Варіант:** 9 — Система бронювання

Проєкт: `lab9v9`.

## Підзадачі варіанту

- `HashSet<string>` — унікальні клієнти. Повторно доданий клієнт не додається (`Add` повертає `false`).
- `Queue<Booking>` — черга бронювань. Обробка за принципом FIFO (першим прийшов — першим обслужений).
- `Stack<Booking>` — історія скасованих бронювань. Принцип LIFO (останнє скасоване повертається першим).

## Опис класів

### Booking

- Властивості: `Id` (int), `ClientName` (string), `Room` (int), `Date` (DateTime)
- Конструктор: `Booking(int id, string clientName, int room, DateTime date)`
- Методи: `ToString()`

### BookingSystem

- Поля: `HashSet<string> _clients`, `Queue<Booking> _bookingQueue`, `Stack<Booking> _cancelledHistory`
- Властивості: `ClientsCount`, `QueueCount`, `CancelledCount`
- HashSet: `AddClient`, `ContainsClient`, `RemoveClient`, `GetClients`
- Queue: `AddBooking` (Enqueue), `PeekNextBooking` (Peek), `ProcessNextBooking` (Dequeue)
- Stack: `CancelNextBooking` (Dequeue + Push), `PeekLastCancelled` (Peek), `RestoreLastCancelled` (Pop + Enqueue)

## Що показує Main

- HashSet: додавання, дублікат, `Contains`, `Remove`, `UnionWith`, `IntersectWith`, `ExceptWith`
- Queue: `Enqueue`, `Peek`, `Dequeue`, порядок обробки FIFO
- Stack: `Push`, `Peek`, `Pop`, порядок LIFO
- порівняння швидкості `List.Contains` і `HashSet.Contains` через `Stopwatch`

## Запуск

```
cd OOP-Kasianchyk
dotnet run --project lab9v9
```

## Приклад виводу

(скріншот)
