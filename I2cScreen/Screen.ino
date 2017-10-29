#include <Wire.h> 
#include <LiquidCrystal_I2C.h>

LiquidCrystal_I2C lcd(0x27, 16, 2); // Для экрана 16х2 (двухстрочный)

void ScreenInit()
{
	lcd.begin();
	lcd.backlight();
}
void PrintHeader()
{
	lcd.setCursor(0, 0);
	lcd.print("Led status:");
}
void PrintLedStatus(char* state)
{
	lcd.clear();
	PrintHeader();
	lcd.setCursor(0, 1);
	lcd.print(state);
}