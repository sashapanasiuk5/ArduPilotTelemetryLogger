# Про плагін

**TelemetryLogger** — це спеціалізований плагін для наземної станції керування Mission Planner, призначений для автономного збору, детального логування телеметричних даних безпілотного літального апарата та експорту звітів у форматах CSV і GPX.

## Вимоги

**Програмне забезпечення**: Mission Planner 1.3.83

**Фреймворк**: .NET Framework 4.7.2

**Середовище розробки**: Visual Studio (для збирання з вихідного коду)

# Як зібрати та встановити плагін

### Збірка

1. Клонуйте репозиторій або скопіюйте папку плагіна в `MissionPlanner\Plugins`.
2. Відкрийте solution `MissionPlanner.sln` у Visual Studio.
3. Зберіть проєкт плагіна у складі solution Mission Planner.
4. Після успішної збірки DLL плагіна має з'явитися в каталозі:

   ```text
   MissionPlanner\bin\Debug\net461\plugins
   ```

### Встановлення

1. Знайдіть зібраний DLL у каталозі, зазначеному вище.
2. Скопіюйте DLL до папки `plugins` уже встановленої версії Mission Planner. Зазвичай це папка `Mission Planner\plugins` у каталозі встановлення програми.
3. Запустіть або перезапустіть Mission Planner, щоб завантажити плагін.

> Якщо плагін не завантажується, перевірте, чи DLL зібраний проти сумісної версії Mission Planner та чи всі потрібні залежності доступні в середовищі запуску.

# Довідник API — TelemetryLogger

## 1. Базовий клас і життєвий цикл плагіна

| Елемент | Сигнатура / тип | Призначення |
|---|---|---|
| Базовий клас | `MissionPlanner.Plugin.Plugin` | Базовий клас плагіна Mission Planner. |
| `Name` | `public override string Name` | Повертає назву плагіна: `TelemetryLoggerPlugin`. |
| `Version` | `public override string Version` | Версія плагіна: `1.0.0`. |
| `Author` | `public override string Author` | Автор плагіна. |
| `Init()` | `public override bool Init()` | Читає `TelemetryLogFrequency` і задає `loopratehz`; якщо налаштування відсутнє, використовує `1 Hz`. |
| `Loaded()` | `public override bool Loaded()` | Реєструє сторінку налаштувань і підписується на події підключення/відключення. |
| `Loop()` | `public override bool Loop()` | Поки є з'єднання, записує телеметрію, перевіряє зміни стану, періодично скидає буфери на диск і накопичує точки GPX. |
| `Exit()` | `public override bool Exit()` | Закриває логери та відписується від подій. |
| `loopratehz` | поле базового класу, числове значення частоти | Частота виклику `Loop()`. У плагіні задається з `TelemetryLogFrequency`. |

## 2. API телеметрії (`Host.cs`)

Дані читаються через `Host.cs` — об'єкт стану Mission Planner. У таблиці вказано фактичні члени, які використовує код плагіна.

| Член | Використання | Тип C# |
|---|---|---|
| `Host.cs.lat` | Широта, CSV `latitude`, GPX | `double`-сумісне числове значення; звірити оголошення у версії MP |
| `Host.cs.lng` | Довгота, CSV `longitude`, GPX | `double`-сумісне числове значення; звірити оголошення у версії MP |
| `Host.cs.altasl` | Висота над середнім рівнем моря, м | `double`-сумісне числове значення; звірити оголошення у версії MP |
| `Host.cs.airspeed` | Повітряна швидкість, м/с у CSV `speed_mps` | `double`-сумісне числове значення; звірити оголошення у версії MP |
| `Host.cs.pitch` | Тангаж, градуси | `double`-сумісне числове значення; звірити оголошення у версії MP |
| `Host.cs.roll` | Крен, градуси | `double`-сумісне числове значення; звірити оголошення у версії MP |
| `Host.cs.yaw` | Курс/рискання, градуси | `double`-сумісне числове значення; звірити оголошення у версії MP |
| `Host.cs.battery_voltage` | Напруга батареї, В | `double`-сумісне числове значення; звірити оголошення у версії MP |
| `Host.cs.current` | Струм, А | `double`-сумісне числове значення; звірити оголошення у версії MP |
| `Host.cs.gpstime` | Час GPS, CSV `gps_time` | `DateTime`-сумісне значення; звірити оголошення у версії MP |
| `Host.cs.armed` | Стан arm/disarm | `bool` |
| `Host.cs.mode` | Назва режиму польоту | `string` |
| `Host.cs.gpsstatus` | Тип GPS-фіксації першого приймача | Перетворюється на `byte`, далі на `MAVLink.GPS_FIX_TYPE` |
| `Host.cs.gpsstatus2` | Тип GPS-фіксації другого приймача | Перетворюється на `byte`, далі на `MAVLink.GPS_FIX_TYPE` |
| `Host.comPort.MAV.lastvalidpacket` | Час останнього валідного MAVLink-пакета; перевірка застарілих даних | У коді використовується як `DateTime` |


## 3. Події з'єднання

| API | Сигнатура, яку використовує плагін | Призначення |
|---|---|---|
| `Host.comPort.MAVDetected` | `EventHandler<(byte, byte)>` у версії API, з якою зібрано плагін | `OnConnected`: запускає файли логів, очищує точки GPX і підписується на зміни стану. |
| `Host.comPort.CommsClose` | `EventHandler` | `OnDisConnected`: закриває логери, формує GPX та відписується від змін стану. |
| `StateTracker.OnStateChanged` | `EventHandler<StateChangedEventArgs>` | Повідомляє про зміну armed, режиму польоту або типу GPS-фіксації. |


