int ledPin = 13;
boolean ledState;


void setup()
{
	pinMode(ledPin, OUTPUT);

	Serial.begin(9600);
	ScreenInit();
}
void loop()
{
	ledState = !ledState;
	digitalWrite(ledPin, ledState);

	if (ledState)
	{
		Serial.println("Led is ON!");
		PrintLedStatus("ON");
	}
	else
	{
		Serial.println("Led is OFF!");
		PrintLedStatus("OFF");
	}

	delay(500);
}