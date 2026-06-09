#include <Arduino.h>
#include <Adafruit_BMP085.h>

Adafruit_BMP085 bmp;

// put function declarations here:
#define ECHOPIN 12
#define TRIGPIN 13

float Temperatura;
float Odleglosc;
float Czas;

String OdczytajCzujnikiJakoJson();
void OdczytZCzujkiOdleg ();


void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
Wire.begin(21, 22);// sda scl
if (!bmp.begin())
{
    Serial.println("Nie wykryto BMP180!");
    while (true);
}
pinMode(TRIGPIN, OUTPUT);
pinMode(ECHOPIN, INPUT);
}

void loop() {
  // put your main code here, to run repeatedly:
String json = OdczytajCzujnikiJakoJson();
Serial.println(json);
delay(1000);
}

// put function definitions here:
String OdczytajCzujnikiJakoJson()
{
Temperatura = bmp.readTemperature(); //odczytanei temperatury z czujki
OdczytZCzujkiOdleg(); //odczytanie odległości z czujki
String json = "{";// wrzucenie danych w jsona
json += "\"temperature\":";
json += String(Temperatura);
json += ",\"distanceCm\":";
json += String(Odleglosc);
json += "}";
return json;
}
void OdczytZCzujkiOdleg() {
digitalWrite(TRIGPIN, LOW);
delayMicroseconds(2);
digitalWrite(TRIGPIN, HIGH);
delayMicroseconds(10);
digitalWrite(TRIGPIN, LOW);
Czas = pulseIn(ECHOPIN, HIGH);
Odleglosc = Czas * 0.034 / 2;
}