## 5. Режими польоту та GPS-фіксація

### Режим польоту

- Плагін читає `Host.cs.mode` як рядок.
- `StateTracker.Update(...)` порівнює поточне значення з попереднім.
- Після зміни створюється подія `MODE SET TO <НАЗВА>`; назва переводиться у верхній регістр.
- У плагіні не виконується окреме перетворення числового MAVLink-коду режиму на текст. Назва береться з `CurrentState.mode`, тож її формування залежить від реалізації Mission Planner та типу апарата.

### GPS-фіксація

Плагін перевіряє обидва поля — `gpsstatus` і `gpsstatus2`. Значення приводяться до `byte`, інтерпретуються як `MAVLink.GPS_FIX_TYPE` і перетворюються на текст через `EnumExtensions.GetDescription()`.

`GetDescription()` повертає атрибут `DescriptionAttribute`, якщо він визначений для елемента enum; інакше повертає назву елемента enum.

Створюються події `GPS1 MODE: <опис>` та `GPS2 MODE: <опис>`. Числові значення й описи відповідають enum `GPS_FIX_TYPE` у використаній збірці MAVLink.

## 6. Формат CSV

### Телеметрія

Файл має префікс `telemetry-`. Заголовок, визначений у коді:

```csv
timestamp,gps_time,latitude,longitude,altitude_asl_m,speed_mps,roll_deg,pitсh_deg,yaw_deg,voltage_V,current_A,is_stale,clock_sync_warning
```

| Поле | Значення |
|---|---|
| `timestamp` | Час комп'ютера наземної станції, `DateTime.UtcNow`, формат `yyyy-MM-dd HH:mm:ss.fff`. |
| `gps_time` | GPS-час із `Host.cs.gpstime`, формат `yyyy-MM-dd HH:mm:ss.fff`. |
| `latitude`, `longitude` | Координати. |
| `altitude_asl_m` | Висота над середнім рівнем моря, метри (`Host.cs.altasl`). |
| `speed_mps` | Повітряна швидкість із `Host.cs.airspeed`; назва поля передбачає м/с. |
| `roll_deg`, `pitch_deg`, `yaw_deg` | Орієнтація в градусах. |
| `voltage_V`, `current_A` | Напруга та струм батареї. |
| `is_stale` | `true`, якщо від останнього валідного пакета минуло понад 3 секунди. |
| `clock_sync_warning` | `true`, якщо різниця між UTC-часом комп'ютера та `gpstime` перевищує 1 секунду. |

### Події

Файл має префікс `events-` і заголовок:

```csv
timestamp,gps_time,event
```

Події, що генеруються `StateTracker`:

- `ARMED` / `DISARMED`;
- `MODE SET TO <MODE>`;
- `GPS1 MODE: <опис>`;
- `GPS2 MODE: <опис>`.

Часова мітка події береться з `DateTime.UtcNow`, а `gps_time` — з `Host.cs.gpstime`.

## 7. Запис на диск і конфігурація

| API / налаштування | Значення |
|---|---|
| `TelemetryLogFrequency` | Частота циклу в налаштуваннях Mission Planner; за відсутності значення `Init()` встановлює `1 Hz`. |
| `TelemetryLogDir` | Тека для логів. Якщо не задана, використовується `Settings.GetUserDataDirectory() + @"logs\TelemetryLogger"`. |
| `TelemetryFileLogger.StartLogging(string logFolderPath)` | Відкриває новий файл із заголовком. |
| `TelemetryFileLogger.Write(BaseLog log)` | Записує рядок CSV. |
| `TelemetryFileLogger.DelayedFlush()` | Скидає буфер, коли минуло щонайменше 2 секунди з попереднього скидання. |
| `TelemetryFileLogger.Flush()` | Викликає `StreamWriter.Flush()`. |
| `TelemetryFileLogger.Dispose()` | Скидає буфер і закриває `StreamWriter`. |

Реалізовано ротацію логів за максимальним розміром файлу. Новий файл створююється при перевищення ліміту у `15 KB`. Фрагменти мають часову мітку у назві та суфікс `_log.txt`.

## 8. GPX

Після відключення `GpxGenerator.Generate(...)` створює GPX 1.1 трек. Кожна точка містить широту, довготу, висоту з `Host.cs.altasl` та UTC-час додавання точки. Файл має назву на кшталт `yyyy-MM-ddTHHmm.gpx`.

## 9. Збірка та залежності

| Параметр | Значення з проєкту |
|---|---|
| Тип збірки | Class Library (`OutputType=Library`) |
| Цільова платформа | `.NET Framework 4.7.2` (`TargetFrameworkVersion=v4.7.2`) |
| Основна збірка плагіна | `TelemetryLogger.dll` |
| API Mission Planner | Проєктні посилання на `MissionPlanner.csproj`, `MAVLink.csproj` та інші проєкти з вихідного дерева Mission Planner |
| NuGet/зовнішні збірки | `OpenTK 3.1.0`, `SharpDX 4.1.0`, `SharpDX.DirectInput 4.1.0` (шляхи вказані через `packages\...`) |
| Розміщення DLL | Тека `plugins/` Mission Planner; вихідні шляхи проєкту налаштовані на `bin\Debug\net461\plugins\` і `bin\Release\net461\plugins\` |


## 10. Підключення до SITL

Для запуску SITL потрібно ввести команду `sim_vehicle.py` в теці `ArduCopter`. Після чого буде розгорнута симуляція дрона на порту UDP 14550