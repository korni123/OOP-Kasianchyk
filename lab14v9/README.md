# Лабораторна робота №14

**Тема:** Аналіз SRP/OCP у відкритих репозиторіях.

## Обрані репозиторії

| Репозиторій | Домен | Версія для аналізу |
|---|---|---|
| [Polly](https://github.com/App-vNext/Polly) | відмовостійкість (retry, timeout, circuit breaker) | коміт `3bac1c8` |
| [Dapper](https://github.com/DapperLib/Dapper) | доступ до бази даних (micro-ORM) | коміт `eb47546` |

Усі посилання нижче ведуть на конкретний коміт, тому номери рядків не зміняться.

## Приклади

### 1. SRP, вдале застосування: Polly, `RetryResilienceStrategy` і `RetryHelper`

Файли: [RetryResilienceStrategy.cs](https://github.com/App-vNext/Polly/blob/3bac1c877ac5b94f0c09cbf4efe4c15fafce639d/src/Polly.Core/Retry/RetryResilienceStrategy.cs#L5-L148) (клас з рядка 5), [RetryHelper.cs](https://github.com/App-vNext/Polly/blob/3bac1c877ac5b94f0c09cbf4efe4c15fafce639d/src/Polly.Core/Retry/RetryHelper.cs#L3-L145) (`GetRetryDelay` на рядку 17, `ApplyJitter` на рядку 50), [RetryStrategyOptions.TResult.cs](https://github.com/App-vNext/Polly/blob/3bac1c877ac5b94f0c09cbf4efe4c15fafce639d/src/Polly.Core/Retry/RetryStrategyOptions.TResult.cs), [RetryResiliencePipelineBuilderExtensions.cs](https://github.com/App-vNext/Polly/blob/3bac1c877ac5b94f0c09cbf4efe4c15fafce639d/src/Polly.Core/Retry/RetryResiliencePipelineBuilderExtensions.cs#L25-L59).

- **Де видно принцип.** Робота розділена між чотирма файлами. `RetryResilienceStrategy` відповідає лише за цикл повторних спроб (`ExecuteCore`, рядок 46). Обчислення затримки (backoff і jitter) винесене в `RetryHelper`. Налаштування лежать в окремому класі `RetryStrategyOptions`, а підключення стратегії до конвеєра зроблено в методі розширення `AddRetry`.
- **Переваги.** Кожен файл має одну причину змінюватись. Математику затримок можна тестувати окремо, без запуску повторів. Клас короткий (148 рядків) і легко читається.
- **Що зміниться при новій функції.** Новий тип затримки додається в `RetryHelper` і enum `DelayBackoffType`, цикл повторів не чіпається. Нова опція (наприклад, ще один делегат) додається в `RetryStrategyOptions`.

### 2. SRP, вдале застосування: Polly, `TimeoutResilienceStrategy`

Файл: [TimeoutResilienceStrategy.cs](https://github.com/App-vNext/Polly/blob/3bac1c877ac5b94f0c09cbf4efe4c15fafce639d/src/Polly.Core/Timeout/TimeoutResilienceStrategy.cs#L6-L104).

- **Де видно принцип.** Клас займається тільки обмеженням часу виконання: створює `CancellationToken` з таймаутом, запускає операцію і, якщо час вийшов, повертає `TimeoutRejectedException`. Він нічого не знає про повтори, fallback чи circuit breaker, їх реалізують окремі класи.
- **Переваги.** Стратегію можна використовувати окремо або в будь-якій комбінації з іншими. Помилка в таймауті не торкнеться повторів.
- **Ризики.** Майже немає. Клас використовує допоміжні класи (наприклад, `CancellationTokenSourcePool`), але вони теж мають одну задачу.
- **Що зміниться при новій функції.** Динамічний таймаут уже підтримується через `TimeoutGenerator`. Для іншої поведінки достатньо написати нову стратегію, не змінюючи цю.

### 3. Порушення SRP: Dapper, клас `SqlMapper` (файл `SqlMapper.cs`)

Файл: [SqlMapper.cs](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.cs#L1-L4207), 4207 рядків.

- **Де видно порушення.** В одному класі зібрано дуже різні задачі: кеш запитів ([рядок 61](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.cs#L61)), відповідність типів .NET і типів бази даних ([рядок 204](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.cs#L204)), реєстр обробників типів ([рядки 345-361](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.cs#L345-L361)), публічні методи `Execute` ([559](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.cs#L559)) і `Query` ([846](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.cs#L846)), заміна літералів у SQL ([2430-2512](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.cs#L2430-L2512)), реєстрація карт типів ([3293-3300](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.cs#L3293-L3300)) і генерація IL-коду для десеріалізації ([3381](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.cs#L3381), у файлі 159 викликів `Emit(OpCodes...)`). Клас розбито на 20 файлів через `partial`, але це все одно один клас.
- **Ризики.** Клас має багато причин змінюватись: нова база даних, новий тип, нове правило кешування, зміна IL. Зміна в одному місці може зачепити інше, бо все ділить спільний статичний стан. Новачку важко зорієнтуватись.
- **Що зміниться при новій функції.** Щоб додати підтримку нового типу чи нової опції запиту, потрібно редагувати той самий величезний клас, ризикуючи зламати чужу частину.

### 4. Порушення SRP: Dapper, метод `QueryImpl<T>`

Файл: [SqlMapper.cs, рядки 1204-1265](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.cs#L1204-L1265).

- **Де видно порушення.** Один метод одночасно: будує ідентифікатор запиту і шукає його в кеші (1206-1207), готує команду (`SetupCommand`), керує станом з'єднання (відкриває і закриває), виконує читач, перебудовує десеріалізатор, якщо змінився набір колонок ([рядок 1230](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.cs#L1230)), читає рядки через `yield return`, а в `finally` скасовує команду, закриває читач і з'єднання.
- **Ризики.** Метод важко тестувати окремо: його неможливо перевірити без бази і з'єднання. Зміна однієї частини (наприклад, керування з'єднанням) вимагає обережності, щоб не зламати іншу. У методі є складні умови з `wasClosed`.
- **Що зміниться при новій функції.** Додавання, наприклад, нового режиму читання або іншого правила кешу змусить змінювати цей самий метод. Схожа логіка повторюється і в сусідніх методах (наприклад, на [рядках 1153](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.cs#L1153) і [1368](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.cs#L1368)), тому зміну доведеться робити в кількох місцях.

### 5. OCP: Polly, `ResilienceStrategy` і `AddStrategy`

Файли: [ResilienceStrategy.cs](https://github.com/App-vNext/Polly/blob/3bac1c877ac5b94f0c09cbf4efe4c15fafce639d/src/Polly.Core/ResilienceStrategy.cs#L6-L27) (абстрактний `ExecuteCore` на рядку 23), [ResiliencePipelineBuilderExtensions.cs](https://github.com/App-vNext/Polly/blob/3bac1c877ac5b94f0c09cbf4efe4c15fafce639d/src/Polly.Core/ResiliencePipelineBuilderExtensions.cs#L69) (`AddStrategy`), приклади використання: [Retry](https://github.com/App-vNext/Polly/blob/3bac1c877ac5b94f0c09cbf4efe4c15fafce639d/src/Polly.Core/Retry/RetryResiliencePipelineBuilderExtensions.cs#L30), [Fallback](https://github.com/App-vNext/Polly/blob/3bac1c877ac5b94f0c09cbf4efe4c15fafce639d/src/Polly.Core/Fallback/FallbackResiliencePipelineBuilderExtensions.cs#L32), [Hedging](https://github.com/App-vNext/Polly/blob/3bac1c877ac5b94f0c09cbf4efe4c15fafce639d/src/Polly.Core/Hedging/HedgingResiliencePipelineBuilderExtensions.cs#L33).

- **Де видно принцип.** Усі стратегії успадковують абстрактний клас `ResilienceStrategy` з одним методом. Конвеєр не знає, які саме стратегії в ньому є, він працює з абстракцією. Нова стратегія підключається методом `AddStrategy`, який приймає будь-яку стратегію.
- **Переваги.** Retry, Fallback, Hedging і Timeout додано без змін у конвеєрі. Користувач бібліотеки теж може написати власну стратегію.
- **Що зміниться при новій функції.** Достатньо створити новий клас-стратегію і метод розширення `AddMyStrategy`, наявний код лишається недоторканим. Додатково розширення можливе без нових класів: наприклад, `DelayGenerator` у стратегії Retry ([рядок 40](https://github.com/App-vNext/Polly/blob/3bac1c877ac5b94f0c09cbf4efe4c15fafce639d/src/Polly.Core/Retry/RetryResilienceStrategy.cs#L40)) дозволяє задати власне правило затримки делегатом.

### 6. OCP: Dapper, `ITypeHandler` і `AddTypeHandler`

Файли: [SqlMapper.ITypeHandler.cs](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.ITypeHandler.cs#L1-L29), [SqlMapper.TypeHandler.cs](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.TypeHandler.cs#L1-L86), [SqlMapper.cs, AddTypeHandler](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.cs#L345-L361), пошук обробника: [рядок 488](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.cs#L488), [1977](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.cs#L1977), [3138](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.cs#L3138).

- **Де видно принцип.** Інтерфейс `ITypeHandler` має два методи: `SetValue` (як передати значення в параметр) і `Parse` (як прочитати значення з бази). Користувач пише свій обробник і реєструє його через `AddTypeHandler`. Dapper під час роботи шукає обробник у словнику `typeHandlers` і використовує його замість стандартної логіки.
- **Переваги.** Підтримку нового типу (JSON-колонки, власного класу `Money` тощо) можна додати без змін у коді Dapper. Схожий механізм `ITypeMap` / `SetTypeMap` ([рядок 3300](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.cs#L3300)) дозволяє змінити відповідність колонок і властивостей.
- **Ризики.** Обробники реєструються в статичному словнику, тому вони діють на весь застосунок, а не на один запит. Вбудована таблиця типів на [рядку 204](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.cs#L204) жорстко задана в коді, тому для неї розширення можливе лише через обробники.
- **Що зміниться при новій функції.** Для нового типу пишемо новий клас-обробник і одним викликом реєструємо його. Код Dapper не змінюється.

## Висновки

SRP у Polly виконується добре: кожна стратегія (retry, timeout) є окремим коротким класом, а допоміжні задачі, як-от обчислення затримки, винесені в окремі файли. Завдяки цьому код легко читати, тестувати і розширювати. У Dapper клас `SqlMapper` порушує SRP: у ньому 4207 рядків і десяток різних відповідальностей від кешу до генерації IL. Метод `QueryImpl` теж робить забагато: кешує, керує з'єднанням, виконує запит і читає результат. Це пояснюється тим, що Dapper створювався як швидкий і компактний інструмент, а продуктивність і стабільність публічного API важливіші за ідеальну структуру. OCP добре реалізований в обох проєктах: `ResilienceStrategy` у Polly і `ITypeHandler` у Dapper дозволяють додавати нову поведінку без зміни існуючого коду. Виходить, що навіть проєкт з порушенням SRP може добре підтримувати OCP, бо це різні принципи. Головна ознака хорошого коду в цьому аналізі: нову функцію можна додати, зачепивши мінімум існуючого коду.

## Пропозиція рефакторингу для найгіршого прикладу

**Найгірший приклад:** клас `SqlMapper` ([SqlMapper.cs](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.cs#L1-L4207)).

**Пропозиція.** Розділити його на невеликі класи, а в `SqlMapper` залишити тільки публічні методи `Query` та `Execute`, які викликають ці класи:

- `QueryCache` — кеш запитів ([рядок 61](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.cs#L61)) і методи роботи з ним
- `TypeRegistry` — таблиця типів ([204](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.cs#L204)), обробники типів ([345-361](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.cs#L345-L361)) і карти типів ([3293-3300](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.cs#L3293-L3300))
- `DeserializerFactory` — генерація IL-коду для десеріалізації ([3381](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.cs#L3381) і далі)
- `CommandRunner` — керування з'єднанням, командою і читачем з `QueryImpl` ([1204-1265](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.cs#L1204-L1265))
- `LiteralReplacer` — заміна літералів у SQL ([2430-2512](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.cs#L2430-L2512))

**Обґрунтування.** Кожен клас матиме одну причину змінюватись, а IL-генерацію можна тестувати без бази даних. Зміна кешу більше не зачепить виконання запитів. Починати треба з `DeserializerFactory`, бо це майже чисті функції від типу і читача, тому ризик найменший. Публічні методи `Query` і `Execute` залишаться на місці, тому код користувачів бібліотеки не зламається.

**Ризик.** Рефакторинг великий, а Dapper дуже чутливий до продуктивності. Додаткові виклики між класами можуть уповільнити код, тому кожен крок треба перевіряти бенчмарками. Якщо потреби в таких змінах немає, робити їх "під патерн" не варто, бо це витрата часу без користі для користувачів.
