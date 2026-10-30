# Лабораторна робота №7

**Тема:** Узагальнені типи (Generics). IRepository з where-обмеженнями та тестами.

**Варіант:** 9 — сутність `Project`

Проєкти: `lab7v9` (основний) і `lab7v9.Tests` (xUnit-тести).

## Опис

### IEntity

Інтерфейс сутності.

- Властивість: `int Id { get; set; }`

### Project : IEntity

Сутність за варіантом.

- Властивості: `Id` (int), `Name` (string), `StartDate` (DateTime), `EndDate` (DateTime)
- Конструктор: `Project(int id, string name, DateTime startDate, DateTime endDate)`
- Методи: `ToString()` — повертає рядок з даними проєкту

### IRepository&lt;T&gt; where T : IEntity

Узагальнений інтерфейс репозиторію. Обмеження `where T : IEntity` дозволяє працювати тільки з сутностями, які мають `Id`.

- `T? GetById(int id)` — повертає елемент за Id або `null`, якщо його немає
- `IEnumerable<T> GetAll()` — повертає всі елементи
- `void Add(T item)` — додає елемент
- `void Update(T item)` — замінює елемент з таким самим Id
- `void Delete(int id)` — видаляє елемент за Id

### InMemoryRepository&lt;T&gt; : IRepository&lt;T&gt; where T : IEntity

Реалізація, яка зберігає дані в пам'яті.

- Поле: `List<T> _items`
- `Add` — кидає `ArgumentNullException` для `null` і `InvalidOperationException`, якщо Id вже є
- `Update` — кидає `KeyNotFoundException`, якщо елемента з таким Id немає
- `Delete` — кидає `KeyNotFoundException`, якщо елемента з таким Id немає
- `GetAll` — повертає копію списку

## Тести (lab7v9.Tests)

Клас `InMemoryRepositoryTests`, 11 тестів:

- Add: додавання елемента, `null`, дублікат Id
- GetById: існуючий Id, неіснуючий Id
- GetAll: список з елементами, порожній репозиторій
- Update: існуючий елемент, неіснуючий елемент
- Delete: існуючий елемент, неіснуючий елемент

## Що показує Main

- створює `InMemoryRepository<Project>` і додає три проєкти
- виводить усі проєкти, шукає за Id (існуючий і неіснуючий)
- оновлює проєкт, видаляє проєкт
- ловить помилку при видаленні неіснуючого елемента

## Запуск

```
dotnet run --project lab7v9
dotnet test
```

## Приклад виводу

<img width="672" height="381" alt="image" src="https://github.com/user-attachments/assets/3983db89-8a2f-4c83-aafb-101ae865f4d0" />


<img width="811" height="212" alt="image" src="https://github.com/user-attachments/assets/4dcdebcc-4e1f-4f9a-b4ba-3e19d0b32a5d" />

