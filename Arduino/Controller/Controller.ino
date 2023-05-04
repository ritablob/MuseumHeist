// variables for LED test
int incomingByte[2];
int ledPin = 11;
bool LED = false;

// rotary encoder stuff
int counter = 0;
bool direction;
int pin_clk_last;
int pin_clk_current;
int pin_clk = 3;
int pin_dt = 4;
int button_pin = 5;

// button stuff
int button = 6;
int buttonValue;
int lastButtonValue;

// light barrier stuff
int lightSensor = 9;
int lightvalue;
int lastlightvalue;

// buzzer stuff
#include "pitches.h"
int buzzerPin = 8;
bool playSound;
int melody[] = {
  NOTE_C4, NOTE_G3, NOTE_G3, NOTE_A3, NOTE_G3, 0, NOTE_B3, NOTE_C4
};

// note durations: 4 = quarter note, 8 = eighth note, etc.:
int noteDurations[] = {

  4, 8, 8, 4, 4, 4, 4, 4
};

void setup() {
  // led test
  pinMode(ledPin, OUTPUT);
  digitalWrite(ledPin, LOW);

  // rotary encoder test
  pinMode(pin_clk,INPUT);
  pinMode(pin_dt,INPUT);
  pinMode(button_pin,INPUT);
  digitalWrite(pin_clk,true);
  digitalWrite(pin_dt,true);
  digitalWrite(button_pin,true);
  pin_clk_last = digitalRead(pin_clk);
  counter = 0;

  // light barrier test
  pinMode (lightSensor, INPUT);
  digitalWrite(lightSensor, HIGH);

  // buzzer stuff
  pinMode(buzzerPin, OUTPUT);

  // button stuff
  pinMode(button, INPUT);
  digitalWrite(button, HIGH);
  playSound = false;

  Serial.begin(9600);
}

void loop() {
  // read data
  if (Serial.available() > 0){
    while (Serial.peek() == 'L'){
      Serial.read();
      incomingByte[0] = Serial.parseInt();
      //led related data
      if (incomingByte[0] == 1) {LED = true;} else if (incomingByte[0] == 0) {LED = false;}
      // buzzer related data
      else if (incomingByte[0] == 3) { if (playSound) {playSound = false; } else {playSound = true;}}
      else if (incomingByte[0] == 4) {playSound = false; digitalWrite (buzzerPin, LOW);}
      // turn everything off at the end of unity
      else if (incomingByte[0] == 5) { TurnEverythingOff();}
    }
    while (Serial.available() > 0){
      Serial.read();
    }
  }
  ledCheck();

  // button stuff
  buttonValue = digitalRead(button);
  if (buttonValue == HIGH)
  {
    if (buttonValue != lastButtonValue)
    {
      lastButtonValue = buttonValue;
      Serial.println("Button Move Released");
    }
  }
  else
  {
    if (buttonValue != lastButtonValue)
    {
      lastButtonValue = buttonValue;
      Serial.println("Button Move Pressed");
    }
  }

  // rotary encoder check
  pin_clk_current = digitalRead(pin_clk);
  if (pin_clk_current != pin_clk_last){
    if (digitalRead(pin_dt) != pin_clk_current){
      // pin clk changed
      counter ++;
      direction = true;
    }
    else {
      // pin_dt changed
      counter --;
      direction = false;
    }
    Serial.println(counter);
  }
  pin_clk_last = pin_clk_current;
  if (!digitalRead(button_pin) && counter != 0){
    counter = 0;
    Serial.println("reset position");
  }

  // light barrier test:
  lightvalue = digitalRead(lightSensor);
  if (lightvalue == HIGH)
  {
    if (lastlightvalue != lightvalue)
    {
      Serial.println("Light Barrier Closed");
    }
  }
  else
  {
    if (lastlightvalue != lightvalue)
    {
      Serial.println("Light Barrier Open");
    }
  }
  lastlightvalue = lightvalue;

  // buzzer alarm
  if (playSound)
  {
    digitalWrite (buzzerPin, HIGH);
  }
  else
  {
    digitalWrite (buzzerPin, LOW);
  }
}

void ledCheck()
{
  if (LED){
    digitalWrite(ledPin, HIGH);
  } else {
    digitalWrite(ledPin, LOW);
  }
  return;
}

void TurnEverythingOff()
{
  LED = false;
  ledCheck();
  playSound = false;
  digitalWrite (buzzerPin, LOW);
}