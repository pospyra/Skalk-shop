# Интернет-магазин "Skalk-shop"

"Skalk-shop" - интернет-магазин электроники, созданный в рамках учебного проекта. Этот проект представляет собой API для интернет-магазина электроники с интересным подходом к управлению каталогом продукции.

Для полноценного взаимодействия с приложением, рекомендуется использовать также фронтендную часть, доступную в репозитории [Skalk-frontend](https://github.com/pospyra/Skalk-shop-frontend). Следуйте инструкциям в репозитории для настройки фронтендной части приложения.

## Особенности проекта

- **Хранение каталога продукции:** Каталог формируется динамически из стороннего [Nexar API](https://nexar.com/api), предоставляя уникальную интеграцию с внешними сервисами.

- **Формирование договора:** При оформлении заказа, автоматически создается договор с необходимыми деталями для корпоративных клиентов.

- **Авторизация и регистрация:** Пароли пользователей хранятся в захешированном виде, обеспечивая высокий уровень конфиденциальности.
  
## Используемые технологии

- ASP.NET Core
- PostgreSQL
- Entity Framework Core 
- Bearer Token Authentication
- [Nexar API](https://nexar.com/api)
  
## Начало работы

1. **Клонирование репозитория:**
   ```bash
   git clone https://github.com/pospyra/Skalk-shop.git

2. **Настройка базы данных:**
    Обновите `ConnectionStrings` в файле `appsettings.json` в проекте SkalkWebApi, установите необходимые параметры для вашей базы данных

    ```json
   "ConnectionStrings":
    {
      "SkalkConnectionDB": "Host=localhost;Database=Skalk;Username=username;Password=password"
    }

  3. **Установка локальных переменных среды**:

     Для использования Nexar API необходимо установить локальные переменные среды с данными для авторизации в вашем приложении NexarAPI.
     <br> _Для этого необходимо зарегистрироваться на [NexarApi](https://identity.nexar.com/Account/Login?ReturnUrl=%2Fconnect%2Fauthorize%2Fcallback%3FauthzId%3D5EFFA7DC4676AD9AEA3C3BEFEC3881F40FE1EBB66A1CD23762591275B634E6AA) и создать приложение._

      PowerShell:
      ```powershell
      $env:NEXAR_CLIENT_ID="ваш-идентификатор"
      $env:NEXAR_CLIENT_SECRET="ваш-секретный-ключ"
      ```
      
      Bash:
      ```bash
      export NEXAR_CLIENT_ID=ваш-идентификатор
      export NEXAR_CLIENT_SECRET=ваш-секретный-ключ
      ```
      <br>
      
      Если у вас нет возможности получить собственные идентификаторы, вы можете использовать следующие тестовые данные. Обратите внимание, что количество запросов ограничено: _100 бесплатных запросов в месяц_.
     
     PowerShell:
      ```powershell
      $env:NEXAR_CLIENT_ID="ae3d55b3-6c42-4a43-b80d-f370e4aea95c"
      $env:NEXAR_CLIENT_SECRET="8g8XFiTU74b2wnji6mMmyxmjeNBCwDIgANV3"
      ```

      Bash:
      ```bash
      export NEXAR_CLIENT_ID="ae3d55b3-6c42-4a43-b80d-f370e4aea95c"
      export NEXAR_CLIENT_SECRET="8g8XFiTU74b2wnji6mMmyxmjeNBCwDIgANV3"
      ```

