Pharmaceutical Vault Monitor

System monitorowania warunków przechowywania leków oparty o ESP32, czujnik temperatury BMP180 oraz czujnik ultradźwiękowy HC-SR04.

Projekt został wykonany z użyciem:

ESP32
PlatformIO
Wokwi Simulator
ASP.NET Core
C#
Opis projektu

System symuluje monitorowanie skarbca farmaceutycznego.

ESP32 odczytuje:

temperaturę z czujnika BMP180,
dystans z czujnika HC-SR04 (symulacja otwartych drzwi).

Dane są wysyłane w formacie JSON przez Serial.

Bridge COM/HTTP odbiera dane z Wokwi przez TCP i udostępnia je jako API HTTP.

Klient C# pobiera dane z API i analizuje:

temperaturę,
stan drzwi,
potencjalne zagrożenia.
Architektura systemu

ESP32 → Wokwi TCP → Bridge COM/HTTP → Client

Wykorzystane technologie
ESP32
Arduino Framework
PlatformIO
BMP180
HC-SR04
Backend
ASP.NET Core Minimal API
TCP Socket
HTTP REST API
Klient
C#
HttpClient
JSON Parsing
Funkcjonalności
Odczyt temperatury
Odczyt odległości
Wysyłanie danych jako JSON
HTTP API
Monitorowanie alarmów
Obsługa błędów
Symulacja działania w Wokwi
Format danych JSON
{
  "temperature": xx.xx,
  "distanceCm": xx.xx
}
Progi alarmowe
Temperatura
minimalna: 2°C
maksymalna: 8°C
Drzwi
alarm przy dystansie > 50 cm
Endpointy HTTP
Health Check
GET /health
Odczyt danych
GET /read
Konfiguracja Wokwi
wokwi.toml
[wokwi]
version = 1

firmware = ".pio/build/esp32doit-devkit-v1/firmware.bin"
elf = ".pio/build/esp32doit-devkit-v1/firmware.elf"

[sim]
serialPort = 4000
Konfiguracja PlatformIO
[env:esp32doit-devkit-v1]
platform = espressif32
board = esp32doit-devkit-v1
framework = arduino

monitor_speed = 115200

lib_deps =
    adafruit/Adafruit BMP085 Library
Połączenia
BMP180
BMP180	ESP32
VCC	3V3
GND	GND
SDA	GPIO21
SCL	GPIO22
HC-SR04
HC-SR04	ESP32
VCC	5V
GND	GND
TRIG	GPIO13
ECHO	GPIO12
Uruchomienie projektu
1. ESP32

Uruchom symulację Wokwi w VS Code.

2. Bridge

Przejdź do folderu:

cd BridgeComHttp

Uruchom:

dotnet run
3. Client

Przejdź do folderu:

cd VaultAlarmClient

Uruchom:

dotnet run
Autor

Franciszek Budnik

Screenshots

Tutaj można dodać screeny z działania projektu.
