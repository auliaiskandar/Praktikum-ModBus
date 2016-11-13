#include <SimpleModbusSlave.h>
#define ledPin 13 // onboard led
#define buttonPin 7 // push button
//////////////// registers of your slave ///////////////////
enum
{
 ADC0,
 ADC1,
 ADC2,
 ADC3,
 ADC4,
 ADC5,
 LED_STATE,
 BUTTON_STATE,
 TOTAL_ERRORS,
 // leave this one
 TOTAL_REGS_SIZE
};
unsigned int holdingRegs[TOTAL_REGS_SIZE]; // function 3 and 16 register array
void setup()
{
 modbus_configure(9600, 1, 2, TOTAL_REGS_SIZE, 0);
 pinMode(ledPin, OUTPUT);
 pinMode(buttonPin, INPUT);
}
void loop()
{
 holdingRegs[TOTAL_ERRORS] = modbus_update(holdingRegs);
 for (byte i = 0; i < 6; i++)
 {
 holdingRegs[i] = analogRead(i);
 delayMicroseconds(50);
 }

 byte buttonState = digitalRead(buttonPin); // read button states
 
 holdingRegs[BUTTON_STATE] = buttonState;

 byte ledState = holdingRegs[LED_STATE];

 if (ledState) // set led
 digitalWrite(ledPin, HIGH);
 if (ledState == 0 || buttonState) // reset led
 {
 digitalWrite(ledPin, LOW);
 holdingRegs[LED_STATE] = 0;
 }
} 

