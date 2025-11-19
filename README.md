Настройка строки подключения

Для корректного запуска приложения необходимо указать строку подключения к вашей базе данных в файле appsettings.json.
Приложение автоматически использует первую строку в секции ConnectionStrings, поэтому строка подключения должна быть расположена первой.

Пример:

{
  "ConnectionStrings": {
    "Default": "Server=localhost,1433;Database=Test;User Id=sa;Password=YourPassword;TrustServerCertificate=True"
  }
}
