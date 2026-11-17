# Самостійна робота №20

**Тема:** Реалізація exponential backoff.

Проєкт: `IndependentWork14`. Висновок про межі застосування Retry — у файлі [REPORT.md](REPORT.md).

## RetryPolicy

- Приватні поля: `_maxRetries` (int), `_initialDelay` (TimeSpan), `_jitterFactor` (double), `_random` (Random)
- Конструктор: `RetryPolicy(int maxRetries, TimeSpan initialDelay, double jitterFactor = 0.2)`
- Метод: `T Execute<T>(Func<T> operation, Func<Exception, bool>? shouldRetry = null)`
  - виконує операцію; при винятку чекає і пробує знову, не більше `maxRetries` разів
  - затримка: `initialDelay * 2^retryAttempt * (1 + random.NextDouble() * jitterFactor * 2 - jitterFactor)`
  - `shouldRetry` (необов'язковий) вирішує, чи повторювати спробу для цього винятку
  - кожна спроба, затримка і причина невдачі виводяться в консоль
  - після останньої невдачі кидає той самий виняток

## Що показує Main

Політика: 4 повтори, початкова затримка 300 мс, jitter 0.2.

1. Операція двічі завершується помилкою, потім успішна
2. Операція завжди завершується помилкою: після всіх спроб кидається виняток
3. `shouldRetry`: повторюється тільки `TimeoutException`, а `ArgumentException` одразу припиняє спроби

## Запуск

```
cd Самостійні/Самостійна
dotnet run --project IndependentWork14
```

## Приклад виводу

(скріншот)
