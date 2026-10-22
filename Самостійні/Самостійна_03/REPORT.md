# Звіт з аналізу інкапсуляції в Open-Source проєкті

**Самостійна робота №3.** Тема: Аналіз інкапсуляції в open-source проєктах. Практики валідації полів.

## 1. Обраний проєкт

- **Назва:** Newtonsoft.Json (Json.NET) — найпопулярніша бібліотека для роботи з JSON у .NET
- **Посилання на GitHub:** https://github.com/JamesNK/Newtonsoft.Json
- **Аналізовані файли** (посилання зафіксовані на коміт `52fa3ae`, щоб номери рядків не «з'їжджали»):
  - [`Src/Newtonsoft.Json/JsonTextWriter.cs`](https://github.com/JamesNK/Newtonsoft.Json/blob/52fa3aef1f2cadcd3a3f874251eddc98d3efbbaa/Src/Newtonsoft.Json/JsonTextWriter.cs)
  - [`Src/Newtonsoft.Json/JsonSerializerSettings.cs`](https://github.com/JamesNK/Newtonsoft.Json/blob/52fa3aef1f2cadcd3a3f874251eddc98d3efbbaa/Src/Newtonsoft.Json/JsonSerializerSettings.cs)
  - [`Src/Newtonsoft.Json/Linq/JObject.cs`](https://github.com/JamesNK/Newtonsoft.Json/blob/52fa3aef1f2cadcd3a3f874251eddc98d3efbbaa/Src/Newtonsoft.Json/Linq/JObject.cs) та [`Linq/JToken.cs`](https://github.com/JamesNK/Newtonsoft.Json/blob/52fa3aef1f2cadcd3a3f874251eddc98d3efbbaa/Src/Newtonsoft.Json/Linq/JToken.cs)

Проєкт обрано, бо він великий, але з дуже зрозумілою структурою (`Src/Newtonsoft.Json/...`), і в ньому є всі прийоми, про які питає завдання: приватні поля, властивості з логікою, read-only властивості, індексатори та перевантажені оператори.

## 2. Аналіз інкапсуляції

### Клас: `JsonTextWriter`

- **Опис класу:** записує JSON у `TextWriter` (файл, рядок, мережевий потік). Наслідується від абстрактного `JsonWriter`.
- **Поля:** усі поля приватні, частина — `readonly`. Ззовні неможливо підмінити внутрішній `TextWriter` чи буфер.
  ```csharp
  private readonly TextWriter _writer;
  private char _indentChar;
  private int _indentation;
  private char _quoteChar;
  private bool _quoteName;
  private char[]? _writeBuffer;
  private char[]? _indentChars;
  ```
  ([рядки 46–55](https://github.com/JamesNK/Newtonsoft.Json/blob/52fa3aef1f2cadcd3a3f874251eddc98d3efbbaa/Src/Newtonsoft.Json/JsonTextWriter.cs#L46-L55))
- **Властивості:** доступ до налаштувань тільки через read-write властивості з логікою в `set`. Наприклад, `IndentChar` при зміні символу ще й скидає закешований масив відступів `_indentChars` — користувач класу про цей кеш навіть не знає:
  ```csharp
  public char IndentChar
  {
      get => _indentChar;
      set
      {
          if (value != _indentChar)
          {
              _indentChar = value;
              _indentChars = null;
          }
      }
  }
  ```
  ([рядки 125–136](https://github.com/JamesNK/Newtonsoft.Json/blob/52fa3aef1f2cadcd3a3f874251eddc98d3efbbaa/Src/Newtonsoft.Json/JsonTextWriter.cs#L125-L136))
- **Конструктор** задає коректний початковий стан (лапки `"`, відступ 2 пробіли) і не дозволяє створити об'єкт без `TextWriter` — `ArgumentNullException` (рядки 151–156).
- **Індексатори/Оператори:** немає.

### Клас: `JsonSerializerSettings`

- **Опис класу:** набір налаштувань серіалізації (форматування, обробка null, глибина вкладеності тощо).
- **Поля:** значення налаштувань зберігаються у полях з модифікатором `internal` і nullable-типом. `null` означає «користувач нічого не задавав», тоді властивість повертає значення за замовчуванням. Константи за замовчуванням теж `internal const`.
  ```csharp
  internal const ReferenceLoopHandling DefaultReferenceLoopHandling = ReferenceLoopHandling.Error;
  internal Formatting? _formatting;
  internal int? _maxDepth;
  internal bool _maxDepthSet;
  ```
  `internal` — компроміс: ззовні бібліотеки поля не видно, але інші класи Json.NET (наприклад `JsonSerializer`) можуть читати їх напряму.
- **Властивості:**
  - звичайні властивості з логікою в `get`: `public Formatting Formatting { get => _formatting ?? DefaultFormatting; set => _formatting = value; }` (рядки 356–360);
  - **автоматичні властивості** там, де жодної логіки не потрібно: `public IList<JsonConverter> Converters { get; set; }`, `public IContractResolver? ContractResolver { get; set; }` (рядки 156, 235).
- **Індексатори/Оператори:** немає.

### Клас: `JObject` (і базовий `JToken`)

- **Опис класу:** JSON-об'єкт у вигляді дерева (LINQ to JSON).
- **Поля:** властивості об'єкта зберігаються в приватній колекції — `private readonly JPropertyKeyedCollection _properties` (рядок 64). Напряму змінити колекцію неможливо, тільки через методи/індексатор.
- **Властивості:** `JToken.Parent` — публічне читання, але **internal set** — змінювати батьківський вузол може тільки сама бібліотека, щоб дерево не зламалося:
  ```csharp
  public JContainer? Parent
  {
      get { return _parent; }
      internal set { _parent = value; }
  }
  ```
  `JContainer.Count => ChildrenTokens.Count` — read-only властивість (тільки `get`).
- **Індексатори:** `JObject` має індексатор за назвою властивості, завдяки якому можна писати `obj["name"] = "Ivan"`. У `get` перевіряється аргумент, у `set` — або оновлюється існуюча властивість, або додається нова з генерацією події `PropertyChanged`:
  ```csharp
  public JToken? this[string propertyName]
  {
      get
      {
          ValidationUtils.ArgumentNotNull(propertyName, nameof(propertyName));
          JProperty? property = Property(propertyName, StringComparison.Ordinal);
          return property?.Value;
      }
      set { /* оновлення або Add(propertyName, value) + OnPropertyChanged */ }
  }
  ```
  ([рядки 361–387](https://github.com/JamesNK/Newtonsoft.Json/blob/52fa3aef1f2cadcd3a3f874251eddc98d3efbbaa/Src/Newtonsoft.Json/Linq/JObject.cs#L361-L387))
- **Перевантажені оператори:** у `JToken` є десятки операторів перетворення типів. Неявний `int → JToken` дозволяє писати `obj["age"] = 20`, а явний `(int)token` перевіряє тип і кидає `ArgumentException`, якщо значення не число:
  ```csharp
  public static implicit operator JToken(int value)
  {
      return new JValue(value);
  }

  public static explicit operator int(JToken value)
  {
      JValue? v = EnsureValue(value);
      if (v == null || !ValidateToken(v, NumberTypes, false))
      {
          throw new ArgumentException("Can not convert {0} to Int32.".FormatWith(...));
      }
      ...
  }
  ```
  ([JToken.cs, рядки 819 і 1730](https://github.com/JamesNK/Newtonsoft.Json/blob/52fa3aef1f2cadcd3a3f874251eddc98d3efbbaa/Src/Newtonsoft.Json/Linq/JToken.cs#L819))

## 3. Практики валідації у властивостях

- **Приклад 1: діапазон значень (`if` + `ArgumentException`).** Відступ не може бути від'ємним (`JsonTextWriter.Indentation`, рядки 90–102):
  ```csharp
  public int Indentation
  {
      get => _indentation;
      set
      {
          if (value < 0)
          {
              throw new ArgumentException("Indentation value must be greater than 0.");
          }
          _indentation = value;
      }
  }
  ```
- **Приклад 2: дозволений набір значень + оновлення залежного стану.** Лапки в JSON можуть бути тільки `"` або `'`. Після успішної зміни перераховуються прапорці екранування символів (`JsonTextWriter.QuoteChar`, рядки 107–120):
  ```csharp
  set
  {
      if (value != '"' && value != '\'')
      {
          throw new ArgumentException(@"Invalid JavaScript string quote character. Valid quote characters are ' and "".");
      }
      _quoteChar = value;
      UpdateCharEscapeFlags();
  }
  ```
- **Приклад 3: валідація + прапорець «значення задано».** `JsonSerializerSettings.MaxDepth` приймає тільки додатні числа, використовує `nameof(value)` для назви параметра, і окремо запам'ятовує, що користувач задав значення сам (рядки 337–350):
  ```csharp
  set
  {
      if (value <= 0)
      {
          throw new ArgumentException("Value must be positive.", nameof(value));
      }
      _maxDepth = value;
      _maxDepthSet = true;
  }
  ```
- **Приклад 4: перевірка на null через допоміжний клас.** У індексаторі `JObject` замість ручного `if` використовується `ValidationUtils.ArgumentNotNull(...)` — однакова перевірка винесена в один утилітний метод і викликається по всій бібліотеці.

- **Висновки щодо валідації:** Json.NET використовує «захисне програмування» — помилка виявляється одразу в момент присвоєння неправильного значення (fail fast), а не пізніше під час запису JSON, коли причину знайти важко. Атрибути `System.ComponentModel.DataAnnotations` у бібліотеці не використовуються, і це логічно: атрибути зручні для моделей у ASP.NET (їх перевіряє фреймворк), а бібліотеці низького рівня потрібна миттєва перевірка в `set`. Недолік — тексти винятків іноді неточні (у `Indentation` написано «greater than 0», хоча 0 дозволено), і не всюди вказується `nameof(value)`.

## 4. Загальні висновки

1. Інкапсуляція в Json.NET дотримана послідовно: всі поля закриті (`private`/`internal`), зовнішній код працює тільки через властивості, індексатори та методи.
2. Властивості — не просто «обгортки» над полями: в них є валідація, скидання кешу (`IndentChar`), перерахунок залежного стану (`QuoteChar`), значення за замовчуванням (`Formatting`).
3. Рівень доступу підбирається під задачу: `internal set` у `Parent` захищає цілісність дерева JSON, а автоматичні властивості використовуються там, де логіка не потрібна, — код від цього коротший.
4. Індексатор і перевантажені оператори роблять API зручним (`obj["age"] = 20`, `(int)obj["age"]`), але всередині все одно стоїть перевірка типів, тож зручність не ламає надійність.
5. Для власних класів варто брати такий самий підхід: приватні поля, валідація в `set` з `ArgumentException`/`ArgumentOutOfRangeException` і `nameof`, read-only властивості для того, що не має змінюватися після створення.
