# Звіт до самостійної роботи №5

## 1. Поліморфізм через `System.Object`

```csharp
public override string ToString()
{
    return $"Customer: {FullName} ({City})";
}

List<object> objects = new List<object> { product, customer, order, new Note("..."), 42, "звичайний рядок" };
foreach (object obj in objects)
{
    Console.WriteLine($"{obj.GetType().Name,-10} -> {obj}");
}
```

**Як проявляється:** змінна має тип `object`, але `ToString()` — віртуальний метод, тому викликається версія реального типу об'єкта. Для `Product`, `Customer`, `Order` — моя версія, для `Note` (без override) — стандартна, яка виводить просто назву класу.

**Переваги:** `Console.WriteLine` вміє виводити будь-який об'єкт, нічого не знаючи про мої класи.

## 2. Поліморфізм через `IEnumerable<string>`

```csharp
public class MyCustomCollection : IEnumerable<string>
{
    private List<string> _items = new List<string>();

    public IEnumerator<string> GetEnumerator()
    {
        foreach (string item in _items)
        {
            yield return item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
```

**Як проявляється:** `foreach` працює з будь-яким класом, що реалізує `IEnumerable`. Метод `PrintAll(string title, IEnumerable<string> items)` однаково приймає мою колекцію, `List<string>` і масив.

**Переваги:** один код для різних колекцій; внутрішній список закритий, і його можна замінити, не змінюючи код, який перебирає колекцію.

## 3. Поліморфізм через абстрактний клас `Stream`

```csharp
static void ProcessStream(Stream stream)
{
    byte[] buffer = new byte[16];
    int read = stream.Read(buffer, 0, buffer.Length);
    ...
}

ProcessStream(fileStream);
ProcessStream(memoryStream);
```

**Як проявляється:** `Stream` — абстрактний клас. `FileStream` читає дані з файлу, `MemoryStream` — з масиву в пам'яті. `ProcessStream` викликає ті самі методи `Read`, `Length`, `Position`, а виконується реалізація конкретного потоку.

**Переваги:** один метод працює з файлами, пам'яттю, мережею. Для перевірки замість файлу можна підставити `MemoryStream`.

## Висновки

У всіх трьох випадках один і той самий код (`Console.WriteLine`, `foreach`, `ProcessStream`) працює з різними класами через спільний базовий тип: `Object`, інтерфейс `IEnumerable` або абстрактний клас `Stream`. Поліморфізм дає гнучкість (нові класи підключаються без змін старого коду) і повторне використання коду.
